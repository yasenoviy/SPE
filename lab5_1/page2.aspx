<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="page2.aspx.cs" Inherits="lab5_1.WebForm2" %>

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
                    <asp:Label ID="L1" runat="server" ></asp:Label>     <br /><br /><br />

<asp:Label ID="Label1" runat="server" Font-Size="24pt" ></asp:Label><br />     <asp:textbox ID="l11" runat="server" Font-Size="24pt" OnTextChanged="l11_TextChanged" ></asp:textbox>    <br /><br />
<asp:Label ID="Label2" runat="server" Font-Size="24pt" ></asp:Label><br />     <asp:textbox ID="l22" runat="server" Font-Size="24pt" OnTextChanged="l22_TextChanged" ></asp:textbox>    <br /><br />
<asp:Label ID="Label3" runat="server" Font-Size="24pt" ></asp:Label><br />     <asp:Image ID="l33" runat="server" />                   <br /><br />
<asp:Label ID="Label4" runat="server" Font-Size="24pt" ></asp:Label><br /><br />     <asp:FileUpload ID="F1" runat="server" Font-Size="24pt" />    
                
                <asp:Button ID="Button1" runat="server" Text="Upload File" OnClick="Button1_Click" Font-Size="16pt" Height="50px" Width="149px"/> <br />
                <asp:Label ID="FileUploadStatus" runat="server"></asp:Label><br /><br />

<asp:Label ID="Label5" runat="server" Font-Size="24pt" ></asp:Label><br />     <asp:label ID="l44" runat="server" Font-Size="24pt" ></asp:label>        <br /><br />
<asp:Label ID="Label6" runat="server" Font-Size="24pt" ></asp:Label><br />     <asp:label ID="l55" runat="server" Font-Size="24pt" ></asp:label>        <br /><br />
<asp:Label ID="Label7" runat="server" Font-Size="24pt" ></asp:Label><br />     <asp:textbox ID="l66" runat="server" Font-Size="24pt" OnTextChanged="l66_TextChanged" ></asp:textbox>    <br /><br />

<asp:Button ID="Button2" runat="server" Text="Зберегти" Height="64px" OnClick="Button2_Click" Width="112px" Font-Size="16pt" />    <asp:Label ID="Label9" runat="server" Text="&emsp;"></asp:Label>
<asp:Button ID="Button3" runat="server" Text="Назад" Height="66px" OnClick="Button3_Click" Width="91px" Font-Size="16pt" />       <asp:Label ID="Label8" runat="server" Text="&emsp;"></asp:Label>
<asp:Button ID="Button4" runat="server" Text="Перевірити" Height="66px" Width="152px" OnClick="Button4_Click" Font-Size="16pt" />

                    <br />                <asp:Label ID="Label10" runat="server" Text=""></asp:Label>

            </center>
        </div>
    </form>
</body>
</html>
