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
<B>Dear Khun $DearName$</B>
<br/>
<br/>

<table border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px"  class="fontA">
<tr class="ItemTable">
	<td align="center">
		<a href="$ApproveURL$">$ApproveEventDisplayName$</a> 
	</td>
	<td align="center">
		<a href="$RejectURL$">$RejectEventDisplayName$</a>
	</td>
</tr>
<tr><td height="1px" colspan="2">
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
</table>
<br/>
  $if(ta)$
  Please $if(IsApproveReject)$ approve $else$ kindly renew $endif$  the travelling authorization No. $RequestId$ of Khun $RequesterName$ as details below :
  <br/>
<br/>
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
	<tr><td height="0px" colspan="5">
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

  $if(mpa)$
  Please $if(IsApproveReject)$ approve $else$ kindly renew $endif$ the mobile phone authorization No. $RequestId$ of Khun $RequesterName$ as details below :
  <br/>
<br/>
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
	<TR class="HeaderTable">
		<TD align=center>Request ID</TD>
		<TD align=center>Subject</TD>
		<TD align=center>Effective DateFrom</TD>
		<TD align=center>Effective DateTo</TD>
		
	</TR>
	<TR class="ItemTable">
		<TD>$mpa.RequestID$</TD>
		<TD>$mpa.Subject$</TD>
		<TD>$mpa.FromDate$</TD>
		<TD>$mpa.ToDate$</TD>
	</TR>
	<tr><td height="0px" colspan="5">
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

  $if(ca)$
  Please $if(IsApproveReject)$ approve $else$ kindly renew $endif$ the car authorization No. $RequestId$ of Khun $RequesterName$ as details below :
  <br/>
<br/>
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
	<TR class="HeaderTable">
		<TD align=center>Request ID</TD>
		<TD align=center>Subject</TD>
		<TD align=center>Effective DateFrom</TD>
		<TD align=center>Effective DateTo</TD>
		
	</TR>
	<TR class="ItemTable">
		<TD>$ca.RequestID$</TD>
		<TD>$ca.Subject$</TD>
		<TD>$ca.FromDate$</TD>
		<TD>$ca.ToDate$</TD>
	</TR>
	<tr><td height="0px" colspan="5">
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
  Please $if(IsApproveReject)$ approve $else$ renew $endif$ the advance request No. $RequestId$ of Khun $RequesterName$ as details below :
  <br/>
<br/>
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
	<TD align=right>$if(adv.IsRepOffice)$ $adv.advdocLocalCurrencyAmount$ $else$ $adv.Amount$ $endif$</TD>
	$endif$
</TR>

$if(advDoc.AdvanceFrList)$
$advDoc.AdvanceFrList:{advFr|
<TR class="ItemTable">
	<TD></TD>
	<TD></TD>
	<TD>$advFr.Currency$</TD>
	<TD align=right>$advFr.Amount$</TD>
</TR>}$ $endif$ }$


<tr><td height="0px" colspan="4">
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
  Please $if(IsApproveReject)$ approve $else$ renew $endif$ the expense reimbursement No. $RequestId$ of Khun $RequesterName$ as details below :
  <br/>
<br/>
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
<TR class="HeaderTable">
	<TD>Request ID</TD>
	<TD>Subject</TD>
    <TD align=center>Amount  $if(exp.IsRepOffice)$  ($exp.SymbolLocal$) $endif$</TD>
</TR>
<TR class="ItemTable">
	<TD>$exp.RequestID$</TD>
	<TD>$exp.Subject$</TD>
	<TD align=right>$if(exp.IsRepOffice)$ $exp.TotalExpenseLocal$ $else$ $exp.Amount$ $endif$</TD>
</TR>
<tr><td height="0px" colspan="3">
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

<b><a href="#_expDetail">initiator and invoice detail</a></b>
<br/>
$endif$
<br/>


$if(fixedAdvanceDataList)$
 Please $if(IsApproveReject)$ approve $else$ renew $endif$ the Fixed Advance Request No. $fixedAdvanceDataList.DocNo$ of Khun $fixedAdvanceDataList.RequesterName$ as details below :
<br/>
<br/>
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
<TR class="HeaderTable">
	<TD>Request Type</TD>
	<TD>Subject</TD>
    <TD>Effective date from – Effective date to</TD>
	<TD>Objective</TD>
	<TD>RequestDateOfAdvance</TD>
	<TD>Amount</TD>
</TR>
<TR class="ItemTable">
	<TD>$fixedAdvanceDataList.FixedAdvanceType$</TD>
	<TD>$fixedAdvanceDataList.Subject$</TD>
	<TD>$fixedAdvanceDataList.EffectiveFromDate$ - $fixedAdvanceDataList.EffectiveToDate$</TD>
	<TD>$fixedAdvanceDataList.Objective$</TD>
	<TD>$fixedAdvanceDataList.RequestDate$</TD>
	<TD>$fixedAdvanceDataList.Amount$</TD>
</TR>
<tr><td height="0px" colspan="3">
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

<br/>
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

<B>เรียนคุณ $DearName$</B>
<br/>
<br/>
  $if(ta)$
  โปรด$if(IsApproveReject)$อนุมัติ$else$ตรวจสอบ$endif$รายการขออนุมัติเดินทางเลขที่ $RequestId$ จากคุณ $RequesterName$ ดังนี้
  <br/>
<br/>
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
	<TR class="HeaderTable">
		<TD align=center>เลขที่เอกสาร</TD>
		<TD align=center>เรื่อง</TD>
		<TD align=center>เดินทางไปที่</TD>
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
	<tr><td height="0px" colspan="5">
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

  $if(mpa)$
  โปรด$if(IsApproveReject)$อนุมัติ$else$ตรวจสอบ$endif$รายการขออนุมัติใช้โทรศัพท์มือถือเลขที่ $RequestId$ จากคุณ $RequesterName$ ดังนี้
  <br/>
<br/>
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
	<TR class="HeaderTable">
		<TD align=center>เลขที่เอกสาร</TD>
		<TD align=center>เรื่อง</TD>
		<TD align=center>จากวันที่</TD>
		<TD align=center>ถึงวันที่</TD>
	</TR>
	<TR class="ItemTable">
		<TD>$mpa.RequestID$</TD>
		<TD>$mpa.Subject$</TD>
		<TD>$mpa.FromDate$</TD>
		<TD>$mpa.ToDate$</TD>
	</TR>
	<tr><td height="0px" colspan="5">
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

  $if(ca)$
  โปรด$if(IsApproveReject)$อนุมัติ$else$ตรวจสอบ$endif$รายการขออนุมัติใช้รถเลขที่  $RequestId$ จากคุณ $RequesterName$ ดังนี้
  <br/>
<br/>
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
	<TR class="HeaderTable">
		<TD align=center>เลขที่เอกสาร</TD>
		<TD align=center>เรื่อง</TD>
		<TD align=center>จากวันที่</TD>
		<TD align=center>ถึงวันที่</TD>
	</TR>
	<TR class="ItemTable">
		<TD>$ca.RequestID$</TD>
		<TD>$ca.Subject$</TD>
		<TD>$ca.FromDate$</TD>
		<TD>$ca.ToDate$</TD>
	</TR>
	<tr><td height="0px" colspan="5">
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
  โปรด$if(IsApproveReject)$อนุมัติ$else$ตรวจสอบ$endif$รายการเบิกเงินทดรองจ่ายเลขที่ $RequestId$ จากคุณ $RequesterName$ ดังนี้
  <br/>
<br/>
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
$advDoc.AdvanceFrList:{advFr|
<TR class="ItemTable">
	<TD></TD>
	<TD></TD>
	<TD>$advFr.Currency$</TD>
	<TD align=right>$advFr.Amount$</TD>
</TR>}$ $endif$ }$


<tr><td height="0px" colspan="4">
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
  โปรด$if(IsApproveReject)$อนุมัติ$else$ตรวจสอบ$endif$รายการเบิกค่าใช่จ่ายเลขที่ $RequestId$ จากคุณ $RequesterName$ ดังนี้
  <br/>
<br/>
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
<TR class="HeaderTable">
	<TD>เลขที่เอกสาร</TD>
	<TD>เรื่อง</TD>
	<TD>จำนวนเงิน  $if(adv.IsRepOffice)$  ($exp.SymbolLocal$) $endif$</TD>
</TR>
<TR class="ItemTable">
	<TD align=center>$exp.RequestID$</TD>
	<TD>$exp.Subject$</TD>
	<TD align=right>$if(exp.IsRepOffice)$ $exp.TotalExpenseLocal$ $else$ $exp.Amount$ $endif$</TD>
</TR>
<tr><td height="0px" colspan="3">
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

<b><a href="#_expDetail">initiator and invoice detail</a></b>
<br/>
$endif$

<br/>

$if(fixedAdvanceDataList)$
  โปรด$if(IsApproveReject)$อนุมัติ$else$ตรวจสอบ$endif$รายการทดรองจ่ายระยะยาวตามใบแจ้งเลขที่ $fixedAdvanceDataList.DocNo$ จากคุณ $fixedAdvanceDataList.RequesterName$ มีรายละเอียดดังนี้
<br/>
<br/>
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
<TR class="HeaderTable">
	<TD>ประเภทการเบิก</TD>
	<TD>เรื่อง</TD>
    <TD>วันที่ใช้เงินได้ – วันสิ้นสุดการใช้เงิน</TD>
	<TD>วัตถุประสงค์</TD>
	<TD>วันที่ต้องการเงินทดรองจ่าย</TD>
	<TD>จำนวนเงิน (บาท) </TD>
</TR>
<TR class="ItemTable">
	<TD>$fixedAdvanceDataList.FixedAdvanceType$</TD>
	<TD>$fixedAdvanceDataList.Subject$</TD>
	<TD>$fixedAdvanceDataList.EffectiveFromDate$ - $fixedAdvanceDataList.EffectiveToDate$</TD>
	<TD>$fixedAdvanceDataList.Objective$</TD>
	<TD>$fixedAdvanceDataList.RequestDate$</TD>
	<TD>$fixedAdvanceDataList.Amount$</TD>
</TR>
<tr><td height="0px" colspan="3">
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

<br/>
$endif$


<br/>
โปรดคลิก<b><a href="$link$/Login.aspx?wfid=$wfid$">ลิ้งค์ </a></b> เพื่อดูรายละเอียด
<br/>
<br/>
ขอแสดงความนับถือ
<br/>
<br/>

$if(exp)$
<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/LINE.png" BORDER="0" ALT="SCG Account">
</div>
<br/>
<a name="_expDetail">

$if(exp.InitiatorList)$
<p>Initial : </p>
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="60%" class="fontA">
<TR class="HeaderTable">
	<TD>No.</TD>
	<TD>Initiator</TD>
    <TD>E-mail Address</TD>
	<TD>Initial Type</TD>
	<TD>SMS</TD>
</TR>
$exp.InitiatorList:{initiator|
<TR class="ItemTable">
	<TD align=center>$initiator.InitiatorSeq$</TD>
	<TD>$initiator.InitiatorName$</TD>
	<TD>$initiator.InitiatorEmail$</TD>
	<TD align=center>$initiator.InitialType$</TD>
	<TD align=center>$if(initiator.IsSMS)$ X $else$ &nbsp; $endif$</TD>
</TR>}$ 
</TABLE>
<br/>
$endif$

$if(exp.Invoices)$
<p>Expense :</p>
<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="70%" class="fontA">
	<TR class="HeaderTable">
		<TD>Item</TD>
		<TD>Invoice No</TD>
		<TD>Invoice Date</TD>
		<TD>Vendor</TD>
		<TD>Base Amount</TD>
		<TD>VAT Amount</TD>
		<TD>WHT Amount</TD>
		<TD>Net Amount</TD>
	</TR>
	$exp.Invoices:{inv|
	<TR class="ItemTable">
		<TD align=center>$inv.ItemNo$</TD>
		<TD align=center>$inv.InvoiceNo$</TD>
		<TD align=center>$inv.InvoiceDate$</TD>
		<TD>$inv.Vendor$</TD>
		<TD align=right>$inv.BaseAmount$</TD>
		<TD align=right>$inv.VatAmount$</TD>
		<TD align=right>$inv.WHTAmount$</TD>
		<TD align=right>$inv.NetAmount$</TD>
	</TR>
	$if(inv.InvoiceItems)$
	<TR class="ItemTable">
		<TD bgcolor="White"></TD>
		<TD colspan="7" cellspacing="0" cellpadding="0">
			<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="100%" class="fontA">
				<TR class="HeaderTable">
					<TD>Cost Center</TD>
					<TD>Expense Code</TD>
					<TD>Internal Order</TD>
					<TD>Description</TD>
					$if(exp.IsForeign)$
					<TD>Currency</TD>
					<TD>Amount</TD>
					<TD>Exchange Rate</TD>
					$endif$
					<TD>Amount $if(exp.IsRepOffice)$ ($exp.SymbolLocal$) $else$ (THB) $endif$</TD>
					<TD>Ref No.</TD>
				</TR>
				$inv.InvoiceItems:{invItem|
				<TR class="ItemTable">
					<TD align=center>$invItem.CostCenter$</TD>
					<TD>$invItem.ExpenseEN$</TD>
					<TD align=center>$invItem.InternalOrder$</TD>
					<TD>$invItem.Description$</TD>
					$if(exp.IsForeign)$
					<TD align=center>$invItem.Currency$</TD>
					<TD align=right>$invItem.AmountCurrency$</TD>
					<TD align=right>$invItem.ExchangeRate$</TD>
					$endif$
					<TD align=right>$if(exp.IsRepOffice)$ $invItem.AmountFinalCurrency$ $else$ $invItem.AmountTHB$ $endif$</TD>
					<TD>$invItem.ReferenceNo$</TD>
				</TR>
				}$
			</TABLE>
		</TD>
	</TR>
	$endif$
	}$ 
</TABLE>
$endif$

</a>
<br/>
$endif$

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
