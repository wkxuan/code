using NewtonsoftCode.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using z;
using z.Exceptions;
using z.Extensions;
using z.Results;

namespace z.Extensions
{
    public static class DataTableExtension
    {
        /// <summary>
        /// 表非空
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsNotNull(this DataTable dt)
        {
            if (dt == null)
                return false;
            if (dt.Rows.Count == 0)
                return false;
            return true;
        }
        /// <summary>
        /// 表只有一行
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsOneLine(this DataTable dt)
        {
            if (IsNotNull(dt))
            {
                if (dt.Rows.Count != 1)
                    return false;
                string str = "";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    str += dt.Rows[0][i].ToString();
                }
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 为表格创建一个枚举文字的列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="enumname"></param>
        /// <param name="newname"></param>
        public static void NewEnumColumns<T>(this DataTable dt, string enumname, string newname) where T : struct
        {
            if (dt.Columns.Contains(newname))
            {
                throw new Exception("表中已包含指定项");
            }
            if (!dt.Columns.Contains(enumname))
            {
                throw new Exception("表中不包含指定枚举项");
            }
            dt.Columns.Add(newname);
            foreach (DataRow dr in dt.Rows)
            {
                var v = EnumExtension.EnumToDictionary<T>();
                if (v.ContainsKey(dr[enumname].ToString().ToInt()))
                {
                    dr[newname] = v[dr[enumname].ToString().ToInt()];
                }
                else
                {
                    dr[newname] = "未知";
                }
            }
        }

        /// <summary>
        /// 转化表格为对象数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class
        {
            if (typeof(T) == typeof(string))
            {
                return dt.Select().Select(a => a[0].ToString() as T).ToList();
            }
            return dt.ToJson().ToObj<List<T>>();
        }

        /// <summary>
        /// 表格下拉
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="IdKey"></param>
        /// <param name="ValueKey"></param>
        /// <param name="IsSelect"></param>
        /// <returns></returns>
        public static List<SelectItem> ToSelectItem(this DataTable dt, string IdKey, string ValueKey, Func<DataRow, bool> IsSelect = null, Func<DataRow, string> MoreObj = null)
        {
            if (!dt.Columns.Contains(IdKey))
            {
                throw new Exception($"字段{IdKey}不在表中");
            }
            if (!dt.Columns.Contains(ValueKey))
            {
                throw new Exception($"字段{ValueKey}不在表中");
            }
            List<SelectItem> res = new List<SelectItem>();
            foreach (DataRow dr in dt.Rows)
            {
                bool isselect = false;
                if (IsSelect != null)
                {
                    isselect = IsSelect(dr);
                }
                string obj = null;
                if (MoreObj != null)
                {
                    obj = MoreObj(dr);
                }
                res.Add(new SelectItem()
                {
                    IsSelected = isselect,
                    Key = dr[IdKey].ToString(),
                    Value = dr[ValueKey].ToString(),
                    Obj = obj
                });
            }
            return res;
        }

        /// <summary>
        /// 将dt的第一行提取出来
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static dynamic ToOneLine(this DataTable dt)
        {
            return dt.ToList<dynamic>().FirstOrDefault();
        }
    }
}
