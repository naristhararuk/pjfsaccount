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
<IMG SRC="$link$/App_Themes/Default/images/email/head.png" BORDER="0" ALT="SCG Account">
</div>

<p> <B>Dear Khun $userName$,</B> </p>

<p>This e-mail has been sent to notify that</p>
	<table border="0" bordercolor="White" cellspacing="0" cellpadding="0" width="320px">
	<tr>
		<td><B>Your user ID is</B><td>
		<td style="width:3%;"></td>
		<td>$userID$<td>
	</tr>
	<tr align="Left">
		<td><B>Automatically new password is </B><td>
		<td></td>
		<td>$password$<td>
	</tr>
	</table>

<br/>
Please click <b><a href="$link$/Login.aspx">here</a></b> to change your password.
<br/>
<br/>
Yours Sincerely,

<br/>
<br/>
<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/LINE.png" BORDER="0" ALT="SCG Account">
</div>
<br/>

<p> <B>เรียน คุณ$userName$</B> </p>

<p>อีเมล์ฉบับนี้ส่งมาเพื่อแจ้งรหัสผ่านที่ระบบฯ สร้างให้อัตโนมัติ คือ</p>
	<table border="0" bordercolor="White" cellspacing="0" cellpadding="0" width="320px">
	<tr>
		<td><B>รหัสผู้ใช้งานของคุณ คือ</B><td>
		<td>$userID$<td>
	</tr>
	<tr align="Left">
		<td><B>รหัสผ่านที่ระบบฯ สร้างให้อัตโนมัติ คือ</B><td>
		<td>$password$<td>
	</tr>
	</table>

<br/>
โปรดคลิกลิ้งค์ <b><a href="$link$/Login.aspx">ที่นี้</a></b> เพื่อทำการเปลี่ยนรหัสผ่าน
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