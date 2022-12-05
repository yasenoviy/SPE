<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="page3.aspx.cs" Inherits="lab5_1.WebForm3" %>

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
                            <asp:Label ID="L1" runat="server" ></asp:Label>     <br />
                            <br />
                            <br />
                            <asp:Label ID="Label10" runat="server" Text="Введіть одноразовий пароль" Font-Size="24pt"></asp:Label>
                            <br />

                            <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged" Font-Size="24pt"></asp:TextBox>

                            <br />
                            <br />
                            <asp:Button ID="Button1" runat="server" Text="Перевірити" Font-Size="16pt" OnClick="Button1_Click" /><asp:Label ID="Label9" runat="server" Text="&emsp;"></asp:Label>
                            <asp:Button ID="Button2" runat="server" Text="Назад" Font-Size="16pt" OnClick="Button2_Click" />

                            <asp:Label ID="Label11" runat="server"></asp:Label>

                            <br />
                            <br />
                            <asp:Label ID="Label12" runat="server" Text="Label"></asp:Label>
                            <br />
                            <br />
                            <asp:Label ID="Label13" runat="server" Text="Label"></asp:Label>

            </center>
        </div>
    </form>
</body>
</html>
