<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyTherapy.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="clickMeButton" runat="server" Text="Click Me" OnClick="clickMeButton_Click" />
        <asp:Label ID="outputlabel" runat="server" />
    </form>
</body>
</html>
