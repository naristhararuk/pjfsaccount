function entsub(myform) {
    if (window.event && window.event.keyCode == 13) {
        myform.submit();
    }
    else {
        return true;
    }
}