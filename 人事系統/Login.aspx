<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="notification" runat="server" Text=""></asp:Label>
        <br />
    
        職員登入<br />
        身份證字號：<asp:TextBox ID="id" runat="server" MaxLength="10" ></asp:TextBox>
        <br />
        密碼　　　：<asp:TextBox ID="passwd" runat="server" TextMode="Password"></asp:TextBox>

        <asp:Button ID="blogin" runat="server" Text="登入" OnClick="login_Click" />
    
    </div>
    </form>
</body>
</html>
