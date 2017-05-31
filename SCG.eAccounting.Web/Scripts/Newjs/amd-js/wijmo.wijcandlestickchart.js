var wijmo;define(["./wijmo.wijchartcore"],function(){var e=this.__extends||function(e,t){function r(){this.constructor=e}for(var n in t)t.hasOwnProperty(n)&&(e[n]=t[n]);r.prototype=t.prototype,e.prototype=new r};(function(t){(function(n){var r="wijcandlestickchart",i="ohlc",s="candlestick",o="hl",u="stroke-width",a=function(){function e(e){this.init(e)}return e.prototype.init=function(e){var t=[],n={},r,i;$.each(e,function(e,n){var r=$.toOADate(n);$.inArray(r,t)===-1&&t.push(r)}),this.count=r=t.length,this.times=t=t.sort(),this.timeDic=n;if(r<2){this.max=t[0]||0,this.min=t[0]||0,this.unit=864e5,r===1&&(n[t[0]]=0);return}for(i=0;i<r;i++)n[t[i]]=i;this._calculateUnit(),this.max=t[this.count-1],this.min=t[0]},e.prototype._calculateUnit=function(){var e=Number.MAX_VALUE,t=1;for(;t<this.count;t++)e=Math.min(e,this.times[t]-this.times[t-1]);this.unit=e},e.prototype.getOATime=function(e){var t=this.count,n=this.times,r;return e<0?this.min+e*this.unit:e>t-1?this.max+(e-t+1)*this.unit:(r=parseInt(e),n[r]+(e-r)*this.unit)},e.prototype.getTime=function(e){return $.fromOADate(this.getOATime(e))},e.prototype.getTimeIndex=function(e){var t=$.toOADate(e),n;return t>this.max?this.count-1+(t-this.max)/this.unit:t<this.min?(t-this.min)/this.unit:this.timeDic&&this.timeDic[t]!=null?this.timeDic[t]:($.each(this.times,function(e,r){if(r>t)return n=e-1,!1}),n+(t-this.times[n])/this.unit)},e.prototype.getCount=function(){return this.times.length},e.prototype.dispose=function(){this.times=null,this.timeDic=null},e}();n.datetimeUtil=a;var f=function(t){function r(){t.apply(this,arguments)}return e(r,t),r.prototype._paintChartArea=function(){var e=this.options;if(this._isSeriesListDataEmpty())return;$.each(e.seriesList,function(e,t){var n=t.data;t.isTrendline||(n.y=[].concat(n.high).concat(n.low))}),t.prototype._paintChartArea.call(this),$.each(e.seriesList,function(e,t){t.isTrendline||delete t.data.y,t.data.originalXValue&&(t.data.x=t.data.originalXValue,delete t.data.originalXValue)})},r.prototype._getXAxisInfoValueLabels=function(e,t){var r=this,i;!t.annoFormatString||t.annoFormatString.length===0?i=n.ChartDataUtil.getTimeDefaultFormat(this.timeUtil.max,this.timeUtil.min):i=t.annoFormatString,r.valueLabelsFromXData=!1,r.xAxisLabelInfoList=[],t.annoMethod!=="valueLabels"||!t.valueLabels||t.valueLabels.length===0?(this.valueLabelsFromXData=!0,this.timeUtil&&$.each(this.timeUtil.times,function(e,t){var n={value:e,text:Globalize.format(r._getXTickText(e),i,r._getCulture())};r.xAxisLabelInfoList.push(n)})):$.each(t.valueLabels,function(e,t){var n,i;typeof t=="string"?(i=t,n=e):typeof t=="object"&&(i=t.text,n=t.value,n!==undefined&&n!==null&&r._isDate(n)?n=r.timeUtil.getTimeIndex(n):n=e),r.xAxisLabelInfoList.push({value:n,text:i})}),e.annoMethod="valueLabels",r.xAxisLabelInfoList&&r.xAxisLabelInfoList.length&&(e.valueLabels=r.xAxisLabelInfoList)},r.prototype._checkSeriesDataEmpty=function(e){var t=this.options.type,n=e.data,r=this._checkEmptyData;if(!n||r(n.x))return!0;if(e.isTrendline)return!e.isValid||!e.innerData?!0:r(n.y);if(r(n.high)||r(n.low))return!0;if(t===i||t===s)if(r(n.open)||r(n.close))return!0;return!1},r.prototype._getLegendStyle=function(e){var t=e.highLow;return t?(t=$.extend({},t),delete t.width,delete t.height,t.stroke||(t.stroke=t.fill),t):e},r.prototype._showHideSeries=function(e,t){var n=["highLow","open","close","path","high","low","openClose"];$.each(e,function(e,r){$.each(n,function(e,n){r[n]&&r[n][t]()}),r.path&&$(r.path.node).data("wijchartDataObj")&&($(r.path.node).data("wijchartDataObj").visible=t==="show")})},r.prototype._showSerieEles=function(e){if(e.isTrendline){n.TrendlineRender.showSerieEles(e);return}this._showHideSeries(e,"show")},r.prototype._hideSerieEles=function(e){if(e.isTrendline){n.TrendlineRender.hideSerieEles(e);return}this._showHideSeries(e,"hide")},r.prototype._paintPlotArea=function(){var e=this.options,t=new l(this.chartElement,{canvas:this.canvas,bounds:this.canvasBounds,tooltip:this.tooltip,widgetName:this.widgetName,axis:e.axis,seriesList:e.seriesList,seriesStyles:e.seriesStyles,seriesHoverStyles:e.seriesHoverStyles,seriesTransition:e.seriesTransition,showChartLabels:e.showChartLabels,textStyle:e.textStyle,chartLabelStyle:e.chartLabelStyle,chartLabelFormatString:e.chartLabelFormatString,shadow:e.shadow,disabled:this._isDisabled(),animation:e.animation,culture:this._getCulture(),type:e.type,wijCSS:e.wijCSS,mouseDown:$.proxy(this._mouseDown,this),mouseUp:$.proxy(this._mouseUp,this),mouseOver:$.proxy(this._mouseOver,this),mouseOut:$.proxy(this._mouseOut,this),mouseMove:$.proxy(this._mouseMove,this),click:$.proxy(this._click,this),timeUtil:this.timeUtil,candlestickFormatter:e.candlestickFormatter,widget:this});t.render()},r.prototype._paintTooltip=function(){var e=this.chartElement.data("fields"),n;t.prototype._paintTooltip.call(this),this.tooltip&&e&&e.trackers&&(n=e.trackers,this.tooltip.setTargets(n),this.tooltip.setOptions({relatedElement:n[0]}))},r.prototype._getTooltipText=function(e,t){var n=$(t.node).data("wijchartDataObj"),r={data:n,label:n.label,x:n.x,high:n.high,low:n.low,open:n.open,close:n.close,target:t,fmt:e};return $.proxy(e,r)()},r.prototype._setDefaultTooltipText=function(e){var t=this.options.type,n;return n=e.label+" - "+Globalize.format(e.x,"d")+"\n High:"+e.high+"\n Low:"+e.low,t!==o&&(n+="\n Open:"+e.open+"\n Close:"+e.close+""),n},r.prototype._getStyles=function(e,t,r,s){function a(e,t,n,r,s){var a={},f=[];if(n===null||n===undefined)return;return e===o?f=["highLow"]:e===i?f=["open","close","highLow"]:f=["fallingClose","risingClose","unchangeClose","highLow"],$.each(f,function(e,i){s&&(t[i]=$.extend(!0,{},t,t[i])),a[i]=$.extend(!0,{},n,t[i]),r&&i=="risingClose"&&(!t[i]||!t[i].fill)&&(a[i].fill=null),i==="fallingClose"&&(!t[i]||!t[i][u])&&(a[i][u]=0);if(i==="open"||i==="close"||i==="highLow")if(!t[i]||!t[i].stroke)a[i].stroke=a[i].fill}),a}var f=r==="seriesStyles",l=n.wijchartcore.prototype._getDefFill(),c=t.length,h=0,p,d=[],v=0;h=Math.max(n.wijchartcore.prototype.options[r].length,f?l.length:0,c),p=$.extend(!0,new Array(h),n.wijchartcore.prototype.options[r]);for(v=0;v<h;v++)v>=c&&(t[v]={}),f&&v<l.length&&(p[v]||(p[v]={}),p[v].fill=l[v]),d.push(a(e,t[v],p[v],f,s));return d},r.prototype._handleChartStyles=function(){var e=this.options;e.seriesStyles=this._getStyles(e.type,e.seriesStyles,"seriesStyles",!1),e.seriesHoverStyles=this._getStyles(e.type,e.seriesHoverStyles,"seriesHoverStyles",!1)},r.prototype._create=function(){this._handleChartStyles(),this.bindXData=!1,t.prototype._create.call(this),this.chartElement.addClass(this.options.wijCSS.wijCandlestickChart)},r.prototype._setOption=function(e,n){e==="axis"?this._handleMaxMinInAxis(n):e==="type"&&(this.options.type=n,this._handleChartStyles(),this.styles={style:this.options.seriesStyles.slice(),hoverStyles:this.options.seriesHoverStyles.slice()}),t.prototype._setOption.call(this,e,n)},r.prototype._seriesListSeted=function(){},r.prototype._handleMaxMinInAxis=function(e){var t=this,n,r=this.options.axis,i=function(e){return t._isDate(e)?t.timeUtil.getTimeIndex(e):t.timeUtil.getTimeIndex($.fromOADate(e))};e&&e.x&&(n=e.x,$.each(["max","min"],function(e,t){var s=n[t];if(s===undefined||s===null)return!0;if(!r||!r.x||r.x[t]!==s)n[t]=i(s)}))},r.prototype._handleXData=function(){var e=this,t=this.options,n=t.seriesList=$.arrayClone(t.seriesList),r=[],i=!1,s=function(e){return e.data&&e.data.x},o=function(t){var n=t.data.x;$.each(n,function(t,r){e._isDate(r)||(n[t]=e.timeUtil.getTime(r))})};if(this.timeUtil&&!this.bindXData)return;this.bindXData=!1,$.each(n,function(e,t){s(t)&&$.isArray(t.data.x)&&(r=r.concat(t.data.x))}),this.timeUtil&&($.each(r,function(t,n){if(e._isDate(n))return i=!0,!1}),i&&(r=[],$.each(n,function(e,t){s(t)&&(o(t),r=r.concat(t.data.x))}),this.timeUtil.dispose(),this.timeUtil=null)),this.timeUtil||(this.timeUtil=new a(r)),$.each(n,function(t,n){s(n)&&$.isArray(n.data.x)&&(n.data.originalXValue=n.data.x,n.data.x=$.map(n.data.x,function(t,n){return e.timeUtil.getTimeIndex(t)}))})},r.prototype._bindData=function(){t.prototype._bindData.call(this)},r.prototype._preHandleSeriesData=function(){this.timeUtil&&(this.timeUtil.dispose(),this.timeUtil=null),this._handleXData(),t.prototype._preHandleSeriesData.call(this)},r.prototype._bindSeriesData=function(e,n,r){var i=this,s=n.data;t.prototype._bindSeriesData.call(this,e,n,r),s.x&&$.isArray(s.x)&&(this.bindXData=!0),$.each(["high","low","open","close"],function(t,n){var r=s[n];r&&r.bind&&(s[n]=i._getBindData(e,r.bind))})},r.prototype.destroy=function(){var e=this.chartElement;e.removeClass(this.options.wijCSS.wijCandlestickChart),t.prototype.destroy.call(this),this.timeUtil&&this.timeUtil.dispose(),e.data("fields",null)},r.prototype.getCandlestick=function(e){var t=this.chartElement.data("fields");return t&&t.chartElements&&t.chartElements.candlestickEles?t.chartElements.candlestickEles[e]||null:null},r.prototype._clearChartElement=function(){var e=this.options,n=this.chartElement.data("fields"),r,i;n&&n.chartElements&&(r=n.chartElements,i=r.candlestickEles,$.each(i,function(e,t){$.each(t,function(e,n){n&&n[0]&&n.wijRemove(),t[e]=null}),i[e]=null}),r.candlestickEles=null,r.group&&r.group.wijRemove(),r.group=null,n.trackers&&n.trackers.wijRemove(),n.trackers=null),t.prototype._clearChartElement.call(this)},r.prototype._calculateParameters=function(e,n){t.prototype._calculateParameters.call(this,e,n);if(e.id==="x"){var r=n.unitMinor,i=n.autoMin,s=n.autoMax,o=this._getCandlestickAdjustment(e,r);i&&(e.min-=o),s&&(e.max+=o),this._calculateMajorMinor(n,e)}},r.prototype._calculateMajorMinor=function(e,r){var i=this,s;r.id==="x"&&(!e.annoFormatString||e.annoFormatString.length===0?r.annoFormatString=n.ChartDataUtil.getTimeDefaultFormat(this.timeUtil.getOATime(r.max),this.timeUtil.getOATime(r.min)):r.annoFormatString=e.annoFormatString),t.prototype._calculateMajorMinor.call(this,e,r),s=e.unitMajor?e.unitMajor:1,i._adjustXValueLabelsByMajorUnit(s,r)},r.prototype._adjustXValueLabelsByMajorUnit=function(e,t){t.id==="x"&&this.valueLabelsFromXData&&(t.valueLabels=[],$.each(this.xAxisLabelInfoList,function(n,r){n%e===0&&t.valueLabels.push(r)}))},r.prototype._getCandlestickAdjustment=function(e,t){var n=0,r;return $.each(this.options.seriesList,function(e,t){t.data&&t.data.x&&$.isArray(t.data.x)&&(n=Math.max(t.data.x.length,n))}),r=(e.max-e.min)/n,r===0?r=t:t<r&&t!==0&&(r=Math.floor(r/t)*t),r},r.prototype._getXTickText=function(e){return this.timeUtil.getCount()===0?"":this.timeUtil.getTime(e)},r.prototype._adjustTickValuesForCandlestickChart=function(e){var n=this,r=this.options.axis.x.annoMethod==="valueLabels",i=[];return r&&e&&$.isArray(e)?($.each(e,function(e,t){i[e]=n.timeUtil.getTimeIndex($.fromOADate(t))}),i):t.prototype._adjustTickValuesForCandlestickChart.call(this,e)},r.prototype._getTickTextForCalculateUnit=function(e,n,r){return n.id==="x"?Globalize.format(this._getXTickText(e),n.annoFormatString,this._getCulture()):t.prototype._getTickTextForCalculateUnit.call(this,e,n,r)},r}(n.wijchartcore);n.wijcandlestickchart=f;var l=function(){function e(e,t){this.element=e,this.options=t,this.bounds=t.bounds,this.width=this.bounds.endX-this.bounds.startX,this.height=this.bounds.endY-this.bounds.startY,this.type=t.type,this.timeUtil=t.timeUtil,this.trackers=t.canvas.set(),this.isHover=!1,this.animationSet=t.canvas.set(),this.fieldsAniPathAttr=[],this.paths=[],this.aniPathsAttr=[]}return e.prototype._paintCandlestickElements=function(e,t,n){var r=e.data,i=Math.min(r.x.length,r.high.length,r.low.length),s=this.options.wijCSS,u=$.extend({candlestickType:this.type,visible:!0},e),a=[],f=this.options.widget,l,c,h,p,d,v;u.type="candlestick",r.open&&r.close&&(i=Math.min(i,r.open.length,r.close.length)),l=this.width/i;for(c=0;c<i;c++)h=this._paintCandlestickGraphic(r,c,l,t,n),d=$.extend({},u),h.path&&(p=$(h.path.node),$.wijraphael.addClass(p,s.canvasObject+" "+s.candlestickChart+" "+s.candlestickChartTracker),p.css("opacity",0),this.type!==o&&$.extend(d,{open:r.open[c],close:r.close[c]}),$.extend(d,{high:r.high[c],low:r.low[c],index:c,x:this.timeUtil.getTime(r.x[c]),candlestickEles:h,style:t,hoverStyle:n}),p.data("wijchartDataObj",d)),e.visible===!1&&$.each(h,function(e,t){t&&t.hide&&t.hide()}),a.push(h),f.dataPoints=f.dataPoints||{},f.pointXs=f.pointXs||[],v=h.x,f.dataPoints[v.toString()]||(f.dataPoints[v.toString()]=[],f.pointXs.push(v)),f.dataPoints[v.toString()].push(d);return a},e.prototype._unbindLiveEvents=function(){var e=this.options.widgetName;this.element.off("."+e,".wijcandlestickchart"),n.TrendlineRender.unbindLiveEvents(this.element,e,this.options.wijCSS)},e.prototype._bindLiveEvents=function(){function s(e){var t=$(e.target);return t.data("wijchartDataObj")}function o(e,n,r){var s={mouseover:t.mouseOver,mouseout:t.mouseOut,mousedown:t.mouseDown,mouseup:t.mouseUp,mousemove:t.mouseMove,click:t.click};if($.isFunction(s[e]))return s[e].call(i,n,r)}var e=this,t=this.options,r="",i=this.element;$.support.isTouchEnabled&&$.support.isTouchEnabled()&&(r="wij"),$.each(["mouseover","mouseout","mousedown","mouseup","mousemove","click"],function(n,i){e.element.on(r+i+"."+t.widgetName,".wijcandlestickchart",function(n){if(t.disabled)return;var r=s(n);o(i,n,r)!==!1&&(i==="mouseover"||i==="mouseout")&&e["_"+i](r),r=null})}),n.TrendlineRender.bindLiveEvents(i,t.widgetName,t.mouseDown,t.mouseUp,t.mouseOver,t.mouseOut,t.mouseMove,t.click,t.disabled,t.wijCSS,!1)},e.prototype._setCandlestickStyleForHover=function(e,t,n,r){var i=function(e,t){e&&t&&e.attr(t)};i(e.highLow,t.highLow),i(e.open,t.open),i(e.close,t.close),i(e.high,t.highLow),i(e.low,t.highLow),n!==undefined&&r!==undefined&&(n>r?i(e.openClose,t.fallingClose):n<r?i(e.openClose,t.risingClose):i(e.openClose,t.unchangeClose))},e.prototype._mouseover=function(e){e.hoverStyle&&(this.isHover=!0,this._setCandlestickStyleForHover(e.candlestickEles,e.hoverStyle,e.open,e.close))},e.prototype._mouseout=function(e){var t=e.style;this.isHover&&(this._setCandlestickStyleForHover(e.candlestickEles,t,e.open,e.close),this._formatCandlestick({high:e.high,low:e.low,open:e.open,close:e.close,hlStyle:t.highLow,oStyle:t.open,cStyle:t.close,index:e.index,eles:e.candlestickEles,fallingCloseStyle:t.fallingClose,risingCloseStyle:t.risingClose,unchangeCloseStyle:t.unchangeClose}),this.isHover=!1)},e.prototype._paintCandlestickGraphic=function(e,t,n,r,a){function G(e){return e?{fill:e.fill,stroke:e.stroke}:null}var f=this.options,l=e.x[t],c=e.high[t],h=e.low[t],p=e.open?e.open[t]:0,d=e.close?e.close[t]:0,v=this.bounds.startX,m=this.bounds.endY,g=this.type!==o,y=f.axis,b=y.x,w=y.y,E=b.min,S=w.min,x=b.max,T=w.max,N=0,C=0,k=this.width/(x-E),L=this.height/(T-S),A,O,M,_,D,P,H,B,j,F,I,q,R,U,z,W,X,V,J,K,Q;return M=r.open,O=r.close,A=r.highLow,z=r.fallingClose,W=r.risingClose,V=r.unchangeClose,_=A[u]||0,j=(l-E)*k+this.bounds.startX,F=m-(c-S)*L,I=j,q=m-(h-S)*L,g&&(this.type===i?(N=M[u]||0,C=O[u]||0):(p>d?(N=C=z[u]||0,K=z.width||0,J=G(z),Q=z):p===d?(N=C=V[u]||0,K=V.width||0,J=G(V),Q=V):(N=C=W[u]||0,K=W.width||0,J=G(W),Q=W),A=$.extend({},J,A),n-=(N+C)/2,n<2&&(n=2)),U=n/2*.8,this.type===s&&(U=Math.min(K/2,U)),U<1&&(U=1),D=j-U,P=m-(p-S)*L,H=j+U,B=m-(d-S)*L,P=Math.max(P,F+N/2),P=Math.min(P,q-N/2),B=Math.max(B,F+C/2),B=Math.min(B,q-C/2)),this.type===i?(R=this._paintOHLC({h:{x:j,y:F},l:{x:I,y:q},o:{x:D,y:P},c:{x:H,y:B}}),R.open.attr(M),R.close.attr(O)):this.type===o?R=this._paintHL({h:{x:j,y:F},l:{x:I,y:q}}):(R=this._paintCandlestick({h:{x:j,y:F},l:{x:I,y:q},o:{x:D,y:P},c:{x:H,y:B}}),R.high.attr(A),R.low.attr(A),R.openClose.attr(Q)),R.highLow&&R.highLow.attr(A),this._formatCandlestick({high:c,low:h,open:p,close:d,hlStyle:A,oStyle:M,cStyle:O,index:t,eles:R,fallingCloseStyle:z,risingCloseStyle:W,unchangeCloseStyle:V}),R.path.attr({"stroke-width":Math.max(K||1,C||1,_||1,N||1),fill:"#000"}),this.trackers.push(R.path),R.x=$.round(j),r.highLow=A,R},e.prototype._formatCandlestick=function(e){var t=this.options.candlestickFormatter,n=e.index,r=e.high,o=e.low,u=e.open,a=e.close,f=e.hlStyle,l=e.oStyle,c=e.cStyle,h=e.fallingCloseStyle,p=e.risingCloseStyle,d=e.unchangeCloseStyle,v=this.type,m,g;t&&(m={high:r,low:o},g={highLow:f},v===i?($.extend(m,{open:u,close:a}),$.extend(g,{open:l,close:c})):v===s&&($.extend(m,{open:u,close:a}),$.extend(g,{fallingClose:h,risingClose:p,unchangeClose:d})),t.call(this,{data:m,eles:e.eles,style:g}))},e.prototype._paintTracker=function(e,t){var n=[],r=[],i,s,o,u;return $.each(e,function(e,t){n.push(t.x),r.push(t.y)}),i=Math.max.apply(null,n),s=Math.min.apply(null,n),o=Math.max.apply(null,r),u=Math.min.apply(null,r),t.rect(s,u,i-s,o-u)},e.prototype._paintOHLC=function(e){var t=e.h,n=e.l,r=e.o,i=e.c,s=this.options.canvas,o=["M",t.x,",",t.y,"V",n.y],u=["M",r.x,",",r.y,"H",t.x],a=["M",t.x,",",i.y,"H",i.x],f,l,c,h;return f=s.path(o.join("")),l=s.path(u.join("")),c=s.path(a.join("")),h=this._paintTracker(e,s),this._setTrackerForCandlestickEle([f,l,c],h),this.group.push(l,c,f),{highLow:f,open:l,close:c,path:h}},e.prototype._setTrackerForCandlestickEle=function(e,t){$.each(e,function(e,n){n.node.tracker=t})},e.prototype._paintHL=function(e){var t=e.h,n=e.l,r=this.options.canvas,i,s;return i=r.path(["M",t.x,",",t.y,"V",n.y].join("")),s=i.clone(),this._setTrackerForCandlestickEle([i],s),this.group.push(i),{highLow:i,path:s}},e.prototype._paintCandlestick=function(e){var t=e.h,n=e.l,r=e.o,i=e.c,s=this.options.canvas,o,u,a,f,l,c,h,p,d;return i.y<r.y?(f=i.y,l=r.y):(f=r.y,l=i.y),c=["M",t.x,",",t.y,"V",f],h=["M",n.x,",",n.y,"V",l],p=["M",r.x,",",f,"H",i.x,"V",l,"H",r.x,"V",f,"Z"],u=s.path(c.join("")),a=s.path(h.join("")),o=s.path(p.join("")),d=this._paintTracker(e,s),this.group.push(u,a,o),this._setTrackerForCandlestickEle([u,a,o],d),{high:u,low:a,openClose:o,path:d}},e.prototype._handleStyle=function(e,t){if(!e)return;return t!==!0&&$.each(["width","height"],function(t,n){e[n]&&(e["stroke-width"]=e[n],delete e[n])}),e},e.prototype._extendStyles=function(e){var t=$.extend(!0,{},e);return t&&(t.highLow=this._handleStyle(t.highLow),t.close=this._handleStyle(t.close),t.open=this._handleStyle(t.open),t.fallingClose=this._handleStyle(t.fallingClose,!0),t.risingClose=this._handleStyle(t.risingClose,!0),t.unchangeClose=this._handleStyle(t.unchangeClose,!0)),t},e.prototype._paintCandlesticks=function(){var e=[],t,r=this,i=r.options,s=i.seriesList,o=i.seriesStyles,u=i.seriesHoverStyles,a=r.element.data("fields")||{},f=a.chartElements||{},l=f.candlestickEles||[],c=i.animation,h=i.seriesTransition,p=i.wijCSS,d=[],v=[],m=i.canvas;$.each(s,function(s,a){var f=r._extendStyles(o[s]),g=r._extendStyles(u[s]);if(i.widget._checkSeriesDataEmpty(a))return!0;a.isTrendline?n.TrendlineRender.renderSingleTrendLine(a,f.highLow,g.highLow,i.axis,undefined,r.fieldsAniPathAttr,c,h,s,r.bounds,m,d,v,r.animationSet,r.aniPathsAttr,p,e):(t=r._paintCandlestickElements(a,f,g),l=l.concat(t),e.push(t))}),a.seriesEles=e,a.trackers=r.trackers,f.candlestickEles=l,f.group=r.group,a.trendLines=d,a.chartElements=f,r.fields=a,r.trackers.toFront()},e.prototype._playAnimation=function(){var e=this.options.animation,t=e&&e.enabled,n,r;t&&(n=e.duration||400,r=e.easing,this.group.attr("clip-rect",[this.bounds.startX,this.bounds.startY,0,this.height].join(",")),this.group.wijAnimate({"clip-rect":[this.bounds.startX,this.bounds.startY,this.width,this.height].join(",")},n,r))},e.prototype._playTrendLineAnimation=function(){var e=this,t=e.options,r=t.animation,i=r&&r.enabled,s=t.seriesTransition,o=e.fields.trendLines,u,a;i&&(u=r.duration||400,a=r.easing||"linear",o&&o.length&&n.TrendlineRender.playAnimation(i,u,a,s,t.bounds,o,e.fieldsAniPathAttr,t.axis,t.widget.extremeValue))},e.prototype.render=function(){this.group=this.options.canvas.group(),this._paintCandlesticks(),this.element.data("fields",this.fields),this._unbindLiveEvents(),this._bindLiveEvents(),this._playAnimation(),this._playTrendLineAnimation()},e}();n.CandlestickChartRender=l;var c=function(t){function n(){t.apply(this,arguments),this.wijCandlestickChart="wijmo-wijcandlestickchart",this.candlestickChart="wijcandlestickchart",this.candlestickChartTracker="candlesticktracker"}return e(n,t),n}(n.wijchartcore_css);n.wijcandlestickchart_css=c;var h=function(n){function r(){n.apply(this,arguments),this.type=s,this.animation={enabled:!0,duration:400,easing:">"},this.wijCSS=new t.chart.wijcandlestickchart_css,this.candlestickFormatter=null,this.showChartLabels=!0,this.chartLabelStyle={},this.chartLabelFormatString="",this.hint=$.extend(!0,{},this.hint,{contentStyle:{"font-size":12}}),this.mouseDown=null,this.mouseUp=null,this.mouseOver=null,this.mouseOut=null,this.mouseMove=null,this.click=null}return e(r,n),r}(n.wijchartcore_options);f.prototype.options=$.extend(!0,{},t.wijmoWidget.prototype.options,new h),f.prototype.widgetEventPrefix=r,$.wijmo.registerWidget(r,f.prototype)})(t.chart||(t.chart={}));var n=t.chart})(wijmo||(wijmo={}))});