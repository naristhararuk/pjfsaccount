var UnLockParameters = new Array();
var READY_STATE_UNINITIALIZED = 0;
var READY_STATE_LOADING = 1;
var READY_STATE_LOADED = 2;
var READY_STATE_INTERACTIVE = 3;
var READY_STATE_COMPLETE = 4;
var READY_STATE_STATUS = 200;

function getXMLHTTPRequest() {
    var xRequest = null;
    if (window.XMLHttpRequest) {
        xRequest = new XMLHttpRequest();
    } else if (typeof ActiveXObject != "undefined") {
        xRequest = new ActiveXObject("Microsoft.XMLHTTP");
    }
    return xRequest;
}
function sendRequest(url, params, HttpMethod, callbackFunc) {
    if (!HttpMethod) {
        HttpMethod = "POST";
    }
    var req = getXMLHTTPRequest();
    if (req) {
        req.onreadystatechange = function() {
            callbackFunc(req);
        };
        req.open(HttpMethod, url, false);
        req.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        req.send(params);
    }
}
function getDialogMessage(MessageID) {
    var args = "MsgID=" + MessageID;
    sendRequest("Messages/GetMessage.aspx", args, "POST", MessageCallback);
}

function MessageCallback(req) {
    if (req.readyState == READY_STATE_COMPLETE) {
        if (req.status == READY_STATE_STATUS) {
            var result = req.responseText;
            eval(result);
            if (result != "") {

                var qMsgHeaser = window.document.getElementById('ctl00_ctl_MsgHeaser_Label');
                qMsgHeaser.innerHTML = ObjMessage.DialogHeader;
                var modalPopupBehavior = $find('ctl00_ctl_Panel_ModalPopupExtender');
                modalPopupBehavior.show();

            }

        }
        else {
            Alert('ERROR: AJAX request status = ' + req.status);
        }

    }
}

function MsgClose() {

    var modalPopupBehavior = $find('ctl00_ctl_Panel_ModalPopupExtender');
    modalPopupBehavior.hide();
    var qPanelShowMessage = window.document.getElementById('ctl00_PanelShowMessage');
    qPanelShowMessage.style.display = "none";

}

function MsgOK() {
    var modalPopupBehavior = $find('ctl00_ctl_Panel_ModalPopupExtender');
    modalPopupBehavior.hide();
    var qPanelShowMessage = window.document.getElementById('ctl00_PanelShowMessage');
    qPanelShowMessage.style.display = "none";
    return true;

}
function MsgCancel() {
    var modalPopupBehavior = $find('ctl00_ctl_Panel_ModalPopupExtender');
    modalPopupBehavior.hide();
    var qPanelShowMessage = window.document.getElementById('ctl00_PanelShowMessage');
    qPanelShowMessage.style.display = "none";
    return false;

}

function TryLock(documentID, userID, objID) {
    var args = "documentID=" + documentID;
    args += "&userID=" + userID;
    if (objID.length > 0) {

        var flag = document.getElementById(objID).checked;

    }
    args += "&LockFlag=" + flag;

    sendRequest("../../../DocumentView/TryLock.aspx", args, "POST", TryLockDocumentViewCallback);
}
function ForceLock(documentID, userID) {
    var args = "documentID=" + documentID;
    args += "&userID=" + userID;
    sendRequest("../../../DocumentView/ForceLock.aspx", args, "POST", ForceLockDocumentViewCallback);
}

function UnLock() {
    var args = "";
    for (var x in UnLockParameters) {
        if (x == "documentID" || x == "userID", x == "txID") {
            args += x + "=" + UnLockParameters[x] + "&";
        }
    }
    sendRequest("../../../DocumentView/UnLock.aspx", args, "POST", UnLockDocumentViewCallback);
}

function TryLockDocumentViewCallback(req) {
    if (req.readyState == READY_STATE_COMPLETE) {
        if (req.status == READY_STATE_STATUS) {
            var result = req.responseText;
            //eval(result);
            if (result != "") {

                // alert(result);
                var lockMsg = window.document.getElementById('ctl00_A_ctlShowDocumentViewLock');
                lockMsg.innerHTML = result;
                //qMsgHeaser.innerHTML = ObjMessage.DialogHeader;
                //var modalPopupBehavior = $find('ctl00_ctl_Panel_ModalPopupExtender');
                //modalPopupBehavior.show();

            }

        }
        else {
            Alert('ERROR: AJAX request status = ' + req.status);
        }

    }
}





function ForceLockDocumentViewCallback(req) {
    if (req.readyState == READY_STATE_COMPLETE) {
        if (req.status == READY_STATE_STATUS) {
            var result = req.responseText;
            //eval(result);
            if (result != "") {

                alert(result);
                //var qMsgHeaser = window.document.getElementById('ctl00_ctl_MsgHeaser_Label');
                //qMsgHeaser.innerHTML = ObjMessage.DialogHeader;
                //var modalPopupBehavior = $find('ctl00_ctl_Panel_ModalPopupExtender');
                //modalPopupBehavior.show();

            }

        }
        else {
            Alert('ERROR: AJAX request status = ' + req.status);
        }

    }
}




function UnLockDocumentViewCallback(req) {
    if (req.readyState == READY_STATE_COMPLETE) {
        if (req.status == READY_STATE_STATUS) {
            var result = req.responseText;
            //eval(result);
            if (result != "") {

                // alert(result);
                //var qMsgHeaser = window.document.getElementById('ctl00_ctl_MsgHeaser_Label');
                //qMsgHeaser.innerHTML = ObjMessage.DialogHeader;
                //var modalPopupBehavior = $find('ctl00_ctl_Panel_ModalPopupExtender');
                //modalPopupBehavior.show();

            }

        }
        else {
            Alert('ERROR: AJAX request status = ' + req.status);
        }

    }
}

function pageLoad() {
    var manager = Sys.WebForms.PageRequestManager.getInstance();
    manager.add_endRequest(EndRequestHandler);
}

function EndRequestHandler(sender, args) {
    if (args.get_error() != undefined) {
        var d = new Date();
        var t_day = d.getDate();
        var t_month = d.getMonth() + 1;
        var t_year = d.getFullYear();
        var t_hour = d.getHours();
        var t_min = d.getMinutes();
        var t_sec = d.getSeconds();

        var msg = args.get_error().message;
        var msgDate = "" + t_day + "/" + t_month + "/" + t_year + " " + t_hour + ":" + t_min + ":" + t_sec + "";
        //msg = msg.substring(msg.indexOf(":", 0) + 1);
        //sendRequest("../../../ServerDateProvider.aspx",args,"",showErrorMessage);

        //alert("An error occurred.Please try again." + "\n" + msgDate + "\n" + msg);
        //alert("An error occurred.Please try again." + "\n" + msgDate + "\n");
        alert("Please try again.\n[Error Detail]\n" + msgDate +"\n" + msg);
        args.set_errorHandled(true);
    }
    //    if (args.get_error() != undefined) {
    //        alert("An error occurred.Please try again.");
    //        args.set_errorHandled(true);
    //    }
}