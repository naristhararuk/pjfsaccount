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

<p> <B>Dear Khun $receiverName$ </B></p>

This e-mail has been sent to notify that
<br/>
your payment for the request: <b>$requestID$</b> is ready.
<br/>
Please print your approved form, and contact at $pbName$.
<br/>
<br/>
    
<b>Payment detail : </b>
$subject$
<br/>
$if(createDate)$ 
	<b>Payment date</b> : $createDate$ $endif$
<br/>
$if(netAmount)$ 
	<b>Amount $if(SymbolLocal)$ ($SymbolLocal$) $endif$ </b> : $netAmount$ $endif$

<br/>
<br/>

$if(advItemList)$
	<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
		<TR class="HeaderTable">
			<TD>Payment Type</TD>
			<TD>Currency</TD>
			<TD>Amount</TD>
		</TR>
		$advItemList:{adv|<TR class="ItemTable">
			<TD>$adv.PaymentType$</TD>
			<TD>$adv.Currency$</TD>
			<TD align=right>$adv.Amount$</TD>
		</TR>}$
	</TABLE>
$endif$

$if(isCheque)$
	<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
		<TR class="HeaderTable">
			<TD>Cheque ID</TD>
			<TD>Bank </TD>
			<TD>Currency</TD>
			<TD>Amount</TD>
		 </TR>
		 <TR class="ItemTable">
			<TD>$chequeNumber$</TD>
			<TD>$chequeBankName$</TD>
			<TD>$currencyDoc$</TD>
			<TD align=right>$amount$</TD>
		</TR>
	</TABLE>
$endif$

<br/>
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

<p> <B>เรียนคุณ $receiverName$ </B></p>

อีเมล์ฉบับนี้ส่งมาเพื่อแจ้งว่า
<br/>
ใบขอเบิกเงินเลขที่ : <b>$requestID$</b> ได้ดำเนินการจ่ายเงินเรียบร้อยแล้ว. 
<br/>
โปรดพิมพ์ใบขอเบิกเงินที่ได้รับการอนุมัติแล้ว  เพื่อติดต่อรับเงินสดที่ $pbName$.
<br/>
<br/>
    
<b>รายละเอียดการจ่ายเงิน :</b>
$subject$
<br/>
$if(createDate)$ 
	<b>วันที่จ่ายเงิน</b> : $createDate$ $endif$
<br/>
$if(netAmount)$ 
	<b>จำนวนเงิน  $if(SymbolLocal)$ ($SymbolLocal$) $endif$ </b> : $netAmount$ $endif$
 
<br/>
<br/>

$if(advItemList)$
	<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
		<TR class="HeaderTable">
			<TD>ประเภทการชำระ</TD>
			<TD>สกุลเงิน</TD>
			<TD>จำนวนเงิน</TD>
		</TR>
		$advItemList:{adv|<TR class="ItemTable">
			<TD>$adv.PaymentType$</TD>
			<TD>$adv.Currency$</TD>
			<TD align=right>$adv.Amount$</TD>
		</TR>}$
	</TABLE>
$endif$

$if(isCheque)$
	<TABLE border="1" bordercolor="White" cellspacing="0" cellpadding="0" width="724px" class="fontA">
		<TR class="HeaderTable">
			<TD>เลขที่เช็ค</TD>
			<TD>ธนาคาร</TD>
			<TD>สกุลเงิน</TD>
			<TD>จำนวนเงิน</TD>
		 </TR>
		 <TR class="ItemTable">
			<TD>$chequeNumber$</TD>
			<TD>$chequeBankName$</TD>
			<TD>$currencyDoc$</TD>
			<TD align=right>$amount$</TD>
		</TR>
	</TABLE>
$endif$

<br/>
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

</body>
</html>