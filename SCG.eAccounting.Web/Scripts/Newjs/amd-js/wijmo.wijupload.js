var wijmo;define(["./wijmo.widget","swfobject"],function(){var e=this.__extends||function(e,t){function r(){this.constructor=e}for(var n in t)t.hasOwnProperty(n)&&(e[n]=t[n]);r.prototype=t.prototype,e.prototype=new r};(function(t){(function(n){var r=jQuery,i="wijupload",s="wijmo-wijupload",o="wijmo-wijupload-fileRow",u="."+o,a="wijmo-wijupload-filesList",f="wijmo-wijupload-commandRow",l="wijmo-wijupload-uploadAll",c="wijmo-wijupload-cancelAll",h="wijmo-wijupload-buttonContainer",p="wijmo-wijupload-upload",d="."+p,v="wijmo-wijupload-cancel",m="."+v,g="wijmo-wijupload-file",y="wijmo-wijupload-progress",b="wijmo-wijupload-loading",w,E,S=function(e){return e.indexOf("\\")>-1&&(e=e.substring(e.lastIndexOf("\\")+1)),e},x=function(e){var t=e.files,n="";return t?(r.each(t,function(e,t){n+=S(t.name)+"; "}),n.length&&(n=n.substring(0,n.lastIndexOf(";")))):n=S(e.value),n},T=function(e){var t=e.files,n=0;return t&&t.length>0&&r.each(t,function(e,t){t.size&&(n+=t.size)}),n};w=function(e,t,n){var i,s=r("input",t),o=function(e){e&&(e.abort(),e=null)},u=function(e){e&&(e=null)},a=function(){var e=this,i=s.get(0),a=i.files,f=[],l=0,c=0,h=function(t,n){var i=new XMLHttpRequest;return i.open("POST",n,!0),i.setRequestHeader("Wijmo-RequestType","XMLHttpRequest"),i.setRequestHeader("Cache-Control","no-cache"),i.setRequestHeader("Wijmo-FileName",encodeURI(t)),i.setRequestHeader("Content-Type","application/octet-stream"),i.upload.onprogress=function(t){if(t.lengthComputable){var n;r.isFunction(e.onProgress)&&(n={supportProgress:!0,loaded:c+t.loaded,total:T(s[0]),fileName:S(e.currentFile.name),fileNameList:x(s[0]).split("; ")},e.onProgress(n))}},i.onreadystatechange=function(t){if(this.readyState===4){var n=this.responseText,i;c+=a[l].size,l++,a.length>l?p(a[l]):r.isFunction(e.onComplete)&&(i={e:t,response:n,supportProgress:!0},e.onComplete(i))}},f.push(i),i},p=function(t){var r=S(t.name),i=h(r,n);e.handleRequest(i,t),e.currentFile=t,i.send(t)};e.fileRow=t,e.inputFile=s,e.upload=function(){p(a[l])},e.cancel=function(){r.each(f,function(e,t){o(t)}),r.isFunction(e.onCancel)&&e.onCancel()},e.destroy=function(){r.each(f,function(e,t){u(t)})},e.updateAction=function(e){n=e},e.handleRequest=null,e.onCancel=null,e.onComplete=null,e.onProgress=null};return i=new a,i},E=function(e,t,n){var i,s=r("input",t),o=s.attr("id"),u="wijUploadForm_"+e,a=r("#"+u),f="wijUploadIfm_"+o,l=!0,c=r('<iframe id="'+f+'" name="'+f+'">'),h=function(e,t){a.empty(),a.attr("target",e.attr("name")),t&&(t.parent().append(t.clone()),a.append(t)),a.submit()},p=function(e){e.attr("src","javascript".concat(":false;"))},d=function(e,t){t&&a&&(a.remove(),a=null),e&&(e.remove(),e=null)},v;return a.length===0&&(a=r('<form method="post" enctype="multipart/form-data"></form>'),a.attr("action",n).attr("id",u).attr("name",u).appendTo("body")),c.css("position","absolute").css("top","-1000px").css("left","-1000px"),c.appendTo("body"),v=function(){var e=this;e.fileRow=t,e.iframe=c,e.inputFile=s,e.upload=function(){var t;h(c,s),r.isFunction(e.onProgress)&&(t={supportProgress:!1,loaded:1,total:1},e.onProgress(t))},e.doPost=function(){h(c)},e.cancel=function(){p(c),r.isFunction(e.onCancel)&&e.onCancel()},e.updateAction=function(e){n=e,a.attr("action",e)},e.destroy=function(e){d(c,e)},e.onCancel=null,e.onComplete=null,e.onProgress=null,c.bind("load",function(t){if(!r.browser.safari&&l&&!e.autoSubmit){l=!1;return}if(c.attr("src")==="javascript".concat(":false;"))return;var n=t.target,i,s,o;try{s=n.contentDocument?n.contentDocument:window.frames[0].document,s.XMLDocument?i=s.XMLDocument:s.body?i=s.body.innerHTML:i=s,r.isFunction(e.onComplete)&&(o={e:t,response:i,supportProgress:!1},e.onComplete(o))}catch(u){i=""}finally{}})},i=new v,i};var N=function(t){function n(){t.apply(this,arguments)}return e(n,t),n.prototype._swfAppendAddtionalData=function(e){e.queueData={files:{},filesSelected:0,filesQueued:0,filesReplaced:0,filesCancelled:0,filesErrored:0,uploadsSuccessful:0,uploadsErrored:0,averageSpeed:0,queueLength:0,queueSize:0,uploadSize:0,queueBytesUploaded:0,uploadQueue:[],errorMsg:""},e.widget=this},n.prototype._swfGetHandlers=function(){var e=this,t=e.element;return{onSelect:function(t){var n=this,r={};if(e._trigger("change",null,t)===!1)return n.cancelUpload(t.id),!1;e._createFileRow(t),e._setAddBtnState(),this.queueData.queueSize+=t.size,this.queueData.files[t.id]=t},onSelectError:function(e,t){t==SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT?alert("File size exceeds the limitation!"):t==SWFUpload.QUEUE_ERROR.QUEUE_LIMIT_EXCEEDED&&alert("Too many files!")},onDialogOpen:function(){this.queueData.filesReplaced=0,this.queueData.filesCancelled=0},onDialogClose:function(t,n,r){var i=this.settings;this.queueData.filesErrored=t-n,this.queueData.filesSelected=t,this.queueData.filesQueued=n-this.queueData.filesCancelled,this.queueData.queueLength=r,e.isStartUpload=!1,e.options.autoSubmit&&(e.uploadAll=!0,e._swfUploadFile()),i.onDialogClose&&i.onDialogClose.call(this,this.queueData)},onUploadStart:function(t){this.bytesLoaded=0,this.queueData.uploadQueue.length==0&&(this.queueData.uploadSize=t.size);if(!e.isStartUpload&&e.uploadAll){if(e._trigger("totalUpload",null,null)===!1)return this.cancelUpload(),!1;e.isStartUpload=!0}if(e._trigger("upload",null,t)===!1)return this.cancelUpload(t.id),!1},onUploadProgress:function(n,i,s){var o=r("#"+n.id,t),u,a,f=Math.round(i/s*100),l=r("."+y,o),c={sender:n.name,loaded:i,total:s},h=this.queueData;l.html(f+"%"),e._trigger("progress",null,c),u=h.queueBytesUploaded+i,a=h.queueSize,e._updateSwfProgress(u,a),e._trigger("totalProgress",null,{loaded:u,total:a})},onUploadError:function(e,n,i){var s=this.settings,o=r("#"+e.id,t),u=r("."+y,o),a="Error";switch(n){case SWFUpload.UPLOAD_ERROR.HTTP_ERROR:a="HTTP Error ("+i+")";break;case SWFUpload.UPLOAD_ERROR.MISSING_UPLOAD_URL:a="Missing Upload URL";break;case SWFUpload.UPLOAD_ERROR.IO_ERROR:a="IO Error";break;case SWFUpload.UPLOAD_ERROR.SECURITY_ERROR:a="Security Error";break;case SWFUpload.UPLOAD_ERROR.UPLOAD_LIMIT_EXCEEDED:alert("The upload limit has been reached ("+i+")."),a="Exceeds Upload Limit";break;case SWFUpload.UPLOAD_ERROR.UPLOAD_FAILED:a="Failed";break;case SWFUpload.UPLOAD_ERROR.SPECIFIED_FILE_ID_NOT_FOUND:break;case SWFUpload.UPLOAD_ERROR.FILE_VALIDATION_FAILED:a="Validation Error";break;case SWFUpload.UPLOAD_ERROR.FILE_CANCELLED:a="Cancelled",this.queueData.queueSize-=e.size,this.queueData.queueLength-=1;if(e.status==SWFUpload.FILE_STATUS.IN_PROGRESS||r.inArray(e.id,this.queueData.uploadQueue)>=0)this.queueData.uploadSize-=e.size;s.onCancel&&s.onCancel.call(this,e),delete this.queueData.files[e.id];break;case SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED:a="Stopped"}u.text(a);var f=this.getStats();this.queueData.uploadsErrored=f.upload_errors},onUploadSuccess:function(n,i,s){var o=this.getStats();this.queueData.uploadsSuccessful=o.successful_uploads,this.queueData.queueBytesUploaded+=n.size,this.queueData.response=s;var u=r("#"+n.id,t),a=this;a.queueData.queueLength-=1,u.fadeOut(1500,function(){u.remove(),e.options.showUploadedFiles&&e._createUploadedFiles(n.name),a.queueData.queueLength||e.commandRow.hide()}),e._trigger("complete",null,{response:s})},onUploadComplete:function(t,n,r){var i=this;!i.queueData.queueLength&&e.uploadAll&&(e._cleanSwfProgress(),e._trigger("totalComplete",null,i.queueData)),e.uploadAll&&e._swfUploadFile()}}},n.prototype._cleanSwfProgress=function(){},n.prototype._updateSwfProgress=function(e,t){},n.prototype._initSwfUploadOptions=function(e,t){var n=this,i=n.element,s,o=n._swfGetHandlers(),u=n.options,a=n.options.swfUploadOptions,f=i.attr("id"),l=f+"_SWFUpload";return r("<input type='file' id='"+l+"'>").appendTo(i),s=r.extend({id:l,swf:"SWFUpload.swf",auto:!1,buttonClass:"",buttonCursor:"hand",buttonImage:null,checkExisting:!1,debug:!1,fileObjName:"Filedata",fileSizeLimit:u.maximumFileSize?u.maximumFileSize:0,fileTypeDesc:"All Files",fileTypeExts:u.accept?u.accept:"*.*",height:t,itemTemplate:!1,method:"post",multi:u.multiple,formData:{},preventCaching:!0,progressData:"percentage",queueID:!1,queueSizeLimit:u.maximumFiles?u.maximumFiles:999,removeCompleted:!0,removeTimeout:3,requeueErrors:!1,successTimeout:30,uploadLimit:0,width:e,uploader:u.action,overrideEvents:[]},a),{assume_success_timeout:s.successTimeout,button_placeholder_id:s.id,button_width:s.width,button_height:s.height,button_text:null,button_text_style:null,button_text_top_padding:0,button_text_left_padding:0,button_action:u.multiple?SWFUpload.BUTTON_ACTION.SELECT_FILES:SWFUpload.BUTTON_ACTION.SELECT_FILE,button_disabled:!1,button_cursor:s.buttonCursor=="arrow"?SWFUpload.CURSOR.ARROW:SWFUpload.CURSOR.HAND,button_window_mode:SWFUpload.WINDOW_MODE.TRANSPARENT,debug:s.debug,requeue_on_error:s.requeueErrors,file_post_name:s.fileObjName,file_size_limit:s.fileSizeLimit,file_types:s.fileTypeExts,file_types_description:s.fileTypeDesc,file_queue_limit:s.queueSizeLimit,file_upload_limit:s.uploadLimit,flash_url:s.swf,prevent_swf_caching:s.preventCaching,post_params:s.formData,upload_url:s.uploader,use_query_string:s.method=="get",file_dialog_complete_handler:o.onDialogClose,file_dialog_start_handler:o.onDialogOpen,file_queued_handler:o.onSelect,file_queue_error_handler:o.onSelectError,swfupload_loaded_handler:s.onSWFReady,upload_complete_handler:o.onUploadComplete,upload_error_handler:o.onUploadError,upload_progress_handler:o.onUploadProgress,upload_start_handler:o.onUploadStart,upload_success_handler:o.onUploadSuccess}},n.prototype._createSWFUpload=function(){var e=this,t=e.element,n=e.addBtn,i,s=e.options.swfUploadOptions,o,u=n.width(),a=n.height(),f=swfobject.getFlashPlayerVersion(),l=f.major>=9;l?(i=e._initSwfUploadOptions(u,a),o=new SWFUpload(i),e.swfupload=o,r("#"+o.movieName).css({position:"absolute","z-index":100,top:0,left:0,width:u,height:a}),e._swfAppendAddtionalData(o)):(alert("Please install flash player."),s&&s.onFallback&&s.onFallback.call())},n.prototype._create=function(){var e=this,n=e.options,i=(new Date).getTime(),s=e.supportXhr();window.wijmoApplyWijTouchUtilEvents&&(r=window.wijmoApplyWijTouchUtilEvents(r)),e.filesLen=0,e.totalUploadFiles=0,e.useXhr=s,e.id=i,e._createContainers(),e._createUploadButton(),n.enableSWFUploadOnIE&&r.browser.msie||n.enableSWFUpload?e._createSWFUpload():e._createFileInput(),e._bindEvents(),e.element.is(":hidden")&&e.element.wijAddVisibilityObserver&&e.element.wijAddVisibilityObserver(function(){e._applyInputPosition(),e.element.wijRemoveVisibilityObserver&&e.element.wijRemoveVisibilityObserver()},"wijupload"),t.prototype._create.call(this)},n.prototype._setOption=function(e,n){var r=this;t.prototype._setOption.call(this,e,n),e==="accept"&&r.input&&r.input.attr("accept",n)},n.prototype._innerDisable=function(){t.prototype._innerDisable.call(this),this._handleDisabledOption(!0,this.upload)},n.prototype._innerEnable=function(){t.prototype._innerEnable.call(this),this._handleDisabledOption(!1,this.upload)},n.prototype._handleDisabledOption=function(e,t){var n=this;e?(n.disabledDiv||(n.disabledDiv=n._createDisabledDiv(t)),n.disabledDiv.appendTo("body")):n.disabledDiv&&(n.disabledDiv.remove(),n.disabledDiv=null)},n.prototype._createDisabledDiv=function(e){var t=this,n,i=e?e:t.upload,s=i.offset(),o=i.outerWidth(),u=i.outerHeight();return n=r("<div></div>").addClass(t.options.wijCSS.stateDisabled).css({"z-index":"99999",position:"absolute",width:o,height:u,left:s.left,top:s.top}),r.browser.msie&&(n.css("background-color","white"),parseInt(r.browser.version)>=9&&n.css("opacity","0.1")),n},n.prototype.destroy=function(){var e=this;e.upload.removeClass(s),e.upload.undelegate(e.widgetName).undelegate("."+e.widgetName),e.input&&e.input.remove(),e.addBtn&&e.addBtn.remove(),e.filesList&&e.filesList.remove(),e.commandRow&&e.commandRow.remove(),e.isCreateByInput===!0&&e.element.css({display:""}).unwrap(),e.uploaders&&(r.each(e.uploaders,function(e,t){t.destroy&&t.destroy(!0),t=null}),e.uploaders=null),e.swfupload&&(e.swfupload.destroy&&e.swfupload.destroy(),e.swfupload=null),e.disabledDiv&&(e.disabledDiv.remove(),e.disabledDiv=null),t.prototype.destroy.call(this)},n.prototype.widget=function(){return this.upload},n.prototype.supportXhr=function(){var e=!1;return typeof (new XMLHttpRequest).upload=="undefined"?e=!1:e=!0,e},n.prototype._createContainers=function(){var e=this,t,n,i=e.element;if(i.is(":input")&&i.attr("type")==="file")e.isCreateByInput=!0,e.maxDisplay=i.attr("multiple")||e.options.multiple?0:1,e.upload=i.css({display:"none"}).wrap("<div>").parent();else{if(!e.element.is("div"))throw'The initial markup must be "DIV", "INPUT[type=file]"';e.maxDisplay=e.options.multiple?0:1,e.upload=i}e.upload.addClass(s),t=r("<ul>").addClass(a).appendTo(e.upload),n=r("<div>").addClass(f).appendTo(e.upload),e.filesList=t,n.hide(),e.commandRow=n,e._createCommandRow(n)},n.prototype._createCommandRow=function(e){var t=this,n=t.options,i=r("<a>").attr("href","#").text("uploadAll").addClass(l).button({icons:{primary:n.wijCSS.iconCircleArrowN},label:t._getLocalization("uploadAll","Upload All")}).button("widget").addClass(n.wijCSS.stateDefault),s=r("<a>").attr("href","#").text("cancelAll").addClass(c).button({icons:{primary:n.wijCSS.iconCancel},label:t._getLocalization("cancelAll","Cancel All")}).button("widget").addClass(n.wijCSS.stateDefault);e.append(i).append(s)},n.prototype._getLocalization=function(e,t){var n=this.options.localization;return n&&n[e]||t},n.prototype._createUploadButton=function(){var e=this,t=e.options,n=r("<a>").attr("href","javascript:void(0);").button({label:e._getLocalization("uploadFiles","Upload files")}).button("widget").addClass(t.wijCSS.stateDefault);n.mousemove(function(t){var r=n.data("ui-button").options.disabled;if(e.input){var i=t.pageX,s=t.pageY;r||e.input.offset({left:i+10-e.input.width(),top:s+10-e.input.height()})}}),e.addBtn=n,e.upload.prepend(n)},n.prototype._applyInputPosition=function(){var e=this,t=e.addBtn,n=t.offset(),r=e.cuurentInput;r.offset({left:n.left+t.width()-r.width(),top:n.top}).height(t.height())},n.prototype._createFileInput=function(){var e=this,t=e.addBtn,n=t.offset(),i=e.element.attr("accept")||e.options.accept,s="wijUpload_"+e.id+"_input"+e.filesLen,o=r("<input>").attr("type","file").prependTo(e.upload),u=e.options.maximumFiles||e.maxDisplay;u!==1&&e.maxDisplay===0&&o.attr("multiple","multiple"),i&&o.attr("accept",i),e.cuurentInput=o,e.filesLen++,o.attr("id",s).attr("name",s).css("position","absolute").offset({left:n.left+t.width()-o.width(),top:n.top}).css("z-index","9999").css("opacity",0).height(t.height()).css("cursor","pointer"),e.input=o,o.bind("change",function(t){var n,i;if(e._trigger("change",t,r(this))===!1)return!1;e._createFileInput(),n=e._createFileRow(r(this)),e._setAddBtnState(),e.options.autoSubmit&&(i=r(d,n),i&&i.click()),o.unbind("change")}),e.uploadAll=!1},n.prototype._createUploadedFiles=function(e){},n.prototype._setAddBtnState=function(){var e=this,t=e.options.maximumFiles||e.maxDisplay,n=e.addBtn,i;if(!t)return;if(!n)return;e.maskDiv||(e.maskDiv=r("<div></div>").css("position","absolute").css("z-index","9999").width(n.outerWidth()).height(n.outerHeight()).appendTo(e.upload).offset(n.offset())),i=r("li",e.filesList),i.length>=t?(n.button({disabled:!0}),e.maskDiv.show(),e.input&&e.input.css("left","-1000px")):(n.button({disabled:!1}),e.maskDiv.hide())},n.prototype._createFileRow=function(e){var t=this,n=t.options,i=r("<li>"),s="",a,f,l,c=r("<span>").addClass(h),d=r("<a>").attr("href","#").text("upload").addClass(p).button({text:!1,icons:{primary:n.wijCSS.iconCircleArrowN},label:t._getLocalization("upload","upload")}).button("widget").addClass(n.wijCSS.stateDefault),m=r("<a>").attr("href","#").text("cancel").addClass(v).button({text:!1,icons:{primary:n.wijCSS.iconCancel},label:t._getLocalization("cancel","cancel")}).button("widget").addClass(n.wijCSS.stateDefault);return i.addClass(o).addClass(n.wijCSS.content).addClass(n.wijCSS.cornerAll),n.enableSWFUploadOnIE&&r.browser.msie||n.enableSWFUpload?(s=e.name,i.attr("id",e.id).data("file",e)):(i.append(e),e.hide(),s=x(e[0])),a=r("<span>"+s+"</span>").addClass(g).addClass(n.wijCSS.stateHighlight).addClass(n.wijCSS.cornerAll),i.append(a),i.append(c),f=r("<span />").addClass(y),c.append(f),c.append(d).append(m),i.appendTo(t.filesList),l=r(u,t.upload),l.length&&(t.commandRow.show(),(!n.enableSWFUploadOnIE||!r.browser.msie)&&!n.enableSWFUpload&&t._createUploader(i),t._resetProgressAll()),i},n.prototype._createUploader=function(e){var t=this,n=r("input",e),i=t.options.action,s=t.options.handleRequest,o;t.useXhr?(o=w(t.id,e,i),o.handleRequest=function(e,n){r.isFunction(s)&&s.call(t,e,n)}):o=E(t.id,e,i),o.onCancel=function(){var e=this;t._trigger("cancel",null,e.inputFile),t.totalUploadFiles===0&&t.uploadAll&&t._trigger("totalComplete")},t._wijUpload()&&(o.onProgress=function(e){var n=r("."+y,this.fileRow),i={sender:e.fileName,loaded:e.loaded,total:e.total,fileNameList:undefined},s=this.inputFile.attr("id");e.supportProgress?(n.html(Math.round(1e3*e.loaded/e.total)/10+"%"),e.fileNameList&&(i.fileNameList=e.fileNameList),t._trigger("progress",null,i),t._progressTotal(s,e.loaded)):n.addClass(b)},o.onComplete=function(e){var n=this,i=n.inputFile.attr("id"),s=t.uploaders[i],o=T(n.inputFile[0]),u=r("."+y,n.fileRow);t._trigger("complete",e.e,r.extend(!0,n.inputFile,e)),u.removeClass(b),u.html("100%"),t._removeFileRow(n.fileRow,s,!0),t._progressTotal(i,o),t.totalUploadFiles--,t.totalUploadFiles===0&&(t.uploadAll||t.options.autoSubmit)&&t._trigger("totalComplete",e.e,e)}),typeof t.uploaders=="undefined"&&(t.uploaders={}),t.uploaders[n.attr("id")]=o},n.prototype._progressTotal=function(e,t){var n=this,r=n.progressAll,i,s;if(!n.uploadAll)return;r&&r.loadedSize&&(r.loadedSize[e]=t,i=n._getLoadedSize(r.loadedSize),s=r.totalSize),n._trigger("totalProgress",null,{loaded:i,total:s})},n.prototype._getLoadedSize=function(e){var t=0;return r.each(e,function(e,n){t+=n}),t},n.prototype._getTotalSize=function(){var e=this,t=0;return e.uploaders&&r.each(e.uploaders,function(e,n){t+=T(n.inputFile[0])}),t},n.prototype._resetProgressAll=function(){this.progressAll={totalSize:0,loadedSize:{}}},n.prototype._wijUpload=function(){return!0},n.prototype._wijcancel=function(e){},n.prototype._upload=function(e,t){},n.prototype._swfUploadFile=function(e){this.swfupload.startUpload(e)},n.prototype._bindEvents=function(){var e=this,t=e.options,n=e.progressAll;e.upload.delegate(m,"click."+e.widgetName,function(i){var s=r(this),o=s.parents(u),a,f;i.preventDefault();if(t.enableSWFUploadOnIE&&r.browser.msie||t.enableSWFUpload){var l=o.data("file");e.swfupload.cancelUpload(l.id),o.fadeOut(1500,function(){o.remove(),e._setAddBtnState(),e.swfupload.queueData.queueLength==0&&e.commandRow.hide()})}else a=r("input",o[0]),f=e.uploaders[a.attr("id")],e._wijcancel(a),e._wijUpload()&&f&&f.cancel(),n&&(n.totalSize-=T(a[0]),n.loadedSize[a.val()]&&delete n.loadedSize[a.val()]),e._removeFileRow(o,f,!1)}),e.upload.delegate(d,"click."+e.widgetName,function(n){var i=r(this),s=i.parents(u),o,a;n.preventDefault();if(t.enableSWFUploadOnIE&&r.browser.msie||t.enableSWFUpload){var f=s.data("file");e.uploadAll=!1,e._wijUpload()?e._swfUploadFile(f.id):e._upload(f.id,!0)}else{o=r("input",s[0]),a=e.uploaders[o.attr("id")];if(e._trigger("upload",n,o)===!1)return!1;if(e.options.autoSubmit){a.autoSubmit=!0;if(e._trigger("totalUpload",n,null)===!1)return!1}e.totalUploadFiles++,e._upload(s),a&&e._wijUpload()&&a.upload()}}),e.upload.delegate("."+l,"click."+e.widgetName,function(n){n.preventDefault();if(t.enableSWFUploadOnIE&&r.browser.msie||t.enableSWFUpload)e.uploadAll=!0,e._wijUpload()?e._swfUploadFile():e._upload(!0,!0);else{e.uploadAll=!0,e.progressAll||e._resetProgressAll();if(e._trigger("totalUpload",n,null)===!1)return!1;e.progressAll.totalSize=e._getTotalSize(),e._wijuploadAll(r(d,e.filesList[0])),e._wijUpload()&&r(d,e.filesList[0]).each(function(e,t){return r(t).click(),e})}}),e.upload.delegate("."+c,"click."+e.widgetName,function(n){n.preventDefault(),t.enableSWFUploadOnIE&&r.browser.msie||t.enableSWFUpload?(r.each(e.swfupload.queueData.files,function(t,n){e.swfupload.cancelUpload(t)}),r(u,e.element).fadeOut(1500,function(){r(this).remove(),e.commandRow.hide()})):(e._resetProgressAll(),r(m,e.filesList[0]).each(function(e,t){r(t).click()}))})},n.prototype._wijuploadAll=function(e){},n.prototype._wijFileRowRemoved=function(e,t,n){this._setAddBtnState()},n.prototype._removeFileRow=function(e,t,n){var i=this,s,o;t&&(s=t.inputFile.attr("id")),e.fadeOut(1500,function(){e.remove(),i._wijFileRowRemoved(e,t.inputFile,n),i.uploaders[s]&&delete i.uploaders[s],o=r(u,i.upload),o.length?(i.commandRow.show(),t&&t.destroy&&t.destroy()):(i.commandRow.hide(),i.totalUploadFiles=0,i._resetProgressAll(),t&&t.destroy&&t.destroy(!0))})},n.prototype._getFileName=function(e){return S(e)},n.prototype._getFileNameByInput=function(e){return x(e)},n.prototype._getFileSize=function(e){return T(e)},n}(t.wijmoWidget);n.wijupload=N;var C=function(){function e(){this.wijCSS={iconCircleArrowN:"ui-icon-circle-arrow-n",iconCancel:"ui-icon-cancel"},this.action="",this.autoSubmit=!1,this.change=null,this.upload=null,this.totalUpload=null,this.progress=null,this.totalProgress=null,this.complete=null,this.totalComplete=null,this.maximumFiles=0,this.multiple=!0,this.accept="",this.enableSWFUploadOnIE=!1,this.enableSWFUpload=!1,this.swfUploadOptions={},this.localization={},this.handleRequest=null}return e}(),k="wijmo-wijupload",L=k+"-";N.prototype.options=r.extend(!0,{},t.wijmoWidget.prototype.options,new C),r.wijmo.registerWidget("wijupload",N.prototype)})(t.upload||(t.upload={}));var n=t.upload})(wijmo||(wijmo={}))});