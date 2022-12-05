<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="task5.aspx.cs" Inherits="task5.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <center>
                <asp:Label ID="Label3" runat="server" Text="Щось на розумному" Font-Size="16pt"></asp:Label>
                <br />
                <br />
                <asp:Label ID="Label1" runat="server" Text="X" Font-Size="16pt"></asp:Label>
                <asp:TextBox ID="TextBox1" runat="server" Font-Size="16pt"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="Label2" runat="server" Text="Y" Font-Size="16pt"></asp:Label>
                <asp:TextBox ID="TextBox2" runat="server" Font-Size="16pt"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="Button1" runat="server" Text="тиць сюди" OnClick="Button1_Click" Font-Size="16pt" />
                <br />
                <br />
                <br />
                <asp:Label ID="Label4" runat="server" Font-Size="24pt" Text=""></asp:Label>
            </center>
        </div>
    </form>
</body>
</html>
