<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentHeader.ascx.cs"
    Inherits="SCG.eAccounting.Web.UserControls.DocumentEditor.Components.DocumentHeader" EnableTheming="true" %>
<br />
<script type="text/javascript">
    window.onload=function(){
       setInterval('blinkIt()',500)
    }
    function blinkIt() {
        if (!document.all) return;
        else
        {
            for(i=0;i<document.all.tags('blink').length;i++){
                s = document.all.tags('blink')[i];
                if(s.innerText.length > 0)
                    s.style.visibility=(s.style.visibility=='visible')?'hidden':'visible';
            }
        }
    }
</script>
<table width="100%" border="0">
    <tr>
        <td colspan="5" align="center">
            <asp:Label ID="ctlDocumentHeader" runat="server" Text="Header" SkinID="SkDocumentHeader1Label"/>
            
        </td>
    </tr>
    <tr>
        <td colspan="5" align="center">
           <br />
            
        </td>
    </tr>
    <tr>
        <td align="left" width="10%">
            <asp:Label ID="ctlStatusLabel" runat="server"  Text="$Status$" SkinID="SkDocumentHeader2Label" />&nbsp;:&nbsp; 
        </td>
        <td align="left" width="30%">
            <asp:Label ID="ctlStatus" runat="server" SkinID="SkGeneralLabel"/>  
        </td>            
        <td align="left">
        </td>
        <td align="left" width="10%">
            <asp:Label ID="ctlAdvanceNoLabel" SkinID="SkDocumentHeader2Label"  runat="server" Text="$No$" />&nbsp;:&nbsp;            
        </td>
        <td align="left" width="43%">
            <asp:Label ID="ctlAdvanceNo"  runat="server" SkinID="SkGeneralLabel"/>
        </td>
    </tr>
    <tr>
        <td width="10%"></td>
        <td colspan="2" width="55%">
            <asp:Label ID ="ctlSeeHistory" runat="server" SkinID="SkOrderLabel"  ForeColor="Red" Text="$See History for Reject's Detail$"/><br />
            <blink><asp:Label ID ="ctlSeeMessage" runat="server" SkinID="SkOrderLabel"  ForeColor="Red" Font-Bold="true" Text="$SeeMessage$"/></blink><br />
            <blink><asp:Label ID ="ctlWarning" runat="server" SkinID="SkOrderLabel"  ForeColor="Red" Font-Bold="true" Text=""/></blink><br />
            <blink><asp:Label ID ="ctlWarningChangeAmount" runat="server" SkinID="SkOrderLabel"  ForeColor="Red" Font-Bold="true" Text=""/></blink>
        </td>
        <td align="left" width="10%">
            <asp:Label ID="ctlDateLabel"  runat="server" SkinID="SkDocumentHeader2Label" Text="$Date$" />&nbsp;:&nbsp;
        </td>
        <td>
            <asp:Label ID="ctlDate"  runat="server" SkinID="SkGeneralLabel" />
        </td>
    </tr>
</table>
