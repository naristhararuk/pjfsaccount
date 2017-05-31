<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Untitled Page</title>
</head>

<style type="text/css">
.fontA {
    font-family:Tahoma;
	font-size:14px;
}
.HeaderTable{
	font-weight:bold;
	background-color:#1d9f34;
	color:White;
	text-align:center;
}
.ItemTable{
	background-color:#a4f4a0;
	color:#2D2D2D;
}
</style>

<body class="fontA">

<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/head.png" BORDER="0" ALT="SCG Account">
</div>

<p><B>Dear Khun $creatorName$,</B></p>
<p>Subject : $subject$</p>
Your request, $requesterID$ has been $WorkFlowStateEventName$
        
$if(remark)$
<br/>
because $reason$ - $remark$.
<br/>
$else$
.
<br/>
$endif$

<br/>
Please click <b><a href="$link$/Login.aspx?wfid=$wfid$">here</a></b> to make change.
<br/>
<br/>
Yours Sincerely,

<br/>
<br/>
<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/LINE.png" BORDER="0" ALT="SCG Account">
</div>
<br/>

<p><B>เรียนคุณ $creatorName$</B></p>
<p>เรื่อง $subject$</p>
ใบแจ้งเลขที่ $requesterID$ ของคุณ ได้ถูก $WorkFlowStateEventName$
        
$if(remark)$
<br/>
เนื่องจาก $reason$ - $remark$.
<br/>
$else$
<br/>
$endif$

<br/>
โปรดคลิกลิ้งค์ <b><a href="$link$/Login.aspx?wfid=$wfid$">ที่นี้</a></b> เพื่อทำการแก้ไข
<br/>
<br/>
ขอแสดงความนับถือ

<br/>
<br/>
<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/LINE.png" BORDER="0" ALT="SCG Account">
</div>
<br/>
  This service was provided by Employee Expense Services - P2P (Purchase to Pay Process), CAD ,Siam Cement Public Co.,Ltd.
  <br/>
Your comments and suggestions are welcome. Please contact us at 
<a href="mailto:myempadm@scg.co.th">myempadm@scg.co.th</a>
<br/>
<br/>
<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/foolter.png" BORDER="0" ALT="SCG Account">
</div>

</body>
</html>