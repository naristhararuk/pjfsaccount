var DialogObjID;
var DialogObjhref;
var Dialogresutlt;
var Dialogobj1;
var aaa;

function ConfirmID(Dialogsrc, Dialogobj2) {
    DialogConfirm(Dialogsrc, 'xx', 'Do you want to delete data?');
//    if (DialogObjID == Dialogobj2.id) {

//        return true;
//    }
//    var Dialogdest;
//    Dialogobj1 = Dialogobj2;
//    PageMethods.GetDialog(Dialogsrc, CallConfirmSuccess, CallFailed, Dialogdest);
//    return false;
}
function GetDialogConfirm(msgID,obj) {
        var a = PageMethods.GetDialogConfirm(msgID, obj);
    DialogConfirm(msgID,'xxx',a);
    return false;

}
function CallConfirmSuccess(Dialogres) {

    var Dialogmsg;
    Dialogmsg = (Dialogres.split("!@#$%"));
    DialogConfirm(Dialogobj1, Dialogmsg[1], Dialogmsg[2]);


    return false;
}
function DialogID(Dialogsrc) {
    var Dialogdest;
    //var b = 
    PageMethods.GetDialog(Dialogsrc);
   // alert(b);
 //  DialogConfirm(Dialogsrc,'xx',b );

    //return false;
}

function GetDialogError(Dialogsrc) {
    var Dialogdest;
    //var b =
    PageMethods.GetDialogError(Dialogsrc);
    // alert(b);
    //  DialogConfirm(Dialogsrc,'xx',b );

    //return false;
    //alert(b);
}


// set the destination textbox value with the ContactName
function CallDialogSuccess(Dialogres) {

    var Dialogmsg;
    Dialogmsg = (Dialogres.split("!@#$%"));
    DialogMsg(Dialogmsg[0], Dialogmsg[1], Dialogmsg[2]);


    return false;
}


// alert message on some failure
function CallFailed(Dialogres, destCtrl) {
    alert(Dialogres);
}

function DialogConfirm(DialogObj, Dialogtopic, Dialogmsg) {
    if (DialogObjID == DialogObj.id) {

        DialogObjID = "";
        return true;
    }
    DialogObjID = DialogObj.id;
    DialogObjhref = DialogObj.href;

    document.getElementById('<%=DivDialog.ClientID %>').style.display = "inline";
    document.getElementById('<%=DialogHeader.ClientID %>').innerHTML = "CONFIRM";
    document.getElementById('<%=DialogTopic.ClientID %>').innerHTML = Dialogtopic;
    document.getElementById('<%=DialogMsg.ClientID %>').innerHTML = Dialogmsg;
    document.getElementById('<%=divSolution.ClientID %>').style.display = "none";
    document.getElementById('<%=divOk.ClientID %>').style.display = "inline";
    document.getElementById('<%=DialogOkButton.ClientID %>').value = "OK";
    document.getElementById('<%=divCancel.ClientID %>').style.display = "inline";
    document.getElementById('<%=DialogCancelButton.ClientID %>').value = "Cancel";

    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.show();
    return false;
}

function DialogMsg(Dialogheader, Dialogtopic, Dialogmsg) {

    document.getElementById('<%=DivDialog.ClientID %>').style.display = "inline";
    document.getElementById('<%=DialogHeader.ClientID %>').innerHTML = Dialogheader;
    document.getElementById('<%=DialogTopic.ClientID %>').innerHTML = Dialogtopic;
    document.getElementById('<%=DialogMsg.ClientID %>').innerHTML = Dialogmsg;
    document.getElementById('<%=divSolution.ClientID %>').style.display = "none";
    document.getElementById('<%=divCancel.ClientID %>').style.display = "inline";
    document.getElementById('<%=divOk.ClientID %>').style.display = "none";
    document.getElementById('<%=DialogCancelButton.ClientID %>').value = "OK";

    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.show();

    return false;
}

function DialogError(Dialogheader, Dialogtopic, Dialogmsg, Dialogsulotion) {

    document.getElementById('<%=DivDialog.ClientID %>').style.display = "inline";
    document.getElementById('<%=divSolution.ClientID %>').style.display = "inline";
    document.getElementById('<%=DialogSolution.ClientID %>').innerHTML = Dialogsulotion;
    document.getElementById('<%=divCancel.ClientID %>').style.display = "inline";
    document.getElementById('<%=divOk.ClientID %>').style.display = "none";
    document.getElementById('<%=DialogCancelButton.ClientID %>').value = "OK";


    return false;
}

function CloseMP() {
    DialogObjID = "";
    document.getElementById('<%=DivDialog.ClientID %>').style.display = "none";
    document.getElementById('<%=DialogCancelButton.ClientID %>').click();

}
function OK() {


    if (DialogObjhref == undefined) {

        document.getElementById('<%=DivDialog.ClientID %>').style.display = "none";
        document.getElementById(DialogObjID).click();
    }
    else {

        var temp = DialogObjhref.split("('");
        var temp1 = temp[1].split("','')");
        DialogObjID = "";
        __doPostBack(temp1[0], '');
        document.getElementById('<%=DivDialog.ClientID %>').style.display = "none";


    }



}