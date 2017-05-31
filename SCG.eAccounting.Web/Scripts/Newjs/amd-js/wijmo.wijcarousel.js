var wijmo;define(["./wijmo.widget","./wijmo.wijpager","./wijmo.wijslider","jquery.bgiframe"],function(){var e=this.__extends||function(e,t){function r(){this.constructor=e}for(var n in t)t.hasOwnProperty(n)&&(e[n]=t[n]);r.prototype=t.prototype,e.prototype=new r};(function(t){(function(n){var r=jQuery,i="wijcarousel",s="wijmo-wijcarousel-",o="wijmo-wijcarousel ",u=s+"list",a=s+"item ",f=s+"clip",l=s+"current",c=s+"horizontal-multi",h=s+"horizontal",p=s+"button",d=s+"vertical-multi",v=s+"vertical",m=s+"button-next",g=s+"button-previous",y=s+"preview",b="<a><span></span></a>",w=".wijmo-wijcarousel-pager,."+p+",."+m+",."+g,E="<div></div>",S='<li class="wijmo-wijcarousel-page"><a></a></li>',x="li.wijmo-wijcarousel-page",T=".wijmo-wijcarousel-text,.wijmo-wijcarousel-caption",N=1,C=function(t){function n(){t.apply(this,arguments)}return e(n,t),n.prototype._handleDisabledOption=function(e,t){var n=this;n._handlePagerState(e),n._applyBtnClass()},n.prototype._handlePagerState=function(e){var t=this,n=t.options,i;t.pager&&!t.pager.is(":hidden")&&(n.pagerType==="numbers"?t.pager.wijpager("option","disabled",e):r.wijmo.wijslider&&n.pagerType==="slider"&&(i=t.pager.find(".ui-slider"),i&&i.length>0&&i.wijslider("option","disabled",e)))},n.prototype._initStates=function(e,t){var n=this;n.count=0,n.currentIdx=e.start,n.timeout=null,n.isHorizontal=e.orientation==="horizontal",n.width=t.width()||640,n.height=t.height()||480,n.offset=0},n.prototype._set_showControlsOnHover=function(e){var t=this;t.container.unbind("mouseenter."+t.widgetName),t.container.unbind("mouseleave."+t.widgetName),e&&t.container.bind("mouseenter."+t.widgetName,function(){if(t._isDisabled())return;t._showControls()}).bind("mouseleave."+t.widgetName,function(){if(t._isDisabled())return;t._hideControls()}),t.container.find(w).stop(!0,!0)[e?"hide":"show"]()},n.prototype._create=function(){var e=this,n=e.options,i=e.element;window.wijmoApplyWijTouchUtilEvents&&(r=window.wijmoApplyWijTouchUtilEvents(r)),e._initStates(n,i),e._createDom(e.isHorizontal),e.list.bind("click."+e.widgetName,r.proxy(e._itemClick,e)),n.showControlsOnHover&&(e.container.bind("mouseenter."+e.widgetName,function(){if(e._isDisabled())return;e._showControls()}).bind("mouseleave."+e.widgetName,function(){if(e._isDisabled())return;e._hideControls()}),e.container.find(w).hide()),n.loadCallback&&r.isFunction(n.loadCallback)&&e._trigger("loadCallback",null,e),!this._isDisabled()&&n.auto&&e.play();try{n.display=parseFloat(n.display)}catch(s){n.display=1}e.element.is(":hidden")&&e.element.wijAddVisibilityObserver&&e.element.wijAddVisibilityObserver(function(){e.refresh(),e.element.wijRemoveVisibilityObserver&&e.element.wijRemoveVisibilityObserver()},"wijcarousel"),t.prototype._create.call(this)},n.prototype._innerDisable=function(){t.prototype._innerDisable.call(this),this.isPlaying&&this.pause(),this._handleDisabledOption(!0,this.element)},n.prototype._innerEnable=function(){t.prototype._innerEnable.call(this),this._handleDisabledOption(!1,this.element)},n.prototype._showControls=function(){this.container.find(w).stop(!0,!0).fadeIn(600,function(){r(this).css("opacity","")})},n.prototype._hideControls=function(){this.container.find(w).stop(!0,!0).fadeOut(600)},n.prototype._applyContainerStyle=function(e,t){var n=this,r=n.options,i={},s=n.list,o=0,u,a=0,f=e?"left":"top",l=0,c,h=e?"width":"height";u=n.itemBound[e?"w":"h"],n.count=n.list.children("li").length,o=n.count*u,n.list[h](o),n.clip[h](r.display*u),n.clip[e?"height":"width"](n.itemBound[e?"h":"w"]);if(!r.loop)i[f]=-u*n.currentIdx,s.css(i);else{c=s.find(">li:first"),r.preview&&!!t&&(c=c.next()),l=c.data("itemIndex");for(l;l<n.currentIdx;l++)s.children("li:first").appendTo(s);if(r.preview){if(!t)for(l=0;l<N;l++)s.children("li:last").prependTo(s);a=-u*N}i[f]=a,s.css(i)}},n.prototype._applyListStyle=function(){var e=this,t=e.options,n=t.orientation==="horizontal",r=0,i,s=n?"width":"height";e.itemBound=e._getItemBound(),i=e.itemBound[n?"w":"h"],r=e.count*i,e.list[s](r)},n.prototype._createButtons=function(e,t){var n=this,i=n.options,s,o=i.wijCSS.stateDisabled;r.each(["prevBtn","nextBtn"],function(u,a){var f=i[a+"Class"],l=a==="nextBtn",c=l?"next":"previous",h=l?m:g,p=l?t:e;f&&f.defaultClass?(o=f.disableClass||o,s=f.hoverClass,n[a]=r('<a class="'+i.wijCSS.stateDefault+'"></a>').addClass(f.defaultClass).mouseover(function(){r(this).hasAllClasses(o)||r(this).addClass(s)}).mouseout(function(){r(this).hasAllClasses(o)||r(this).removeClass(s)})):n[a]=r(n._createBtn(h,p)),n[a].bind("click."+n.widgetName,function(e){if(n._isDisabled())return;var t=r(this);if(t.hasAllClasses(i.wijCSS.stateDisabled))return;n[c].call(n)}).appendTo(n.container);if(r.browser.msie&&n.clip.find("iframe").length>0){var d=n.element.children("iframe.bgiframe."+a+"Frame");d.length===0&&(d=r('<iframe class="bgiframe '+a+'Frame" frameborder="0" tabindex="-1" src="javascript: false;" style="position:absolute;top:0px;left:0px;opacity:0;" />'),d.width(n[a].outerWidth()),d.height(n[a].outerHeight()),n.container.append(d))}})},n.prototype._applyBtnClass=function(){var e=this,t=e.options,n=t.wijCSS.stateDisabled,i=t.wijCSS.stateHover;r.each(["prevBtn","nextBtn"],function(r,s){var o=t[s+"Class"],u,a=e[s];o&&(n=o.disableClass||n,i=o.hoverClass||i),u=s==="prevBtn"?e.currentIdx<=0:e.currentIdx+t.display>=e.count,u&&!t.loop||e._isDisabled()?a.removeClass(i).addClass(n):a.hasAllClasses(n)&&a.removeClass(n)})},n.prototype._applyBtnStyle=function(e){var t=this,n=0,i,s=t.options.wijCSS,o={collision:"none",of:t.container,my:e?"right{offset} center":"center bottom{offset}",at:e?"right center":"center bottom"},u={collision:"none",of:t.container,my:e?"left{offset} center":"center top{offset}",at:e?"left center":"center top"},a=t.options.buttonPosition==="inside",f,l,c=["cornerBottom","cornerTop","cornerRight","cornerLeft"],h=(e?1:0)*2+(a?1:0),p=(e?1:0)*2+(a?0:1),d=function(e,t,n,i,s){var o="";e.removeClass(t).addClass(n),!isNaN(i)&&i!==0&&(i>0?o="+"+i:o="-"+ -i),s.my=s.my.replace(/\{offset\}/,o),e.position(s);if(r.browser.msie){var u=e.next("iframe.bgiframe");u.length&&(u.css("left",e.css("left")),u.css("top",e.css("top")))}};n=e?t.prevBtn.width():t.prevBtn.height(),i=a?0:n,f=s[c[h]],l=s[c[p]],d(t.nextBtn,l,f,i,o),d(t.prevBtn,f,l,-i,u),t._applyBtnClass()},n.prototype._createDom=function(e){var t=this,n=t.options,i=t.element,s,a,f,l;if(i.is("div"))t.list=i.children("ul:eq(0)"),t.container=t.element,t.list.length||(t.list=r("<ul></ul>").appendTo(t.container));else{if(!i.is("ul"))return;t.list=i,t.container=i.parent()}t.itemBound=t._getItemBound(),e?(s=n.display>1&&!n.preview?c:h,a=n.wijCSS.iconArrowRight,f=n.wijCSS.iconArrowLeft):(s=n.display>1&&!n.preview?d:v,a=n.wijCSS.iconArrowDown,f=n.wijCSS.iconArrowUp),t.list.addClass(u).addClass(n.wijCSS.helperClearFix),l=o+n.wijCSS.widget+" "+s+" "+(n.preview?y+" ":"")+(n.display>1?n.wijCSS.content+" "+n.wijCSS.cornerAll:""),t.container.addClass(l),t.container.attr("dir","ltr"),t._createItems(e),t._createClip(e),t._applyContainerStyle(e),t._createButtons(f,a),t._applyBtnStyle(e),n.preview&&t._createPreview(),n.showTimer&&t._createTimer(),n.showPager&&t._createPager(),n.showControls&&t._createControls()},n.prototype._createPreview=function(){var e=this,t=e.options,n,r,i=t.orientation==="horizontal",s=i?"left":"top",o=i?"right":"bottom",u,a;r=e.itemBound[i?"w":"h"],n=e.offset=Math.round(r/4),u=t.display*r+n*2,e.clip[i?"width":"height"](u),e.list.css(s,function(e,t){return parseFloat(t)+n}),e.clip.css(s,function(e,t){return-n}),e.container.css("margin-"+s,n+"px").css("margin-"+o,n+"px"),e.list.find(T).hide();for(a=0;a<t.display;a++)e._getItemByIndex(e.currentIdx+a).find(T).show()},n.prototype._createItemsFromData=function(e){var t=this;if(!r.isArray(e)||!e.length||!r.isPlainObject(e[0])||r.isEmptyObject(e[0]))return;t.list.empty(),r.each(e,function(e,n){var i;if(!r.isPlainObject(n))return!0;i=t._generateMarkup(n),t.list.append(i)})},n.prototype._generateMarkup=function(e){var t,n;if(!r.isPlainObject(e))return;return t=r("<li></li>"),typeof e.linkUrl=="string"&&e.linkUrl&&(n=r("<a>").attr("href",e.linkUrl),n.appendTo(t)),typeof e.imageUrl=="string"&&e.imageUrl&&r("<img>").attr("src",e.imageUrl).appendTo(n||t),typeof e.caption=="string"&&e.caption&&r("<span>").html(e.caption).appendTo(t),t},n.prototype._createItems=function(e){var t=this,n,i,s=t.options,o,u;if(t.list){t._createItemsFromData(s.data),t.list.children("li").each(function(e,n){var i=r(n);t._createItem(i,e)}),o=t.list.children("li").eq(t.currentIdx).addClass(l);if(s.preview&&s.display>1){n=u=o;for(i=0;i<s.display-1;i++)u=u.next(),n=n.add(u);n.find("div.wijmo-wijcarousel-mask").css({opacity:0})}}},n.prototype._createItem=function(e,t){var n=this,i=n.options,s,o;return s=e.addClass(a).addClass(i.wijCSS.helperClearFix).find("img:eq(0)").attr("role","img").addClass("wijmo-wijcarousel-image"),n._applyItemBound(e),o=e.children("span:eq(0)").hide(),n._createCaption(e,s,o),i.preview?e.children("div.wijmo-wijcarousel-mask").length===0&&r('<div class="wijmo-wijcarousel-mask">').appendTo(e):e.children("div.wijmo-wijcarousel-mask").remove(),e.data("itemIndex",t),e},n.prototype._applyItemBound=function(e){var t=this,n=t.itemBound.w,r=t.itemBound.h;if(!t.itemWidth||!t.itemHeight)e.width(n).height(r),t.itemWidth=n-(e.outerWidth(!0)-n),t.itemHeight=r-(e.outerHeight(!0)-r);e.width(t.itemWidth).height(t.itemHeight)},n.prototype._createCaption=function(e,t,n){var i=this,s=i.options,o,u,a,f,l;s.showCaption&&(o=n.html()||t.attr("title"),o&&o.length&&(a=r("<span></span>").html(o),f=r(E).addClass([s.wijCSS.helperClearFix,"wijmo-wijcarousel-caption"].join(" ")).appendTo(e),u=r(E).addClass([s.wijCSS.helperClearFix,s.wijCSS.content,"wijmo-wijcarousel-text"].join(" ")).append(a).appendTo(e),i._applyCaptionStyle(f,u)))},n.prototype._applyCaptionStyle=function(e,t){var n=e.add(t),i;n.width(this.itemWidth),r.browser.webkit?(i=t.children("span").css("display","inline-block").height(),t.children("span").css("display","")):i=t.children("span").height(),n.height(i)},n.prototype._showCaption=function(e,t){var n=this,r=n._getItemByIndex(e),i=r.find(".wijmo-wijcarousel-text"),s=r.find(".wijmo-wijcarousel-caption");i.length&&i.fadeIn(300,function(){e<t&&n._showCaption(e+1,t)}),s.length&&(s.show(),s.animate({opacity:.5},300))},n.prototype._hideCaption=function(){this.element.find(T).hide()},n.prototype._createClip=function(e){this.clip=this.list.wrap("<div></div>").parent().addClass(f)},n.prototype._createBtn=function(e,t){var n=r(b),i=this.options;return n.addClass(i.wijCSS.stateDefault+" "+e).attr("role","button").mouseover(function(){r(this).hasAllClasses(i.wijCSS.stateDisabled)||r(this).addClass(i.wijCSS.stateHover)}).mouseout(function(){r(this).hasAllClasses(i.wijCSS.stateDisabled)||r(this).removeClass(i.wijCSS.stateHover)}).children("span:eq(0)").addClass(i.wijCSS.icon+" "+t),n},n.prototype._createControls=function(){var e=this,t=e.options,n={collision:"none",of:e.container,my:"center bottom",at:"center bottom"};t.control&&(e.controls=r(t.control).css({position:"absolute"}).appendTo(e.container),r.extend(n,t.controlPosition),e.controls.position(n))},n.prototype._createTimer=function(){var e=this,t=e.options;e._createPausePlay(),e.progressBar=r("<div></div>").addClass("wijmo-wijcarousel-timerbar-inner "+t.wijCSS.cornerAll).css({width:"0%"}).attr("role","progressbar"),e.timer=r("<div></div>").addClass("wijmo-wijcarousel-timerbar "+t.wijCSS.cornerAll).appendTo(e.container).append(e.progressBar).append(e.playPauseBtn)},n.prototype._createPausePlay=function(){var e=this,t=e.options;e.playPauseBtn=r(e._createBtn(p,t.auto?t.wijCSS.iconPause:t.wijCSS.iconPlay)).bind("click."+e.widgetName,function(){if(e._isDisabled())return;var n=r(this).children("span:eq(0)");e[n.hasAllClasses(t.wijCSS.iconPlay)?"play":"pause"]()})},n.prototype._createPagingItem=function(e,t,n,i){var s=r(S).attr({role:"tab","aria-label":i+1,title:i+1}).addClass(this.options.wijCSS.stateDefault),o,u;if(e)s.addClass("wijmo-wijcarousel-dot");else{if(!(t&&t.images&&t.images[i]))return;o=t.imageWidth,u=t.imageHeight,o&&u&&s.width(o).height(u),s.children("a").append(r("<img>").attr("src",t.images[i]))}n.append(s)},n.prototype._createPaging=function(e){var t=this,n=t.options,i,s,o,u=n.thumbnails,a=e==="dots";t.container.append(t.pager=r('<div><ul class="wijmo-list '+n.wijCSS.cornerAll+" "+n.wijCSS.helperClearFix+' role="tablist"></ul></div>')),a||t.pager.addClass("wijmo-wijcarousel-thumbnails"),s=t.pager.children("ul.wijmo-list");for(i=0;i<t.count;i++)t._createPagingItem(a,u,s,i);o=t.pager.find("li").eq(t.currentIdx),o.length&&t._activePagerItem(o),u&&r.each(["mousedown","mouseup","mouseover","mouseout","click"],function(e,t){var n=u[t];n&&typeof n=="string"&&window[n]&&(u[t]=window[n])}),t.pager.bind("mouseover."+t.widgetName,function(e){if(t._isDisabled())return;t._pageingEvents(e,"mouseover",u,a,function(e){e.addClass(n.wijCSS.stateHover)})}).bind("mouseout."+t.widgetName,function(e){if(t._isDisabled())return;t._pageingEvents(e,"mouseout",u,a,function(e){e.removeClass(n.wijCSS.stateHover)})}).bind("click."+t.widgetName,function(e){if(t._isDisabled())return;t._pageingEvents(e,"click",u,a,function(e){t.scrollTo(e.index()),t._activePagerItem(e)})}),a||r.each(["mousedown","mouseup"],function(e,n){r.isFunction(u[n])&&t.pager.bind(n,function(e){if(t._isDisabled())return;var i=r(e.target).closest(x);i.length&&u[n].call(i,e)})})},n.prototype._pageingEvents=function(e,t,n,i,s){var o=r(e.target).closest(x);o.length&&(r.isFunction(s)&&s.call(e,o),!i&&r.isFunction(n[t])&&n[t].call(o,e))},n.prototype._activePagerItem=function(e){var t=this.options.wijCSS.stateActive;e.addClass(t).attr("aria-selected","true").siblings("li").removeClass(t).removeAttr("aria-selected")},n.prototype._createWijSlider=function(){var e=this,t=e.options.sliderOrientation,n={orientation:t,range:!1,min:0,max:e.count-1,step:1,value:e.currentIdx,buttonClick:function(t,n){var r=n.value;e.scrollTo(r)},slide:function(t,n){var r=n.value;e.scrollTo(r)}},i=r("<div></div>").css("margin-bottom","10px").css(t==="horizontal"?"width":"height","200px").appendTo(e.container);e.pager=i.wijslider(n).parent().wrap("<div>").parent().addClass("wijmo-wijcarousel-slider-wrapper")},n.prototype._createWijPager=function(){var e=this,t={pageCount:e.count,pageIndex:e.currentIdx,pageButtonCount:e.count,mode:"numeric",pageIndexChanged:function(t,n){var r=n.newPageIndex;e.scrollTo(r)}},n=r("<div></div>").appendTo(e.container);e.pager=n.wijpager(t).css({position:"absolute"})},n.prototype._createPager=function(){var e=this,t=e.options,n={collision:"none",of:e.container,my:"right top",at:"right bottom"};if(t.pagerType==="numbers")e._createWijPager();else if(t.pagerType==="dots"||t.pagerType==="thumbnails")e._createPaging(t.pagerType),e.pager.css({position:"absolute"});else{if(!r.wijmo.wijslider||t.pagerType!=="slider")return;e._createWijSlider(),t.sliderOrientation!=="horizontal"&&(n.my="left center",n.at="right center")}e.pager.width(e.pager.width()+1),r.extend(n,t.pagerPosition),t.pagerPosition=n,e.pager.addClass("wijmo-wijcarousel-pager").position(n)},n.prototype._setOption=function(e,n){var i=this,s=i.options,o,u,a,f=s.orientation==="horizontal",l;if(e==="pagerPosition"||e==="animation"||e==="controlPosition")r.extend(!0,s[e],n);else{a=s[e],t.prototype._setOption.call(this,e,n);switch(e){case"showControls":case"showPager":case"showTimer":if(n===a)break;o=e.replace(/show/i,"").toLowerCase(),u=e.replace(/show/i,"_create"),n===!0?i[o]?i[o].jquery&&i[o].show():i[u]():i[o].hide(),i._reCreatePager();break;case"loop":case"orientation":case"display":case"preview":n!==a&&(l=i.currentIdx,i._wijdestroy(),i._create(),i.scrollTo(l));break;case"data":i._createItems(f),i._applyContainerStyle(f),i._applyBtnClass(),s.showPager&&i._createPager();break;case"showCaption":n?i._createItems(f):i.element.find(T).remove();break;case"buttonPosition":i._applyBtnStyle(f);break;case"pagerType":n!==a&&i._reCreatePager();break;case"showControlsOnHover":i._set_showControlsOnHover(n);break;case"thumbnails":i._reCreatePager();break;case"auto":i[n?"play":"pause"]();break;default:}}},n.prototype._reCreatePager=function(){var e=this,t=e.options,n=e._isDisabled();e.pager&&e.pager.jquery&&(e.pager.remove(),e.pager=null),t.showPager&&(e._createPager(),n&&e._handlePagerState(n))},n.prototype._getItemBound=function(){var e={},t=this,n=t.options;return n.orientation==="horizontal"?e={w:Math.round(t.width/n.display),h:t.height}:e={w:t.width,h:Math.round(t.height/n.display)},e},n.prototype._stopAnimation=function(){var e=this;e.isPlaying&&e.progressBar&&e.progressBar.stop().css({width:"0%"}),e.list.stop(!0,!0)},n.prototype._itemClick=function(e){var t=r(e.target),n=this,i=n.options,s=n.currentIdx,o=t.closest("li."+a),u=o.data("itemIndex"),f,l;if(n._isDisabled()){e.preventDefault();return}if(o.length>0){if(i.preview&&u<s||u>s+i.display-1)f=n._getItemByIndex(s).index(),l=o.index()>f?"next":"previous",n[l]();n._trigger("itemClick",e,{index:u,el:o})}},n.prototype._wijdestroy=function(){var e=this,t=e.options.wijCSS;e.container.removeClass(["wijmo-wijcarousel",t.widget,"wijmo-wijcarousel-horizontal","wijmo-wijcarousel-vertical"].join(" ")),e.timeout&&(e.list.stop(!0),window.clearTimeout(e.timeout),e.timeout=null),e.options.showTimer&&e.progressBar&&e.progressBar.stop(!0),e.list.unwrap().removeClass("wijmo-wijcarousel-list").removeClass(t.helperClearFix).unbind("."+e.widgetName).removeAttr("style").children("li").each(function(e,n){var i=r(n);i.removeClass(["wijmo-wijcarousel-item",t.helperClearFix,l].join(" ")),i.find("img").removeClass("wijmo-wijcarousel-image"),i.children(T).remove(),i.css({width:"",height:""}),i.removeData("itemIndex")}),e.itemWidth=e.itemHeight=undefined,e.element.find(w+",.wijmo-wijcarousel-timerbar").remove(),e.pager&&(e.pager.remove(),e.pager=null),e.timer&&(e.timer.remove(),e.timer=null),e.element.find("li>span").css("display","")},n.prototype.destroy=function(){this._wijdestroy(),r.Widget.prototype.destroy.apply(this)},n.prototype._resetDom=function(){var e=this;e._applyListStyle(),e.list.children("li").each(function(t,n){var i=r(n),s=i.children("div.wijmo-wijcarousel-caption"),o=i.children("div.wijmo-wijcarousel-text");e._applyItemBound(i),e._setStyle(s.add(o),function(){e._applyCaptionStyle(s,o)})}),e._applyContainerStyle(e.isHorizontal,!0),e.options.preview&&e._createPreview(),e._setStyle(e.prevBtn.add(e.nextBtn),function(){e._applyBtnStyle(e.isHorizontal)}),e._setStyle(e.pager,function(){e.pager.position(e.options.pagerPosition)}),e._setStyle(e.controls,function(){e.controls.position(e.options.controlPosition)})},n.prototype._setStyle=function(e,t){var n=!0;if(!e||e.length===0)return;e.css("display")=="none"&&(n=!1,e.css("display","")),t.call(this),n||e.css("display","none")},n.prototype.refresh=function(){var e=this,t=e.options,n=e.element;e.width=n.width()||640,e.height=n.height()||480,e.itemWidth=e.itemHeight=0,this.currentIdx=t.start,e._resetDom(),e._setCurrentPagerIndex(),e.pause()},n.prototype.play=function(){var e=this,t=e.options;if(e.isPlaying||e._isDisabled())return;if(t.interval===0)return e.pause();if(t.showTimer&&e.progressBar)e.progressBar.css({width:"0%"}),e.playPauseBtn.children("span:eq(0)").removeClass(t.wijCSS.iconPlay).addClass(t.wijCSS.iconPause),e.progressBar.animate({width:"100%"},t.interval,null,function(){e._scroll("next",t.step)});else{if(e.timeout)return;e.timeout=window.setTimeout(function(){e.next()},t.interval)}e.isPlaying=!0},n.prototype.pause=function(){var e=this,t=e.options;if(t.showTimer&&e.progressBar)e.progressBar.stop(!0).css({width:"0%"}),e.playPauseBtn.children("span:eq(0)").removeClass(t.wijCSS.iconPause).addClass(t.wijCSS.iconPlay);else{if(e.timeout===null)return;window.clearTimeout(e.timeout),e.timeout=null}e.isPlaying=!1},n.prototype.next=function(){var e=this,t=e.options.step;if(typeof t!="number"||t<1)return;e._stopAnimation(),e._scroll("next",t)},n.prototype.previous=function(){var e=this,t=e.options.step;if(typeof t!="number"||t<1)return;e._stopAnimation(),e._scroll("previous",t)},n.prototype.scrollTo=function(e){var t=this,n,r;t._stopAnimation(),n=e>t.currentIdx?"next":"previous",r=Math.abs(e-t.currentIdx),e!==t.currentIdx&&t._scroll(n,r)},n.prototype._scroll=function(e,t){var n=this,i=n.options,s=n.list,o=0,u,a,f=i.orientation==="horizontal",c={},h=n.itemBound[f?"w":"h"],p=f?"left":"top",d=t,v={},m=0,g=n.currentIdx*h;if(!i.loop){if(e==="next")if(n.currentIdx+i.display+t<=n.count)o=-g-h*d;else{if(!(n.currentIdx+i.display<n.count))return;d=n.count-n.currentIdx-i.display,o=-g-h*d}else n.currentIdx-t>=0?o=-g+h*d:n.currentIdx>0&&(d=n.currentIdx,o=0);o!==undefined&&(c[p]=o+n.offset,n._doAnimation(e,c,d))}else{i.preview&&(o=m=-h*N+n.offset);if(e==="next")o+=-h*d,c[p]=o;else{for(a=0;a<t;a++)o-=h,s.children("li:last").prependTo(s);v[p]=o,c[p]=m,s.css(v)}n._doAnimation(e,c,t,p,h)}if(i.preview){u=r();for(a=0;a<i.display;a++)u=u.add(n._getItemByIndex(n.currentIdx+a).find("div.wijmo-wijcarousel-mask:eq(0)"));u.stop(!0).animate({opacity:.6},i.animation.duration/2,function(){n._getItemByIndex(n.currentIdx).removeClass(l)})}},n.prototype._doAnimation=function(e,t,n,i,s){var o=this,u=o.list,a=o.options,f,l,c={complete:null},h,p={},d;if(!o.list.children().length)return;e==="next"?d=o.currentIdx+n:d=o.currentIdx-n,a.loop?(d=d%o.count+(d<0?o.count:0),h=function(){var t=0;a.preview&&(t=o.offset-N*s);if(e==="next"){for(l=0;l<n;l++)u.children("li:first").appendTo(u);i&&(p[i]=t,u.css(p))}o.currentIdx=d,o._setCurrentState(o.currentIdx)}):h=function(){o.currentIdx=d,o._setCurrentState(o.currentIdx)},c.complete=h,f={index:o.currentIdx,to:d,el:o._getItemByIndex(o.currentIdx)};if(!o._trigger("beforeScroll",null,f))return;a.animation&&a.animation.complete&&(a.animation.complete=undefined),r.extend(c,a.animation),o._hideCaption(),u.animate(t,c)},n.prototype._setCurrentState=function(e){var t=this,n=t.options,i,s,o,u=e+n.display-1,a,f,c;n.loop?t.isPlaying&&(window.clearTimeout(t.timeout),t.timeout=null,t.isPlaying=!1,t.play()):(o=e+n.display<t.count,t.isPlaying&&o?(window.clearTimeout(t.timeout),t.timeout=null,t.isPlaying=!1,t.play()):t.progressBar&&t.pause(),t._applyBtnClass());if(n.preview){f=r(),a=t._getItemByIndex(e);for(c=0;c<n.display;c++)f=f.add(t._getItemByIndex(e+c).find("div.wijmo-wijcarousel-mask:eq(0)"));f.animate({opacity:0},n.animation.duration/2,function(){a.addClass(l),t._showCaption(e,u)})}else t._showCaption(e,u);t._setCurrentPagerIndex(),s={firstIndex:e,lastIndex:u},t._trigger("afterScroll",null,s)},n.prototype._setCurrentPagerIndex=function(){var e=this,t=e.currentIdx,n;e.pager&&(r.wijmo.wijslider&&e.pager.find(":wijmo-wijslider").length?e.pager.find(":wijmo-wijslider:eq(0)").wijslider("value",t):r.wijmo.wijpager&&e.pager.is(":wijmo-wijpager")?e.pager.wijpager("option","pageIndex",t):e.pager.jquery&&(n=e.pager.find(">ul:eq(0)>li").eq(t),n.length&&e._activePagerItem(n)))},n.prototype._getItemByIndex=function(e){var t=this,n=t.list,i,s=e%t.count;return t.options.loop?(n.children("li").each(function(e){var t=r(this);if(t.data("itemIndex")===s)return i=t,!1}),i):n.children("li").eq(s)},n.prototype._collectionChanged=function(e){var t=this;t.count=t.count+(e==="add"?1:-1),t._applyListStyle(),t.pager&&t.pager.jquery&&(t.pager.remove(),t.pager=null,t._createPager()),t._applyBtnClass()},n.prototype.add=function(e,t){var n=this,i,s;if(typeof e=="string")i=r(e);else if(e.jquery)i=e;else{if(!r.isPlainObject(e))return;i=n._generateMarkup(e)}i.is("li")||(i=i.wrap("<li></li>").parent()),s=i,typeof t=="number"&&t>=0&&t<n.count?(s.insertBefore(n._getItemByIndex(t)),n.list.children("li").each(function(e,n){var i=r(n);i.data("itemIndex")>=t&&i.data("itemIndex",i.data("itemIndex")+1)})):(t==0&&s.appendTo(n.list),s.insertAfter(n._getItemByIndex(n.count-1))),n._createItem(i,t!==undefined?t:n.count),n._collectionChanged("add")},n.prototype.remove=function(e){var t=this,n;typeof e=="number"&&e>=0&&e<t.count?(n=t._getItemByIndex(e),t.list.children("li").each(function(t,n){var i=r(n);i.data("itemIndex")>e&&i.data("itemIndex",i.data("itemIndex")-1)})):n=t._getItemByIndex(t.count-1),n&&n.remove(),t._collectionChanged("remove")},n}(t.wijmoWidget);n.wijcarousel=C;var k=function(){function e(){this.wijMobileCSS={header:"ui-header ui-bar-a",content:"ui-body-a",stateDefault:"ui-btn ui-btn-a",stateHover:"ui-btn-down-a",stateActive:"ui-btn-down-b",iconPlay:"ui-icon-arrow-r",iconPause:"ui-icon-grid"},this.initSelector=":jqmData(role='wijcarousel')",this.data=[],this.auto=!1,this.interval=5e3,this.showTimer=!1,this.buttonPosition="inside",this.showPager=!1,this.prevBtnClass={defaultClass:"",hoverClass:"",disableClass:""},this.nextBtnClass={defaultClass:"",hoverClass:"",disableClass:""},this.pagerType="numbers",this.thumbnails={mouseover:null,mouseout:null,mousedown:null,mouseup:null,click:null,imageWidth:58,imageHeight:74,images:[]},this.pagerPosition={},this.orientation="horizontal",this.sliderOrientation="horizontal",this.loop=!0,this.animation={queue:!0,disable:!1,duration:1e3,easing:"linear"},this.start=0,this.display=1,this.preview=!1,this.step=1,this.showControls=!1,this.control="",this.controlPosition={},this.showCaption=!0,this.showControlsOnHover=!1,this.itemClick=null,this.beforeScroll=null,this.afterScroll=null,this.loadCallback=null}return e}();C.prototype.options=r.extend(!0,{},t.wijmoWidget.prototype.options,new k),r.wijmo.registerWidget("wijcarousel",C.prototype)})(t.carousel||(t.carousel={}));var n=t.carousel})(wijmo||(wijmo={}))});