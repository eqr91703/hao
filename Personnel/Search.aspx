<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="Search" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js"></script>
    <script>
        //查詢function
        function Search() {
            //查詢資料
            $.ajax({
                type: "post",
                url: "Backstage.aspx",
                data: "btn=search&name=" + $("#search_name")[0].value +
                "&id=" + $("#search_id")[0].value +
                "&phone=" + $("#search_phone")[0].value +
                "&birday=" + $("#search_birday")[0].value +
                "&gender=" + $('input[name=search_gender]:checked').val(),
                dataType: "json",
                success: function (msg) {
                    parent.TableCreate(msg);
                },
                error: function () {
                    alert('例外狀況，有問題～～');
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server"></form>

    <div id="div" runat="server">           
        <br />
        多重模糊查詢功能<br />
        姓名：<input id="search_name" type="text" />
        身份證字號：<input id="search_id" type="text" maxlength="10" />
        手機：<input id="search_phone" type="text" maxlength="10"/>
        生日：<input id="search_birday" type="date" />
        性別：<input id="search_boy" type="radio" name="search_gender" value="男" />男
        <input id="search_girl" type="radio" name="search_gender" value="女" />女
        <input id="Radio1" type="radio" name="search_gender" value="" checked="checked" />不選擇
        <br />
        <button id="search" type="button" onclick="Search()">查詢</button>
        <br />
        <br />
    </div>

</body>
</html>
