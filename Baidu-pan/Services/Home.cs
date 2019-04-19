using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Home
    {
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="UserPwd"></param>
        /// <returns></returns>
        public Model.User GetUser(string UserName,string UserPwd) { 
            using(var db=new Model.BaiduSkyDriveEntities()){
                return db.Set<Model.User>().FirstOrDefault(x=>x.User_Name==UserName && x.User_Password==UserPwd);
            }
        }

       

        public List<Model.Folder> GetFolderList(int UserId) {
            using (var db = new Model.BaiduSkyDriveEntities()) {
                return db.Set<Model.Folder>()
                    .Where(x => x.FounderID == UserId && x.FatherFolderID==0)
                    .OrderByDescending(x=>x.EstablishTime)
                    .ToList();
            }
        }



        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<Model.Store_data> GetAll(int UserID) { 
            using(var db=new  Model.BaiduSkyDriveEntities()){
                return db.Set<Model.Store_data>()
                    .Where(x => x.User_ID == UserID && x.Folder_ID==0)
                    .OrderByDescending(x=>x.EstablishTime)
                    .ToList();
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Insert(Model.Store_data data) {
            using (var db = new Model.BaiduSkyDriveEntities()) {
                db.Entry<Model.Store_data>(data).State = System.Data.Entity.EntityState.Added;
                return db.SaveChanges();
            }
        }


        /// <summary>
        /// 根据文件id查询文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.Store_data GetFile(int id) {
            using (var db = new Model.BaiduSkyDriveEntities()) {
                return db.Set<Model.Store_data>().Find(id);
            }
        }


        /// <summary>
        /// 新增文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public int InsertFolder(Model.Folder folder) {
            using (var db = new Model.BaiduSkyDriveEntities()) {
                db.Entry<Model.Folder>(folder).State = System.Data.Entity.EntityState.Added;
                return db.SaveChanges();
            }
        }

       
        public int GetNewFolderId(int UserId) {
            using (var db = new Model.BaiduSkyDriveEntities()) {
                return db.Set<Model.Folder>().Where(x => x.FounderID == UserId).Max(x => x.Folder_ID);
            }
        }

        public Model.Folder GetFolder(string FolderName) {
            using (var db = new Model.BaiduSkyDriveEntities()) {
                return db.Set<Model.Folder>().FirstOrDefault(x => x.Folder_Name == FolderName);
            }                                                         
        }

        /// <summary>
        /// 根据文件名查询文件夹
        /// </summary>
        /// <param name="FolderName"></param>
        /// <returns></returns>
        public Model.Folder GetFolderByName(string FolderName,int UserId) {
            using (var db = new Model.BaiduSkyDriveEntities()) {
                return db.Set<Model.Folder>().FirstOrDefault(x => x.Folder_Name == FolderName && x.FounderID==UserId);
            }
        }

        /// <summary>
        /// 根据父ID查询文件夹
        /// </summary>
        /// <param name="Folder"></param>
        /// <returns></returns>
        public List<Model.Folder> GetFolderByID(int FolderID)
        {
            var db = new Model.BaiduSkyDriveEntities();
            
                return db.Set<Model.Folder>()
                    .Where(x => x.FatherFolderID == FolderID)
                    .OrderByDescending(x => x.EstablishTime)
                    .ToList();
            
            
        }

        /// <summary>
        /// 根据文件夹id查询文件
        /// </summary>
        /// <param name="FolderID"></param>
        /// <returns></returns>
        public List<Model.Store_data> GetShareDataByID(int FolderID)
        {
            using (var db = new Model.BaiduSkyDriveEntities())
            {
                return db.Set<Model.Store_data>()
                    .Where(x => x.Folder_ID == FolderID)
                    .OrderByDescending(x => x.EstablishTime)
                    .ToList();
            }
        }
    }
}
