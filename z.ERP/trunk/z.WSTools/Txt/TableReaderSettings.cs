using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Extensions;

namespace z.WSTools.Txt
{
    public class TableReaderSettings : TxtReaderSettings
    {
        /// <summary>
        /// 每一行的分隔符
        /// </summary>
        public string[] RowSplit
        {
            get;
            set;
        }

        /// <summary>
        /// 每一列的分隔符
        /// </summary>
        public string[] ColumnSplit
        {
            get; set;
        }

        /// <summary>
        /// 每一行的标题
        /// 1开始
        /// </summary>
        public Dictionary<int, string> RowSettings
        {
            get;
            set;
        }

        public override DataTable ReadTable()
        {
            DataTable dt = new DataTable();
            RowSettings.OrderBy(a => a.Key).ForEach(rs =>
            {
                dt.Columns.Add(rs.Value);
            });
            string str = ReadText();
            string[] rows = str.Split(RowSplit, StringSplitOptions.None);
            rows.ForEach(r =>
            {
                if (r.IsEmpty())
                    return;
                DataRow dr = dt.NewRow();
                string[] cs = r.Split(ColumnSplit, StringSplitOptions.None);
                if (cs.Length != dt.Columns.Count)
                    throw new Exception($"数据行{r}元素个数为{cs.Length},定义个数为{dt.Columns.Count},不符");
                cs.ForEach2((c, i) =>
                {
                    dr[i] = c;
                });
                dt.Rows.Add(dr);
            });
            return dt;
        }
    }
}
