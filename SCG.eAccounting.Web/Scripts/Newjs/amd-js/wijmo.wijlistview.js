var wijmo;define(["./wijmo.widget"],function(){var e=this.__extends||function(e,t){function r(){this.constructor=e}for(var n in t)t.hasOwnProperty(n)&&(e[n]=t[n]);r.prototype=t.prototype,e.prototype=new r};(function(t){(function(n){var r=jQuery,i="wijlistview",s=function(t){function n(){t.apply(this,arguments)}return e(n,t),n.prototype._baseWidget=function(){return r.mobile.listview},n.prototype._getListViewFromData=function(){return this.element.data("wijmo-wijlistview")},n.prototype._create=function(){var e=this._getListViewFromData();r.mobile.version.slice(0,4)=="1.2."?this.element.data("listview",e):this.element.data("mobile-listview",e),this.element.addClass("wijmo-wijlistview"),t.prototype._create.call(this);var n=this.element.closest("div[data-role='content']");n&&n.length>0||this.element.wrap("<div data-role='content' class='ui-content' />"),this._resetFilterable()},n.prototype._resetFilterable=function(){var e=this.element.data("mobile-filterable");e&&e._setWidget&&e._setWidget(this._getListViewFromData())},n.prototype._createSubPages=function(){var e=this.element.find("li").filter(function(){return r(this).find("ul, ol").length>0});r.mobile.listview.prototype._createSubPages.apply(this,arguments),e.length>0&&e.each(function(){var e=r(this),t=e.find("a");e.addClass("WijListviewNestedLink"),t.addClass("WijListviewNestedLink")})},n}(t.wijmoWidget);n.wijlistview=s;if(r.mobile){s.prototype.widgetName="wijlistview",s.prototype.widgetEventPrefix="listview",s.prototype.options=r.extend({},r.mobile.listview.prototype.options,t.wijmoWidget.prototype.options,{initSelector:":jqmData(role='wijlistview')"}),r.wijmo.registerWidget(i,r.mobile.listview,s.prototype);if(r.mobile.version.slice(0,4)=="1.2."){s.prototype.options.filter=!1,s.prototype.options.filterPlaceholder="Filter items...",s.prototype.options.filterTheme="c",s.prototype.options.filterReveal=!1;var o=function(e,t,n){return e.toString().toLowerCase().indexOf(t)===-1};s.prototype.options.filterCallback=o,r(document).delegate("ul.wijmo-wijlistview,ol.wijmo-wijlistview","listviewcreate",function(){var e=r(this),t=e.data("listview");if(!t.options.filter)return;t.options.filterReveal&&e.children().addClass("ui-screen-hidden");var n=r("<form>",{"class":"ui-listview-filter ui-bar-"+t.options.filterTheme,role:"search"}),i=r("<input>",{placeholder:t.options.filterPlaceholder}).attr("data-"+r.mobile.ns+"type","search").jqmData("lastval","").bind("keyup change",function(){var n=r(this),i=this.value.toLowerCase(),s=null,u=n.jqmData("lastval")+"",a=!1,f="",l,c=t.options.filterCallback!==o;t._trigger("beforefilter","beforefilter",{input:this}),n.jqmData("lastval",i),c||i.length<u.length||i.indexOf(u)!==0?s=e.children():(s=e.children(":not(.ui-screen-hidden)"),!s.length&&t.options.filterReveal&&(s=e.children(".ui-screen-hidden")));if(i){for(var h=s.length-1;h>=0;h--)l=r(s[h]),f=l.jqmData("filtertext")||l.text(),l.is("li:jqmData(role=list-divider)")?(l.toggleClass("ui-filter-hidequeue",!a),a=!1):t.options.filterCallback(f,i,l)?l.toggleClass("ui-filter-hidequeue",!0):a=!0;s.filter(":not(.ui-filter-hidequeue)").toggleClass("ui-screen-hidden",!1),s.filter(".ui-filter-hidequeue").toggleClass("ui-screen-hidden",!0).toggleClass("ui-filter-hidequeue",!1)}else s.toggleClass("ui-screen-hidden",!!t.options.filterReveal);t._refreshCorners()}).appendTo(n).textinput();t.options.inset&&n.addClass("ui-listview-filter-inset"),n.bind("submit",function(){return!1}).insertBefore(e)})}}})(t.listview||(t.listview={}));var n=t.listview})(wijmo||(wijmo={}))});