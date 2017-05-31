<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
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

<br/>

<B>Dear Khun $userName$,</B><br /><br />

<table width="724px" bordercolor="White" cellspacing="0" cellpadding="0" class="fontA">
<tr class="HeaderTable">
	<td colspan="3">Welcome to e-Xpense.</td>
</tr>
<tr class="ItemTable">
	<td style="width:5%align:left"></td>
	<td>
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    Siam Cement Public Co.,Ltd (Corporate Account) would like to welcome you to our new web based employee expense system version 2.0.   In this new version, the system will be more convenience to use. Our system will serve in English and Thai Language version.<br><br>
$if(isNotAdUser)$		
<font color='red'>	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; *** <u><b>suggestion for password changing</b></u> ***
		<br>"In order to support and maintain the efficiency of e-Xpense system, users are not necessary to immediately change password when receive this e-mail. Password can be changed later but not excess 90 days from the date of this e-mail"
</font>
$endif$ 
	</td>
	<td style="width:5%"></td>
</tr>
</table>

<br />
The following is your login information.
<br/>
<table border="0" bordercolor="White" cellspacing="0" cellpadding="0" width="200px">
	<tr>
		<td><B>User ID : </B><td>
		<td>$userID$<td>
	</tr>
	<tr>
		<td><B>Password : </B><td>
		<td>$passwordEN$<td>
	</tr>
</table>

<br />
Please click <b><a href="$link$">here</a></b> $if(isNotAdUser)$ to change your password.$endif$
<br/>
<br/>
Yours Sincerely,

<br/>
<br/>
<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/LINE.png" BORDER="0" ALT="SCG Account">
</div>

<B>เรียน คุณ$userName$</B><br /><br />

<table width="724px" bordercolor="White" cellspacing="0" cellpadding="0" class="fontA">
<tr class="HeaderTable">
	<td colspan="3">ยินดีต้อนรับเข้าสู่ระบบ e-Xpense</td>
</tr>
<tr class="ItemTable">
	<td style="width:5%align:left"></td>
	<td>
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		บริษัทปูนซิเมนต์ไทย จำกัด (มหาชน)(Corporate Account) ยินดีต้อนรับเข้าสู่ระบบการเบิกเงินทดรองจ่ายและค่าใช้จ่ายพนักงาน ซึ่งได้ออกแบบการใช้งานใหม่ให้อยู่บนรูปแบบเว็บเพ็จ      ปัจจุบันได้มีการปรับปรุงการให้บริการเป็นเวอร์ชั่น  2.0 เพื่อให้เกิดประสิทธิภาพและอำนวยความสะดวกในการใช้งานโดยระบบฯ จะรองรับการใช้งานแบบ 2 ภาษา คือ ภาษาไทยและภาษาอังกฤษ<br><br>
$if(isNotAdUser)$		
	<font color='red'>	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; *** <u><b>ข้อแนะนำการเปลี่ยนแปลงรหัสผ่าน</b></u> ***
		<br>"ผู้ใช้งานไม่จำเป็นต้องทำการเปลี่ยนแปลงรหัสผ่านทันทีเมื่อได้รับอีเมล์นี้ โดยผู้ใช้งานยังสามารถทำการเปลี่ยนรหัสผ่านในภายหลังได้ แต่ต้องไม่เกิน 90 วันนับจากวันที่ได้รับอีเมล์นี้ ทั้งนี้ เพื่อให้ระบบฯ สามารถใช้งานได้อย่างมีประสิทธิภาพ"
	</font>
$endif$ 
	</td>
	<td style="width:5%"></td>
</tr>
</table>

<br />
ข้อมูลการใช้งานเพื่อเข้าสู่ระบบ คือ
<br/>
<table border="0" bordercolor="White" cellspacing="0" cellpadding="0" width="200px">
	<tr>
		<td><B>รหัสผู้ใช้งาน : </B><td>
		<td>$userID$<td>
	</tr>
	<tr>
		<td><B>รหัสผ่าน : </B><td>
		<td>$passwordTH$<td>
	</tr>
</table>

<br />
โปรดคลิกลิ้งค์ <b><a href="$link$/Login.aspx">ที่นี้</a></b> $if(isNotAdUser)$ เพื่อทำการเปลี่ยนแปลงรหัสผ่าน$endif$
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