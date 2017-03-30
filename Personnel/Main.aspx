<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>人事系統</title>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js"></script>
    <script>
        //登出
        function SignOut() {
            $.ajax({
                type: "post",
                url: "Backstage.aspx",
                data: "btn=signOut",
                success: function (msg) {
                    if (msg == "登出成功") {
                        window.location.replace("Login.aspx");
                    }
                },
                error: function () {
                    alert('例外狀況，有問題～～');
                }
            });
        }

        //查詢功能iframe
        function Search_iframe() {
            $("#iframe1").attr("src", "Search.aspx");
        }

        //刪除功能iframe
        function Delete_iframe() {
            $("#iframe1").attr("src", "Delete.aspx");
        }
        
        //新增功能iframe
        function Insert_iframe() {
            $("#iframe1").attr("src", "Insert.aspx");
        }

        //修改功能iframe
        function Update_iframe() {
            $("#iframe1").attr("src", "Update.aspx");
        }

        //$(document).ready(function () {
            //起始創建表格
            $.ajax({
                type: "post",
                url: "Backstage.aspx",
                data: "btn=ready",
                dataType: "json",
                success: function (msg) {
                    TableCreate(msg);
                },
                error: function () {
                    alert('例外狀況，有問題～～');
                }
            });
        //});

        function TableCreate(msg) {
            $("#table1").empty();
            $("#table1").append('<tr class="headerrow"><th>姓名</th><th>身份證字號</th><th>手機</th><th>生日</th><th>性別</th><th>密碼</th><th>權限</th></tr>');
            $.each(msg, function (index, item) {
                $("#table1").append(
                 "<tr><td>" + item.name + "</td>" +
                     "<td>" + item.id + "</td>" +
                     "<td>" + item.phone + "</td>" +
                     "<td>" + item.birthday + "</td>" +
                     "<td>" + item.gender + "</td>" +
                     "<td>" + item.passwd + "</td>" +
                     "<td>" + item.rank + "</td></tr>"
                );
            })
        }

        //iframe1自動調整高度
        $(function () {
            $("#iframe1").load(function () {
                $(this).height($(this).contents().find("body").height() + 40);
            });
        });

    </script>
    <style>
        /*tr:nth-child(even) {background: #CCC}
        tr:nth-child(odd) {background-color: #FAFAFA;}*/
        .fancytable{border:1px solid #cccccc; width:100%;border-collapse:collapse;}
        .fancytable td{border:1px solid #cccccc; color:#555555;text-align:center;line-height:28px;}
        .headerrow{ background-color:#0066cc;}
        .headerrow th{ color:#ffffff; text-align:center;}   
    </style>
</head>
<body>
    <form id="form1" runat="server"></form>
    <div id="div" runat="server">
    人事系統<br /><br />

    <div id="div1" runat="server">
    <button id="search_iframe" type="button" onclick="Search_iframe()">查詢功能</button>&nbsp;
    <button id="insert_iframe" type="button" onclick="Insert_iframe()">新增功能</button>&nbsp;
    <button id="update_iframe" type="button" onclick="Update_iframe()">修改功能</button>&nbsp;
    <button id="delete_iframe" type="button" onclick="Delete_iframe()">刪除功能</button><br/>
    </div>

    <iframe id="iframe1" src="Search.aspx"  width="100%" height="100%" frameborder="0" ></iframe>

    <div id="div2">
        <table id="table1" class="fancytable" border='1' rules='all'>          
        </table>
    </div>
    
    <br /><button id="SignOut" type="button" onclick="SignOut()">登出</button>
    </div>
</body>
</html>
