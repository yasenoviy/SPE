<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="page5.aspx.cs" Inherits="lab5_1.WebForm5" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #Button1 {
            height: 50px;
            width: 209px;
        }
    </style>
</head>
<center>
    <body>
        <form id="form1" runat="server">
            <div>
                <asp:Label ID="L1" runat="server" ></asp:Label>     <br />
                            <br />
                            <br />
                <asp:Label runat="server" Text="IP address       " Font-Size="24pt"></asp:Label>
    <br />
    <asp:TextBox ID="TextBox1" runat="server" Font-Size="16pt"></asp:TextBox>
    </br></br>

    <asp:Label runat="server" Text="Port      " Font-Size="24pt"></asp:Label>
    <br />
    <asp:TextBox ID="TextBox2" runat="server" Font-Size="16pt"></asp:TextBox>
    </br></br>

    <asp:Label runat="server" Text="Email       " Font-Size="24pt"></asp:Label>
    <br />
    <asp:TextBox ID="TextBox3" runat="server" Font-Size="16pt"></asp:TextBox>
    </br></br>

    <asp:Button ID="Button1" runat="server" Text="Додати" Font-Size="16pt" OnClick="Button1_Click" />&nbsp;
    <asp:Button ID="Button2" runat="server" Text="Назад" Font-Size="16pt" OnClick="Button2_Click" Height="50px" Width="103px" />
                <br />
                <br />
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            </div>
        </form>
    </body>



    
&nbsp;</center>
</html>
