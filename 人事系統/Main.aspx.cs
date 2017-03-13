using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

public partial class Main : BasePage //這邊固定要繼承BasePage來取得公用的方法屬性以及事件
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //判斷是否正常登入
        if ((Session["Login"] == null) || ((Session["Login"].ToString() != "OK")))
        {
            Response.Write("請正常登入");
            form1.Visible = false;
        }
        else
        {
            //檢查網頁是否第一次執行
            if (!Page.IsPostBack)
            {
                //呈現初始頁面
                switch (Session["rank"].ToString())
                {
                    //一般職員
                    case "1":
                        SQLCom("職員初始頁");
                        div2.Visible = false;                        
                        break;
                    //管理員
                    case "2":
                        SQLCom("管理員初始頁");
                        break;
                    default:
                        break;
                }
            }
        }
    }

    //登出紐
    protected void SignOut_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("Login.aspx");
    }

    //查詢紐
    protected void Search_Click(object sender, EventArgs e)
    {
        SQLCom("查詢");
    }

    //新增紐
    protected void insert_Click(object sender, EventArgs e)
    {
        //檢查各欄位是否有填寫，也可以在此判斷資料格式是否輸入正確
        if (i_tname.Text=="")
        {
            notification.Text = "姓名未填寫";
        }
        else if(i_tid.Text=="")
        {
            notification.Text = "身份證字號未填寫";
        }
        else if (i_tphone.Text == "")
        {
            notification.Text = "手機未填寫";
        }
        else if (i_tbirday.Text == "")
        {
            notification.Text = "生日未填寫";
        }
        else if (!i_rboy.Checked && !i_rgirl.Checked)
        {
            notification.Text = "性別未填寫";
        }
        else if (i_tpasswd.Text == "")
        {
            notification.Text = "密碼未填寫";
        }
        else if (i_tpasswd.Text != i_tpasswd2.Text)
        {
            notification.Text = "重新確認密碼";
        }
        //若都有填寫，檢查資料庫是否有重複身份證字號
        else
        {
            SqlCommand cmd = new SqlCommand(); //宣告SqlCommand物件
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataReader dr = null; //宣告SqlDataReader物件，不用NEW初始值設定為NULL

            int i = 0;
            try
            {
                cmd.Connection.Open(); //開啟資料庫連線

                cmd.Parameters.AddWithValue("@id", i_tid.Text);
                cmd.CommandText = "Select * from Personnel WHERE id=@id";
                dr = cmd.ExecuteReader(); //執行命令
                if (!dr.Read())
                {
                    i = 1;
                }
                else
                {
                    notification.Text = "身份證字號重複";
                    cmd.CommandText = "Select * from Personnel";
                }          
            }
            //錯誤捕獲區塊
            catch (Exception ex) //ex用於除錯時方便查看用 ，正式上線時請務必不要直接顯示ex於畫面上
            {
                Response.Write(ex.Message);
            }
            //最終無論如何 必定會執行的區塊　用來放置釋放或關閉資源
            finally
            {
                cmd.Parameters.Clear();
                //判斷是否已關閉
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
            //若身份證字號無重複，清空notification.Text，將資料寫入資料庫
            if (i==1)
            {
                notification.Text = "";
                SQLCom("新增");
            }
        }
    }

    //刪除紐
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        SqlCommand cmd = new SqlCommand(); //宣告SqlCommand物件
        cmd.Connection = new SqlConnection(this.ConnStr);
        SqlDataReader dr = null; //宣告SqlDataReader物件，不用NEW初始值設定為NULL

        try
        {
            cmd.Connection.Open(); //開啟資料庫連線

            cmd.Parameters.AddWithValue("@id", GridView1.DataKeys[e.RowIndex].Value);
            cmd.CommandText = "delete from Personnel WHERE id=@id";

            dr = cmd.ExecuteReader(); //執行命令            

        }
        //錯誤捕獲區塊
        catch (Exception ex) //ex用於除錯時方便查看用 ，正式上線時請務必不要直接顯示ex於畫面上
        {
            Response.Write(ex.Message);
        }
        //最終無論如何 必定會執行的區塊　用來放置釋放或關閉資源
        finally
        {
            cmd.Parameters.Clear();
            //判斷是否已關閉
            if (cmd.Connection.State != ConnectionState.Closed)
                cmd.Connection.Close();
        }
        SQLCom("管理員初始頁");
    }

    //更新
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox upname,upphone, upbirday, upgender, uppasswd, uprank;
        upname = (TextBox)GridView1.Rows[e.RowIndex].Cells[2].Controls[0];
        upphone = (TextBox)GridView1.Rows[e.RowIndex].Cells[4].Controls[0];
        upbirday = (TextBox)GridView1.Rows[e.RowIndex].FindControl("TextBox1");
        upgender = (TextBox)GridView1.Rows[e.RowIndex].Cells[6].Controls[0];
        uppasswd = (TextBox)GridView1.Rows[e.RowIndex].Cells[7].Controls[0];
        uprank = (TextBox)GridView1.Rows[e.RowIndex].Cells[8].Controls[0];

        SqlCommand cmd = new SqlCommand(); //宣告SqlCommand物件
        cmd.Connection = new SqlConnection(this.ConnStr);
        SqlDataReader dr = null; //宣告SqlDataReader物件，不用NEW初始值設定為NULL

        try
        {
            cmd.Connection.Open(); //開啟資料庫連線

            cmd.Parameters.AddWithValue("@name",upname.Text);
            cmd.Parameters.AddWithValue("@id", GridView1.DataKeys[e.RowIndex].Value);
            cmd.Parameters.AddWithValue("@phone", upphone.Text);
            cmd.Parameters.AddWithValue("@birthday", upbirday.Text);
            cmd.Parameters.AddWithValue("@gender", upgender.Text);
            cmd.Parameters.AddWithValue("@passwd", uppasswd.Text);
            cmd.Parameters.AddWithValue("@rank", uprank.Text);
            cmd.CommandText = "update Personnel set [name] = @name, [phone] = @phone, [birthday]=@birthday, [gender]=@gender," +
                "[passwd]=@passwd,[rank]=@rank where [id] = @id";

            dr = cmd.ExecuteReader(); //執行命令            

        }
        //錯誤捕獲區塊
        catch (Exception ex) //ex用於除錯時方便查看用 ，正式上線時請務必不要直接顯示ex於畫面上
        {
            Response.Write(ex.Message);
        }
        //最終無論如何 必定會執行的區塊　用來放置釋放或關閉資源
        finally
        {
            cmd.Parameters.Clear();
            //判斷是否已關閉
            if (cmd.Connection.State != ConnectionState.Closed)
                cmd.Connection.Close();
        }

        GridView1.EditIndex = -1;
        SQLCom("管理員初始頁");
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {   //----編輯模式----
        GridView1.EditIndex = e.NewEditIndex;
        SQLCom("查詢");
        //----畫面上的GridView，已經事先設定好「DataKeyName」屬性 = id ----
        //----所以編輯時，主索引鍵id 欄位會自動變成「唯讀」----
    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {   //---離開「編輯」模式----
        GridView1.EditIndex = -1;
        SQLCom("查詢");
    }

    protected void SQLCom(String s)
    {
        SqlCommand cmd = new SqlCommand(); //宣告SqlCommand物件
        cmd.Connection = new SqlConnection(this.ConnStr);
        SqlDataReader dr = null; //宣告SqlDataReader物件，不用NEW初始值設定為NULL

        //以下try catch finally為固定寫法，且釋放資源必定要寫在finally裡
        //偵錯
        try
        {            
            switch (s)
            {
                //一般職員初始頁面
                case "職員初始頁":
                    cmd.Parameters.AddWithValue("@id", Session["id"].ToString());
                    cmd.CommandText = "Select * from Personnel WHERE id=@id";
                    break;
                //管理員初始頁面
                case "管理員初始頁":
                    cmd.CommandText = "Select * from Personnel ";                    
                    break;
                //查詢
                case "查詢":
                    String where=null;
                    int hasand = 0;

                    if (s_tname.Text.Length>0)
                    {
                        cmd.Parameters.AddWithValue("@name", s_tname.Text);
                        where = "name like '%'+@name+'%' ";
                        hasand++;
                    }
                    if (s_tid.Text.Length>0)
                    {
                        cmd.Parameters.AddWithValue("@id", s_tid.Text);
                        if (hasand>0)
                        {
                            where += "AND id like '%'+@id+'%' ";
                        }
                        else
                        {
                            where = "id like '%'+@id+'%' ";
                            hasand++;
                        }
                    }
                    if (s_tphone.Text.Length>0)
                    {
                        cmd.Parameters.AddWithValue("@phone", s_tphone.Text);
                        if (hasand > 0)
                        {
                            where += "AND phone like '%'+@phone+'%' ";
                        }
                        else
                        {
                            where = "phone like '%'+@phone+'%' ";
                            hasand++;
                        }
                    }
                    if (s_tbirday.Text.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@birthday", s_tbirday.Text);
                        if (hasand > 0)
                        {
                            where += "AND birthday like '%'+@birthday+'%' ";
                        }
                        else
                        {
                            where = "birthday like '%'+@birthday+'%' ";
                            hasand++;
                        }
                    }
                    if (s_rboy.Checked)
                    {
                        cmd.Parameters.AddWithValue("@gender",s_rboy.Text);
                        if (hasand > 0)
                        {
                            where += "AND gender like '%'+@gender+'%' ";
                        }
                        else
                        {
                            where = "gender like '%'+@gender+'%' ";
                            hasand++;
                        }
                    }
                    else if (s_rgirl.Checked)
                    {
                        cmd.Parameters.AddWithValue("@gender", s_rgirl.Text);
                        if (hasand > 0)
                        {
                            where += "AND gender like '%'+@gender+'%' ";
                        }
                        else
                        {
                            where = "gender like '%'+@gender+'%' ";
                            hasand++;
                        }
                    }
                    if (where != null)
                    {
                        if (where.Length > 0)
                        {
                            cmd.CommandText = "Select * from Personnel where " + where;
                        }
                        else
                        { cmd.CommandText = "Select * from Personnel"; }
                    }
                    else
                    {cmd.CommandText = "Select * from Personnel";}
                    
                    break;
                //新增
                case "新增":
                    cmd.Parameters.AddWithValue("@name", i_tname.Text);
                    cmd.Parameters.AddWithValue("@id", i_tid.Text);
                    cmd.Parameters.AddWithValue("@phone", i_tphone.Text);
                    cmd.Parameters.AddWithValue("@birthday",i_tbirday.Text);
                    if (i_rboy.Checked)
                    {
                        cmd.Parameters.AddWithValue("@gender", i_rboy.Text);
                    }
                    else if (i_rgirl.Checked)
                    {
                        cmd.Parameters.AddWithValue("@gender", i_rgirl.Text);
                    }
                    cmd.Parameters.AddWithValue("@passwd", i_tpasswd.Text);
                    cmd.Parameters.AddWithValue("@rank", i_drank.SelectedValue);

                    cmd.CommandText = "INSERT INTO Personnel([name],[id],[phone],[birthday],[gender],[passwd],[rank])" +
                                      "VALUES (@name,@id,@phone,@birthday,@gender,@passwd,@rank)" +
                                      "SELECT * from Personnel";                                             
                    break;
                default:
                    break;
            }
            cmd.Connection.Open(); //開啟資料庫連線
            dr = cmd.ExecuteReader(); //執行命令
            GridView1.DataSource = dr;
            GridView1.DataBind();
            if (Session["rank"].ToString()=="1")
            {
                for(int i = 0; i < GridView1.Rows.Count; i++) 
               { 
	                GridView1.HeaderRow.Cells[7].Visible = false; 
	                GridView1.Rows[i].Cells[7].Visible = false;
                    GridView1.HeaderRow.Cells[8].Visible = false;
                    GridView1.Rows[i].Cells[8].Visible = false; 
                }
            }
        }
        //錯誤捕獲區塊
        catch (Exception ex) //ex用於除錯時方便查看用 ，正式上線時請務必不要直接顯示ex於畫面上
        {
            Response.Write("111"+ex.Message + "SQLCoㄏㄏ");
        }
        //最終無論如何 必定會執行的區塊　用來放置釋放或關閉資源
        finally
        {
            cmd.Parameters.Clear();
            //判斷是否已關閉
            if (cmd.Connection.State != ConnectionState.Closed)
                cmd.Connection.Close();
        }
    }

}