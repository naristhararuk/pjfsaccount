var hour_count = 0;
var min_count = 0;
var sec_count = 0;
var server_hour;
var server_min;
var server_sec;
function dateTime(hour, min, sec, day, mon) {
    var month;
    var year = window.document.getElementById('ctl00_ctl_Server_Year_Lable').getAttribute("innerHTML").toString();
    var now = new Date();
    var result = day;

    if (window.document.getElementById('ctl00_ctl_DefaultCulture_Label').getAttribute("innerHTML").toString() == "th-TH") {
        month = new Array("ม.ก.", "ก.พ.", "มี.ค.", "เม.ย.", "พ.ค.", "มิ.ย.",
	"ก.ค.", "ส.ค.", "ก.ย.", "ต.ค.", "พ.ย.", "ธ.ค.");
        result += " " + month[mon - 1] + " ";
    } else {

        month = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun",
	"Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
        result += "-" + month[mon - 1] + "-";
    }


    result += year;
    result += "   " + hour + ":";

    if (min < 10) {
        result += "0" + min + ":";
    }
    else {
        result += min + ":";
    }

    if (sec < 10) {
        result += "0" + sec;
    }
    else {
        result += sec;
    }

    return result;
}

function timeupdate(hour, min, sec, day, month) {
    sec++;
    if (sec >= 60) {
        sec = 0;
        min++;
    }
    if (min >= 60) {
        min = 0;
        hour++;
    }
    if (hour >= 24) {
        hour = 0;
    }

    window.document.getElementById('ctl00_ctl_DisplayTime_Label').setAttribute("innerHTML", dateTime(hour, min, sec, day, month));  //;
    setTimeout("timeupdate(" + hour + "," + min + "," + sec + "," + day + "," + month + ")", 1000);
}