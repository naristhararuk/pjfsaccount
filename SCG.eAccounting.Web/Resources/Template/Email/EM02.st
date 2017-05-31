<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
 <HEAD>
  <TITLE> New Document </TITLE>
  <META NAME="Generator" CONTENT="EditPlus">
  <META NAME="Author" CONTENT="">
  <META NAME="Keywords" CONTENT="">
  <META NAME="Description" CONTENT="">
 </HEAD>

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

 <BODY class="fontA">

<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/head.png" BORDER="0" ALT="SCG Account">
</div>

<br/>
<B>Dear All ,</B>
<br/>
<br/>
This e-mail has been sent to you for your notification that request $DocumentNo$ ของ Khun $RequesterName$
<br/>
has been $WorkFlowStateEventName$ by Khun $ResponseBy$

$if(remark)$
<br/>
<br/>
Because $Reason$ $remark$
<br/>
$else$
<br/>
$endif$

  $if(ta)$
  <TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
  <TR class="HeaderTable">
	<TD align=center>Request ID</TD>
	<TD align=center>Subject</TD>
	<TD align=center>Travel To</TD>
	<TD align=center>From Date</TD>
	<TD align=center>To Date</TD>
  </TR>
  <TR class="ItemTable">
	<TD>$ta.RequestID$</TD>
	<TD>$ta.Subject$</TD>
	<TD>$ta.TravelTo$</TD>
	<TD>$ta.FromDate$</TD>
	<TD>$ta.ToDate$</TD>
  </TR>
  <tr><td height="1px" colspan="5">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</td></tr>
  </TABLE>
	$endif$

<br/>
	$if(advList)$
$advList:{advDoc|
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
$advDoc:{adv|
<TR class="HeaderTable">
	<TD align=center>Request ID</TD>
	<TD align=center>Subject</TD>
	<TD align=center>RequestDateOfAdvance</TD>
	$if(advDoc.AdvanceFrList)$
	<TD align=center></TD>
	$else$
	<TD align=center>Amount  $if(adv.IsRepOffice)$  ($adv.SymbolLocal$) $endif$</TD>
	$endif$
</TR>

<TR class="ItemTable">
	<TD align=center>$adv.RequestID$</TD>
	<TD>$adv.Subject$</TD>
	<TD>$adv.RequestDateOfAdvance$</TD>
	$if(advDoc.AdvanceFrList)$
	<TD align=center></TD>   
	$else$
	<TD align=right>$if(adv.IsRepOffice)$  $adv.advdocLocalCurrencyAmount $  $else$    $ adv.Amount$  $endif$</TD>
	$endif$
</TR>

$if(advDoc.AdvanceFrList)$
$advDoc.AdvanceFrList:{advFr|<TR class="ItemTable">
	<TD></TD>
	<TD></TD>
	<TD>$advFr.Currency$</TD>
	<TD align=right>$advFr.Amount$</TD>
</TR>}$ $endif$ }$

  <tr><td height="1px" colspan="4">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</td></tr>
</TABLE>  }$
	$endif$
<br/>

$if(exp)$
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
<TR class="HeaderTable">
	<TD>Request ID</TD>
	<TD>Subject</TD>
	<TD>Amount  $if(exp.IsRepOffice)$  ($exp.SymbolLocal$) $endif$</TD>
</TR>
<TR class="ItemTable">
	<TD align=center>$exp.RequestID$</TD>
	<TD>$exp.Subject$</TD>
	<TD align=right>$if(exp.IsRepOffice)$ $exp.TotalExpenseLocal$ $else$ $exp.Amount$ $endif$</TD>
</TR>
  <tr><td height="1px" colspan="3">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</td></tr>
</TABLE>
$endif$
<br/>

Please click <b><a href="$link$/Login.aspx?wfid=$wfid$">here</a></b> for more details.
<br/>
<br/>
Yours Sincerely,

<br/>
<br/>
<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/LINE.png" BORDER="0" ALT="SCG Account">
</div>
<br/>

<B>เรียนทุกท่าน</B>
<br/>
<br/>
อีเมล์ฉบับนี้ส่งมาเพื่อแจ้งให้ทราบว่าใบแจ้งเลขที่ $DocumentNo$ ของคุณ$RequesterName$
<br/>
ได้รับการ $WorkFlowStateEventName2$ จากคุณ $ResponseBy$

$if(remark)$
<br/>
<br/>
ด้วยเหตุผล $Reason$ $remark$
<br/>
$else$
<br/>
$endif$

  $if(ta)$
  <TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
  <TR class="HeaderTable">
	<TD align=center>เลขที่เอกสาร</TD>
	<TD align=center>เรื่อง</TD>
	<TD align=center>เดินทางไป</TD>
	<TD align=center>จากวันที่</TD>
	<TD align=center>ถึงวันที่</TD>
  </TR>
  <TR class="ItemTable">
	<TD>$ta.RequestID$</TD>
	<TD>$ta.Subject$</TD>
	<TD>$ta.TravelTo$</TD>
	<TD>$ta.FromDate$</TD>
	<TD>$ta.ToDate$</TD>
  </TR>
    <tr><td height="1px" colspan="5">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</td></tr>
  </TABLE>
	$endif$

<br/>
	$if(advList)$
$advList:{advDoc|
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
$advDoc:{adv|
<TR class="HeaderTable">
	<TD align=center>เลขที่เอกสาร</TD>
	<TD align=center>เรื่อง</TD>
	<TD align=center>วันที่ต้องการเงินทดรองจ่าย</TD>
	$if(advDoc.AdvanceFrList)$
	<TD align=center></TD>
	$else$
	<TD align=center>จำนวนเงิน  $if(adv.IsRepOffice)$  ($adv.SymbolLocal$) $endif$</TD>
	
	$endif$
</TR>


<TR class="ItemTable">
	<TD align=center>$adv.RequestID$</TD>
	<TD>$adv.Subject$</TD>
	<TD>$adv.RequestDateOfAdvance$</TD>
	$if(advDoc.AdvanceFrList)$
	<TD align=center></TD>   
	$else$
	<TD align=right>$if(adv.IsRepOffice)$  $adv.advdocLocalCurrencyAmount $  $else$    $ adv.Amount$  $endif$</TD>
	$endif$
</TR>

$if(advDoc.AdvanceFrList)$
$advDoc.AdvanceFrList:{advFr|<TR class="ItemTable">
	<TD></TD>
	<TD></TD>
	<TD>$advFr.Currency$</TD>
	<TD align=right>$advFr.Amount$</TD>
</TR>}$ $endif$ }$
  <tr><td height="1px" colspan="4">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</td></tr>
</TABLE>  }$
	$endif$
<br/>

$if(exp)$
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
<TR class="HeaderTable">
	<TD>เลขที่เอกสาร</TD>
	<TD>เรื่อง</TD>
	<TD>จำนวนเงิน  $if(exp.IsRepOffice)$  ($exp.SymbolLocal$) $endif$</TD>
</TR>
<TR class="ItemTable">
	<TD align=center>$exp.RequestID$</TD>
	<TD>$exp.Subject$</TD>
	<TD align=right>$if(exp.IsRepOffice)$ $exp.TotalExpenseLocal$ $else$ $exp.Amount$ $endif$</TD>
</TR>

  <tr><td height="1px" colspan="4">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</td></tr>
</TABLE>
$endif$
<br/>

โปรดคลิกลิ้งค์ <b><a href="$link$/Login.aspx?wfid=$wfid$">ที่นี้</a></b> เพื่อดูรายละเอียด
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

 </BODY>
</HTML>
