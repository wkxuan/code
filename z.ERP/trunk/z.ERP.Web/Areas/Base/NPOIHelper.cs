using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using z.Extensions;

namespace z.ERP.Web.Areas.Base
{

    public class NPOIHelper
    {
        #region Excel
        /// <summary>
        /// 导出列名
        /// </summary>
        public static System.Collections.SortedList ListColumnsName;
        public static string baseDir;
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="filePath"></param>
        public static string ExportExcel(DataTable dtSource,string fileName, Dictionary<string, string> cols)
        {        
            string filePath = $@"{fileName}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xls";
            baseDir = $@"{IOExtension.GetBaesDir()}\File\Output\";
            IOExtension.MakeDir(true, baseDir);
            string outpath = baseDir + filePath;
            DeleteFolder(baseDir);
            CreateCols(cols);

            if (ListColumnsName == null || ListColumnsName.Count == 0)
                throw (new Exception("请对ListColumnsName设置要导出的列明！"));

            HSSFWorkbook excelWorkbook = CreateExcelFile();
            InsertRow(dtSource, excelWorkbook);
            SaveExcelFile(excelWorkbook, outpath);

            return "/File/Output/" + filePath;
        }
        /// <summary>
        /// 保存Excel文件
        /// </summary>
        /// <param name="excelWorkBook"></param>
        /// <param name="filePath"></param>
        protected static void SaveExcelFile(HSSFWorkbook excelWorkBook, string filePath)
        {
            FileStream file = null;
            try
            {   
                file = new FileStream(filePath, FileMode.Create);
                excelWorkBook.Write(file);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }
        /// <summary>
        /// 创建Excel文件
        /// </summary>
        /// <param name="filePath"></param>
        protected static HSSFWorkbook CreateExcelFile()
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            return hssfworkbook;
        }
        /// <summary>
        /// 创建excel表头
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="excelSheet"></param>
        protected static void CreateHeader(ISheet excelSheet)
        {
            int cellIndex = 0;
            IRow newRow = excelSheet.CreateRow(0);
            //循环导出列
            foreach (System.Collections.DictionaryEntry de in ListColumnsName)
            {
                NPOI.SS.UserModel.ICell newCell = newRow.CreateCell(cellIndex);
                newCell.SetCellValue(de.Value.ToString());
                cellIndex++;
            }
        }
        /// <summary>
        /// 创建excel表头内容
        /// </summary>
        protected static void CreateCols(Dictionary<string, string> cols)
        {
            cols.Remove("undefined");
            var list = cols.Keys;
            ListColumnsName = new SortedList(new NoSort());
            foreach(var item in list)
            {
                ListColumnsName.Add(item, cols[item]);
            }
        }
        /// <summary>
        /// 插入数据行
        /// </summary>
        protected static void InsertRow(DataTable dtSource, HSSFWorkbook excelWorkbook)
        {
            int rowCount = 0;
            int sheetCount = 1;
            ISheet newsheet = excelWorkbook.CreateSheet("数据"+ sheetCount);
            //创建表头
            CreateHeader(newsheet);
            //循环数据源导出数据集
            foreach (DataRow dr in dtSource.Rows)
            {
                rowCount++;
                //超出10000条数据 创建新的工作簿
                if (rowCount == 10000)
                {
                    rowCount = 1;
                    sheetCount++;
                    newsheet = excelWorkbook.CreateSheet("数据" + sheetCount);
                    CreateHeader(newsheet);
                }

                IRow newRow = newsheet.CreateRow(rowCount);
                InsertCell(dtSource, dr, newRow, newsheet, excelWorkbook);
            }
        }
        /// <summary>
        /// 导出数据行
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="drSource"></param>
        /// <param name="currentExcelRow"></param>
        /// <param name="excelSheet"></param>
        /// <param name="excelWorkBook"></param>
        protected static void InsertCell(DataTable dtSource, DataRow drSource, IRow currentExcelRow, ISheet excelSheet, HSSFWorkbook excelWorkBook)
        {
            for (int cellIndex = 0; cellIndex < ListColumnsName.Count; cellIndex++)
            {
                //列名称
                string columnsName = ListColumnsName.GetKey(cellIndex).ToString();
                NPOI.SS.UserModel.ICell newCell = null;
                System.Type rowType = drSource[columnsName].GetType();
                string drValue = drSource[columnsName].ToString().Trim();
                switch (rowType.ToString())
                {
                    case "System.String"://字符串类型
                        drValue = drValue.Replace("&", "&");
                        drValue = drValue.Replace(">", ">");
                        drValue = drValue.Replace("<", "<");
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(drValue);
                        break;
                    case "System.DateTime"://日期类型
                        DateTime dateV;
                        DateTime.TryParse(drValue, out dateV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(dateV);

                        //格式化显示
                        ICellStyle cellStyle = excelWorkBook.CreateCellStyle();
                        IDataFormat format = excelWorkBook.CreateDataFormat();
                        cellStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:mm:ss");
                        newCell.CellStyle = cellStyle;
                        break;
                    case "System.Boolean"://布尔型
                        bool boolV = false;
                        bool.TryParse(drValue, out boolV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(boolV);
                        break;
                    case "System.Int16"://整型
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Byte":
                        int intV = 0;
                        int.TryParse(drValue, out intV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(intV.ToString());
                        break;
                    case "System.Decimal"://浮点型
                    case "System.Double":
                        double doubV = 0;
                        double.TryParse(drValue, out doubV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(doubV);
                        break;
                    case "System.DBNull"://空值处理
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue("");
                        break;
                    default:
                        throw (new Exception(rowType.ToString() + "：类型数据无法处理!"));
                }
            }
        }
        /// <summary>
        /// 删除指定文件加下的文件（删除文件创建是当前日期前的文件）
        /// </summary>
        protected static void DeleteFolder(string dir)
        {
            var nowDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    var fileCreateDate = Convert.ToDateTime(fi.CreationTime.ToShortDateString());
                    if(nowDate> fileCreateDate)
                    {
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                        File.Delete(d);//直接删除其中的文件  
                    }
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        DeleteFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }
        #endregion
       
    }
    public class NoSort : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            return -1;
        }
    }
}