var wijmo;define(["jquery","./wijmo.wijutil"],function(){(function(e){(function(e){var t=jQuery,n,r,i;n=function(e){var t=this;t.data={},t.reader=null,t.proxy=null,t.items=[],t.loading=null,t.loaded=null,t._constructor(e)},window.wijdatasource=n,t.extend(n.prototype,{_constructor:function(e){t.extend(this,e)},load:function(e,n){var r=this,i=r.proxy;if(t.isFunction(r.loading)&&r.loading(r,e)===!1)return;i?i.request(r,r.loaded,e):((r.items.length===0||n)&&this.read(),t.isFunction(r.loaded)&&r.loaded(r,e))},read:function(){var e=this,t=e.data;t&&e.reader?e.reader.read(e):e.items=e.data}}),r=function(e){t.isFunction(window.wijmoASPNetParseOptions)&&wijmoASPNetParseOptions(e),t.isArray(e)&&(this.fields=e)},window.wijarrayreader=r,t.extend(r.prototype,{read:function(e){t.isArray(e.data)?e.items=this._map(e.data):e.items=[]},_map:function(e){var n=this,r=[];return n.fields===undefined||n.fields.length===0?(t.extend(!0,r,e),r):(t.each(e,function(e,i){var s={};t.each(n.fields,function(e,n){n.mapping&&typeof n.mapping&&window[n.mapping]&&(n.mapping=window[n.mapping]);if(t.isFunction(n.mapping))return s[n.name]=n.mapping(i),!0;var r=n.mapping!==undefined?n.mapping:n.name,o=i[r];o===undefined&&(n.defaultValue!==undefined?o=n.defaultValue:o=i),s[n.name]=o}),r.push(s)}),r)}}),i=function(e){this.options=e,t.isFunction(window.wijmoASPNetParseOptions)&&wijmoASPNetParseOptions(e)},window.wijhttpproxy=i,t.extend(i.prototype,{request:function(e,n,r){var i=this,s,o;s=t.extend({},this.options),o=s.success,s.success=function(u){t.isFunction(o)&&o(u),i._complete(u,e,n,s,r)},t.ajax(s)},_complete:function(e,n,r,i,s){n.data=i.key!==undefined?e[i.key]:e,n.read(),t.isFunction(r)&&r(n,s)}})})(e.checkbox||(e.checkbox={}));var t=e.checkbox})(wijmo||(wijmo={}))});