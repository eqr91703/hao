<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>人事系統</title>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js"></script>
    <script>
        //登入
        function Login() {
            if ($("#id")[0].value.search(/^[A-Z](1|2)\d{8}$/) == -1) {
                alert("身份證字號格式錯誤");
            }
            else {
                $.ajax({
                    type: "post",
                    url: "Backstage.aspx",
                    data: "btn=login&id=" + $("#id")[0].value + "&password=" + $("#password")[0].value,
                    success: function (msg) {
                        if (msg == "登入成功") {
                            window.location.replace("Main.aspx");
                        }
                        else {
                            alert('身份證字號或是密碼有錯！');
                        }
                    },
                    error: function () {
                        alert('例外狀況，有問題～～');
                    }
                });
            }
        }
    </script>
</head>
<body>

    <h2>人事系統</h2>
    <label>職員登入</label>
    <br />
    <label>身份證字號：</label><input type="text" id="id"/>
    <br />
    <label>密碼　　　：</label><input type="password" id="password"/>       
    <br />
    <button type="button" id="login" onclick = "Login()">登入</button>

</body>
</html>
