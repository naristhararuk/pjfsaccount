<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlertMessageFadeOut.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.AlertMessageFadeOut" %>
<center>
    <div style="height:0px;" style="border: solid 1px #808080; border-width: 0px 0px; position: absolute; top: 45%; width:90%;">
        <div id="MessageZone" runat="server" style="text-align:center;height:0px;overflow:hidden;">
            <asp:Label ID="ctlMessage" runat="server"></asp:Label>
        </div>
    </div>
</center>
<ajaxToolkit:AnimationExtender id="AnimateExtender" runat="server" TargetControlID="MessageZone">
    <Animations>
        <OnLoad>
            <Sequence>
                <Parallel duration="1" Fps="30">
                    <Resize HeightScript="25" />
                    <FadeIn AnimationTarget="MessageZone" minimumOpacity=".05" maximumOpacity="0.8" />
                    <Color AnimationTarget="MessageZone" PropertyKey="backgroundColor" StartValue="#FFFFFF" EndValue="#FFFF00" />
                </Parallel>

                <Parallel duration="1" Fps="30"/>

                <Parallel duration="1" Fps="30">
                    <StyleAction Attribute="overflow" Value="hidden" />
                    <FadeOut AnimationTarget="MessageZone" minimumOpacity=".05" maximumOpacity="0.8" />
                    <Color AnimationTarget="MessageZone" PropertyKey="backgroundColor" StartValue="#FFFF00" EndValue="#FFFFFF" />
                    <Resize HeightScript="0" />
                    <StyleAction Attribute="display" Value="none" />
                </Parallel>
            </Sequence>
        </OnLoad>
    </Animations>
</ajaxToolkit:AnimationExtender>