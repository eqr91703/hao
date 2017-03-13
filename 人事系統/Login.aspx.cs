using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

public partial class Login : BasePage //這邊固定要繼承BasePage來取得公用的方法屬性以及事件
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void login_Click(object sender, EventArgs e)
    {

        SqlCommand cmd = new SqlCommand(); //宣告SqlCommand物件
        cmd.Connection = new SqlConnection(this.ConnStr);
        SqlDataReader dr = null; //宣告SqlDataReader物件，不用NEW初始值設定為NULL

        //以下try catch finally為固定寫法，且釋放資源必定要寫在finally裡
        //偵錯
        try
        {

            //使用SqlCommand物件的CommandText屬性設置SQL命令
            cmd.CommandText = "Select * from Personnel WHERE id=@id AND passwd=@passwd";
            //這是呼叫SP的方式，與上面的其實大同小異，可以當做是直接在SQL的查詢工具那邊下命令
            //cmd.CommandText = "EXEC pro_XXXXX_XXX @xxx";

            //參數化查詢條件，可以有效防止SQL injection(資料隱碼攻擊)
            //cmd.Parameters.Add("@id", SqlDbType.Int).Value = Request[id.Text];
            cmd.Parameters.AddWithValue("@id", id.Text);
            cmd.Parameters.AddWithValue("@passwd", passwd.Text);

            cmd.Connection.Open(); //開啟資料庫連線

            dr = cmd.ExecuteReader(); //執行命令

            if (!dr.Read())
            {
                notification.Text = "身份證字號或是密碼有錯！"; 
            }
            else 
            {
                Session["id"] = dr["id"].ToString();
                Session["rank"] = dr["rank"].ToString();
                Session["Login"] = "OK";   //--通過帳號與密碼的認證，就獲得 Session。
                Response.Redirect("Main.aspx");
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

}