<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Delete.aspx.cs" Inherits="Delete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js"></script>
    <script>
        //刪除function
        function Delete() {
            //檢查身份證字號格式
            if ($("#delete_id")[0].value.search(/^[A-Z](1|2)\d{8}$/) == -1) {
                alert("身份證字號錯誤");
            }
            else {
                //比對id
                $.ajax({
                    type: "post",
                    url: "Backstage.aspx",
                    data: "btn=idcheck&id=" + $("#delete_id")[0].value,
                    success: function (msg) {
                        if (msg == "id重複") {
                            //刪除資料
                            $.ajax({
                                type: "post",
                                url: "Backstage.aspx",
                                data: "btn=delete&id=" + $("#delete_id")[0].value,
                                dataType: "json",
                                success: function (msg) {
                                    parent.TableCreate(msg);
                                },
                                error: function () {
                                    alert('例外狀況，有問題～～');
                                }
                            });
                        }
                        else {
                            alert("查無此人，請檢查欲刪除之身份證字號");
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
    <form id="form1" runat="server"></form>

    <div id="div" runat="server">
        刪除功能<br />
        欲刪除之身份證字號：<input id="delete_id" type="text" maxlength="10"/>
        <br />
        <button id="delete" type="button" onclick="Delete()">刪除</button>
    </div>

</body>
</html>
