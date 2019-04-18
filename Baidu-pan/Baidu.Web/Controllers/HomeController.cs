using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Baidu.Web.Controllers
{
    
    public class HomeController : Controller
    {
        Services.Home service = new Services.Home();
        // GET: Home
        public ActionResult Index()
        {
            Session["Folder"] = "";
            ViewBag.Folder = service.GetFolderList(Convert.ToInt32(Request.Cookies["MyCook"]["UserID"]));
            return View(service.GetAll(Convert.ToInt32(Request.Cookies["MyCook"]["UserID"])));
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            List<string> paths = new List<string>();
            if (Request.Files.Count == 0)
            {
                //Directory.CreateDirectory("");
                //Request.Files.Count 文件数为0上传不成功
                return View();
            }

            //文件大小不为0
            HttpFileCollectionBase files = Request.Files;
            //取得目标文件夹的路径
            string target = Server.MapPath("~/Upload/");
            int cg = 0;
            int sb = -1;
            int x = 0;
            for (int i = 0; i < files.Count; i++)
            {
                
                if (files[i].ContentLength == 0)
                {

                    //文件大小（以字节为单位）为0时，做一些操作
                    //失败文件数，失败+1
                    sb = sb + 1;
                    //return Content("<script>alert('文件大小为0，上传失败')</script>");
                }
                else
                {
                    string filename = files[i].FileName;//取得文件名字
                    //判断上传文件是否包含文件夹，含/就是包含文件夹路径的
                    if (filename.Contains("/"))
                    {
                        //根据/分割，以便将文件名与文件夹路径分开
                        string[] b2 = filename.Split('/');
                        //获得文件名
                        string wenjian = b2[b2.Length - 1];
                        //将数组转为泛型集合操作
                        List<string> newb2 = new List<string>(b2);
                        //排除集合最后一个元素，也就是文件名，得到文件夹路径
                        newb2.Remove(newb2.Last<string>());
                        //定义一个文件夹路径变量
                        string lujin = null;
                        //遍历去除文件名的文件路径集合
                        
                        foreach (var item in newb2)
                        {
                            //拼接文件路径
                            lujin += item + "\\";
                            if (x > 0)
                            {

                                Model.Folder folder = new Model.Folder
                                {
                                    FounderID = Convert.ToInt32(Request.Cookies["MyCook"]["UserID"]),
                                    Folder_Name = item,
                                    FatherFolderID = service.GetNewFolderId(Convert.ToInt32(Request.Cookies["MyCook"]["UserID"])),
                                    DeleteState = 0,
                                    EstablishTime = DateTime.Now
                                };
                                if (service.GetFolder(item) == null)
                                {
                                    service.InsertFolder(folder);
                                    x = x + 1;
                                }
                               
                            }
                            else {
                                Model.Folder folder = new Model.Folder
                                {
                                    FounderID = Convert.ToInt32(Request.Cookies["MyCook"]["UserID"]),
                                    Folder_Name = item,
                                    FatherFolderID = 0,
                                    DeleteState = 0,
                                    EstablishTime = DateTime.Now
                                };
                                if (service.GetFolder(item) == null)
                                {
                                    service.InsertFolder(folder);
                                    x = x + 1;
                                }
                               
                   
                            }
                            //E:\C#作业\Baidu-pan\Baidu.Web\Upload\文件夹上传测试文档\测试2\5c70e0a0-90a0-4d95-b7c9-c95284f41ee2.txt
                            
                           
                        }
                        //创建文件夹路径
                        Directory.CreateDirectory(target + lujin);

                        string newfilename=filename;

                        //使用GUID 全球唯一标识符给文件重名称：GUID + 后缀名
                        newfilename = Guid.NewGuid() + filename.Substring(filename.LastIndexOf("."));

                        //获取存储的目标地址，也就是文件夹位置
                        string path = target+lujin + newfilename;
                        //保存文件
                        files[i].SaveAs(path);
                        //成功上传文件数，成功+1
                        cg = cg + 1;
                        paths.Add(path);

                                      
                        //将文件信息保存到数据库
                        Model.Store_data data = new Model.Store_data 
                        {
                            User_ID = Convert.ToInt32(Request.Cookies["MyCook"]["UserID"]),
                            DataRoute = path,
                            Real_FileName = wenjian,
                            File_Size = files[i].ContentLength.ToString(),
                            Folder_ID = service.GetNewFolderId(Convert.ToInt32(Request.Cookies["MyCook"]["UserID"])),
                            EstablishTime=DateTime.Now,
                            DeleteState=0,
                            SuffixName = filename.Substring(filename.LastIndexOf("."))
                            
                        };
                        service.Insert(data);
                       
                    }
                    else
                    {
                        string newfilename = filename;

                        //使用GUID 全球唯一标识符给文件重名称：GUID + 后缀名
                        newfilename = Guid.NewGuid() + filename.Substring(filename.LastIndexOf("."));

                        //获取存储的目标地址，也就是文件夹位置
                        string path = target + newfilename;
                        files[i].SaveAs(path);
                        cg = cg + 1;
                        paths.Add(path);

                        //将文件信息保存到数据库
                        Model.Store_data data = new Model.Store_data
                        {
                            User_ID = Convert.ToInt32(Request.Cookies["MyCook"]["UserID"]),
                            DataRoute = path,
                            Real_FileName = filename,
                            File_Size = files[i].ContentLength.ToString(),
                            Folder_ID = 0,
                            EstablishTime = DateTime.Now,
                            DeleteState = 0,
                            SuffixName = filename.Substring(filename.LastIndexOf("."))

                        };

                        service.Insert(data);
                       
                    }
                }

            }

            return Content(@"<script> alert('成功" + cg + "个，失败" + sb + "个'); window.location.href = '/Home/Index'; </script>");
        }

        public ActionResult Login()
        {
            HttpCookie cok = Request.Cookies["MyCook"];
            cok.Values.Remove("UserID");//移除键值为userid的值
            return RedirectToAction("Index", "Denglu");
        }

        public ActionResult OpenFolder() {
            string FolderName = Request.QueryString["FolderName"];
            int FolderId = service.GetFolderByName(FolderName, Convert.ToInt32(Request.Cookies["MyCook"]["UserID"])).Folder_ID;
            ViewBag.Folder = service.GetFolderByID(FolderId);
            if (Session["Folder"] == null)
            {
                Session["Folder"] = FolderName+"/";
            }
            else { 
                string odd=Session["Folder"].ToString();
                string[] pan = odd.Split('/');
                int jg = 0;
                for (int i = 0; i < pan.Length; i++)
                {
                    if (FolderName.Equals(pan[i]))
                    {
                        jg = jg + 1;
                    }
                } 
                if (jg == 0)
                {
                    Session["Folder"] = odd + "/" + FolderName;
                }
                else {
                    int lenght = FolderName.Length;
                    string newFolder = null;
                    for (int i = 0; i < pan.Length; i++)
                    {
                        if (pan[i].Length != lenght)
                        {
                            newFolder = pan[i] + "/";
                        }
                        else {
                            break;
                        }
                    }

                    newFolder = newFolder + FolderName + "/";
                    Session["Folder"] = newFolder;
                }
                
            }
            string OddFolder = Session["Folder"].ToString();
            string[] Folders=OddFolder.Split('/');
            List<string> FolderList = new List<string>(Folders);
            ViewBag.FatherFolderName = FolderList;
            return View(service.GetShareDataByID(FolderId));
        
        }

        public ActionResult OpenFolder2(string FolderName)
        {
            Model.Folder Folder = service.GetFolderByName(FolderName, Convert.ToInt32(Request.Cookies["MyCook"]["UserID"]));
            var list  = service.GetFolderByID(Folder.Folder_ID);
            
            var list1 = list.Select(x=>new {x.Folder_ID,x.Folder_Name,EstablishTime= x.EstablishTime.ToString("yyyy-MM-dd")});
            return Json(list1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OpenFolder3(string FolderName)
        {
            Model.Folder Folder = service.GetFolderByName(FolderName, Convert.ToInt32(Request.Cookies["MyCook"]["UserID"]));
            var list = service.GetShareDataByID(Folder.Folder_ID);

            var list1 = list.Select(x => new { x.Store_data_ID, x.Real_FileName,x.File_Size, EstablishTime = x.EstablishTime.ToString("yyyy-MM-dd") });
            return Json(list1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Download() {
            string fileID = Request.QueryString["id"];//取得文件id
            Model.Store_data data = service.GetFile(Convert.ToInt32(fileID));
            return File(new FileStream(data.DataRoute, FileMode.Open), "text/plain",
            data.Real_FileName);
        }
    }
}