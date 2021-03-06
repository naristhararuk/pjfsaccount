/*
 *
 * Wijmo Library 3.20152.78
 * http://wijmo.com/
 *
 * Copyright(c) GrapeCity, Inc.  All rights reserved.
 * 
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * licensing@wijmo.com
 * http://wijmo.com/widgets/license/
 * ----
 * Credits: Wijmo includes some MIT-licensed software, see copyright notices below.
 */
/*
* Wijmo KnockoutJS Binding Factory.
*
* Depends:
*  knockoutjs.js
*
*/

(function (define) {
    define(["knockout", "jquery"], function (ko, $) {
        //extend ko.numericObservable
        ko.numericObservable = function (initialValue) {
            var _actual = ko.observable(initialValue);

            var result = ko.dependentObservable({
                read: function () {
                    return _actual();
                },
                write: function (newValue) {
                    var parsedValue = parseFloat(newValue);
                    _actual(isNaN(parsedValue) ? newValue : parsedValue);
                }
            });

            return result;
        };

        var _removingNode = false;
        // Refactor ko.removeNode : For fixing case 95545
        // If jquery has included, when ko remove the node,
        // the "remove" event of jquery.ui.widget will be triggered firstly, widget will invoke "destroy" method, 
        // the wapper will be destroyed, the original widget element will not be removed since it thought that its parent no longer exists. 
        var removeNodeFunc = ko.removeNode;
        ko.removeNode = function (node) {
            _removingNode = true;

            if (removeNodeFunc) {
                removeNodeFunc.call(ko, node);
            }

            _removingNode = false;
        }

        ko.wijmo = ko.wijmo || {};

        ko.wijmo.toOption = function (name, value) {
            switch (name.toLowerCase()) {
                case "data":
                case "datasource":
                    if (typeof value === "object" && !$.isArray(value) && !$.isPlainObject(value)) {
                        return value;
                    }
                    break;
            }

            return ko.toJS(value);
        };
        ko.wijmo.toOptions = function (obj) {
            var options = {};
            $.each(obj, function (name, value) {
                options[name] = ko.wijmo.toOption(name, value);
            });
            return options;
        };

        ko.wijmo.updateWriteableObservableValue = function (observableValue, newValue) {
            if (ko.isWriteableObservable(observableValue)) {
                var tempValue = observableValue();
                if (($.isPlainObject(tempValue) || $.isArray(tempValue)) && newValue) {
                    $.each(tempValue, function (key, value) {
                        ko.wijmo.updateWriteableObservableValue(value, newValue[key]);
                    });
                } else {
                    tempValue = newValue;
                }
                observableValue(tempValue);
            } else {
                if (($.isPlainObject(observableValue) || $.isArray(observableValue)) && newValue) {
                    $.each(observableValue, function (key, value) {
                        ko.wijmo.updateWriteableObservableValue(value, newValue[key]);
                    });
                } else {
                    observableValue = newValue;
                }
            }
        };

        ko.wijmo.customBindingFactory = function () {
            var self = this;

            self.customBinding = function (options) {
                var binding = {},
                    widgetName = options.widgetName,
                    namespace = "wijmo",
                    widget,
                    vAccessor,
                    updatingFromEvents = false,
                    updatingEventSource = null,
                    updatingFromOtherObservables = false;

                binding.init = function (element, valueAccessor, allBindingAccessor, viewModel) {
                    //element: The DOM element involved in this binding
                    //valueAccessor: A JavaScript function that you can call to get the current model property 
                    //  that is involved in this binding. Call this without passing any parameters 
                    //  (i.e., call valueAccessor()) to get the current model property value.
                    //allBindingsAccessor: A JavaScript function that you can call to get all the model properties 
                    //  bound to this DOM element. Like valueAccessor, call it without any parameters to get the 
                    //  current bound model properties.
                    //viewModel: The view model object that was passed to ko.applyBindings. 
                    //  Inside a nested binding context, this parameter will be set to the current data item 
                    //  (e.g., inside a with: person binding, viewModel will be set to person).
                    var va = ko.utils.unwrapObservable(valueAccessor()),
                        opts;
                    // for #72442 issue: when original element's position changed, the va change to original element, it correct.
                    if (va === element) return;

                    //init widget
                    var opts = ko.wijmo.toOptions(va);

                    // attach proxies
                    $.each(va, function (key, value) {
                        var option;
                        if (options.observableOptions && (option = options.observableOptions[key]) && option.proxy) {
                            if (option.proxy.validSource(value)) {
                                opts[key] = option.proxy.create(value, options); // replace original value with the proxy.
                                hasProxy(element, widgetName, key, true); // indicate that proxy is set
                            }
                        }
                    });

                    // create widget
                    widget = $(element)[widgetName](opts).data(namespace + "-" + widgetName);

                    // attach events
                    $.each(va, function (key, value) {
                        if (!options.observableOptions || !options.observableOptions[key]) {
                            return true;
                        }
                        var observableOption = options.observableOptions[key],
                            optType = observableOption.type;
                        if (!ko.isObservable(value)) {
                            return true;
                        }
                        //attach event.
                        var attachEvents = observableOption.attachEvents;
                        if (attachEvents) {
                            $.each(attachEvents, function (idx, ev) {
                                ko.utils.registerEventHandler(element, widgetName + ev.toLowerCase(), function () {
                                    // add vAccessor and update it in update event, because sometimes the reference of
                                    // value accessor  will be updated by customer.
                                    vAccessor = $(element).data("vAccessor");
                                    var v = vAccessor[key],
                                        newVal,
                                        widgetInst = $(element).data(namespace + "-" + widgetName);

                                    if (updatingFromOtherObservables) {
                                        return;
                                    }

                                    updatingFromEvents = true;
                                    updatingEventSource = element;

                                    if ($.isFunction(observableOption.onChange)) {
                                        if (v) {
                                            observableOption.onChange.call(observableOption, widgetInst, v, arguments);
                                        } else {
                                            observableOption.onChange.call(observableOption, widgetInst, value, arguments);
                                        }
                                    } else {
                                        newVal = $(element)[widgetName]("option", key);
                                        //TODO: If newVal is reference type, we should extend it before assignment
                                        if (ko.isWriteableObservable(v)) {
                                            ko.wijmo.updateWriteableObservableValue(v, newVal);
                                        } else if (ko.isWriteableObservable(value)) {
                                            ko.wijmo.updateWriteableObservableValue(value, newVal);
                                        }
                                    }

                                    updatingEventSource = null;
                                    updatingFromEvents = false;
                                });
                            });
                        }
                    });

                    if (widget && widget.element && widget.element[0]) {
                        ko.utils.domNodeDisposal.addDisposeCallback(widget.element[0], function (node) {
                            if (_removingNode && node.parentNode) {
                                node.parentNode.removeChild(node);
                            }
                        });
                    }
                };

                binding.update = function (element, valueAccessor, allBindingAccessor, viewModel) {
                    //element: The DOM element involved in this binding
                    //valueAccessor: A JavaScript function that you can call to get the current model property 
                    //  that is involved in this binding. Call this without passing any parameters 
                    //  (i.e., call valueAccessor()) to get the current model property value.
                    //allBindingsAccessor: A JavaScript function that you can call to get all the model properties 
                    //  bound to this DOM element. Like valueAccessor, call it without any parameters to get the 
                    //  current bound model properties.
                    //viewModel: The view model object that was passed to ko.applyBindings. 
                    //  Inside a nested binding context, this parameter will be set to the current data item 
                    //  (e.g., inside a with: person binding, viewModel will be set to person).

                    var valueUnwrapped = ko.utils.unwrapObservable(valueAccessor());
                    //vAccessor = valueUnwrapped;
                    $(element).data("vAccessor", valueUnwrapped);
                    $.each(valueUnwrapped, function (key, value) {
                        //The observable can be used like following: style: { width: percentMax() * 100 + '%' },
                        //the style.width is not an observable value and cannot be observed in ko.computed.
                        //So we need to check if the value is updated in binding.update.
                        var observableOption = options.observableOptions[key];
                        if (observableOption) {
                            var optType = observableOption.type,
                                $element = $(element),
                               val = ko.wijmo.toOption(key, ko.utils.unwrapObservable(value)),
                                hash = $element.data(widgetName + '_ko'),
                               widgetVal = hash && (key in hash)
                                   ? hash[key]
                                    : $element[widgetName]("option", key),

                                widgetInst = $element.data(namespace + '-' + widgetName),
                                proxy = observableOption.proxy,
                                proxyInst = proxy && getProxyInst(element, widgetName, key);

                            if ((updatingFromEvents && (element === updatingEventSource)) || (proxyInst && proxy.ignoreChanges(widgetInst, proxyInst))) {
                                return true;
                            }

                            if (optType && optType === 'numeric') {
                                var parsedVal = parseFloat(val);
                                val = isNaN(parsedVal) ? val : parsedVal;
                            }

                            if (proxyInst) {
                                widgetVal = proxy.getValueToCompare(proxyInst);
                            }

                            if (!equals(val, widgetVal)) {
                                updateOptions(element, widgetName, key, val, options.observableOptions);
                            }
                        }
                    });
                };

                executeOptions = function (element, widgetName, observableOptions) {
                    var data = $(element).data(widgetName + '_ko'), hash = (data) ? data : {};
                    if (!$.isEmptyObject(hash)) {
                        $(element).data(widgetName + '_ko', 0);
                        for (var key in hash) {
                            var val = hash[key],
                                proxy = observableOptions[key].proxy,
                                proxyInst = proxy && getProxyInst(element, widgetName, key);

                            if (proxyInst && proxy.needRefresh($(element).data("vAccessor"), val)) {
                                proxy.refresh(proxyInst);
                            } else {
                                if (proxyInst) {
                                    hasProxy(element, widgetName, key, false); // clear proxy marker
                                }

                                $(element)[widgetName]("option", key, val);
                            }
                        }
                    }
                };

                updateOptions = function (element, widgetName, key, val, observableOptions) {
                    var data = $(element).data(widgetName + '_ko'), hash = (data) ? data : {};
                    hash[key] = val;
                    $(element).data(widgetName + '_ko', hash);
                    setTimeout(function () {
                        updatingFromOtherObservables = true;
                        executeOptions(element, widgetName, observableOptions);
                        $(element).trigger("optionsUpdated"); // #69329 customer asks for a callback timing
                        updatingFromOtherObservables = false;
                    }, 100);
                };

                equals = function (sourceValue, targetValue, parentSourceValue) {
                    var equal = false;
                    if ((sourceValue === undefined) || (sourceValue === null)) {
                        return false;
                    }
                    if (sourceValue === targetValue) {
                        return true;
                    }
                    if ((targetValue === undefined) || (targetValue === null) || (sourceValue.constructor !== targetValue.constructor)) {
                        return false;
                    }
                    if ($.isPlainObject(sourceValue)) {
                        if (sourceValue.constructor !== Object.prototype.constructor || targetValue.constructor !== Object.prototype.constructor) {
                            return false;
                        }
                        equal = true;
                        $.each(sourceValue, function (key, val) {
                            if (parentSourceValue && (val === parentSourceValue)) { // avoid recursion (#7256, BreezeJS)
                                equal = false;
                                return false;
                            }
                            if (typeof targetValue[key] === 'undefined') {
                                equal = false;
                                return false;
                            }
                            if (!equals(val, targetValue[key], sourceValue)) {
                                equal = false;
                                return false;
                            }
                        });
                    } else if ($.isArray(sourceValue)) {
                        if (sourceValue.length !== targetValue.length) {
                            return false;
                        }
                        equal = true;
                        $.each(sourceValue, function (idx, val) {
                            if (!equals(val, targetValue[idx])) {
                                equal = false;
                                return false;
                            }
                        });
                    } else if (isDate(sourceValue)) {
                        return sourceValue == targetValue;
                    }
                    return equal;
                };

                isDate = function (obj) {
                    if (!obj) {
                        return false;
                    }
                    return (typeof obj === 'object') && obj.constructor === Date;
                };

                hasProxy = function (element, widgetName, optionName, boolValue) {
                    var marker = widgetName + '_' + optionName + '_proxy';

                    if (arguments.length === 4) {
                        $(element).data(marker, boolValue);
                    } else {
                        return !!$(element).data(marker);
                    }
                }

                getProxyInst = function (element, widgetName, optionName) {
                    if (hasProxy(element, widgetName, optionName)) {
                        return $(element)[widgetName]('option', optionName);
                    }

                    return undefined;
                }

                ko.bindingHandlers[options.widgetName] = binding;
            };
        };

        ko.wijmo.customBindingFactory = new ko.wijmo.customBindingFactory();

        var createCustomBinding = ko.wijmo.customBindingFactory.customBinding.bind(ko.wijmo.customBindingFactory);

        //Wijmo Bindings
        createCustomBinding({
            widgetName: "wijbarchart",
            observableOptions: {
                disabled: {},
                stacked: {},
                header: {
                },
                dataSource: {},
                seriesList: {
                    type: 'array',
                    attachEvents: ['serieschanged']
                },
                seriesStyles: {
                    type: 'array'
                },
                seriesHoverStyles: {
                    type: 'array'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijbubblechart",
            observableOptions: {
                disabled: {},
                dataSource: {},
                seriesList: {
                    type: 'array',
                    attachEvents: ['serieschanged']
                },
                seriesStyles: {
                    type: 'array'
                },
                seriesHoverStyles: {
                    type: 'array'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijcompositechart",
            observableOptions: {
                disabled: {},
                dataSource: {},
                seriesList: {
                    type: 'array',
                    attachEvents: ['serieschanged']
                },
                seriesStyles: {
                    type: 'array'
                },
                seriesHoverStyles: {
                    type: 'array'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijlinechart",
            observableOptions: {
                disabled: {},
                dataSource: {},
                seriesList: {
                    type: 'array',
                    attachEvents: ['serieschanged']
                },
                seriesStyles: {
                    type: 'array'
                },
                seriesHoverStyles: {
                    type: 'array'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijpiechart",
            observableOptions: {
                disabled: {},
                dataSource: {},
                seriesList: {
                    type: 'array',
                    attachEvents: ['serieschanged']
                },
                seriesStyles: {
                    type: 'array'
                },
                seriesHoverStyles: {
                    type: 'array'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijscatterchart",
            observableOptions: {
                disabled: {},
                dataSource: {},
                seriesList: {
                    type: 'array',
                    attachEvents: ['serieschanged']
                },
                seriesStyles: {
                    type: 'array'
                },
                seriesHoverStyles: {
                    type: 'array'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijcandlestickchart",
            observableOptions: {
                disabled: {},
                dataSource: {},
                seriesList: {
                    type: 'array',
                    attachEvents: ['serieschanged']
                },
                seriesStyles: {
                    type: 'array'
                },
                seriesHoverStyles: {
                    type: 'array'
                }
            }
        })

        createCustomBinding({
            widgetName: "wijlineargauge",
            observableOptions: {
                disabled: {},
                min: {
                    type: 'numeric'
                },
                max: {
                    type: 'numeric'
                },
                value: {
                    type: 'numeric'
                },
                face: {},
                pointer: {},
                labels: {},
                disabled: {},
                ranges: {
                    type: 'array'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijradialgauge",
            observableOptions: {
                disabled: {},
                face: {},
                pointer: {},
                cap: {},
                labels: {},
                min: {
                    type: 'numeric'
                },
                max: {
                    type: 'numeric'
                },
                value: {
                    type: 'numeric'
                },
                ranges: {
                    type: 'array'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijslider",
            observableOptions: {
                disabled: {},
                animate: {},
                max: {
                    type: 'numeric'
                },
                min: {
                    type: 'numeric'
                },
                orientation: {},
                range: {},
                step: {
                    type: 'numeric'
                },
                value: {
                    type: 'numeric',
                    attachEvents: ['change', 'slide']
                },
                values: {
                    type: 'array',
                    attachEvents: ['change', 'slide']
                },
                dragFill: {},
                minRange: {
                    type: 'numeric'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijprogressbar",
            observableOptions: {
                disabled: {},
                value: {
                    type: 'numeric',
                    attachEvents: ['change']
                },
                labelAlign: {},
                maxValue: {
                    type: 'numeric'
                },
                minValue: {
                    type: 'numeric'
                },
                fillDirection: {},
                orientation: {},
                labelFormatString: {},
                toolTipFormatString: {},
                indicatorIncrement: {
                    type: 'numeric'
                },
                indicatorImage: {},
                animationDelay: {
                    type: 'numeric'
                },
                animationOptions: {}
            }
        });

        createCustomBinding({
            widgetName: "wijrating",
            observableOptions: {
                disabled: {},
                min: {
                    type: 'numeric'
                },
                max: {
                    type: 'numeric'
                },
                value: {
                    type: 'numeric',
                    attachEvents: ['rated', 'reset']
                },
                count: {
                    type: 'numeric'
                },
                totalValue: {
                    type: 'numeric'
                },
                split: {
                    type: 'numeric'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijflipcard",
            observableOptions: {
                disabled: {},
                height: {
                    type: 'numeric'
                },
                width: {
                    type: 'numeric'
                },
                triggerEvent: {
                }
            }
        });

        createCustomBinding({
            widgetName: "wijgallery",
            observableOptions: {
                disabled: {},
                autoPlay: {},
                showTimer: {},
                interval: {
                    type: 'numeric'
                },
                data: {
                    type: 'array'
                },
                showCaption: {},
                showCounter: {},
                showPager: {},
                thumbnails: {},
                thumbsDisplay: {
                    type: 'numeric'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijcarousel",
            observableOptions: {
                disabled: {},
                auto: {},
                showTimer: {},
                interval: {
                    type: 'numeric'
                },
                data: {
                    type: 'array'
                },
                loop: {},
                showPager: {},
                showCaption: {},
                display: {
                    type: 'numeric'
                },
                preview: {},
                step: {
                    type: 'numeric'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijsplitter",
            observableOptions: {
                disabled: {},
                showExpander: {},
                splitterDistance: {
                    type: 'numeric',
                    attachEvents: ['sized']
                },
                fullSplit: {}
            }
        });

        createCustomBinding({
            widgetName: "wijsuperpanel",
            observableOptions: {
                disabled: {},
                allowResize: {},
                autoRefresh: {},
                mouseWheelSupport: {},
                showRounder: {}
            }
        });

        createCustomBinding({
            widgetName: "wijtooltip",
            observableOptions: {
                disabled: {},
                closeBehavior: {},
                mouseTrailing: {},
                showCallout: {},
                showDelay: {
                    type: 'numeric'
                },
                hideDelay: {
                    type: 'numeric'
                },
                content: {},
                title: {},
                group: {},
                controlheight: {},
                controlwidth: {},
                cssClass: {},
                calloutFilled: {},
                modal: {},
                triggers: {}
            }
        });

        createCustomBinding({
            widgetName: "wijvideo",
            observableOptions: {
                disabled: {},
                fullScreenButtonVisible: {},
                showControlsOnHover: {}
            }
        });

        createCustomBinding({
            widgetName: "wijtabs",
            observableOptions: {
                disabled: {},
                collapsible: {}
            }
        });

        createCustomBinding({
            widgetName: "wijexpander",
            observableOptions: {
                disabled: {},
                allowExpand: {},
                expanded: {
                    attachEvents: ['aftercollapse', 'afterexpand']
                },
                expandDirection: {}
            }
        });

        createCustomBinding({
            widgetName: "wijdialog",
            observableOptions: {
                disabled: {},
                autoOpen: {},
                draggable: {},
                modal: {},
                contentUrl: {},
                resizable: {}
            }
        });

        createCustomBinding({
            widgetName: "wijcalendar",
            observableOptions: {
                disabled: {},
                showTitle: {},
                showWeekDays: {},
                showWeekNumbers: {},
                showOtherMonthDays: {},
                showDayPadding: {},
                allowPreview: {},
                allowQuickPick: {},
                popupMode: {},
                selectedDates: {
                    type: 'array',
                    attachEvents: ['selecteddateschanged'],
                    onChange: function (widgetInstance, viewModelValue, originalEventArgs) {
                        var dates = originalEventArgs[1].dates;
                        if (ko.isObservable(viewModelValue)) {
                            viewModelValue(dates);
                        } else {
                            viewModelValue = dates;
                        }
                    }
                }
            }
        });

        createCustomBinding({
            widgetName: "wijaccordion",
            observableOptions: {
                disabled: {},
                requireOpenedPane: {},
                selectedIndex: {
                    attachEvents: ['selectedindexchanged']
                }
            }
        });

        createCustomBinding({
            widgetName: "wijtree",
            observableOptions: {
                disabled: {},
                allowTriState: {},
                autoCheckNodes: {},
                autoCollapse: {},
                showCheckBoxes: {},
                showExpandCollapse: {},
                nodes: {
                    type: "array",
                    attachEvents: ['nodeCheckChanged', 'nodeCollapsed', 'nodeExpanded',
                        'nodeTextChanged', 'selectedNodeChanged']
                }
            }
        });

        createCustomBinding({
            widgetName: "wijgrid",
            observableOptions: {
                disabled: {},
                pageIndex: {
                    type: 'numeric'
                },
                pageSize: {
                    type: 'numeric'
                },
                totalRows: {
                    type: 'numeric'
                },
                data: {
                    type: 'array',
                    proxy: {
                        validSource: function (source) {
                            return wijmo.grid.koDataView.validSource(source);
                        },
                        needRefresh: function (valueAccessor, unwrappedValue) {
                            var modelValue = valueAccessor["data"];

                            return (wijmo.grid.koDataView.validSource(modelValue)
                                && !wijmo.data.isDataView(unwrappedValue)); // an attempt to change the data option to a new dataView? Do not refresh (#66592).
                        },
                        create: function (originalArray) {
                            return new wijmo.grid.koDataView(originalArray);
                        },
                        getValueToCompare: function (dataView) {
                            return dataView.getPlainSource();
                        },
                        ignoreChanges: function (widgetInstance, dataView) {
                            return widgetInstance.element.data('ignoreChanges') === true;
                        },
                        refresh: function (dataView) {
                            dataView.refresh();
                        }
                    },
                    attachEvents: ['beforecellupdate', 'aftercelledit'],
                    onChange: function (widgetInstance, viewModelValue, originalEventArgs) {
                        var e = originalEventArgs[0];
                        widgetInstance.element.data('ignoreChanges', (e.type.indexOf('beforecellupdate') >= 0)); // ignore all changes until the aftercelledit event will not be called.
                    }
                }
            }
        });

        createCustomBinding({
            widgetName: "wijevcal",
            observableOptions: {
                disabled: {},
                eventsData: {
                    type: 'array',
                    attachEvents: ['eventsdatachanged']
                },
                appointments: {
                    type: 'array',
                    attachEvents: ['eventsdatachanged']
                }
            }
        });

        createCustomBinding({
            widgetName: "wijpager",
            observableOptions: {
                disabled: {},
                pageCount: { type: "numeric" },
                pageIndex: {
                    type: "numeric",
                    attachEvents: ['pageindexchanged']
                }
            }
        });

        createCustomBinding({
            widgetName: "wijeditor",
            observableOptions: {
                disabled: {},
                editorMode: {},
                showPathSelector: {},
                mode: {},
                showFooter: {},
                text: {
                    type: 'string',
                    attachEvents: ['textChanged']
                }
            }
        });

        createCustomBinding({
            widgetName: "wijlist",
            observableOptions: {
                disabled: {},
                listItems: {
                    type: 'array'
                },
                dataSource: {},
                selectionMode: {},
                autoSize: {},
                maxItemsCount: {
                    type: 'numeric'
                },
                addHoverItemClass: {},
                keepHightlightOnMouseLeave: {}
            }
        });

        createCustomBinding({
            widgetName: "wijcombobox",
            observableOptions: {
                disabled: {},
                data: {
                    type: 'array'
                },
                dataSource: {},
                labelText: {},
                showTrigger: {},
                triggerPosition: {},
                autoFilter: {},
                autoComplete: {},
                highlightMatching: {},
                selectionMode: {},
                isEditable: {},
                selectedIndex: {
                    attachEvents: ['changed']
                },
                selectedValue: {
                    attachEvents: ['changed']
                },
                text: {
                    attachEvents: ['changed']
                },
                inputTextInDropDownList: {
                    attachEvents: ['changed']
                }
            }
        });

        createCustomBinding({
            widgetName: "wijmenu",
            observableOptions: {
                disabled: {},
                trigger: {},
                triggerEvent: {},
                mode: {},
                checkable: {},
                orientation: {}
            }
        });

        createCustomBinding({
            widgetName: "wijtextbox",
            observableOptions: {
                disabled: {}
            }
        });

        createCustomBinding({
            widgetName: "wijdropdown",
            observableOptions: {
                disabled: {}
            }
        });

        createCustomBinding({
            widgetName: "wijcheckbox",
            observableOptions: {
                disabled: {},
                checked: {
                    type: 'bool',
                    attachEvents: ['changed']
                }
            }
        });

        createCustomBinding({
            widgetName: "wijradio",
            observableOptions: {
                disabled: {},
                checked: {
                    type: 'bool',
                    attachEvents: ['changed']
                }
            }
        });

        createCustomBinding({
            widgetName: "wijribbon",
            observableOptions: {
                disabled: {}
            }
        });

        createCustomBinding({
            widgetName: "wijinputdate",
            observableOptions: {
                disabled: {},
                date: {
                    attachEvents: ['dateChanged', 'textChanged']
                },
                minDate: {},
                maxDate: {}
            }
        });

        createCustomBinding({
            widgetName: "wijinputmask",
            observableOptions: {
                disabled: {},
                text: {
                    attachEvents: ['textChanged']
                }
            }
        });

        createCustomBinding({
            widgetName: "wijinputnumber",
            observableOptions: {
                disabled: {},
                maxValue: {
                    type: 'numeric'
                },
                minValue: {
                    type: 'numeric'
                },
                value: {
                    type: 'numeric',
                    attachEvents: ['valueChanged', 'textChanged']
                }
            }
        });

        createCustomBinding({
            widgetName: "wijinputtext",
            observableOptions: {
                disabled: {},
                text: {
                    attachEvents: ['textChanged']
                }
            }
        });

        //jQuery UI Bindings

        createCustomBinding({
            widgetName: "accordion",
            observableOptions: {
                disabled: {}
            }
        });

        createCustomBinding({
            widgetName: "autocomplete",
            observableOptions: {
                disabled: {},
                source: {}
            }
        });

        createCustomBinding({
            widgetName: "button",
            observableOptions: {
                disabled: {},
                label: {}
            }
        });

        createCustomBinding({
            widgetName: "datepicker",
            observableOptions: {
                disabled: {}
            }
        });

        createCustomBinding({
            widgetName: "dialog",
            observableOptions: {
                disabled: {},
                autoOpen: {},
                draggable: {},
                modal: {},
                resizable: {}
            }
        });

        createCustomBinding({
            widgetName: "progressbar",
            observableOptions: {
                disabled: {},
                value: {
                    type: 'numeric'
                }
            }
        });

        createCustomBinding({
            widgetName: "slider",
            observableOptions: {
                disabled: {},
                value: {
                    type: 'numeric',
                    attachEvents: ['change']
                },
                min: {
                    type: 'numeric'
                },
                max: {
                    type: 'numeric'
                },
                values: {
                    type: 'array',
                    attachEvents: ['change']
                }
            }
        });

        createCustomBinding({
            widgetName: "tabs",
            observableOptions: {
                disabled: {},
                selected: {
                    type: 'numeric'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijdatepager",
            observableOptions: {
                disabled: {},
                firstDayOfWeek: {
                    type: 'numeric'
                },
                viewType: {},
                nextTooltip: {},
                prevTooltip: {}
            }
        });

        createCustomBinding({
            widgetName: "wijlightbox",
            observableOptions: {
                disabled: {},
                textPosition: {},
                showCounter: {},
                showNavButtons: {},
                showTimer: {},
                showControlsOnHover: {},
                clickPause: {},
                keyNav: {},
                modal: {},
                closeOnEscape: {},
                closeOnOuterClick: {},
                autoPlay: {},
                delay: {
                    type: 'numeric'
                },
                loop: {}
            }
        });

        createCustomBinding({
            widgetName: "wijpopup",
            observableOptions: {
                disabled: {},
                ensureOuterMost: {},
                autoHide: {},
                showEffect: {},
                hideEffect: {}
            }
        });

        /*
        createCustomBinding({
            widgetName: "wijupload",
            observableOptions: {
                disabled: {},
                autoSubmit: {},
                multiple: {},
                maximumFiles: {
                    type: 'numeric'
                },
                enableSWFUploadOnIE: {},
                enableSWFUpload: {}
                }
        });
        */

        createCustomBinding({
            widgetName: "wijwizard",
            observableOptions: {
                disabled: {},
                navButtons: {},
                autoPlay: {},
                delay: {
                    type: 'numeric'
                },
                loop: {},
                backBtnText: {},
                nextBtnText: {}
            }
        });

        createCustomBinding({
            widgetName: "wijsparkline",
            observableOptions: {
                disabled: {},
                type: {},
                width: {},
                height: {},
                valueAxis: {},
                origin: {},
                min: {},
                max: {},
                columnWidth: {},
                tooltipFormat: {},
                bind: {},
                data: {
                    type: 'array'
                },
                seriesList: {
                    type: 'array'
                },
                seriesStyles: {
                    type: 'array'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijtreemap",
            observableOptions: {
                disabled: {},
                width: {
                    type: 'numeric'
                },
                height: {
                    type: 'numeric'
                },
                type: {},
                valueBinding: {},
                labelBinding: {},
                colorBinding: {},
                showLabel: {},
                labelFormatter: {},
                showTooltip: {},
                tooltipOptions: {},
                showTitle: {},
                titleHeight: {
                    type: 'numeric'
                },
                titleFormatter: {},
                minColor: {},
                minColorValue: {
                    type: 'numeric'
                },
                midColor: {},
                midColorValue: {
                    type: 'numeric'
                },
                maxColor: {},
                maxColorValue: {
                    type: 'numeric'
                },
                showBackButtons: {},
                data: {
                    type: 'array'
                }
            }
        });

        createCustomBinding({
            widgetName: "wijfileexplorer",
            observableOptions: {
                mode: {},
                searchPatterns: {},
                initPath: {},
                viewPaths: {
                    type: 'array'
                },
                viewMode: {},
                allowFileExtensionRename: {},
                allowMultipleSelection: {},
                allowPaging: {},
                pageSize: {
                    type: 'numeric'
                },
                shortcuts: {},
                enableOpenFile: {},
                enableCreateNewFolder: {},
                enableCopy: {},
                enableFilteringOnEnterPressed: {},
                currentFolder: {
                    attachEvents: ["folderChanged"]
                },
                visibleControls: {},
                treePanelWidth: {
                    type: 'numeric'
                },
                hostUri: {},
                actionUri: {},
                disabled: {}
            }
        });

        createCustomBinding({
            widgetName: "wijmaps",
            observableOptions: {
                source: {},
                center: {
                    attachEvents: ["centerChanged"]
                },
                zoom: {
                    type: 'numeric',
                    attachEvents: ["zoomChanged"]
                },
                showTools: {},
                targetZoom: {
                    type: 'numeric',
                    attachEvents: ["targetZoomChanged"]
                },
                targetZoomSpeed: {
                    type: 'numeric'
                },
                targetCenter: {
                    attachEvents: ["targetCenterChanged"]
                },
                targetCenterSpeed: {
                    type: 'numeric'
                },
                layers: {
                    type: 'array'
                },
                disabled: {}
            }
        });

        window.ko = ko;
    });

})(typeof define !== "undefined" ? define : function (deps, body) {
    body(ko, jQuery);
});