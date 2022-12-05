<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="page4.aspx.cs" Inherits="lab5_1.WebForm4" %>

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
                   <asp:Label ID="L1" runat="server" ></asp:Label>     
                   <br />
                   <br />
            
                   <asp:Label ID="L2" runat="server" Font-Size="24pt"></asp:Label>
                   <br />
                   <br />
                   <asp:Label ID="L3" runat="server" Font-Size="24pt"></asp:Label>

                   <br />
                   <br />

                <asp:Button ID="Button1" runat="server" Text="На головну" Font-Size="16pt" OnClick="Button1_Click" />
            </center>
        </div>
    </form>
</body>
</html>
