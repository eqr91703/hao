using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public partial class SQL : BasePage
{
    string s_id, s_password, s_name, s_phone, s_birday, s_gender,s_passwd,s_rank;
    protected void Page_Load(object sender, EventArgs e)
    {
        s_id = s_password = s_name = s_phone = s_birday = s_gender = s_passwd = s_rank = "";
        //事件選擇
        string s_btn = Request.Form["btn"];
        switch (s_btn)
        {
            //test
            case "test":
                s_id = Request.Form["id"];
                Response.Write("");
                break;
            //登入
            case "login":
                s_id = Request.Form["id"];
                s_password = Request.Form["password"];
                Data_Check("logincheck");
                break;
            //確認id
            case "idcheck":
                s_id = Request.Form["id"];
                Data_Check("idcheck");
                break; 
            //登出
            case "signOut":
                Session.Clear();
                Session.Abandon();
                Response.Write("登出成功");
                break;
            //起始創建表格
            case "ready":
                TableCreate("ready");
                break;
            //刪除
            case "delete":
                s_id = Request.Form["id"];
                TableCreate("delete");
                break;
            //查詢
            case "search":
                s_name = Request.Form["name"];
                s_id = Request.Form["id"];
                s_phone = Request.Form["phone"];
                s_birday = Request.Form["birday"];
                s_gender = Request.Form["gender"];
                TableCreate("search");
                break;
            //新增
            case "insert":
                s_name = Request.Form["name"];
                s_id = Request.Form["id"];
                s_phone = Request.Form["phone"];
                s_birday = Request.Form["birday"];
                s_gender = Request.Form["gender"];
                s_passwd = Request.Form["passwd"];
                s_rank = Request.Form["rank"];
                TableCreate("insert");
                break;
            //修改
            case "update":
                s_name = Request.Form["name"];
                s_id = Request.Form["id"];
                s_phone = Request.Form["phone"];
                s_birday = Request.Form["birday"];
                if (Request.Form["gender"] == "男")
                { s_gender = "男"; }
                else if (Request.Form["gender"]=="女")
                { s_gender = "女"; }                
                s_passwd = Request.Form["passwd"];
                s_rank = Request.Form["rank"];
                TableCreate("update");
                break;
            default:
                Response.Write("事件選擇區例外");
                break;
        }
    }

    //確認資料(登入、id是否重複)
    protected void Data_Check(String s)
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
                //登入確認
                case "logincheck":
                    cmd.CommandText = "Select * from Personnel WHERE id=@id AND passwd=@passwd";
                    cmd.Parameters.AddWithValue("@id", s_id);
                    cmd.Parameters.AddWithValue("@passwd", s_password);
                    break;
                //id確認
                case "idcheck":
                    cmd.CommandText = "Select * from Personnel WHERE id=@id";
                    cmd.Parameters.AddWithValue("@id", s_id);
                    break;
                default:
                    break;
            }

            cmd.Connection.Open(); //開啟資料庫連線

            dr = cmd.ExecuteReader(); //執行命令

            if (dr.Read())
            {
                switch (s)
                {
                    case "logincheck":
                        Session["id"] = dr["id"].ToString();
                        Session["rank"] = dr["rank"].ToString();                        
                        Session["Login"] = "OK";   //--通過帳號與密碼的認證，就獲得 Session。
                        Response.Write("登入成功");
                        break;
                    case "idcheck":
                        Response.Write("id重複");
                        break;
                    default:
                        break;
                }
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
    }

    //表格創建
    protected void TableCreate(String s)
    {
        SqlCommand cmd = new SqlCommand(); //宣告SqlCommand物件
        cmd.Connection = new SqlConnection(this.ConnStr); //設定連線字串
        SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
        DataTable dt = new DataTable(); //宣告DataTable物件

        //以下try catch finally為固定寫法，且釋放資源必定要寫在finally裡
        //偵錯
        try
        {
            //依不同事件設定不同語法
            switch (s)
            {
                //起始
                case "ready":
                    cmd.CommandText = "Select * from Personnel";
                    break;
                //刪除
                case "delete":
                    cmd.CommandText = "DELETE FROM Personnel WHERE id=@id Select * from Personnel";
                    cmd.Parameters.AddWithValue("@id", s_id);
                    break;
                //查詢
                case "search":
                    String s_where=null;
                    int i_hasand = 0;

                    if (s_name.Length>0)
                    {
                        cmd.Parameters.AddWithValue("@name", s_name);
                        s_where = "name like '%'+@name+'%' ";
                        i_hasand++;
                    }
                    if (s_id.Length>0)
                    {
                        cmd.Parameters.AddWithValue("@id", s_id);
                        if (i_hasand > 0)
                        {
                            s_where += "AND id like '%'+@id+'%' ";
                        }
                        else
                        {
                            s_where = "id like '%'+@id+'%' ";
                            i_hasand++;
                        }
                    }
                    if (s_phone.Length>0)
                    {
                        cmd.Parameters.AddWithValue("@phone", s_phone);
                        if (i_hasand > 0)
                        {
                            s_where += "AND phone like '%'+@phone+'%' ";
                        }
                        else
                        {
                            s_where = "phone like '%'+@phone+'%' ";
                            i_hasand++;
                        }
                    }
                    if (s_birday.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@birthday", s_birday);
                        if (i_hasand > 0)
                        {
                            s_where += "AND birthday like '%'+@birthday+'%' ";
                        }
                        else
                        {
                            s_where = "birthday like '%'+@birthday+'%' ";
                            i_hasand++;
                        }
                    }
                    if (s_gender.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@gender", s_gender);
                        if (i_hasand > 0)
                        {
                            s_where += "AND gender like '%'+@gender+'%' ";
                        }
                        else
                        {
                            s_where = "gender like '%'+@gender+'%' ";
                            i_hasand++;
                        }
                    }
                    if (s_where != null)
                    {
                        if (s_where.Length > 0)
                        {
                            cmd.CommandText = "Select * from Personnel where " + s_where;
                        }
                        else
                        { cmd.CommandText = "Select * from Personnel"; }
                    }
                    else
                    {cmd.CommandText = "Select * from Personnel";}
                    break;
                //新增
                case "insert":
                    cmd.Parameters.AddWithValue("@name", s_name);
                    cmd.Parameters.AddWithValue("@id", s_id);
                    cmd.Parameters.AddWithValue("@phone", s_phone);
                    cmd.Parameters.AddWithValue("@birthday",s_birday);
                    cmd.Parameters.AddWithValue("@gender", s_gender);
                    cmd.Parameters.AddWithValue("@passwd", s_passwd);
                    cmd.Parameters.AddWithValue("@rank", s_rank);

                    cmd.CommandText = "INSERT INTO Personnel([name],[id],[phone],[birthday],[gender],[passwd],[rank])" +
                                      "VALUES (@name,@id,@phone,@birthday,@gender,@passwd,@rank)" +
                                      "SELECT * from Personnel";
                    break;
                //修改
                case "update":
                    String s_set = null;
                    int i_hascomma = 0;

                    if (s_name.Length>0)
                    {
                        cmd.Parameters.AddWithValue("@name", s_name);
                        s_set = "name = @name";
                        i_hascomma++;
                    }
                    if (s_phone.Length>0)
                    {
                        cmd.Parameters.AddWithValue("@phone", s_phone);
                        if (i_hascomma > 0)
                        {
                            s_set += ",phone = @phone";
                        }
                        else
                        {
                            s_set = "phone = @phone";
                            i_hascomma++;
                        }
                    }
                    if (s_birday.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@birthday", s_birday);
                        if (i_hascomma > 0)
                        {
                            s_set += ",birthday = @birthday";
                        }
                        else
                        {
                            s_set = "birthday = @birthday";
                            i_hascomma++;
                        }
                    }
                    if (s_gender.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@gender", s_gender);
                        if (i_hascomma > 0)
                        {
                            s_set += ",gender = @gender";
                        }
                        else
                        {
                            s_set = "gender = @gender";
                            i_hascomma++;
                        }
                    }
                    if (s_rank.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@rank", s_rank);
                        if (i_hascomma > 0)
                        {
                            s_set += ",rank = @rank";
                        }
                        else
                        {
                            s_set = "rank = @rank";
                            i_hascomma++;
                        }
                    }
                    if (s_set != null)
                    {
                        if (s_set.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@id", s_id);
                            cmd.CommandText = "update Personnel set " + s_set + " where [id] = @id Select * from Personnel";
                        }
                        else
                        { cmd.CommandText = "Select * from Personnel"; }
                    }
                    else
                    {cmd.CommandText = "Select * from Personnel";}
                    break;
                default:
                    break;
            }

            cmd.Connection.Open(); //開啟資料庫連線

            da.SelectCommand = cmd; //執行
            da.Fill(dt); //結果存放至DataTable

            cmd.Connection.Close(); //關閉連線

            //將DataTable資料轉成JSON字串，日期格式轉yyyy-MM-dd
            string str_json = JsonConvert.SerializeObject(dt, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" });

            Response.Write(str_json);
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
    }
}