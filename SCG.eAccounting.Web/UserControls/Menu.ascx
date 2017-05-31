<%@ Control Language="C#" AutoEventWireup="true" EnableTheming="true" CodeBehind="Menu.ascx.cs" Inherits="SCG.eAccounting.Web.UserControls.Menu" %>

<style type="text/css">
    .level1
    {
        color           : #003d52;
        background-color: White;
        font-family     : Tahoma;
        Font-Size       : 12px;
        font-weight     : bold;
        height          : 18px
    }

    .level2
    {
        color: #7bcff1;
        background-color: #2c76b5;
        font-family     : Tahoma;
        Font-Size       : 12px;
        font-weight     : bold;
    }

    .level3
    {
        color: White;
        background-color: #7dd3f6;
        font-family     : Tahoma;
        Font-Size       : 12px;
        font-weight     : bold;
    }
    
    .levelsecound
    {
        color: Blue;
        font-family     : Tahoma;
        Font-Size       : 12px;
        font-weight     : bold;
        height          : 18px
    }

    .hoverstyle
    {
        color: White;
        background-color: #44b5e1;
        font-family     : Tahoma;
        Font-Size       : 12px;
        font-weight     : bold;
    }
    
    .selectedstyle
    {
        color: White;
    } 

    .toolbar
    {
        font-family     : Tahoma;
        Font-Size       : 12px;
        font-weight     : bold;
        /*filter          : progid:DXImageTransform.Microsoft.Gradient(gradientType=0,startColorStr=#d7e4f0,endColorStr=#44b5e1);*/
		/*filter: progid:DXImageTransform.Microsoft.Gradient(gradientType=0,startColorStr='#d7e4f0',endColorStr='#44b5e1')progid:DXImageTransform.Microsoft.DropShadow(color='#d7e4f0', OffX=0, OffY=0, Positive='true');*/
		/*filter: progid:DXImageTransform.Microsoft.Gradient(gradientType=0,startColorStr='#d7efdb',endColorStr='#4dde47')progid:DXImageTransform.Microsoft.DropShadow(color='#d7efdb', OffX=0, OffY=0, Positive='true');*/
		filter         : progid:DXImageTransform.Microsoft.gradient( startColorstr='#d7efdb', endColorstr='#4dde47',GradientType=0 );
		background-image: -ms-linear-gradient(top, #d7efdb 0%, #4dde47 100%);
	}
	.toolbar1
	{
		font-family: Tahoma;
		font-size: 12px;
		font-weight: bold;
		/*color: #bb0301;*/ 
		background-color: #dd8883;
		
	}
</style>

<asp:Menu ID="Menu1" runat="server" >
 
    <LevelMenuItemStyles>
        <asp:MenuItemStyle CssClass="level1"/>
        <asp:MenuItemStyle CssClass="levelsecound"/>
        <asp:MenuItemStyle CssClass="levelsecound"/>
    </LevelMenuItemStyles>
    
    <DynamicMenuStyle CssClass="toolbar" />
    <DynamicHoverStyle CssClass="toolbar1" />
</asp:Menu>
