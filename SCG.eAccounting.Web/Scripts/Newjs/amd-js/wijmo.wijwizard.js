var wijmo;define(["./wijmo.widget","./wijmo.wijsuperpanel","./wijmo.wijpopup","jquery.cookie"],function(){var e=this.__extends||function(e,t){function r(){this.constructor=e}for(var n in t)t.hasOwnProperty(n)&&(e[n]=t[n]);r.prototype=t.prototype,e.prototype=new r};(function(t){(function(n){var r=jQuery,i={WIJWIZARD:"wijwizard",LOADWIJWIZARD:"load.wijwizard",CACHEWIJWIZARD:"cache.wijwizard",SPINNERWIJWIZARD:"spinner.wijwizard",INTIDWIZARD:"intId.wijwizard",DESTROYWIJWIZARD:"destroy.wijwizard"},s={stepHeaderTemplate:"<li><h1>#{title}</h1>#{desc}</li>",panelTemplate:"<div></div>",spinner:"<em>Loading&#8230;</em>"},o=function(t){function n(){t.apply(this,arguments)}return e(n,t),n.prototype._create=function(){var e=this;this._defaults=s,this._configStr=i,this._isPlaying=!1,this.element.is(":hidden")&&this.element.wijAddVisibilityObserver&&this.element.wijAddVisibilityObserver(function(){e.element.wijRemoveVisibilityObserver&&e.element.wijRemoveVisibilityObserver(),e._pageLize(!1)},this._configStr.WIJWIZARD),this._pageLize(!0),t.prototype._create.call(this)},n.prototype._init=function(){var e=this.options;!this._isDisabled()&&e.autoPlay&&this.play()},n.prototype._setOption=function(e,n){t.prototype._setOption.call(this,e,n);switch(e){case"activeIndex":this.show(n);break;case"navButtons":case"backBtnText":case"nextBtnText":this._createButtons();break;case"delay":this._isPlaying&&(this.stop(),this.play());break;case"autoPlay":n===!0?this.play():this.stop();default:this._pageLize(!1)}},n.prototype.destroy=function(){var e=this.options,n=this,i=[e.wijCSS.wijwizard,e.wijCSS.widget,e.wijCSS.helperClearFix].join(" "),s=[e.wijCSS.widget,e.wijCSS.helperReset,e.wijCSS.wijwizardSteps,e.wijCSS.helperClearFix].join(" "),o=[e.wijCSS.header,e.wijCSS.cornerAll,e.wijCSS.priorityPrimary,e.wijCSS.prioritySecondary].join(" "),u=[e.wijCSS.stateDefault,e.wijCSS.wijwizardActived,e.wijCSS.stateActive,e.wijCSS.stateHover,e.wijCSS.stateFocus,e.wijCSS.stateDisabled,e.wijCSS.wijwizardPanel,e.wijCSS.content,e.wijCSS.wijwizardHide].join(" ");this.abort(),this.stop(),this._removeScroller(),this._removeButtons(),this.element.unbind(".wijwizard").removeClass(i).removeData(this._configStr.WIJWIZARD),this.list&&this.list.removeClass(s).removeAttr("role"),this.lis&&(this.lis.removeClass(o).removeAttr("role"),r.each(this.lis,function(){r.data(this,n._configStr.DESTROYWIJWIZARD)?r(this).remove():r(this).removeAttr("aria-selected")})),r.each(this.panels,function(){var e=r(this).unbind(".wijwizard");e.removeAttr("role"),r.each(["load","cache"],function(t,n){e.removeData(n+".wijwizard")}),r.data(this,n._configStr.DESTROYWIJWIZARD)?e.remove():e.removeClass(u).css({position:"",left:"",top:""}).removeAttr("aria-hidden")}),this.container.replaceWith(this.container.contents()),e.cookie&&this._cookie(null,e.cookie),t.prototype.destroy.call(this)},n.prototype._pageLize=function(e){var t=this.options,n=this,i=/^#.+/;this.list=this.element.children("ol,ul").eq(0),this.list&&this.list.length===0&&(this.list=this.element.find("."+t.wijCSS.wijwizardSteps).eq(0),this.list&&this.list.length===0&&(this.list=null)),this.list&&(this.lis=r("li",this.list));if(e){this.panels=r("> div",this.element),r.each(this.panels,function(e,t){var s=r(t).attr("src");s&&!i.test(s)&&r.data(t,n._configStr.LOADWIJWIZARD,s.replace(/#.*$/,""))});var s=[t.wijCSS.wijwizard,t.wijCSS.widget,t.wijCSS.helperClearFix].join(" "),o=[t.wijCSS.widget,t.wijCSS.helperReset,t.wijCSS.wijwizardSteps,t.wijCSS.helperClearFix].join(" "),u=[t.wijCSS.header,t.wijCSS.cornerAll].join(" "),a=[t.wijCSS.wijwizardContent,t.wijCSS.widget,t.wijCSS.content,t.wijCSS.cornerAll].join(" "),f=[t.wijCSS.wijwizardPanel,t.wijCSS.content].join(" ");this.element.addClass(s),this.list&&(this.list.addClass(o).attr("role","tablist"),this.lis.addClass(u).attr("role","tab")),this.container=r("<div/>"),this.container.addClass(a),this.container.append(this.panels),this.container.appendTo(this.element),this.panels.addClass(f).attr("role","tabpanel"),t.activeIndex===undefined?(typeof t.activeIndex!="number"&&t.cookie&&(t.activeIndex=parseInt(this._cookie(undefined,undefined),10)),typeof t.activeIndex!="number"&&this.panels.filter("."+t.wijCSS.wijwizardActived).length&&(t.activeIndex=this.panels.index(this.panels.filter("."+t.wijCSS.wijwizardActived))),t.activeIndex=t.activeIndex||(this.panels.length?0:-1)):t.activeIndex===null&&(t.activeIndex=-1),t.activeIndex=t.activeIndex>=0&&this.panels[t.activeIndex]||t.activeIndex<0?t.activeIndex:0,this.panels.addClass(t.wijCSS.wijwizardHide).attr("aria-hidden",!0),t.activeIndex>=0&&this.panels.length&&(this.panels.eq(t.activeIndex).removeClass(t.wijCSS.wijwizardHide).addClass(t.wijCSS.wijwizardActived).attr("aria-hidden",!1),this.load(t.activeIndex)),this._createButtons()}else this.panels=r("> div",this.container),t.activeIndex=this.panels.index(this.panels.filter("."+t.wijCSS.wijwizardActived));this._addScrollForContent(),this._refreshStep(),this._initScroller(),t.cookie&&this._cookie(t.activeIndex,t.cookie),t.cache===!1&&this.panels.removeData(this._configStr.CACHEWIJWIZARD);if(t.showOption===undefined||t.showOption===null)t.showOption={};this._normalizeBlindOption(t.showOption);if(t.hideOption===undefined||t.hideOption===null)t.hideOption={};this._normalizeBlindOption(t.hideOption),this.panels.unbind(".wijwizard")},n.prototype._removeButtons=function(){this.backBtn&&this.backBtn.unbind(".wijwizard"),this.nextBtn&&this.nextBtn.unbind(".wijwizard"),this.buttons&&(this.buttons.remove(),this.buttons=undefined)},n.prototype._createButtons=function(){var e=this,t=this.options,n,i=t.backBtnText,s=t.nextBtnText,o=[t.wijCSS.widget,t.wijCSS.stateDefault,t.wijCSS.cornerAll,t.wijCSS.button,t.wijCSS.buttonTextOnly].join(" "),u=[t.wijCSS.wijwizardPrev,t.wijCSS.stateDefault,t.wijCSS.cornerRight].join(" "),a=[t.wijCSS.wijwizardNext,t.wijCSS.stateDefault,t.wijCSS.cornerLeft].join(" ");this._removeButtons();if(t.navButtons==="none")return;this.buttons||(n=t.navButtons,n==="auto"&&(n=this.list?"common":"edge"),this.buttons=r("<div/>"),this.buttons.addClass(t.wijCSS.wijwizardButtons),n==="common"?(this.backBtn=r("<a href='#'/>").addClass(o).append("<span class='"+t.wijCSS.buttonText+"'>"+i+"</span>").appendTo(this.buttons).attr("role","button"),this.nextBtn=r("<a href='#'/>").addClass(o).append("<span class='"+t.wijCSS.buttonText+"'>"+s+"</span>").appendTo(this.buttons).attr("role","button")):(this.backBtn=r("<a href='#'/>").addClass(u).append("<span class='"+t.wijCSS.icon+" "+t.wijCSS.iconArrowLeft+"'></span>").appendTo(this.buttons).attr("role","button"),this.nextBtn=r("<a href='#'/>").addClass(a).append("<span class='"+t.wijCSS.icon+" "+t.wijCSS.iconArrowRight+"'></span>").appendTo(this.buttons).attr("role","button")),this.buttons.appendTo(this.element)),this._setupEvent()},n.prototype._setupEvent=function(e,t){var n=this,r=this.options,i=e||this.backBtn,s=t||this.nextBtn;if(!i||!s)return;i.bind({"click.wijwizard":function(){return n.back(),!1},"mouseover.wijwizard":n._eventHandler().addState(r.wijCSS.stateHover,i),"mouseout.wijwizard":n._eventHandler().removeState(r.wijCSS.stateHover,i),"mousedown.wijwizard":n._eventHandler().addState(r.wijCSS.stateActive,i),"mouseup.wijwizard":n._eventHandler().removeState(r.wijCSS.stateActive,i)}),s.bind({"click.wijwizard":function(){return n.next(),!1},"mouseover.wijwizard":n._eventHandler().addState(r.wijCSS.stateHover,s),"mouseout.wijwizard":n._eventHandler().removeState(r.wijCSS.stateHover,s),"mousedown.wijwizard":n._eventHandler().addState(r.wijCSS.stateActive,s),"mouseup.wijwizard":n._eventHandler().removeState(r.wijCSS.stateActive,s)})},n.prototype._eventHandler=function(){var e=this,t=this.options,n=function(n,r){return function(){if(e._isDisabled())return;r.is(":not(."+t.wijCSS.stateDisabled+")")&&r.addClass(n)}},r=function(t,n){return function(){if(e._isDisabled())return;n.removeClass(t)}};return{addState:n,removeState:r}},n.prototype._refreshStep=function(){var e=this.options;this.lis&&(this.lis.removeClass(e.wijCSS.priorityPrimary).addClass(e.wijCSS.prioritySecondary).attr("aria-selected",!1),e.activeIndex>=0&&e.activeIndex<=this.lis.length-1&&(this.lis&&this.lis.eq(e.activeIndex).removeClass(e.wijCSS.prioritySecondary).addClass(e.wijCSS.priorityPrimary).attr("aria-selected",!0),this.scrollWrap&&this.scrollWrap.wijsuperpanel("scrollChildIntoView",this.lis.eq(e.activeIndex)))),this.buttons&&!e.loop&&(this.backBtn[e.activeIndex<=0?"addClass":"removeClass"](e.wijCSS.stateDisabled).attr("aria-disabled",e.activeIndex===0),this.nextBtn[e.activeIndex>=this.panels.length-1?"addClass":"removeClass"](e.wijCSS.stateDisabled).attr("aria-disabled",e.activeIndex>=this.panels.length-1))},n.prototype._initScroller=function(){if(!this.lis||!this.element.is(":visible"))return;var e=0;r.each(this.lis,function(){e+=r(this).outerWidth(!0)}),this.element.innerWidth()<e?(this.scrollWrap===undefined&&(this.list.wrap("<div class='scrollWrap'></div>"),this.scrollWrap=this.list.parent(),r.effects&&r.effects.save?r.effects.save(this.list,["width","height","overflow"]):r.save&&r.save(this.list,["width","height","overflow"])),this.list.width(e+8),this.scrollWrap.height(this.list.outerHeight(!0)),this.scrollWrap.wijsuperpanel({allowResize:!1,hScroller:{scrollBarVisibility:"hidden"},vScroller:{scrollBarVisibility:"hidden"}})):this._removeScroller()},n.prototype._removeScroller=function(){this.scrollWrap&&(this.scrollWrap.wijsuperpanel("destroy").replaceWith(this.scrollWrap.contents()),this.scrollWrap=undefined,r.effects&&r.effects.restore?r.effects.restore(this.list,["width","height","overflow"]):r.restore&&r.restore(this.list,["width","height","overflow"]))},n.prototype._cookie=function(e,t){var n=this.cookie||(this.cookie=this.options.cookie.name);return r.cookie.apply(null,[n].concat(r.makeArray(arguments)))},n.prototype._normalizeBlindOption=function(e){e.blind===undefined&&(e.blind=!1),e.fade===undefined&&(e.fade=!1),e.duration===undefined&&(e.duration=200);if(typeof e.duration=="string")try{e.duration=parseInt(e.duration,10)}catch(t){e.duration=200}},n.prototype._ui=function(e){return{panel:e,index:this.panels.index(e)}},n.prototype._removeSpinner=function(){this.element.removeClass(this.options.wijCSS.tabsLoading);var e=this.element.data(this._configStr.SPINNERWIJWIZARD);e&&(this.element.removeData(this._configStr.SPINNERWIJWIZARD),e.remove())},n.prototype._showPanel=function(e){var t=this,n=this.options,r=e,i;r.addClass(n.wijCSS.wijwizardActived),(n.showOption.blind||n.showOption.fade)&&n.showOption.duration>0?(i={duration:n.showOption.duration},n.showOption.blind&&(i.height="toggle"),n.showOption.fade&&(i.opacity="toggle"),r.hide().removeClass(n.wijCSS.wijwizardHide).animate(i,n.showOption.duration||"normal","linear",function(){t._resetStyle(r),r.wijTriggerVisibility&&r.wijTriggerVisibility(),t._trigger("show",null,t._ui(r[0])),t._removeSpinner(),r.attr("aria-hidden",!1),t._trigger("activeIndexChanged",null,t._ui(r[0]))})):(r.removeClass(n.wijCSS.wijwizardHide).attr("aria-hidden",!1),r.wijTriggerVisibility&&r.wijTriggerVisibility(),this._trigger("show",null,this._ui(r[0])),this._removeSpinner(),this._trigger("activeIndexChanged",null,this._ui(r[0])))},n.prototype._hidePanel=function(e){var t=this,n=this,r=this.options,i=e,s;i.removeClass(r.wijCSS.wijwizardActived),(r.hideOption.blind||r.hideOption.fade)&&r.hideOption.duration>0?(s={duration:r.hideOption.duration},r.hideOption.blind&&(s.height="toggle"),r.hideOption.fade&&(s.opacity="toggle"),i.animate(s,r.hideOption.duration||"normal","linear",function(){i.addClass(r.wijCSS.wijwizardHide).attr("aria-hidden",!0),t._resetStyle(i),t.element.dequeue(t._configStr.WIJWIZARD)})):(i.addClass(r.wijCSS.wijwizardHide).attr("aria-hidden",!0),this.element.dequeue(this._configStr.WIJWIZARD))},n.prototype._resetStyle=function(e){e.css({display:""}),r.support.opacity||e[0].style.removeAttribute("filter")},n.prototype._addScrollForContent=function(){var e=this,t=e.element.height();if(!this.element.is(":visible"))return;e.buttons&&(t-=e.buttons.outerHeight(!0)),e.list&&(t-=e.list.outerHeight(!0)),t-=e.container.outerHeight(!0)-e.container.innerHeight()-(e.container.innerHeight()-e.container.height()),t<e.container.height()&&e.container.height(t),e.container.css("overflow","auto")},n.prototype.add=function(e,t,n){e===undefined&&(e=this.panels.length),t===undefined&&(t="Step "+e);var i=this,s=this.options,o=r(s.panelTemplate||i._defaults.panelTemplate).data(this._configStr.DESTROYWIJWIZARD,!0),u=[s.wijCSS.wijwizardPanel,s.wijCSS.content,s.wijCSS.cornerAll,s.wijCSS.wijwizardHide].join(" "),a=[s.wijCSS.header,s.wijCSS.cornerAll,s.wijCSS.prioritySecondary].join(" "),f;return o.addClass(u).attr("aria-hidden",!0),e>=this.panels.length?this.panels.length>0?o.insertAfter(this.panels[this.panels.length-1]):o.appendTo(this.container):o.insertBefore(this.panels[e]),this.list&&this.lis&&(f=r((s.stepHeaderTemplate||i._defaults.stepHeaderTemplate).replace(/#\{title\}/g,t).replace(/#\{desc\}/g,n)),f.addClass(a).data(this._configStr.DESTROYWIJWIZARD,!0),e>=this.lis.length?f.appendTo(this.list):f.insertBefore(this.lis[e])),this._pageLize(!1),this.panels.length===1&&(s.activeIndex=0,f.addClass(s.wijCSS.priorityPrimary),o.removeClass(s.wijCSS.wijwizardHide).addClass(s.wijCSS.wijwizardActived).attr("aria-hidden",!1),this.element.queue(this._configStr.WIJWIZARD,function(){i._trigger("show",null,i._ui(i.panels[0]))}),this._refreshStep(),this.load(0)),this._trigger("add",null,this._ui(this.panels[e])),this},n.prototype.remove=function(e){var t=this.options,n=this.panels.eq(e).remove();return this.lis.eq(e).remove(),e<t.activeIndex&&t.activeIndex--,this._pageLize(!1),n.hasClass(t.wijCSS.wijwizardActived)&&this.panels.length>=1&&this.show(e+(e<this.panels.length?0:-1)),this._trigger("remove",null,this._ui(n[0])),this},n.prototype.show=function(e){var t=this;if(e<0||e>=this.panels.length)return this;if(this.element.queue(this._configStr.WIJWIZARD).length>0)return this;var n=this.options,i={nextIndex:0,nextPanel:null},s,o;r.extend(i,this._ui(this.panels[n.activeIndex])),i.nextIndex=e,i.nextPanel=this.panels[e];if(this._trigger("validating",null,i)===!1)return this;s=this.panels.filter(":not(."+n.wijCSS.wijwizardHide+")"),o=this.panels.eq(e),n.activeIndex=e,this.abort(),n.cookie&&this._cookie(n.activeIndex,n.cookie),this._refreshStep();if(!o.length)throw"jQuery UI wijwizard: Mismatching fragment identifier.";return s.length&&this.element.queue(this._configStr.WIJWIZARD,function(){t._hidePanel(s)}),this.element.queue(this._configStr.WIJWIZARD,function(){t._showPanel(o)}),this.load(e),this},n.prototype.next=function(){var e=this.options,t=e.activeIndex+1;return this._isDisabled()?!1:(e.loop&&(t%=this.panels.length),t<this.panels.length?(this.show(t),!0):!1)},n.prototype.back=function(){var e=this.options,t=e.activeIndex-1;return this._isDisabled()?!1:(e.loop&&(t=t<0?this.panels.length-1:t),t>=0?(this.show(t),!0):!1)},n.prototype._popupSpinner=function(){var e=this,t=e.element,n=e.options,i;t.addClass(n.wijCSS.tabsLoading);if(!n.spinner)return;i=t.data(e._configStr.SPINNERWIJWIZARD),i||(i=r("<div/>"),i.addClass(n.wijCSS.wijwizardSpinner),i.html(n.spinner||e._defaults.spinner),i.appendTo(document.body),t.data(e._configStr.SPINNERWIJWIZARD,i),i.wijpopup({showEffect:"blind",hideEffect:"blind"})),i.wijpopup("show",{of:t,my:"center center",at:"center center"})},n.prototype.load=function(e){var t=this,n=t.options,i=t.panels.eq(e)[0],s=t.element,o=r.data(i,t._configStr.LOADWIJWIZARD);return t.abort(),!o||s.queue(t._configStr.WIJWIZARD).length!==0&&r.data(i,t._configStr.CACHEWIJWIZARD)?(s.dequeue(t._configStr.WIJWIZARD),t):(t._popupSpinner(),t.xhr=r.ajax(r.extend({},n.ajaxOptions,{url:o,dataType:"html",success:function(s,o){r(i).html(s),n.cache&&r.data(i,t._configStr.CACHEWIJWIZARD,!0),t._trigger("load",null,t._ui(t.panels[e]));try{n.ajaxOptions&&n.ajaxOptions.success&&n.ajaxOptions.success(s,o)}catch(u){}},error:function(r,s){t._trigger("load",null,t._ui(t.panels[e]));try{n.ajaxOptions&&n.ajaxOptions.error&&n.ajaxOptions.error(r,s,e,i)}catch(o){}}})),s.dequeue(t._configStr.WIJWIZARD),t)},n.prototype.abort=function(){return this.element.queue([]),this.panels.stop(!1,!0),this.element.queue(this._configStr.WIJWIZARD,this.element.queue(this._configStr.WIJWIZARD).splice(-2,2)),this.xhr&&(this.xhr.abort(),delete this.xhr),this._removeSpinner(),this},n.prototype.url=function(e,t){return this.panels.eq(e).removeData(this._configStr.CACHEWIJWIZARD).data(this._configStr.LOADWIJWIZARD,t),this},n.prototype.count=function(){return this.panels.length},n.prototype.stop=function(){var e=this.element.data(this._configStr.INTIDWIZARD);e&&(window.clearInterval(e),this.element.removeData(this._configStr.INTIDWIZARD)),this._isPlaying=!1},n.prototype.play=function(){var e=this,t=this.options,n,r=this.panels.length;this.element.data(this._configStr.INTIDWIZARD)||(n=window.setInterval(function(){var n=t.activeIndex+1;e._isPlaying=!0;if(n>=r){if(!t.loop){e.stop();return}n=0}e.show(n)},t.delay),this.element.data(this._configStr.INTIDWIZARD,n))},n}(t.wijmoWidget);n.wijwizard=o;var u=function(){function e(){this.wijCSS={wijwizard:"wijmo-wijwizard",wijwizardButtons:"wijmo-wijwizard-buttons",wijwizardPrev:"wijmo-wijwizard-prev",wijwizardNext:"wijmo-wijwizard-next",wijwizardSteps:"wijmo-wijwizard-steps",wijwizardContent:"wijmo-wijwizard-content",wijwizardPanel:"wijmo-wijwizard-panel",wijwizardActived:"wijmo-wijwizard-actived",wijwizardHide:"wijmo-wijwizard-hide",wijwizardSpinner:"wijmo-wijwizard-spinner"},this.wijMobileCSS={header:"ui-header ui-bar-a",content:"ui-body-b",stateDefault:"ui-btn ui-btn-a",stateHover:"ui-btn-down-a",stateActive:"ui-btn-down-a"},this.navButtons="auto",this.autoPlay=!1,this.delay=3e3,this.loop=!1,this.hideOption={fade:!0},this.showOption={fade:!0,duration:400},this.ajaxOptions=null,this.cache=!1,this.cookie=null,this.stepHeaderTemplate="",this.panelTemplate="",this.spinner="",this.backBtnText="back",this.nextBtnText="next",this.add=null,this.remove=null,this.activeIndexChanged=null,this.show=null,this.load=null,this.validating=null}return e}();o.prototype.options=r.extend(!0,{},t.wijmoWidget.prototype.options,new u),r.wijmo.registerWidget("wijwizard",o.prototype)})(t.wizard||(t.wizard={}));var n=t.wizard})(wijmo||(wijmo={}))});