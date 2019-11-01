using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.Mvc;
using z.DBHelper.DBDomain;
using z.ERP.Services;
using z.Exceptions;
using z.LogFactory;
using z.MVC5.Results;
using z.SSO;
using z.SSO.Model;
using z.Verify;

namespace z.ERP.Web.Areas.Base
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            //_db = new OracleDbHelper(ConfigExtension.GetConfig("connection"));
            service = new ServiceBase();
            //service.SetDb(_db);
        }

        //DbHelperBase _db;

        /// <summary>
        /// 快速保存
        /// </summary>
        /// <param name="infos"></param>
        public IEnumerable<string> CommonSave(IEnumerable<TableEntityBase> infos)
        {
            return service.CommonService.CommonSave(infos);
        }

        /// <summary>
        /// 快速保存
        /// </summary>
        /// <param name="info"></param>
        public string CommonSave(TableEntityBase info)
        {
            return service.CommonService.CommonSave(info);
        }

        /// <summary>
        /// 快速删除
        /// </summary>
        /// <param name="infos"></param>
        public void CommenDelete(IEnumerable<TableEntityBase> infos)
        {
            service.CommonService.CommenDelete(infos);
        }

        /// <summary>
        /// 快速删除
        /// </summary>
        /// <param name="info"></param>
        public void CommenDelete(TableEntityBase info)
        {
            service.CommonService.CommenDelete(info);
        }

        public T Select<T>(T t) where T : TableEntityBase, new()
        {
            return service.CommonService.Select(t);
        }

        public List<T> SelectList<T>(T t) where T : TableEntityBase, new()
        {
            return service.CommonService.SelectList(t);
        }

        /// <summary>
        /// 获取对象的验证类
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EntityVerify<TEntity> GetVerify<TEntity>(TEntity entity) where TEntity : TableEntityBase
        {
            return service.GetVerify(entity);
        }
        /// <summary>
        /// 当前登录对象
        /// </summary>
        protected Employee employee
        {
            get
            {
                return UserApplication.GetUser<Employee>();
            }
        }
        protected ServiceBase service
        {
            get;
            set;
        }

        protected LogWriter Log
        {
            get
            {
                return new LogWriter("Controller");
            }
        }
        //验证功能按钮权限
        public UIResult CheckMenu(List<MenuAuthority> MenuAuthority)
        {
            var data = new List<MenuAuthority>();
            for (var i = 0; i < MenuAuthority.Count; i++)
            {
                var obj = new MenuAuthority();
                obj.id = MenuAuthority[i].id;
                obj.authority = MenuAuthority[i].authority;
                if (string.IsNullOrEmpty(MenuAuthority[i].authority)|| employee.HasPermission(MenuAuthority[i].authority))
                {
                    obj.enable = true;
                }
                else
                {
                    obj.enable = false;
                }
                data.Add(obj);
            }
            return new UIResult(data);
        }

        public string Import()
        {
            return SaveFileServer(); ;
        }
        public UIResult ImportExcel(string fileUrl)
        {
            var backData = new ImportMsg();

            try
            {
                DataTable dt = ExcelConversionDataTable(fileUrl);
                backData = ImportExcelDataHandle(dt);
            }
            catch (Exception exc)
            {
                backData.SuccFlag = false;
                backData.Message = exc.Message;
            }
            finally
            {
                if (System.IO.File.Exists(fileUrl))
                {
                    System.IO.File.Delete(fileUrl);
                }
            }
            return new UIResult(backData);
        }
        public virtual ImportMsg ImportExcelDataHandle(DataTable dt)
        {
            return new ImportMsg();
        }
        /// <summary>
        /// 将上传的文件保存到服务器指定位置
        /// </summary>
        /// <returns></returns>
        private static string SaveFileServer()
        {
            string filrUrl = "";
            System.Web.HttpFileCollection filelist = System.Web.HttpContext.Current.Request.Files;
            if (filelist != null && filelist.Count > 0)
            {
                for (int i = 0; i < filelist.Count; i++)
                {
                    HttpPostedFile file = filelist[i];
                    string filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    try
                    {
                        string dir = @"/File/Import/";
                        DirectoryInfo di = new DirectoryInfo(dir);
                        if (!di.Exists) { di.Create(); }
                        filrUrl = dir + filename;

                        file.SaveAs(filrUrl);
                    }
                    catch (Exception ex)
                    {
                        throw new LogicException("上传文件写入失败：" + ex.Message);
                    }
                }
            }
            else
            {
                throw new LogicException("上传的文件信息不存在！");
            }
            return filrUrl;
        }
        /// <summary>
        /// 将Excel转化成DataTable
        /// </summary>
        /// <returns></returns>
        private static DataTable ExcelConversionDataTable(string fileUrl)
        {
            string table = "TABLE";
            const string cmdText = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;\"";
            DataTable dt = null;
            //建立连接
            OleDbConnection conn = new OleDbConnection(string.Format(cmdText, fileUrl));
            try
            {
                //打开连接
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //获取Excel的第一个Sheet名称
                string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString().Trim();
                //查询sheet中的数据
                string strSql = "select * from [" + sheetName + "]";
                OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds, table);
                dt = ds.Tables[0];

                return dt;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}