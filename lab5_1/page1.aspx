<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="page1.aspx.cs" Inherits="lab5_1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #form1 {
            height: 835px;
        }
    </style>
</head>
<center>
    <body>
        <form id="form1" runat="server">
            <div>
              <h1 style="margin-left: 40px">Мої TCP-Служби:</h1> 
            </div>
        
                <asp:Table ID="T1" runat="server" ></asp:Table>
        
            <br />

            <asp:Button ID="Button1" runat="server" Height="90px" Text="Додати нову службу" Width="326px" OnClick="Button1_Click" Font-Size="16pt" />
            <br />
            &nbsp;</form>
    </body>
</center>
</html>
