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

<br/>
<B>Dear Khun $sendToUserName$ ,</B>
<br/>
<br/>

$if(N)$

This remind e-mail is being sent as it has been 90 days since the last password change
<br/>
for e-Xpense system as SCG IT Policy.
<br/>
<br/>
If you do not change  your password within the next  <font color='red'>$N$</font> day(s), 
<br/>
Your password will expire and will be unable to access e-Xpense system.
<br/>
<br/>
To change  your password, please use your web browser and go to e-Xpense 
<A HREF="http://exp.scg.co.th" target="blank">http://exp.scg.co.th</A>.
<br/>
Please select  <b>change password</B><br/><br/>
If you have any questions, please contact  02-586-5832 Or 02-586-2963 or e-Mail myempadm@scg.co.th

$else$ 
Your password was expired.<br/><br/>
If you have any questions, please contact  02-586-5832 Or 02-586-2963 or e-Mail myempadm@scg.co.th

$endif$
 
<br/>
<br/>
Yours Sincerely,<br>
e-Xpense Administrator
 <br/>
<br/>
<div aling="LEFT">
<IMG SRC="$link$/App_Themes/Default/images/email/LINE.png" BORDER="0" ALT="SCG Account">
</div>
<br/>

<B>เรียนคุณ $sendToUserName$ </B>
<br/>
<br/>

$if(N)$

อีเมล์ฉบับนี้ส่งมาเพื่อแจ้งเตือนท่านให้ทำการเปลี่ยนรหัสผ่านที่กำลังจะหมดอายุในระบบ e-Xpense
<br/>
โดยนับ 90 วันนับหลังจากการเปลี่ยนรหัสผ่านครั้งสุดท้ายตามนโยบาย SCG IT
<br/>
<br/>
หากไม่เปลี่ยนรหัสผ่านภายใน   <font color='red'>$N$</font> วัน
<br/>
รหัสผ่านของท่านจะไม่สามารถเข้าใช้งานระบบ e-Xpense ได้
<br/>
<br/>
การเปลี่ยนรหัสผ่านสามารถทำได้โดยใช้เว็บเบราเซอร์เข้าสู่ระบบฯ ที่  
<A HREF="http://exp.scg.co.th" target="blank">http://exp.scg.co.th</A>
<br/>
แล้วทำการเลือก <b>change password</B><br/><br/>
หากท่านมีข้อสงสัยเพิ่มเติม สามารถติดต่อ ที่เบอร์โทรศัพท์ 02-586-5832 หรือ 02-586-2963 หรือ ส่งอีเมล์มาที่ myempadm@scg.co.th 
$else$ 
รหัสผ่านของคุณหมดอายุแล้ว<br/><br/>
หากท่านมีข้อสงสัยเพิ่มเติม สามารถติดต่อ ที่เบอร์โทรศัพท์ 02-586-5832 หรือ 02-586-2963 หรือ ส่งอีเมล์มาที่ myempadm@scg.co.th 

$endif$
 
<br/>
<br/>
ขอแสดงความนับถือ<br>
e-Xpense Administrator
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