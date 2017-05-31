var wijmo;define(["jquery"],function(){(function(e){(function(e){function n(e,t){var n=Date.prototype.toJSON;Date.prototype.toJSON=i;try{return JSON.stringify(e,t)}finally{Date.prototype.toJSON=n}}function r(e,t){var n=e.toString();while(n.length<t)n="0"+n;return n}function i(){var e=this;return r(e.getFullYear(),4)+"-"+r(e.getMonth()+1,2)+"-"+r(e.getDate(),2)+"T"+r(e.getHours(),2)+":"+r(e.getMinutes(),2)+":"+r(e.getSeconds(),2)+"."+r(e.getMilliseconds(),3)}function s(e,t){var n=e.sender,r=e.contentType,i=e.serviceUrl;n?n(t,e):r=="application/json"?c(t,i,e):o(t,i)}function o(e,n){var r='<input type="hidden" name="type" value="application/json"/>';r+='<input type="hidden" name="data" value="'+l(e)+'" />';var i=t("<iframe style='display: none' src='about:blank'></iframe>").appendTo("body");i.ready(function(){var e=u(i);e.write("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'></head><body><form method='Post' accept-charset='utf-8' action='"+n+"'>"+r+"</form>dummy windows for postback</body></html>");var s=t(e).find("form");s.submit()})}function u(e){var t=e[0].contentWindow||e[0].contentDocument;return t.document&&(t=t.document),t}function l(e){return e.replace(a,function(e){return"&"+f[e]})}function c(e,t,n){var r=new XMLHttpRequest;r.open("post",t),r.send(e),r.responseType="blob",r.onreadystatechange=function(){r.response&&n.receiver&&n.receiver(r.response,n)}}var t=jQuery;e.toJSON=n,e.exportFile=s;var a=/[<>&\r\n"']/gm,f={"<":"lt;",">":"gt;","&":"amp;","\r":"#13;","\n":"#10;",'"':"quot;","'":"apos;"};(function(e){e[e.Xls=0]="Xls",e[e.Xlsx=1]="Xlsx",e[e.Csv=2]="Csv",e[e.Pdf=3]="Pdf",e[e.Png=4]="Png",e[e.Jpg=5]="Jpg",e[e.Bmp=6]="Bmp",e[e.Gif=7]="Gif",e[e.Tiff=8]="Tiff"})(e.ExportFileType||(e.ExportFileType={}));var h=e.ExportFileType;(function(e){e[e.Content=0]="Content",e[e.Options=1]="Options"})(e.ExportMethod||(e.ExportMethod={}));var p=e.ExportMethod;(function(e){e[e.Standard=0]="Standard",e[e.TrueType=1]="TrueType",e[e.Embedded=2]="Embedded"})(e.FontType||(e.FontType={}));var d=e.FontType;(function(e){e[e.Low=0]="Low",e[e.Medium=1]="Medium",e[e.Default=2]="Default",e[e.High=3]="High"})(e.ImageQuality||(e.ImageQuality={}));var v=e.ImageQuality;(function(e){e[e.Default=-1]="Default",e[e.None=0]="None",e[e.BestSpeed=1]="BestSpeed",e[e.BestCompression=9]="BestCompression"})(e.CompressionType||(e.CompressionType={}));var m=e.CompressionType;(function(e){e[e.NotPermit=0]="NotPermit",e[e.Standard40=2]="Standard40",e[e.Standard128=3]="Standard128",e[e.Aes128=4]="Aes128"})(e.PdfEncryptionType||(e.PdfEncryptionType={}));var g=e.PdfEncryptionType;(function(e){e[e.Custom=0]="Custom",e[e.Letter=1]="Letter",e[e.LetterSmall=2]="LetterSmall",e[e.Tabloid=3]="Tabloid",e[e.Ledger=4]="Ledger",e[e.Legal=5]="Legal",e[e.Statement=6]="Statement",e[e.Executive=7]="Executive",e[e.A3=8]="A3",e[e.A4=9]="A4",e[e.A4Small=10]="A4Small",e[e.A5=11]="A5",e[e.B4=12]="B4",e[e.B5=13]="B5",e[e.Folio=14]="Folio",e[e.Quarto=15]="Quarto",e[e.Standard10x14=16]="Standard10x14",e[e.Standard11x17=17]="Standard11x17",e[e.Note=18]="Note",e[e.Number9Envelope=19]="Number9Envelope",e[e.Number10Envelope=20]="Number10Envelope",e[e.Number11Envelope=21]="Number11Envelope",e[e.Number12Envelope=22]="Number12Envelope",e[e.Number14Envelope=23]="Number14Envelope",e[e.CSheet=24]="CSheet",e[e.DSheet=25]="DSheet",e[e.ESheet=26]="ESheet",e[e.DLEnvelope=27]="DLEnvelope",e[e.C5Envelope=28]="C5Envelope",e[e.C3Envelope=29]="C3Envelope",e[e.C4Envelope=30]="C4Envelope",e[e.C6Envelope=31]="C6Envelope",e[e.C65Envelope=32]="C65Envelope",e[e.B4Envelope=33]="B4Envelope",e[e.B5Envelope=34]="B5Envelope",e[e.B6Envelope=35]="B6Envelope",e[e.ItalyEnvelope=36]="ItalyEnvelope",e[e.MonarchEnvelope=37]="MonarchEnvelope",e[e.PersonalEnvelope=38]="PersonalEnvelope",e[e.USStandardFanfold=39]="USStandardFanfold",e[e.GermanStandardFanfold=40]="GermanStandardFanfold",e[e.GermanLegalFanfold=41]="GermanLegalFanfold",e[e.IsoB4=42]="IsoB4",e[e.JapanesePostcard=43]="JapanesePostcard",e[e.Standard9x11=44]="Standard9x11",e[e.Standard10x11=45]="Standard10x11",e[e.Standard15x11=46]="Standard15x11",e[e.InviteEnvelope=47]="InviteEnvelope",e[e.LetterExtra=50]="LetterExtra",e[e.LegalExtra=51]="LegalExtra",e[e.TabloidExtra=52]="TabloidExtra",e[e.A4Extra=53]="A4Extra",e[e.LetterTransverse=54]="LetterTransverse",e[e.A4Transverse=55]="A4Transverse",e[e.LetterExtraTransverse=56]="LetterExtraTransverse",e[e.APlus=57]="APlus",e[e.BPlus=58]="BPlus",e[e.LetterPlus=59]="LetterPlus",e[e.A4Plus=60]="A4Plus",e[e.A5Transverse=61]="A5Transverse",e[e.B5Transverse=62]="B5Transverse",e[e.A3Extra=63]="A3Extra",e[e.A5Extra=64]="A5Extra",e[e.B5Extra=65]="B5Extra",e[e.A2=66]="A2",e[e.A3Transverse=67]="A3Transverse",e[e.A3ExtraTransverse=68]="A3ExtraTransverse",e[e.JapaneseDoublePostcard=69]="JapaneseDoublePostcard",e[e.A6=70]="A6",e[e.JapaneseEnvelopeKakuNumber2=71]="JapaneseEnvelopeKakuNumber2",e[e.JapaneseEnvelopeKakuNumber3=72]="JapaneseEnvelopeKakuNumber3",e[e.JapaneseEnvelopeChouNumber3=73]="JapaneseEnvelopeChouNumber3",e[e.JapaneseEnvelopeChouNumber4=74]="JapaneseEnvelopeChouNumber4",e[e.LetterRotated=75]="LetterRotated",e[e.A3Rotated=76]="A3Rotated",e[e.A4Rotated=77]="A4Rotated",e[e.A5Rotated=78]="A5Rotated",e[e.B4JisRotated=79]="B4JisRotated",e[e.B5JisRotated=80]="B5JisRotated",e[e.JapanesePostcardRotated=81]="JapanesePostcardRotated",e[e.JapaneseDoublePostcardRotated=82]="JapaneseDoublePostcardRotated",e[e.A6Rotated=83]="A6Rotated",e[e.JapaneseEnvelopeKakuNumber2Rotated=84]="JapaneseEnvelopeKakuNumber2Rotated",e[e.JapaneseEnvelopeKakuNumber3Rotated=85]="JapaneseEnvelopeKakuNumber3Rotated",e[e.JapaneseEnvelopeChouNumber3Rotated=86]="JapaneseEnvelopeChouNumber3Rotated",e[e.JapaneseEnvelopeChouNumber4Rotated=87]="JapaneseEnvelopeChouNumber4Rotated",e[e.B6Jis=88]="B6Jis",e[e.B6JisRotated=89]="B6JisRotated",e[e.Standard12x11=90]="Standard12x11",e[e.JapaneseEnvelopeYouNumber4=91]="JapaneseEnvelopeYouNumber4",e[e.JapaneseEnvelopeYouNumber4Rotated=92]="JapaneseEnvelopeYouNumber4Rotated",e[e.Prc16K=93]="Prc16K",e[e.Prc32K=94]="Prc32K",e[e.Prc32KBig=95]="Prc32KBig",e[e.PrcEnvelopeNumber1=96]="PrcEnvelopeNumber1",e[e.PrcEnvelopeNumber2=97]="PrcEnvelopeNumber2",e[e.PrcEnvelopeNumber3=98]="PrcEnvelopeNumber3",e[e.PrcEnvelopeNumber4=99]="PrcEnvelopeNumber4",e[e.PrcEnvelopeNumber5=100]="PrcEnvelopeNumber5",e[e.PrcEnvelopeNumber7=102]="PrcEnvelopeNumber7",e[e.PrcEnvelopeNumber8=103]="PrcEnvelopeNumber8",e[e.PrcEnvelopeNumber9=104]="PrcEnvelopeNumber9",e[e.PrcEnvelopeNumber10=105]="PrcEnvelopeNumber10",e[e.Prc16KRotated=106]="Prc16KRotated",e[e.Prc32KRotated=107]="Prc32KRotated",e[e.Prc32KBigRotated=108]="Prc32KBigRotated",e[e.PrcEnvelopeNumber1Rotated=109]="PrcEnvelopeNumber1Rotated",e[e.PrcEnvelopeNumber2Rotated=110]="PrcEnvelopeNumber2Rotated",e[e.PrcEnvelopeNumber3Rotated=111]="PrcEnvelopeNumber3Rotated",e[e.PrcEnvelopeNumber4Rotated=112]="PrcEnvelopeNumber4Rotated",e[e.PrcEnvelopeNumber5Rotated=113]="PrcEnvelopeNumber5Rotated",e[e.PrcEnvelopeNumber6Rotated=114]="PrcEnvelopeNumber6Rotated",e[e.PrcEnvelopeNumber7Rotated=115]="PrcEnvelopeNumber7Rotated",e[e.PrcEnvelopeNumber8Rotated=116]="PrcEnvelopeNumber8Rotated",e[e.PrcEnvelopeNumber9Rotated=117]="PrcEnvelopeNumber9Rotated",e[e.PrcEnvelopeNumber10Rotated=118]="PrcEnvelopeNumber10Rotated"})(e.PaperKind||(e.PaperKind={}));var y=e.PaperKind})(e.exporter||(e.exporter={}));var t=e.exporter})(wijmo||(wijmo={}))});