<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Insert.aspx.cs" Inherits="Insert" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js"></script>
    <script>
        //新增function
        function Insert() {
            //檢查輸入資料
            if ($("#i_tname")[0].value == "") {
                alert("姓名未填寫");
            }
            else if ($("#i_tid")[0].value == "") {
                alert("身份證字號未填寫");
            }
            else if ($("#i_tid")[0].value.search(/^[A-Z](1|2)\d{8}$/) == -1) {
                alert("身份證字號格式錯誤");
            }
            else if ($("#i_tphone")[0].value == "") {
                alert("手機號碼未填寫");
            }
            else if ($("#i_tphone")[0].value.search(/^[09]{2}\d{8}$/) == -1) {
                alert("手機號碼格式錯誤");
            }
            else if ($("#i_tbirday")[0].value == "") {
                alert("生日未填寫");
            }
            else if ($('input[name=i_gender]:checked').val() == "") {
                alert("性別未填寫");
            }
            else if ($("#i_tpasswd")[0].value == "") {
                alert("密碼未填寫");
            }
            else {
                //比對id
                $.ajax({
                    type: "post",
                    url: "Backstage.aspx",
                    data: "btn=idcheck&id=" + $("#i_tid")[0].value,
                    success: function (msg) {
                        if (msg == "id重複") {
                            alert("新增失敗，已有此身份證之資料");
                        }
                        else {
                            //新增資料
                            $.ajax({
                                type: "post",
                                url: "Backstage.aspx",
                                data: "btn=insert&name=" + $("#i_tname")[0].value +
                                "&id=" + $("#i_tid")[0].value +
                                "&phone=" + $("#i_tphone")[0].value +
                                "&birday=" + $("#i_tbirday")[0].value +
                                "&gender=" + $('input[name=i_gender]:checked').val() +
                                "&passwd=" + $("#i_tpasswd")[0].value +
                                "&rank=" + $("#i_drank").find(":selected").val(),
                                dataType: "json",
                                success: function (msg) {
                                    parent.TableCreate(msg);
                                },
                                error: function () {
                                    alert('例外狀況，有問題～～');
                                }
                            });
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
        <br />
        新增功能<br />
        <br />
        姓名：<input id="i_tname" type="text" />
        <br />
        身份證字號：<input id="i_tid" type="text" maxlength="10"/>
        <br />
        手機：<input id="i_tphone" type="text" maxlength="10"/>
        <br />
        生日：<input id="i_tbirday" type="date" />
        <br />
        性別：<input id="i_rboy" type="radio" name="i_gender" value="男" />男
        <input id="i_rgirl" type="radio" name="i_gender" value="女" />女
        <br />
        密碼：<input id="i_tpasswd" type="password" />  
        <br />
        權限：<select id="i_drank">
            <option value= "1">一般職員</option>
            <option value="2">管理者</option>
        </select>
        <br />
        <button id="insert" type="button" onclick="Insert()">新增</button>
        <br /><br />             
    </div>

</body>
</html>
