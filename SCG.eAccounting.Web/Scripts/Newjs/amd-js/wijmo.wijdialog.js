var wijmo;define(["./wijmo.widget","./wijmo.wijutil","jquery.bgiframe"],function(){var e=this.__extends||function(e,t){function r(){this.constructor=e}for(var n in t)t.hasOwnProperty(n)&&(e[n]=t[n]);r.prototype=t.prototype,e.prototype=new r};(function(t){(function(n){var r=jQuery,i="wijdialog",s=function(t){function n(){t.apply(this,arguments)}return e(n,t),n.prototype._create=function(){return r.ui.dialog.prototype._create.apply(this,arguments)},n.prototype._init=function(){return r.ui.dialog.prototype._init.apply(this,arguments)},n.prototype._destroy=function(){return r.ui.dialog.prototype._destroy.apply(this,arguments)},n.prototype._appendTo=function(){return r.ui.dialog.prototype._appendTo.apply(this,arguments)},n.prototype._setOptions=function(){return r.ui.dialog.prototype._setOptions.apply(this,arguments)},n.prototype._setOption=function(e,t){return r.ui.dialog.prototype._setOption.apply(this,arguments)},n.prototype.widget=function(){return r.ui.dialog.prototype.widget.apply(this,arguments)},n.prototype.close=function(){return r.ui.dialog.prototype.close.apply(this,arguments)},n.prototype.isOpen=function(){return r.ui.dialog.prototype.isOpen.apply(this,arguments)},n.prototype.moveToTop=function(){return r.ui.dialog.prototype.moveToTop.apply(this,arguments)},n.prototype._moveToTop=function(){return r.ui.dialog.prototype._moveToTop.apply(this,arguments)},n.prototype.open=function(){return r.ui.dialog.prototype.open.apply(this,arguments)},n.prototype._focusTabbable=function(){return r.ui.dialog.prototype._focusTabbable.apply(this,arguments)},n.prototype._keepFocus=function(){return r.ui.dialog.prototype._keepFocus.apply(this,arguments)},n.prototype._createWrapper=function(){return r.ui.dialog.prototype._createWrapper.apply(this,arguments)},n.prototype._createTitlebar=function(){return r.ui.dialog.prototype._createTitlebar.apply(this,arguments)},n.prototype._title=function(){return r.ui.dialog.prototype._title.apply(this,arguments)},n.prototype._createButtonPane=function(){return r.ui.dialog.prototype._createButtonPane.apply(this,arguments)},n.prototype._createButtons=function(){return r.ui.dialog.prototype._createButtons.apply(this,arguments)},n.prototype._makeDraggable=function(){return r.ui.dialog.prototype._makeDraggable.apply(this,arguments)},n.prototype._makeResizable=function(){return r.ui.dialog.prototype._makeResizable.apply(this,arguments)},n.prototype._minHeight=function(){return r.ui.dialog.prototype._minHeight.apply(this,arguments)},n.prototype._position=function(){return r.ui.dialog.prototype._position.apply(this,arguments)},n.prototype._size=function(){return r.ui.dialog.prototype._size.apply(this,arguments)},n.prototype._blockFrames=function(){return r.ui.dialog.prototype._blockFrames.apply(this,arguments)},n.prototype._unblockFrames=function(){return r.ui.dialog.prototype._unblockFrames.apply(this,arguments)},n.prototype._createOverlay=function(){return r.ui.dialog.prototype._createOverlay.apply(this,arguments)},n.prototype._destroyOverlay=function(){return r.ui.dialog.prototype._destroyOverlay.apply(this,arguments)},n.prototype._trackFocus=function(){var e;this._on(this.widget(),{focusin:function(t){this._untrackInstance(),this._trackingInstances().unshift(this),e=r(t.target).parents(".ui-dialog-content"),e&&e.length>0&&(this._focusedElement=r(t.target))}})},n}(t.JQueryUIWidget);n.JQueryUIDialog=s;var o=function(){function e(){this.wijCSS={wijdialog:"wijmo-wijdialog",wijdialogZone:"wijmo-wijdialog-defaultdockingzone",uiFront:"ui-front",uiDialog:"ui-dialog",wijdialogOverlay:"ui-dialog-overlay",uiDialogContent:"ui-dialog-content",wijdialogCaptionButton:"wijmo-wijdialog-captionbutton",wijdialogHasFrame:"wijmo-wijdialog-hasframe",iconPinW:"ui-icon-pin-w",iconPinS:"ui-icon-pin-s",iconRefresh:"ui-icon-refresh",iconCarat1N:"ui-icon-carat-1-n",iconCarat1S:"ui-icon-carat-1-s",iconMinus:"ui-icon-minus",iconExtlink:"ui-icon-extlink",iconNewWin:"ui-icon-newwin",uiDialogClose:"ui-dialog-titlebar-close",uiDialogTitleBar:"ui-dialog-titlebar",uiDialogButtonPanel:"ui-dialog-buttonpane",uiDialogButtons:"ui-dialog-buttons",wijdialogTitleBarClose:"",wijdialogTitleBarPin:"",wijdialogTitleBarRefresh:"",wijdialogTitleBarToggle:"",wijdialogTitleBarMinimize:"",wijdialogTitleBarMaximize:"",wijdialogTitleBarRestore:""},this.appendTo="body",this.autoOpen=!0,this.buttons=[],this.closeOnEscape=!0,this.closeText="Close",this.dialogClass="",this.draggable=!0,this.height="auto",this.hide=null,this.maxHeight=null,this.maxWidth=null,this.minHeight=150,this.minWidth=150,this.modal=!1,this.position={my:"center",at:"center",of:window},this.resizable=!0,this.show=null,this.title=null,this.width=300,this.stack=!0,this.beforeClose=null,this.close=null,this.create=null,this.drag=null,this.dragStart=null,this.dragStop=null,this.focus=null,this.open=null,this.resize=null,this.resizeStart=null,this.resizeStop=null,this.zIndex=1e3,this.initSelector="",this.captionButtons={},this.collapsingAnimation=null,this.expandingAnimation=null,this.contentUrl="",this.minimizeZoneElementId="",this.buttonCreating=null,this.stateChanged=null,this.blur=null}return e}(),u="wijmo-wijdialog-titlebar-",a=["blind","bounce","clip","drop","explode","fold","size","shake","slide","transfer"],f=function(t){function n(){t.apply(this,arguments)}return e(n,t),n.prototype._create=function(){var e=this,n=e.options,i=n.wijCSS,s=e.element,o=s.attr("title");e.buttonKeys={},r.ui.dialog.maxZ=r.ui.dialog.maxZ||100,s.uniqueId(),r.isArray(n.buttons)&&r.each(n.buttons,function(e,t){var n=t.click;n&&typeof n=="string"&&window[n]&&(t.click=window[n])}),e.form=s.closest("form[id]"),t.prototype._create.call(this),e._replaceClasses(s,["ui-dialog-content","ui-widget-content"],[i.uiDialogContent,i.content]),e._replaceClasses(e.uiDialog,["ui-dialog","ui-widget","ui-widget-content","ui-corner-all","ui-front"],[i.uiDialog,i.widget,i.content,i.cornerAll,i.uiFront,i.wijdialog,e.options.dialogClass]),o&&e.uiDialog.attr("title",o),e._replaceClasses(e.uiDialogTitlebar,["ui-dialog-titlebar","ui-widget-header","ui-corner-all","ui-helper-clearfix"],[i.uiDialogTitleBar,i.header,i.cornerAll,i.helperClearFix]),e._initWijWindow(),e._bindWindowResize(),e._attachDraggableResizableEvent(),e._originalPosition=n.position,e.uiDialog.css({zIndex:n.zIndex}),e.isPin=!1,e._initialCloseOption(),e._bindFormOnSubimt()},n.prototype._bindFormOnSubimt=function(){var e=this;if(e.options.appendTo!=="body"||e.form.length===0)return;e.formOnSubmit=e.form[0].onsubmit,e.form[0].onsubmit=function(){return e.minimized?e.uiDialog.parent().appendTo(e.form):e.uiDialog.appendTo(e.form),e.formOnSubmit?e.formOnSubmit():!0},e.uiDialog.find("input[type='submit'], button[type='submit']").on("click.wijdialog",function(){e.form.submit()})},n.prototype._unbindFormOnSubmit=function(){var e=this;if(e.options.appendTo!=="body"||e.form.length===0)return;e.uiDialog.find("input[type='submit'], button[type='submit']").off("click.wijdialog"),e.form[0].onsubmit=null,e.formOnSubmit&&(e.form[0].onsubmit=e.formOnSubmit)},n.prototype._replaceClasses=function(e,t,n){e.removeClass(t.join(" ")).addClass(n.join(" "))},n.prototype._createButtonPane=function(){var e=this,n=e.options.wijCSS;t.prototype._createButtonPane.call(this),e._replaceClasses(e.uiDialogButtonPane,["ui-dialog-buttonpane","ui-widget-content","ui-helper-clearfix"],[n.uiDialogButtonPanel,n.content,n.helperClearFix])},n.prototype._createButtons=function(){t.prototype._createButtons.call(this),this.uiDialog.hasClass("ui-dialog-buttons")&&this._replaceClasses(this.uiDialog,["ui-dialog-buttons"],[this.options.wijCSS.uiDialogButtons])},n.prototype._makeDraggable=function(){t.prototype._makeDraggable.call(this);var e=this.uiDialog.draggable("option","cancel");e||(e=""),e+=(e===""?".":", .")+this.options.wijCSS.wijdialogCaptionButton+", "+"input",this.uiDialog.draggable("option","cancel",e)},n.prototype._createOverlay=function(){var e=this,n=this.options.wijCSS;t.prototype._createOverlay.call(this),e.overlay&&e._replaceClasses(e.overlay,["ui-widget-overlay","ui-front","ui-dialog-overlay"],[n.overlay,n.uiFront,n.wijdialogOverlay])},n.prototype._handleDisabledOption=function(e){var t=this;t.uiDialog.removeClass(t.options.wijCSS.stateDisabled),e?(t.disabledDiv||(t.disabledDiv=t._createDisabledDiv()),t.disabledDiv.appendTo(t.options.appendTo),r.browser.msie&&t.uiDialog.data("ui-draggable")&&t.uiDialog.draggable("disable"),this.uiDialog.addClass(this.options.wijCSS.stateDisabled)):(t._destroyDisabledDiv(),r.browser.msie&&t.uiDialog.data("ui-draggable")&&t.uiDialog.draggable("enable"))},n.prototype._createDisabledDiv=function(){var e=this,t,n=e.uiDialog,i=n.offset(),s=n.outerWidth(),o=n.outerHeight();return t=r("<div></div>").addClass(e.options.wijCSS.stateDisabled).css({"z-index":"99999",position:"absolute",width:s,height:o,left:i.left,top:i.top}),r.browser.msie&&(t.css("background-color","white"),r.browser.version==="9.0"&&t.css("opacity","0.1")),t},n.prototype._destroyDisabledDiv=function(){var e=this;e.disabledDiv&&(e.disabledDiv.remove(),e.disabledDiv=null)},n.prototype._sizeDisabledDiv=function(){var e=this,t=e.uiDialog;e.disabledDiv&&e.disabledDiv.css({width:t.outerWidth(!0),height:t.outerHeight(!0)})},n.prototype._positionDisabledDiv=function(){var e=this,t=e.uiDialog.offset();e.disabledDiv&&e.disabledDiv.css({left:t.left,top:t.top})},n.prototype._size=function(){t.prototype._size.call(this),this._sizeDisabledDiv()},n.prototype._destroyResizable=function(){this.options.resizable&&r.fn.resizable&&this.uiDialog.resizable("destroy")},n.prototype._destroyDraggable=function(){this.options.draggable&&r.fn.draggable&&this.uiDialog.draggable("destroy")},n.prototype._unbindCaptionButtons=function(){var e=this,t=e.options.wijCSS,n=r("."+t.wijdialogCaptionButton,e.uiDialog);n&&n.length>0&&n.off().remove()},n.prototype._clearVariables=function(){var e=this;e._originalPosition=undefined,e.uiDialog=undefined,e.isPin=undefined,e.disabledDiv=undefined,e.innerFrame=undefined,e.uiDialogButtonPane=undefined,e.contentWrapper=undefined,e.form=undefined,e.toggleHeight=undefined,e.minimized=undefined,e.collapsed=undefined,e.maximized=undefined,e._toggleHeight=undefined,e.normalState=undefined,e.initWidth=undefined,e.initHeight=undefined,e.copy=undefined,e._isOpen=undefined,e.formOnSubmit=undefined,e.opener=undefined,e.document=undefined,e.buttonKeys&&(r.each(e.buttonKeys,function(t){e[t]&&(e[t]=undefined)}),e.buttonKeys=undefined),e.uiDialogTitlebar=undefined},n.prototype.destroy=function(){var e=this,n=e.options.wijCSS;e._destroyDisabledDiv(),e._unbindFormOnSubmit(),e.uiDialog.unbind(".wijdialog"),e._unbindWindowResize(),e._destoryIframeMask(),e.element.removeUniqueId(),e.element.removeAttr("scrollTop").removeAttr("scrollLeft"),e._destroyResizable(),e._destroyDraggable(),e._unbindCaptionButtons(),t.prototype.destroy.call(this),e.element.removeClass(n.uiDialogContent).removeClass(n.content),e.element.unbind(".wijdialog").removeData("wijdialog").removeData(this.widgetFullName),e._clearVariables()},n.prototype._attachDraggableResizableEvent=function(){var e=this,t=e.uiDialog,n=e.options;n.draggable&&t.draggable&&t.bind("dragstop.wijdialog",function(t){e._saveNormalState(),e._destoryIframeMask()}).bind("dragstart.wijdialog",function(t){e._createIframeMask()}),n.resizable&&t.resizable&&t.bind("resizestop.wijdialog",function(t){e._saveNormalState(),e._destoryIframeMask()}).bind("resizestart.wijdialog",function(t){e._createIframeMask(),e.initWidth===undefined&&e.initHeight===undefined&&(e.initWidth=e.uiDialog.width(),e.initHeight=e.uiDialog.height())})},n.prototype._createIframeMask=function(){var e=this;e.innerFrame&&(e.mask=r("<div style='width:100%;height:100%;position:absolute;top:0px;left:0px;z-index:"+(r.ui.dialog.maxZ+1)+"'></div>").appendTo(e.uiDialog))},n.prototype._destoryIframeMask=function(){var e=this;e.innerFrame&&e.mask&&(e.mask.remove(),e.mask=undefined)},n.prototype._initWijWindow=function(){var e=this,t=!0;e._createCaptionButtons(),e._checkUrl(),e.uiDialog.bind("mousedown.wijdialog",function(t){var n=t.target;!r.contains(e.element[0],n)&&!(r(n).closest("."+e.options.wijCSS.wijdialogCaptionButton).length>0)&&e.uiDialog.focus()}).bind("mouseenter.wijdialog",function(e){t=!0}).bind("mouseleave.wijdialog",function(e){t=!1}).bind("focusout.wijdialog",function(n){t||e._trigger("blur",n,{el:e.element})})},n.prototype._moveToTop=function(e,t){var n=this,i=n.options,s;return i.modal&&!t||!i.stack&&!i.modal?(n._trigger("focus",e),!1):(i.zIndex>r.ui.dialog.maxZ&&(r.ui.dialog.maxZ=i.zIndex),n.overlay&&(r.ui.dialog.maxZ+=1,n.overlay.css("z-index",r.ui.dialog.maxZ)),s={scrollTop:n.element.scrollTop(),scrollLeft:n.element.scrollLeft()},r.ui.dialog.maxZ+=1,n.uiDialog.css("z-index",r.ui.dialog.maxZ),n.element.attr(s),n._trigger("focus",e),!1)},n.prototype._checkUrl=function(){var e=this,t=e.options,n=t.contentUrl,i=r('<iframe style="width:100%;height:99%;" frameborder="0"></iframe>');typeof n=="string"&&n.length>0&&(e.element.addClass(t.wijCSS.wijdialogHasFrame),e.element.append(i),e.innerFrame=i),e.contentWrapper=e.element},n.prototype._setOption=function(e,n){var i=this,s=i._isDisabled(),o;t.prototype._setOption.call(this,e,n);if(e==="disabled"){i.options.disabled=n,o=i._isDisabled();if(s===o)return;i._handleDisabledOption(o)}else if(e==="contentUrl"){if(i.getState()==="minimized"||!i.innerFrame&&!n)return;i.innerFrame?i.innerFrame.attr("src",n):(i._checkUrl(),i.innerFrame.attr("src",n))}else e==="captionButtons"?i._createCaptionButtons():e==="minimizeZoneElementId"?i.getState()==="minimized"&&r("#"+n).length>0&&r("#"+n).append(i.uiDialog):e==="stack"?n||i.uiDialog.css("z-index",i.options.zIndex):e==="close"&&i._initialCloseOption()},n.prototype._createCaptionButtons=function(){var e=[],t=this,n=t.options,i,s=n.wijCSS,o={pin:{visible:!0,click:t.pin,title:t._getLocalizedString("pin","Pin"),iconClassOn:s.iconPinW,iconClassOff:s.iconPinS},refresh:{visible:!0,click:t.refresh,title:t._getLocalizedString("refresh","Refresh"),iconClassOn:s.iconRefresh},toggle:{visible:!0,click:t.toggle,title:t._getLocalizedString("toggle","Toggle"),iconClassOn:s.iconCarat1N,iconClassOff:s.iconCarat1S},minimize:{visible:!0,click:t.minimize,title:t._getLocalizedString("minimize","Minimize"),iconClassOn:s.iconMinus},maximize:{visible:!0,click:t.maximize,title:t._getLocalizedString("maximize","Maximize"),iconClassOn:s.iconExtlink},close:{visible:!0,text:n.closeText,click:t.close,title:t._getLocalizedString("close","Close"),iconClassOn:s.iconClose}},u=n.captionButtons,a=t.uiDialogTitlebar,f=a.children("."+n.wijCSS.uiDialogClose+", ."+n.wijCSS.wijdialogCaptionButton);t._off&&t._off(f,"click"),f.remove(),r.each(o,function(t,n){u&&u[t]&&r.extend(n,u[t]),e.push({button:t,info:n})}),t._trigger("buttonCreating",null,e);for(i=0;i<e.length;i++)t._createCaptionButton(e[i],a)},n.prototype._createCaptionButton=function(e,t,n){var i=this,s=i.options.wijCSS,o,a=t.children("."+u+e.button),f=e.info,l=r("<span></span>"),c=e.button+"Button";if(f.visible){if(a.size()===0){l.addClass([s.icon,f.iconClassOn].join(" ")).text(f.text||e.button),o=r('<a href="#"></a>').append(l).addClass([u+e.button,s["wijdialogTitleBar"+e.button.charAt(0).toUpperCase()+e.button.substring(1)],s.cornerAll,s.wijdialogCaptionButton].join(" ")).attr("role","button").attr("title",f.title?f.title:"").hover(function(){o.addClass(s.stateHover)},function(){o.removeClass(s.stateHover)}).click(function(e){return l.hasAllClasses(f.iconClassOff)?l.removeClass(f.iconClassOff):l.addClass(f.iconClassOff),r.isFunction(f.click)&&f.click.apply(i,arguments),!1});if(n)return o;o.appendTo(t)}i[c]=o,i.buttonKeys[c]=!0}else a.remove()},n.prototype.pin=function(){var e=this,t=e.isPin,n=e.pinButton.children("span"),r=e.options.wijCSS;t?n.removeClass(r.iconPinS):n.length&&(n.hasAllClasses(r.iconPinS)||n.addClass(r.iconPinS)),e._enableDisableDragger(!t),e.isPin=!t},n.prototype.refresh=function(){var e=this.innerFrame;e!==undefined&&e.attr("src",e.attr("src"))},n.prototype.toggle=function(){var e=this,t=e.toggleButton.children("span"),n=e.options.wijCSS;if(e.minimized)return;e.collapsed?(e.collapsed=!1,t.hasAllClasses(n.iconCarat1S)&&t.removeClass(n.iconCarat1S),e._expandDialogContent(!0)):(e.collapsed=!0,t.hasAllClasses(n.iconCarat1S)||t.addClass(n.iconCarat1S),e._collapseDialogContent(!0))},n.prototype._expandDialogContent=function(e){var t=this,n=t.options,i=n.expandingAnimation;t.uiDialog.height("auto"),e&&i!==null?t.contentWrapper.show(i.animated,i.options,i.duration,function(e){t.uiDialog.css("height",t._toggleHeight),r.isFunction(i.callback)&&i.callback(e),n.resizable&&t._enableDisableResizer(!1)}):(t.contentWrapper.show(),n.resizable&&t._enableDisableResizer(!1),t.uiDialog.css("height",t.toggleHeight))},n.prototype._collapseDialogContent=function(e,t){var n=this,r=n.options,i=r.collapsingAnimation;r.resizable&&n._enableDisableResizer(!0),t||(n._toggleHeight=n.uiDialog[0].style.height),n.uiDialog.height("auto"),e&&i!==null?n.contentWrapper.hide(i.animated,i.options,i.duration):n.contentWrapper.hide(),n._enableDisableDragger(n.isPin)},n.prototype._enableDisableResizer=function(e){var t=this.uiDialog;if(!this.options.resizable)return;t.resizable({disabled:e}),e&&t.removeClass(this.options.wijCSS.stateDisabled)},n.prototype._enableDisableDragger=function(e){var t=this.uiDialog;if(!this.options.draggable)return;t.draggable({disabled:e}),e&&t.removeClass(this.options.wijCSS.stateDisabled)},n.prototype._position=function(){var e=this.options.position,t=[],n=[0,0],i;if(e){if(typeof e=="string"||typeof e=="object"&&"0"in e)t=e.split?e.split(" "):[e[0],e[1]],t.length===1&&(t[1]=t[0]),r.each(["left","top"],function(e,r){+t[e]===t[e]&&(n[e]=t[e],t[e]=r)}),e={my:t[0]+(n[0]<0?n[0]:"+"+n[0])+" "+t[1]+(n[1]<0?n[1]:"+"+n[1]),at:t.join(" ")};e=r.extend({},r.ui.dialog.prototype.options.position,e)}else e=r.ui.dialog.prototype.options.position;i=this.uiDialog.is(":visible"),i||this.uiDialog.show(),this.uiDialog.position(e),this._positionDisabledDiv(),i||this.uiDialog.hide()},n.prototype.minimize=function(){var e=this,t=e.uiDialog,n=e.options,i=n.wijCSS,s=null,o=r("<div></div>"),u=r("<div></div>"),a,f,l,c,h,p,d={},v={},m="uiDialog";if(e.minimized)return;c=t.position(),d={width:t.width(),height:t.height()},p=e.getState(),e.maximized?(e.maximized=!1,e.restoreButton.remove(),r(window).unbind(".onWinResize")):(e.collapsed&&e._expandDialogContent(!1),e._saveNormalState()),e._enableDisableResizer(!0),e.collapsed&&e._collapseDialogContent(!1),o.appendTo(e.options.appendTo).css({top:t.offset().top,left:t.offset().left,height:t.innerHeight(),width:t.innerWidth(),position:"absolute"}),e.contentWrapper.hide(),e.uiDialogButtonPane.length&&e.uiDialogButtonPane.hide(),t.height("auto"),t.width("auto"),e._doButtonAction(e.minimizeButton,"hide"),e._restoreButton(!0,e.minimizeButton,"After"),e._doButtonAction(e.pinButton,"hide"),e._doButtonAction(e.refreshButton,"hide"),e._doButtonAction(e.toggleButton,"hide"),e._doButtonAction(e.maximizeButton,"show"),r.browser.webkit&&r("."+i.wijdialogCaptionButton,t).css("float","left"),e.innerFrame&&(m="copy",e[m]=t.clone(),e[m].empty(),e.uiDialogTitlebar.appendTo(e[m])),n.minimizeZoneElementId.length>0&&(s=r("#"+n.minimizeZoneElementId)),s!==null&&s.size()>0?s.append(e[m]):(a=r("."+i.wijdialogZone),a.size()===0&&(a=r('<div class="'+i.wijdialogZone+'"></div>'),r(document.body).append(a)),a.append(e[m]).css("z-index",t.css("z-index"))),e[m].css("position","static"),e[m].css("float","left"),r.browser.msie&&r.browser.version==="6.0"&&(f=r(document).scrollTop(),l=document.documentElement.clientHeight-a.height()+f,a.css({position:"absolute",left:"0px",top:l})),u.appendTo("body").css({top:e[m].offset().top,left:e[m].offset().left,height:e[m].innerHeight(),width:e[m].innerWidth(),position:"absolute"}),t.hide(),e.innerFrame&&e[m].hide(),o.effect("transfer",{to:u,className:i.content},100,function(){o.remove(),u.remove(),e[m].show(),e.minimized=!0,h=t.position(),v={width:t.width(),height:t.height()},e._enableDisableDragger(!0),e._trigger("resize",null,{originalPosition:c,originalSize:d,position:h,size:v}),e._trigger("stateChanged",null,{originalState:p,state:"minimized"})})},n.prototype._doButtonAction=function(e,t){e!==undefined&&(e.removeClass(this.options.wijCSS.stateHover),e[t]())},n.prototype.maximize=function(){var e=this,t=r(window),n=e.uiDialog,i={},s={},o,u,a;if(e.maximized)return;e._enableDisableDragger(!1),a=n.position(),i={width:n.width(),height:n.height()},e.minimized?e.restore():(e.collapsed&&e._expandDialogContent(!1),e._saveNormalState(),o="normal"),e.maximized=!0,e.maximizeButton!==undefined&&(e.maximizeButton.hide(),e._restoreButton(!0,e.maximizeButton,"Before")),r.browser.webkit&&r("."+this.options.wijCSS.wijdialogCaptionButton).css("float",""),e._onWinResize(e,t),e.collapsed&&e._collapseDialogContent(!1),e.collapsed||e._enableDisableDragger(!0),e._enableDisableResizer(!0),u=n.position(),s={width:n.width(),height:n.height()},e._trigger("resize",null,{originalPosition:a,originalSize:i,position:u,size:s}),o==="normal"&&e._trigger("stateChanged",null,{originalState:"normal",state:"maximized"})},n.prototype._bindWindowResize=function(){var e=this,t=r(window),n=e.element.attr("id").replace(/-/g,""),i="resize."+n,s="scroll."+n;t.bind(i,function(){e.maximized&&e._onWinResize(e,t)}),r.browser.msie&&r.browser.version==="6.0"&&t.bind(s+" "+i,function(){var t,n,i;e.minimized&&(n=r(document).scrollTop(),i=e.uiDialog.parent(),t=document.documentElement.clientHeight-i.height()+n,i.css("top",t))})},n.prototype._unbindWindowResize=function(){var e=this.element.attr("id").replace(/-/g,"");r(window).unbind("."+e)},n.prototype._saveNormalState=function(){var e=this,t=e.uiDialog,n=e.element;if(e.maximized)return;e.normalState={width:parseFloat(t.css("width")),left:parseFloat(t.css("left")),top:parseFloat(t.css("top")),height:parseFloat(t.css("height")),innerHeight:parseFloat(n.css("height")),innerWidth:parseFloat(n.css("width")),innerMinWidth:parseFloat(n.css("min-width")),innerMinHeight:parseFloat(n.css("min-height"))}},n.prototype._onWinResize=function(e,t){var n=e.uiDialog;n.css({top:t.scrollTop(),left:t.scrollLeft()}),n.setOutWidth(t.width()),n.setOutHeight(t.height()),e.options.width=n.width(),e.options.height=n.height(),e._size(),e.collapsed&&(n.height("auto"),e.contentWrapper.hide())},n.prototype._restoreButton=function(e,t,n){var r=this,i={button:"restore",info:{visible:e,click:r.restore,iconClassOn:this.options.wijCSS.iconNewWin}},s=r._createCaptionButton(i,r.uiDialogTitlebar,!0);e&&(s["insert"+n](t),r.restoreButton=s)},n.prototype._appendToBody=function(e){this.innerFrame?(this.uiDialogTitlebar.prependTo(e),e.show()):e.appendTo(this.options.appendTo)},n.prototype.restore=function(){var e=this,t=e.uiDialog,n=e.options.contentUrl,i={},s={},o,u,a,f=r("<div></div>"),l=r("<div></div>"),c="uiDialog";e.minimized?(e.minimized=!1,e.innerFrame&&(c="copy",e[c]||(c="uiDialog")),a=e[c].position(),i={width:e[c].width(),height:e[c].height()},f.appendTo(e.options.appendTo).css({top:e[c].offset().top,left:e[c].offset().left,height:e[c].innerHeight(),width:e[c].innerWidth(),position:"absolute"}),t.css("position","absolute"),t.css("float",""),e._appendToBody(t),e._enableDisableResizer(!1),e.isPin||e._enableDisableDragger(!1),e._restoreToNormal(),e.contentWrapper.show(),e.uiDialogButtonPane.length&&e.uiDialogButtonPane.show(),l.appendTo(e.options.appendTo).css({top:t.offset().top,left:t.offset().left,height:t.innerHeight(),width:t.innerWidth(),position:"absolute"}),t.hide(),f.effect("transfer",{to:l,className:this.options.wijCSS.content},150,function(){t.show(),o=t.position(),s.width=t.width(),s.height=t.height(),f.remove(),l.remove(),e.copy&&e.copy.remove(),e._trigger("resize",null,{originalPosition:a,originalSize:i,position:o,size:s}),u=e.getState(),e._trigger("stateChanged",null,{originalState:"minimized",state:u})}),e.collapsed&&e._collapseDialogContent(),e._doButtonAction(e.minimizeButton,"show"),e._doButtonAction(e.restoreButton,"remove"),e._doButtonAction(e.pinButton,"show"),e._doButtonAction(e.refreshButton,"show"),e._doButtonAction(e.toggleButton,"show"),r.browser.webkit&&r("."+this.options.wijCSS.wijdialogCaptionButton).css("float",""),typeof n=="string"&&n.length>0&&(e.innerFrame||e._checkUrl(),e.innerFrame.attr("src")!==n&&e.innerFrame.attr("src",n),e.element.css("width",e.normalState.width))):e.maximized&&(e.maximized=!1,a=t.position(),i={width:t.width(),height:t.height()},r(window).unbind(".onWinResize"),e.collapsed&&e._expandDialogContent(),e._enableDisableResizer(!1),e.isPin||e._enableDisableDragger(!1),e._restoreToNormal(),e.contentWrapper.show(),e.collapsed&&e._collapseDialogContent(),e.maximizeButton!==undefined&&(e.maximizeButton.show(),e._restoreButton(!1,e.maximizeButton,"before")),o=t.position(),s={width:t.width(),height:t.height()},e._trigger("resize",null,{originalPosition:a,originalSize:i,position:o,size:s}),u=e.getState(),e._trigger("stateChanged",null,{originalState:"maximized",state:u}))},n.prototype.getState=function(){var e=this;return e.minimized?"minimized":e.maximized?"maximized":"normal"},n.prototype.reset=function(){var e=this;e.getState()==="normal"?e._reset():(e.element.one(this.widgetEventPrefix+"statechanged",function(){e._reset()}),e.restore())},n.prototype._reset=function(){var e=this;e.normalState={},e.initHeight&&e.initWidth&&(e.options.width=e.initWidth,e.options.height=e.initHeight,e._size()),e._setOption("position",e._originalPosition)},n.prototype.open=function(){var e=this,n=e.options;(n.hide==="drop"||n.hide==="bounce")&&r.browser.msie&&e.uiDialog.css("filter","auto"),e.innerFrame?(e._isIE9()?(n.show&&a.indexOf(n.show)>-1&&(n.show=null),window.setTimeout(function(){e.innerFrame.attr("src",n.contentUrl)},200)):e.innerFrame.attr("src",n.contentUrl),e.minimized?(e._setOpener(),e.uiDialogTitlebar.show(),e._isOpen=!0,e._trigger("open")):t.prototype.open.call(this)):e.minimized?(e._setOpener(),e.uiDialog.show(),e._isOpen=!0,e._trigger("open")):(t.prototype.open.call(this),e.uiDialog.wijTriggerVisibility()),e.collapsed&&e._collapseDialogContent(!1,!0),e._isDisabled()&&(e.disabledDiv?e.disabledDiv.show():e._handleDisabledOption(!0))},n.prototype._isDisabled=function(){var e=this.options;return e.disabledState===!0||e.disabled===!0},n.prototype._setOpener=function(){var e=this;!e.opener&&e.document&&e.document[0]&&(e.opener=r(e.document[0].activeElement))},n.prototype._isIE9=function(){return r.browser.msie&&parseInt(r.browser.version)===9},n.prototype.close=function(){var e=this,n=e.options;e.innerFrame&&e._isIE9()&&n.hide&&a.indexOf(n.hide)>-1&&(n.hide=null),t.prototype.close.call(this)},n.prototype._initialCloseOption=function(){var e=this,t=e.options.close;e.options.close=function(n){e._closeDialogHelper(),t&&typeof t=="function"&&t(n)}},n.prototype._closeDialogHelper=function(){var e=this;e.innerFrame&&(e.innerFrame.attr("src",""),e.minimized&&e.uiDialogTitlebar.hide()),e.disabledDiv&&e._isDisabled()&&e.disabledDiv.hide()},n.prototype._restoreToNormal=function(){var e=this,t=e.uiDialog,n=e.element,r=e.normalState;t.css({width:r.width,left:r.left,top:r.top,height:r.height}),n.css({height:r.innerHeight,width:r.innerWidth,"min-width":r.innerMinWidth,"min-height":r.innerMinHeight}),e.options.width=t.width(),e.options.height=t.height()},n.prototype._getLocalizedString=function(e,t){var n=this.options.localization;return n&&n[e]?n[e]:t},n}(s);n.wijdialog=f,r.ui&&r.ui.dialog&&(r.extend(r.ui.dialog.overlay,{create:function(e){this.instances.length===0&&(setTimeout(function(){r.ui.dialog.overlay.instances.length&&r(document).bind(r.ui.dialog.overlay.events,function(t){if(r(t.target).zIndex()<r.ui.dialog.overlay.maxZ&&!r.contains(e.element[0],t.target))return!1})},1),r(document).bind("keydown.dialog-overlay",function(n){var r=t.getKeyCodeEnum();e.options.closeOnEscape&&!n.isDefaultPrevented()&&n.keyCode&&n.keyCode===r.ESCAPE&&(e.close(n),n.preventDefault())}),r(window).bind("resize.dialog-overlay",r.ui.dialog.overlay.resize));var n=(this.oldInstances.pop()||r("<div></div>").addClass(e.options.wijCSS.overlay)).appendTo(document.body).css({width:this.width(),height:this.height()});return r.fn.bgiframe&&n.bgiframe(),this.instances.push(n),n},height:function(){var e,t;return r.browser.msie?(e=Math.max(document.documentElement.scrollHeight,document.body.scrollHeight),t=Math.max(document.documentElement.offsetHeight,document.body.offsetHeight),e<t?r(window).height()+"px":e+"px"):r(document).height()+"px"},width:function(){var e,t;return r.browser.msie?(e=Math.max(document.documentElement.scrollWidth,document.body.scrollWidth),t=Math.max(document.documentElement.offsetWidth,document.body.offsetWidth),e<t?r(window).width()+"px":e+"px"):r(document).width()+"px"}}),f.prototype.options=r.extend(!0,{},t.wijmoWidget.prototype.options,r.ui.dialog.prototype.options,new o),f.prototype.disable=function(){return this._setOption("disabled",!0)},f.prototype.enable=function(){return this._setOption("disabled",!1)},r.wijmo.registerWidget(i,r.ui.dialog,f.prototype))})(t.dialog||(t.dialog={}));var n=t.dialog})(wijmo||(wijmo={}))});