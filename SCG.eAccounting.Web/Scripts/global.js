/* global.js version 1.2 */

/***********************************
window.onload = function() - all 
scripting activity that needs to be 
called onload should be place in this 
function call.
***********************************/

//window.onload = function(){	
//	parentHighlight("topNav");
//	parentHighlight("textTopNav");
//	parentHighlight("sideNav");

//	MenuMemory("topNav");
//	MenuMemory("tabNav");
//	MenuMemory("tabNavMB");
//	MenuMemory("textTopNav");

//	// if you want to use the submenu system the second value must be true
//	MenuMemory("sideNav", true);

//	
//	if (document.getElementById("siteSpecific").getElementsByTagName("UL").length > 0) {	
//		MenuMemory("siteSpecific");
//	}

//	initializePopup();
//}

	
/*
initializePopup() - needs to be run onload of the page. By rolling 
through the links onload we maintain the ability to open links 
into a new window while still meeting accessibility standards.
*/
function initializePopup() {
	var anchors = document.getElementsByTagName('a');
	for (var i=0;i<anchors.length;i++) {
		if(anchors.item(i).className.match(/newWindow|pdfWindow|external/)){
			anchors.item(i).onkeypress = openNewWindow;
			anchors.item(i).onclick = openNewWindow;
		}

		if (anchors.item(i).className.indexOf('newWindow') != -1) {
			anchors.item(i).setAttribute("title", "Pop-up Window");
		} else if (anchors.item(i).className.indexOf('pdfWindow') != -1) {
			anchors.item(i).setAttribute("title", "PDF file");
		} else if (anchors.item(i).className.indexOf('external') != -1) {
			anchors.item(i).setAttribute("title", "External link");
		}
	}
}

/*
openNewWindow() - opens, or if already open reloads, a new 
window with the height and width set in the links class 
attribute. 
*/
var popUp; // This variable is global to allow the "popup" window to be closed and reopened
function openNewWindow() {

	var width = getDimension(this.className.match(/w\d{1,4}/) + "");
  var height = getDimension(this.className.match(/h\d{1,4}/) + "");	

  var features = 'scrollbars, resizable,';
	features += 'height=' +height+ ", ";
	features += 'width=' +width;

	if(popUp) {
		//if the popup window did exist but was closed we will get an error.
		try {
			popUp.resizeTo(width,height);
		}
		catch(err) {
			//we need to tell this page the window was closed.
			popUp.close();
		}
	}
  
	popUp = window.open(this.href, '_new', features);
	popUp.focus();

  return false;
}

/*
getDimension() - Extracts the integer height/width from the links class attribute (a string).
attribValue - is the string value from which we want to extract the height/width
*/
function getDimension(attribValue) {
	var defaultValue = 300; //if no height/width is found this is what will be used
	var minDem = 0; //minimum height/width we will allow before we use the defaultValue

	attribValue = attribValue.replace(/w{1}|h{1}/, '');
	attribValue = parseInt(attribValue);
	
  if (attribValue > minDem) {    
    return attribValue;
  } 
	else {
    return defaultValue;
  }
 }


 function IsMaxLength(obj, maxLength) {
 	if (obj.value.length >= maxLength) {
 		obj.value = obj.value.substring(0, maxLength);
 	}
 }

function ConfirmDelete(gvClientID)
{
	var ctrlParent = document.getElementById(gvClientID);
	var blCheck = false;
	var ctrlChild = "ctlSelect";
	var Inputs = ctrlParent.getElementsByTagName("input");

	for(var i=0; i<Inputs.length; ++i)// วน Loop Control โดยเช็คว่า Type คือ Checkbox และ indexOf Child ลูก
	{
		if(Inputs[i].type == 'checkbox' && Inputs[i].id.indexOf(ctrlChild, 0) >= 0)
		{
			if(Inputs[i].checked) // ถ้าเจอ Checkbox ที่ ไม่ Check Header เอา Check ออก
			{
				blCheck = true;
				break;
			}
		}
	}
    
    if (blCheck) {
    	if (confirm('Do you want to delete?')) {
    		return true;
    	}
    	else {
    		return false;
    	}
		//return confirm('Do you want to delete?');
	}
	else
	{
		//alert('Please select data to delete');
		return false;
	}
}

function ConfirmOperation(message)
{
	return confirm(message);
}

//function ConfirmOperation(gvClientID, confirmMessage, alertMessage)
//{
//	var ctrlParent = document.getElementById(gvClientID);
//	var blCheck = false;
//	var ctrlChild = "ctlSelect";
//	var Inputs = ctrlParent.getElementsByTagName("input");

//	for(var i=0; i<Inputs.length; ++i)// วน Loop Control โดยเช็คว่า Type คือ Checkbox และ indexOf Child ลูก
//	{
//		if(Inputs[i].type == 'checkbox' && Inputs[i].id.indexOf(ctrlChild, 0) >= 0)
//		{
//			if(Inputs[i].checked) // ถ้าเจอ Checkbox ที่ ไม่ Check Header เอา Check ออก
//			{
//				blCheck = true;
//				break;
//			}
//		}
//	}
//	
//	if (blCheck)
//	{
//		return confirm(confirmMessage);
//	}
//	else
//	{
//		alert(alertMessage);
//		return false;
//	}
//}

//function ConfirmSave()
//{
//	return confirm('Do you want to save?');
//}

//function OpenReportPage(url)
//{
//    var features = 'scrollbars, resizable,';
//	//features += 'height=600';
//	//features += 'width=800';
//    
//    window.open(url,"",features);
//}

function CheckboxesCheckUncheck(objChk, objFlag, gvClientID, chkHeadClientID)
{
	//กำหนด Element หลัก ในที่นี้คือ GridView
	//var ctrlParent = document.getElementById('<%= grdUser.ClientID %>');
	//var ctrlchkHeader = document.getElementById('<%= grdUser.HeaderRow.FindControl("chkBoxHeader").ClientID %>');
	
	var ctrlParent = document.getElementById(gvClientID);
	var ctrlchkHeader = document.getElementById(chkHeadClientID);
	
	//กำหนด Element ลูก ในที่นี้คือ Checkbox ControlID
	var ctrlChild = "ctlSelect";

	//Get Control ที่อยู่ใน Parent ในที่นี้คือ GridView
	var Inputs = ctrlParent.getElementsByTagName("input");

	if(objFlag == 0) // Check All
	{
		for(var i=0; i<Inputs.length; ++i)// วน Loop Control โดยเช็คว่า Type คือ Checkbox และ indexOf Child ลูก
		{
			if((Inputs[i].type == 'checkbox' && Inputs[i].id.indexOf(ctrlChild, 0) >= 0) && !Inputs[i].disabled)
				Inputs[i].checked = objChk.checked;
		}
	}
	else // Check Child ว่า Check หมดให้ Check All ถ้าไม่หมดเอา Check All ออก
	{
		var blCheck;
		blCheck = true;

		for(var i=0; i<Inputs.length; ++i)// วน Loop Control โดยเช็คว่า Type คือ Checkbox และ indexOf Child ลูก
		{
			if(Inputs[i].type == 'checkbox' && Inputs[i].id.indexOf(ctrlChild, 0) >= 0)
			{
				if(!Inputs[i].checked) // ถ้าเจอ Checkbox ที่ ไม่ Check Header เอา Check ออก
				{
					blCheck = false;
					break;
				}
			}
		}

		if (ctrlchkHeader != null) {
		    if (blCheck) // ถ้า Child Check ทั้งหมด ให้ Header Check ด้วย
		    {
		        ctrlchkHeader.checked = true;
		    }
		    else // ถ้าเจอ Checkbox ที่ ไม่ Check Header เอา Check ออก
		    {
		        ctrlchkHeader.checked = false;
		    }
		}
	}
}

//* Key Alphabhet or number function return 0 1 2 3 4 5 6 7 8 9 0 a-z A-Z only */
function keyAlphaNumeric() {
    if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122))) {
        event.returnValue = false;
    }
}

function isKeyInt()
{
	var returnValue = false;
    
	if (event.keyCode <= 57 && event.keyCode >= 48)
	{
		returnValue = true;
	}
	else
	{
		event.keyCode = 0;
	}
	
	return returnValue;
}

function isKeyFloat()
{
	var returnValue = false;	
	
	if (event.keyCode==46)
		returnValue = true;
	
	if (!returnValue) 
	{
		if ( event.keyCode<=57 && event.keyCode>=48) 
			returnValue =  true;
		else 
			event.keyCode = 0;
	}
	return returnValue;
}
function Show(name) 
{
    document.getElementById(name).style.visibility = 'visible';
}
function Hidden(name) 
{
    document.getElementById(name).style.visibility = 'hidden';
}
function FieldSetHidden(name) {
    document.getElementById(name).style.display = 'none';
}
function FieldSetShow(name) {
    document.getElementById(name).style.display = 'block';
}

/* ---- Function for control cookie, use on LeftMenu.ascx ---- */
function getIndexFromCookie() {
    ad.set_SelectedIndex(parseFloat(GetCookie("selectIndex")));
}

function saveIndexInCookie(){
    DeleteCookie("selectIndex");
    SetCookie("selectIndex",ad.get_SelectedIndex());
}

function getCookieVal(offset){   
    var endstr=document.cookie.indexOf(";",offset);   
    if(endstr==-1)   
      endstr=document.cookie.length;   
    return unescape(document.cookie.substring(offset,endstr));   
}   

function FixCookieDate(data){   
    var base=new Date(0);   
    var skew=base.getTime();   
    if(skew>0)   
      date.setTime(date.getTime() - skew);   
}   

function GetCookie(name){   
    var arg=name+"=";   
    var alen=arg.length;   
    var clen= document.cookie.length;   
    var i = 0;   
    while(i< clen){   
      var j = i + alen;   
      if(document.cookie.substring(i,j) == arg)   
          return getCookieVal(j);   
      i = document.cookie.indexOf(" ",i) + 1;   
      if(i == 0) break;   
    }   
    return null;   
}   

function SetCookie(name,value,expires,path,domain,secure) {   
    try
    {
    document.cookie = name + "=" + escape(value) +   
      ((expires) ? "; expires=" + expires.toGMTString() : "") +   
      ((path) ? "; path=" + path : "") +   
      ((domain) ? "; domain=" + domain : "") +
      ((secure) ? "; secure" : "");alert(expires.toGMTString());
}
catch (exception) { }
}   

function DeleteCookie(name,path,domain)   {   
    if(GetCookie(name)){   
      document.cookie = name + "==" +   
          ((path) ? "; path=" + path : "") +   
          "; expires=The,01-Jan-70   00:00:01   GMT";   
    }   
}   

var expdate = new Date();
FixCookieDate(expdate);
expdate.setTime(expdate.getTime() + (365 * 24 * 60 * 60 * 1000));

/* Function for disable Ctrl + V */
function disablePasteOption() {
    if (window.event.ctrlKey) {
        if (window.event.keyCode == 86) {
            event.returnValue = false;
        }
    }

    if (window.event.shiftKey) {
        event.returnValue = false;
        return false;
    }

    return event.returnValue;
}

/* ----------- Add format controller method by Anuwat S ----------- */
// ========================================================================================================= //
// *******	Auther	: MAX
// *******	Purpose	: Find cursor position on textbox.
// *******	Date	: 02/05/2007
// ========================================================================================================= //
function getPositionInText(el) {
    var sel, rng, r2, i = -1;

    if (typeof el.selectionStart == "number") {
        i = el.selectionStart;
    }
    else if (document.selection && el.createTextRange) {
        sel = document.selection;
        if (sel) {
            r2 = sel.createRange();
            rng = el.createTextRange();
            rng.setEndPoint("EndToStart", r2);
            i = rng.text.length;
        }
    }
    else {
        el.onkeyup = null;
        el.onclick = null;
    }

    return i;
}

// ========================================================================================================= //
// *******	Auther	: MAX
// *******	Purpose	: Control float input object when input character
// *******	Date	: 12/05/2007
// *******	Remark	: Call on event onkeypress only!
// ========================================================================================================= //
function isKeyFloat(precision, scale, allowMinus) {
    var returnValue = false;

    // Prevent input '.' when no digit
    if (scale == 0 && event.keyCode == 46)
        event.keyCode = 0;

    // '.' character
    if (event.keyCode == 46)
        returnValue = true;

    // '-' character
    if (event.keyCode == 45) {
        returnValue = allowMinus;
        event.keyCode = 0;
        // Check allow minus value and prevent input duplicate '-' character
        if (allowMinus && (event.srcElement.value.indexOf('-') == -1))
            event.srcElement.value = '-' + event.srcElement.value;
    }

    // Move '-' character into left side

    var numArray = event.srcElement.value.split('.');
    // Prevent input '.' more than 1 character
    if (numArray.length > 1 && event.keyCode == 46) {
        event.keyCode = 0;
    }

    if (!returnValue) {
        // Input number only
        if (event.keyCode <= 57 && event.keyCode >= 48 && event.keyCode != 46) {
            returnValue = true;
            // Check scale length
            if (getPositionInText(event.srcElement) > event.srcElement.value.indexOf('.') && numArray.length > 1) {
                if (numArray[1].length >= scale) {
                    event.keyCode = 0;
                }
            }
            // Check precision length
            else if (numArray[0].length >= precision) {
                event.keyCode = 0;
            }
        }
        else {
            event.keyCode = 0;
        }
    }

    return returnValue;
}

// ========================================================================================================= //
// *******	Auther	: MAX
// *******	Purpose	: Control digit character
// *******	Date	: 02/05/2007
// ========================================================================================================= //
function digitFormat(scale) {
    var numArray = event.srcElement.value.split('.')
    var digitNum = '';
    if (numArray.length > 1) {
        digitNum = numArray[1];
    }
    padCount = scale - digitNum.length;
    for (i = 0; i < padCount; i++) {
        digitNum = digitNum + '0';
    }
    if (numArray[0] == '') numArray[0] = '0';

    event.srcElement.value = setFormatNumber(numArray[0]);

    if (scale != 0) event.srcElement.value = event.srcElement.value + '.' + digitNum;
}

// ========================================================================================================= //
// *******	Auther	: MAX
// *******	Purpose	: Insert comma into numeric character
// *******	Date	: 02/05/2007
// ========================================================================================================= //
function setFormatNumber(numStr) {
    var CommaDelimiter = ',';
    var returnValue = '';

    if (numStr.indexOf(',') == -1) {
        try {
            while (numStr.indexOf('0') == 0) {
                numStr = numStr.substring(1);
            }

            for (i = numStr.length; i >= 3; i = i - 3) {
                returnValue = numStr.substring(i - 3, i) + ',' + returnValue;
            }
            returnValue = numStr.substring(0, i) + ',' + returnValue.substring(0, returnValue.length - 1);

            if (returnValue.substring(returnValue.length - 1) == ',') {
                returnValue = returnValue.substring(0, returnValue.length - 1);
            }
            if (returnValue.indexOf(',') == 0) {
                returnValue = returnValue.substring(1, returnValue.length);
            }
            if (returnValue == '') {
                returnValue = '0';
            }
        }
        catch (exception) {
            alert("Invalid Format Number");
        }
    }
    else
        returnValue = numStr;

    return returnValue;
}

// For replace all oldChar by newChar in src string
function replaceAll(src, oldChar, newChar) {
    if (src.indexOf(oldChar) > -1)
        return replaceAll(src.replace(oldChar, newChar), oldChar, newChar);
    else
        return src;
}
/* --------- End add format controller method by Anuwat S --------- */

/* --- Function for validate input money value data */
// onKeyPress="return(currencyFormat(this,',','.',event))"
function currencyFormat(fld, milSep, decSep, e, maxlength, scale, allowMinus) {
    if (scale == undefined) scale = 2;
    var precision = maxlength - scale;

    if (allowMinus == undefined) allowMinus = false;

    if (event.srcElement.ondeactivate == null)
    {
        event.srcElement.ondeactivate = function() {
            var isMinus = event.srcElement.value.indexOf('-') > -1;
            event.srcElement.value = replaceAll(event.srcElement.value, ',', '');
            event.srcElement.value = replaceAll(event.srcElement.value, '-', '');
            //alert(scale);
            digitFormat(scale);
            //alert(event.srcElement.value);
            if (isMinus) event.srcElement.value = '-' + event.srcElement.value;
        }
    }

    return isKeyFloat(precision, scale, allowMinus);
/*
    var sep = 0;
    var key = '';
    var i = j = 0;
    var len = len2 = 0;
    var strCheck = '0123456789';
    var aux = aux2 = '';
    var whichCode = (window.Event) ? e.which : e.keyCode;
    if (whichCode == 13)
        return true;  // Enter

    key = String.fromCharCode(whichCode);  // Get key value from key code

    if (strCheck.indexOf(key) == -1) {
        fld.focus();
        var lSelect = fld.createTextRange();
        lSelect.moveStart("character", fld.value.length);
        lSelect.moveEnd("character", 0);
        lSelect.select();

        return false;  // Not a valid key
    }

    var valueNum = fld.value.replace(".", "");

    for (i = 0; i < maxlength; i++)
        valueNum = valueNum.replace("\,", "");

    //alert(valueNum);

    if (valueNum.length == (maxlength)) {
        fld.focus();
        var lSelect = fld.createTextRange();
        lSelect.moveStart("character", fld.value.length);
        lSelect.moveEnd("character", 0);
        lSelect.select();

        return false;
    }

    len = fld.value.length;
    for (i = 0; i < len; i++)
        if ((fld.value.charAt(i) != '0') && (fld.value.charAt(i) != decSep))
        break;

    aux = '';

    for (; i < len; i++)
        if (strCheck.indexOf(fld.value.charAt(i)) != -1)
        aux += fld.value.charAt(i);

    aux += key;
    len = aux.length;
    if (len == 0) fld.value = '';
    if (len == 1) fld.value = '0' + decSep + '0' + aux;
    if (len == 2) fld.value = '0' + decSep + aux;

    if (len > 2) {
        aux2 = '';
        for (j = 0, i = len - 3; i >= 0; i--) {
            if (j == 3) {
                aux2 += milSep;
                j = 0;
            }
            aux2 += aux.charAt(i);
            j++;
        }

        fld.value = '';
        len2 = aux2.length;
        for (i = len2 - 1; i >= 0; i--)
            fld.value += aux2.charAt(i);

        fld.value += decSep + aux.substr(len - 2, len);
    }

    fld.focus();

    var lSelect = fld.createTextRange();
    lSelect.moveStart("character", fld.value.length);
    lSelect.moveEnd("character", 0);
    lSelect.select();

    return false;
    */
}

function formatCurrency(obj) {
    num = obj.value;
    num = num.toString().replace(/\$|\,/g, '');

    if (isNaN(num))
        num = "0";

    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +

	num.substring(num.length - (4 * i + 3));
    num = (((sign) ? '' : '-') + num + '.' + cents);

    obj.value = num;
}

function formatCurrencyLabel(obj) {
    num = obj.innerHTML;
    num = num.toString().replace(/\$|\,/g, '');

    if (isNaN(num))
        num = "0";

    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +

	num.substring(num.length - (4 * i + 3));
    num = (((sign) ? '' : '-') + num + '.' + cents);

    obj.innerHTML = num;
}

// ========================================================================================================= //
// *******	Auther	: From internet
// *******	Purpose	: Padding character
// *******	Date	: 22/06/2009
// ========================================================================================================= //
function PadLeft(val, ch, num) {
    var re = new RegExp(".{" + num + "}$");
    var pad = "";
    if (!ch) ch = " ";
    do {
        pad += ch;
    } while (pad.length < num);
    return re.exec(pad + val)[0];
}
function PadRight(val, ch, num) {
    var re = new RegExp("^.{" + num + "}");
    var pad = "";
    if (!ch) ch = " ";
    do {
        pad += ch;
    } while (pad.length < num);
    return re.exec(val + pad)[0];
}
// ========================================================================================================= //
// *******	Auther	: MAX
// *******	Purpose	: Get length of selection character in textbox
// *******	Date	: 22/06/2009
// ========================================================================================================= //
function GetSelectionLength(id) {
    var element = document.getElementById(id);
    if (document.selection) {
        // The current selection
        var range = document.selection.createRange();
        return range.text.length;
    }
}
// ========================================================================================================= //
// *******	Auther	: MAX
// *******	Purpose	: Set cursor position in textbox
// *******	Date	: 22/06/2009
// ========================================================================================================= //
function SetCaretPosition(elemId, caretPos) {
    var elem = document.getElementById(elemId);

    if (elem != null) {
        if (elem.createTextRange) {
            var range = elem.createTextRange();
            range.move('character', caretPos);
            range.select();
        }
        else {
            if (elem.selectionStart) {
                elem.focus();
                elem.setSelectionRange(caretPos, caretPos);
            }
            else
                elem.focus();
        }
    }
}
// ========================================================================================================= //
// *******	Group	: Mask Edit input date for textbox
// *******	Auther	: MAX
// *******	Purpose	: To control Mask Edit in textbox
// *******	Date	: 22/06/2009
// ========================================================================================================= //
// ================== Declare variable ==================
var dateTypeFormat = "dd/mm/yyyy";
var dateMaskFormat = "__/__/20__";
var numCollection = new Array();
numCollection[48] = 0
numCollection[49] = 1
numCollection[50] = 2
numCollection[51] = 3
numCollection[52] = 4
numCollection[53] = 5
numCollection[54] = 6
numCollection[55] = 7
numCollection[56] = 8
numCollection[57] = 9
// ======================================================

var OnChangeDate = null;
var originalDateValue = '';

function DateFormatControlInit(cnt_id) {
    var txtDate = event.srcElement;
    txtDate.style.textAlign = "center";
    txtDate.setAttribute("onkeydown", DateKeydownControl);
    txtDate.setAttribute("onkeypress", DateKeypressControl);
    txtDate.setAttribute("onbeforedeactivate", DateB4DActivateControl);
    if (txtDate.onchange != null) OnChangeDate = txtDate.onchange;
    if (txtDate.value == "")
        txtDate.value = dateTypeFormat;
    originalDateValue = txtDate.value;
}

function DateFocusControl() {
    var txtDate = event.srcElement;

    SetCaretPosition(txtDate.id, 1);
}

function DateB4DActivateControl() {
    var txtDate = event.srcElement;
    if (txtDate.value == dateMaskFormat || txtDate.value == dateTypeFormat)
        txtDate.value = '';
    else
        txtDate.value = replaceAll(txtDate.value, '_', '0');

    if (txtDate.value != '' && txtDate.value != originalDateValue && OnChangeDate != null) {
        OnChangeDate();
        OnChangeDate = null;
    }
}

function DateKeypressControl() {
    event.cancelBubble = true;
    var txtDate = event.srcElement;
    var currentCursorPosition = getPositionInText(event.srcElement);

    // If at last position, do nothing
    if (currentCursorPosition == 10) return false;

    // If input character, break event and exit function 
    if (!isKeyInt()) return false;

    // If on position of century (20), break event and exit function
    if (currentCursorPosition == 6 || currentCursorPosition == 7) {
        SetCaretPosition(event.srcElement.id, 8);
        return false;
    }

    txtDate.value = txtDate.value.substring(0, currentCursorPosition) +
					numCollection[event.keyCode] +
					txtDate.value.substring(currentCursorPosition + 1);

    // If key number on position index = 1 (last digit of day), move focus to fist digit of month
    if (currentCursorPosition == 1)
        SetCaretPosition(event.srcElement.id, 3);
    // If key number on position index = 4 (last digit of month), move focus to year digit
    else if (currentCursorPosition == 4)
        SetCaretPosition(event.srcElement.id, 8);
    else
        SetCaretPosition(event.srcElement.id, currentCursorPosition + 1);

    return false;
}

function DateKeydownControl() {
    var txtDate = event.srcElement;

    if (txtDate.value == dateTypeFormat) {
        txtDate.value = dateMaskFormat;
        SetCaretPosition(txtDate.id, 0)
    }

    // 8 : backspace
    // 46 : delete
    if (event.keyCode == 8 || event.keyCode == 46) {
        var dateOutput = '';
        var charArray = txtDate.value.split('');
        var currentCursorPosition = getPositionInText(txtDate);
        var selectedLength = GetSelectionLength(txtDate.id);
        if (selectedLength == 0) selectedLength = 1;
        if (selectedLength == 1 && event.keyCode == 8) currentCursorPosition -= 1;

        for (i = 0; i < 10; i++) {
            // cannot delete '/' character
            // cannot delete century (20)
            // in selection
            if (charArray[i] != '/' &&
					i != 6 && i != 7 &&
					i >= currentCursorPosition && i < (currentCursorPosition + selectedLength))
                dateOutput = dateOutput + '_';
            else
                dateOutput = dateOutput + charArray[i];
        }

        txtDate.value = dateOutput;
        SetCaretPosition(txtDate.id, currentCursorPosition);
        return false;
    }
}
// ========================================================================================================= //

// ========================================================================================================= //
// *******	Auther	: MAX
// *******	Purpose	: Add date into date string 'dd/mm/yyyy'
// *******	Date	: 01/07/2009
// ========================================================================================================= //
function AddDate(strDate, addNum)
{
    // Convert to format 'yyyy/mm/dd' before set to date object
    var dateObj = new Date(strDate.substring(6) + '/' + strDate.substring(3, 5) + '/' + strDate.substring(0, 2));
    dateObj.setDate(dateObj.getDate() + addNum);
    return PadLeft(dateObj.getDate(), '0', '2') + '/' + PadLeft(dateObj.getMonth() + 1, '0', '2') + '/' + dateObj.getFullYear();
}
// ========================================================================================================= //

// ========================================================================================================= //
// *******	Auther	: MAX
// *******	Purpose	: Calculate Request Date of Remittance on Advance document
// *******	Date	: 01/07/2009
// ========================================================================================================= //
//function CalculateReqDateOfRemit(reqDateObjID, dueDateObjID, incrementDate, actionObjID)
//{
//    alert(actionObjID);
//    var actionField = document.getElementById(actionObjID);
//    var beginDateObj = event.srcElement;
//    var reqDateObj = document.getElementById(reqDateObjID);
//    var dueDateObj = document.getElementById(dueDateObjID);
//    if (beginDateObj.value != '')
//        reqDateObj.value = AddDate(beginDateObj.value, incrementDate);
//    else
//        reqDateObj.value = '';
//    dueDateObj.innerHTML = reqDateObj.value;
//    actionField.value = reqDateObj.value;
//}
// ========================================================================================================= //

function fncNotKeyBackSpace(keyStroke) {
    var t = window.event.srcElement.type;
    var keyCode = (document.layers) ? keyStroke.which : event.keyCode;
    var leftArrowKey = 37;
    var rightArrowKey = 39;
    var backSpaceKey = 8;
    var escKey = 27;
    var enterKey = 13;

    if (t && (t == 'text' || t == 'textarea' || t == 'file' || t == 'password')) {
        if (window.event.srcElement.readOnly) {
            if (keyCode == backSpaceKey)
                return false;
        }
        else {
            if ((window.event.altKey && window.event.keyCode == leftArrowKey) || (window.event.altKey && window.event.keyCode == rightArrowKey))
                return false;
        }
    }
    else {
        if ((window.event.altKey && window.event.keyCode == leftArrowKey) || (keyCode == escKey) || (keyCode == backSpaceKey) || (window.event.altKey && window.event.keyCode == rightArrowKey))
            return false;
    }
}