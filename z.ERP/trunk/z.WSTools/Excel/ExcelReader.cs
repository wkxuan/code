using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.WSTools.Excel
{
    public class ExcelReader
    {
        string _filepath;
        public ExcelReader(string filepath)
        {
            _filepath = filepath;
        }

        /// <summary>
        /// 读取表格,只读取第一个sheet
        /// </summary>
        /// <param name="TitleRow">标题所在的行,就是excel里的行,从0开始,如果有多行,取起始行</param>
        /// <param name="TitleHeigh">标题行如果是多行,有几行</param>
        /// <returns></returns>
        public DataTable ReadTable(int TitleRow, int TitleHeigh = 1)
        {
            DataTable dt = new DataTable();
            Workbook workbook = new Workbook(_filepath);
            Cells cells = workbook.Worksheets[0].Cells;
            if (cells.MaxDataRow < TitleRow)
            {
                throw new Exception("找不到标题行");
            }
            for (int i = 0; i < cells.MaxDataColumn + 1; i++)
            {
                string title = "";
                for (int j = 0; j < TitleHeigh; j++)
                {
                    string s = cells[TitleRow - 1 + j, i]?.StringValue?.Trim();
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        title = s;
                    }
                }
                dt.Columns.Add(title);
            }
            for (int i = TitleRow + TitleHeigh - 1; i < cells.MaxDataRow + 1; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < cells.MaxDataColumn + 1; j++)
                {
                    dr[j] = cells[i, j]?.StringValue?.Trim();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 读并且一行一行的执行,执行成功的数据删掉,剩下的留着返回,只读取第一个sheet
        /// </summary>
        /// <param name="newFilePath">返回的文件的地址</param>
        /// <param name="Process">一行一行的执行,字典里是这一行的数据,返回执行是否成功,执行成功会删掉数据,不要抛出异常</param>
        /// <param name="TitleRow">标题所在的行,就是excel里的实际行,如果有多行,取起始行</param>
        /// <param name="TitleHeigh">标题行如果是多行,有几行</param>
        /// <param name="AllData">在Process前给出所有数据</param>
        /// <returns>是否有执行失败的数据</returns>
        public bool ReadAndProcess(string newFilePath, Func<Dictionary<string, string>, bool> Process, int TitleRow, int TitleHeigh = 1,
           Action<DataTable> AllData = null)
        {
            bool res = true;
            Dictionary<string, string> FuncDic = new Dictionary<string, string>();
            List<string> titleList = new List<string>();
            List<int> DeleteRowList = new List<int>();
            Workbook workbook = new Workbook(_filepath);
            Cells cells = workbook.Worksheets[0].Cells;
            if (cells.MaxDataRow < TitleRow)
            {
                throw new Exception("找不到标题行");
            }
            //填充标题
            for (int i = 0; i < cells.MaxDataColumn + 1; i++)
            {
                string title = "";
                for (int j = 0; j < TitleHeigh; j++)
                {
                    string s = cells[TitleRow - 1 + j, i].StringValue.Trim();
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        title = s;
                    }
                }
                titleList.Add(title);
                if (!FuncDic.ContainsKey(title))
                {
                    FuncDic.Add(title, "");
                }
            }
            if (TitleRow + TitleHeigh - 1 >= cells.MaxDataRow + 1)
            {
                throw new Exception("没有找到数据");
            }
            if (AllData != null)
            {
                DataTable dt = cells.ExportDataTableAsString(TitleRow + TitleHeigh - 2, 0, cells.MaxDataRow - TitleHeigh + 1, cells.MaxColumn + 1, true);
                AllData(dt);
            }
            //遍历每一行,调用处理方法
            for (int i = TitleRow + TitleHeigh - 1; i < cells.MaxDataRow + 1; i++)
            {
                for (int j = 0; j < cells.MaxDataColumn + 1; j++)
                {
                    FuncDic[titleList[j]] = cells[i, j].StringValue.Trim();
                }
                if (Process != null)
                {
                    if (Process(FuncDic))
                    {
                        DeleteRowList.Add(i);
                    }
                    else
                    {
                        res = false; //有一个不用删的,就说明还有数据
                    }
                }
            }
            DeleteRowList.Sort((a, b) => b.CompareTo(a));
            DeleteRowList.ForEach(a =>
            {
                cells.DeleteRow(a);
            });
            workbook.Save(newFilePath);
            return res;
        }

        /// <summary>
        /// 读取excel的一个格子
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <returns></returns>
        public string ReadOneField(int row, int col)
        {
            DataTable dt = new DataTable();
            Workbook workbook = new Workbook(_filepath);
            Cells cells = workbook.Worksheets[0].Cells;
            if (cells.MaxDataRow < row)
            {
                throw new Exception("没有那么多行");
            }
            if (cells.MaxDataColumn < col)
            {
                throw new Exception("没有那么多列");
            }
            return cells[row, col]?.StringValue?.Trim();
        }
    }
}
