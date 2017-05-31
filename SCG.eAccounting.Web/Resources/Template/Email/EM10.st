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

 <BODY class="fontA">

<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/head.png" WIDTH="724" HEIGHT="124" BORDER="0" ALT="">
</div>

    <p> <B>Dear $requesterName$,</B> </p>
    <p> Your advance from the advance form,$advanceID$,is overdue for clearing expense <font color='red'>$numberday$</font> days. </p>
	<p> Please contact SCG Account for clearing. </p>
	<br/>
    <p> Please clear your advance within <font color='red'>$clearDate$</font> </p>
    <p> $remark$ </p>
	<br/>
	<p>$remarkAutoEN$</p>
    <p>Please click <b><a href="$link$/Login.aspx?wfid=$wfid$">here</a></b> to more detail.</p>
	<br/>
    <p> Thank you.</p>
    <div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/LINE.png" BORDER="0" ALT="SCG Account">
</div>
<br/>
<br/>
<p> <B>เรียนคุณ $requesterName$</B> </p>
<p> อีเมล์ฉบับนี้ส่งมาเพื่อแจ้งว่า</p>
<p><p> ใบขอเบิกเงินทดรองจ่ายเลขที่ : $advanceID$ ของท่าน เลยกำหนด เบิกค่าใช้จ่ายมาแล้ว  <font color='red'>$numberday$</font> วัน</p></p>
<p> กรุณาติดต่อ หน่วยงานบัญชี สำหรับการคืนเงินทดรองจ่าย หรือ ส่งเอกสารเบิกค่าใช้จ่าย</p>
<p> ทั้งนี้ กรุณาเคลียร์เงินทดรองจ่ายภายใน วันที่ <font color='red'>$clearDate$</font> </p>
<p> $remark$ </p>
<br/>
<p>$remarkAutoTH$</p>
<p> โปรดคลิกลิ้งค์ <b><a href="$link$/Login.aspx?wfid=$wfid$">ที่นี้</a></b> เพื่อดูรายละเอียด</p>
<br/><br/>
<p> ขอแสดงความนับถือ </p>
<p> Administrator e-Xpense</p>


<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/foolter.png" WIDTH="724" HEIGHT="62" BORDER="0" ALT="">
</div>

</body>
</html>
