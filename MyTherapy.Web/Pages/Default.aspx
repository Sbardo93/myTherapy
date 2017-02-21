<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyTherapy.Web.Default" %>

<%@ Register Src="~/UserControls/ucSmartGrid.ascx" TagPrefix="uc" TagName="ucSmartGrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    Default Page
    <br />
    <uc:ucSmartGrid runat="server" ID="ucUtenti"/>
</asp:Content>
