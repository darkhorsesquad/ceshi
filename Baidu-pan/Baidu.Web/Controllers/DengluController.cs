using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Baidu.Web.Controllers
{
    public class DengluController : Controller
    {

        Services.Home Home = new Services.Home();
        // GET: Denglu
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Model.User user)
        {
            Model.User User=Home.GetUser(user.User_Name,user.User_Password);
            if(User!=null){
                HttpCookie cookie = new HttpCookie("MyCook");
                cookie.Values.Add("UserID",User.User_ID.ToString());
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index","Home");
            }
            return View();
        }
    }
}