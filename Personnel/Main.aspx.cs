using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ////清除瀏覽器快取(?
        ////避免登出後按上一頁仍可看到資料
        //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //HttpContext.Current.Response.Cache.SetNoServerCaching();
        //HttpContext.Current.Response.Cache.SetNoStore();
        ////判斷是否正常登入
        //if ((Session["Login"] == null) || ((Session["Login"].ToString() != "OK")))
        //{
        //    Response.Write("請正常登入");
        //    div.Visible = false;
        //}
        //else
        //{
        //    //檢查網頁是否第一次執行
        //    if (!Page.IsPostBack)
        //    {
        //        //一般職員隱藏增修刪功能
        //        if (Session["rank"].ToString() == "1")
        //        {
        //            div1.Visible = false;
        //        }
        //    }
        //}
    }
}