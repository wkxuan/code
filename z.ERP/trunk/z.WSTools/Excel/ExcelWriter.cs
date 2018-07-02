using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace z.WSTools.Excel
{
    public class ExcelWriter
    {
        public static string imgTrim = "{{img}}";  //图片标志
        public static string LinkTrim = "{{Link}}";  //超链接标志
        public static string LinkTrim_Title = "{{}}";  //超链接标志
        string _filepath = "";
        string _targetpath = "";
        WorkbookDesigner designer = new WorkbookDesigner();
        List<Action<WorkbookDesigner>> otherFuns = new List<Action<WorkbookDesigner>>();
        bool isOpen = false;
        public ExcelWriter(string filepath, string targetpath)
        {
            _filepath = filepath;
            _targetpath = targetpath;
            designer.Open(_filepath);
            isOpen = true;
        }

        /// <summary>
        /// 设置单个项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="str"></param>
        public void SetString(string name, string str)
        {
            if (!isOpen)
            {
                throw new Exception("表格已保存,不能编辑了");
            }
            designer.SetDataSource(name, str);
        }

        /// <summary>
        /// 设置循环项
        /// </summary>
        /// <param name="dt"></param>
        public void SetTable(DataTable dt)
        {
            if (!isOpen)
            {
                throw new Exception("表格已保存,不能编辑了");
            }
            designer.SetDataSource(dt);
        }

        /// <summary>
        /// 设置循环项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Infos"></param>
        /// <param name="name"></param>
        /// <param name="funcs">对每个项有额外的处理方式</param>
        public void SetInfos<T>(IList<T> Infos, string name = "table", Dictionary<string, Func<T, string>> funcs = null) where T : class
        {
            List<PropertyInfo> PropertyList = Infos[0].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            DataTable dt = new DataTable(name);
            foreach (PropertyInfo item in PropertyList)
            {
                dt.Columns.Add(item.Name);
            }
            if (funcs != null)
            {
                foreach (var v in funcs)
                {
                    if (!dt.Columns.Contains(v.Key))
                    {
                        dt.Columns.Add(v.Key);
                    }
                }
            }
            foreach (var info in Infos)
            {
                DataRow dr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    PropertyInfo item = PropertyList.FirstOrDefault(a => a.Name == dc.ColumnName);
                    Func<T, string> f = funcs?.FirstOrDefault(a => a.Key == dc.ColumnName).Value;
                    if (item != null)
                    {
                        var obj = item.GetValue(info, null);
                        if (obj != null)
                        {
                            dr[dc.ColumnName] = obj.ToString();
                        }
                    }
                    if (f != null)
                    {
                        dr[dc.ColumnName] = f(info);
                    }
                }
                dt.Rows.Add(dr);
            }
            SetTable(dt);
        }

        /// <summary>
        /// 做一些其他事情
        /// </summary>
        /// <param name="fun"></param>
        public void OtherDo(Action<WorkbookDesigner> fun)
        {
            if (!isOpen)
            {
                throw new Exception("表格已保存,不能编辑了");
            }
            otherFuns.Add(fun);
        }

        /// <summary>
        /// 处理
        /// </summary>
        public void Process()
        {
            designer.Process();
            //处理图片/超链接
            foreach (Worksheet ws in designer.Workbook.Worksheets)//工作表
            {
                foreach (Cell c in ws.Cells)//行
                {
                    if (!c.IsMerged)
                    {
                        string s = c.StringValue;
                        if (s.StartsWith(imgTrim)) //图片的处理
                        {
                            string url = c.StringValue.Replace(imgTrim, "");
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(url))
                                {
                                    System.IO.Stream objImage;
                                    System.Net.WebClient objwebClient;
                                    objwebClient = new System.Net.WebClient();
                                    objImage = new System.IO.MemoryStream(objwebClient.DownloadData(url));
                                    double cw = ws.Cells.GetColumnWidth(c.Column);
                                    double rh = ws.Cells.GetRowHeight(c.Row);
                                    Aspose.Cells.Drawing.PictureCollection pictures = ws.Pictures;
                                    pictures.Add(c.Row, c.Column, c.Row + 1, c.Column + 1, objImage);
                                }
                                c.PutValue("");
                            }
                            catch
                            {
                                c.PutValue("图片" + url + "下载失败");
                            }
                        }
                        if (s.StartsWith(LinkTrim)) //超链接的处理
                        {
                            string str = c.StringValue.Replace(LinkTrim, "");
                            string title, url;
                            string[] arr = str.Split(new string[] { LinkTrim_Title }, StringSplitOptions.RemoveEmptyEntries);
                            if (arr.Length == 2)
                            {
                                title = arr[0];
                                url = arr[1];
                            }
                            else
                            {
                                title = url = str;
                            }
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(url))
                                {
                                    int intIndex = ws.Hyperlinks.Add(c.Row, c.Column, c.Row + 1, c.Column + 1, url);
                                    Hyperlink hlink = ws.Hyperlinks[intIndex];
                                    hlink.TextToDisplay = title;
                                }
                            }
                            catch
                            {
                                c.PutValue(title);
                            }
                        }
                    }
                }
            }
            //额外处理
            if (otherFuns != null && otherFuns.Count > 0)
            {
                otherFuns.ForEach(a => a(designer));
            }
            designer.Workbook.Save(_targetpath);
            isOpen = false;
        }
    }
}
