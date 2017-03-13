<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

    <div id="div1">   
        人事系統<br />
        <br />
        查詢功能<br />
    
        姓名：<asp:TextBox ID="s_tname" runat="server"></asp:TextBox>
        身份證字號：<asp:TextBox ID="s_tid" runat="server"></asp:TextBox>
        手機：<asp:TextBox ID="s_tphone" runat="server"></asp:TextBox>
        生日：<asp:TextBox ID="s_tbirday" runat="server"  TextMode="Date"></asp:TextBox>
        性別：<asp:RadioButton ID="s_rboy" runat="server" GroupName="s_gender" Text="男" />
        <asp:RadioButton ID="s_rgirl" runat="server" GroupName="s_gender" Text="女" />
        <asp:RadioButton ID="RadioButton1" runat="server" GroupName="s_gender" Text="不選擇" Checked="True" />
        <br />
        <asp:Button ID="search" runat="server" Text="查詢" OnClick="Search_Click" />
        <br />
        <br />

        <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" OnRowDeleting="GridView1_RowDeleting" DataKeyNames="id"
            OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing"
            OnRowUpdating="GridView1_RowUpdating" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" >
            <AlternatingRowStyle BackColor="#F7F7F7" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="update" runat="server" CausesValidation="True" CommandName="Update" Text="更新"></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="取消"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="edit" runat="server" CausesValidation="False" CommandName="Edit" Text="編輯"  Visible='<%# Session["rank"].ToString().Equals("2") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="delete" runat="server" CausesValidation="True" CommandName="Delete" Text="刪除" Visible='<%# Session["rank"].ToString().Equals("2") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="name" HeaderText="姓名" SortExpression="name"></asp:BoundField>
                <asp:TemplateField HeaderText="身份證字號" SortExpression="id">
                    <EditItemTemplate>
                        <asp:Label ID="Label" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="phone" HeaderText="手機" SortExpression="phone"></asp:BoundField>
                <asp:TemplateField HeaderText="生日" SortExpression="birthday">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("birthday", "{0:yyyy/MM/dd}") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("birthday", "{0:yyyy/MM/dd}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="gender" HeaderText="性別" SortExpression="gender"></asp:BoundField>
                <asp:BoundField DataField="passwd" HeaderText="密碼" SortExpression="passwd" ></asp:BoundField>
                <asp:BoundField DataField="rank" HeaderText="權限" SortExpression="rank" ></asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
            <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
            <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
            <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
            <SortedAscendingCellStyle BackColor="#F4F4FD" />
            <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
            <SortedDescendingCellStyle BackColor="#D8D8F0" />
            <SortedDescendingHeaderStyle BackColor="#3E3277" />
        </asp:GridView>   

    </div>
    <div id="div2" runat="server">
        <br />
        新增功能<br />
        <br />
        姓名：<asp:TextBox ID="i_tname" runat="server"></asp:TextBox>
        <br />
        身份證字號：<asp:TextBox ID="i_tid" runat="server"></asp:TextBox>
        <br />
        手機：<asp:TextBox ID="i_tphone" runat="server"></asp:TextBox>
        <br />
        生日：<asp:TextBox ID="i_tbirday" runat="server" TextMode="Date"></asp:TextBox>
        <br />
        性別：<asp:RadioButton ID="i_rboy" runat="server" GroupName="i_gender" Text="男" />
        <asp:RadioButton ID="i_rgirl" runat="server" GroupName="i_gender" Text="女" />
        <br />
        密碼：<asp:TextBox ID="i_tpasswd" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        確認密碼：<asp:TextBox ID="i_tpasswd2" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        權限：<asp:DropDownList ID="i_drank" runat="server">
            <asp:ListItem Value="1">一般職員</asp:ListItem>
            <asp:ListItem Value="2">管理者</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Button ID="insert" runat="server" Text="新增" OnClick="insert_Click" />
        <br />
        <asp:Label ID="notification" runat="server" Font-Size="30pt" ForeColor="Red"></asp:Label>
        <br />
    </div>
        <asp:Button ID="SignOut" runat="server" OnClick="SignOut_Click" Text="登出" />
    </form>
  
</body>
</html>
