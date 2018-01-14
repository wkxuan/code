using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;
using z.Extensions;
using z.Extensiont;

namespace z.ERP.Model.Vue
{
    public class TreeModel
    {
        public string code
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string title
        {
            get; set;
        }

        public bool expand
        {
            get; set;
        }

        public TreeModel[] children
        {
            get; set;
        }

        public bool disabled
        {
            get; set;
        }

        public bool disableCheckbox
        {
            get; set;
        }
        public bool selected
        {
            get; set;
        }
        public bool @checked
        {
            get; set;
        }

        /// <summary>
        /// 生成树
        /// </summary>
        /// <typeparam name="TEntity">数据类型</typeparam>
        /// <param name="infos">数据</param>
        /// <param name="code">code字段</param>
        /// <param name="func">转换方法</param>
        /// <returns></returns>
        public static IEnumerable<TreeModel> Create<TEntity>(List<TEntity> infos, Expression<Func<TEntity, string>> code, Func<TEntity, TreeModel> func) where TEntity : EntityBase
        {
            TEntity ft = infos.FirstOrDefault(info => code.Compile()(info).Length < 2);
            if (ft != null)
            {
                var thist = func(ft);
                thist.children = _Create(infos, code, func);
                return new List<TreeModel>() { thist };
            }
            else
            {
                return _Create(infos, code, func);
            }
        }

        static TreeModel[] _Create<TEntity>(List<TEntity> infos, Expression<Func<TEntity, string>> code, Func<TEntity, TreeModel> func, string pcode = null) where TEntity : EntityBase
        {
            if (infos.IsEmpty())
                return null;
            return infos.Where(a => pcode == null ? code.Compile()(a).Length == 2 : code.Compile()(a).LeftLike(pcode) && code.Compile()(a).Length == pcode.Length + 2).Select(a =>
            {
                var v = func(a);
                v.children = _Create(infos, code, func, v.code);
                return v;
            }).ToArray();
        }

        public static string GetNewKey<TEntity>(List<TEntity> infos, Expression<Func<TEntity, string>> code, string key, string Tar) where TEntity : EntityBase
        {
            if (Tar.ToLower() == "tj")
            {
                List<string> keys = infos.Where(info => code.Compile()(info).LeftLike(key.Substring(0, key.Length - 2)) && code.Compile()(info).Length == key.Length).Select(info => code.Compile()(info)).ToList();
                if (keys.IsEmpty())
                    return "01";
                else
                {
                    return _NewKey(keys);
                }
            }
            else if (Tar.ToLower() == "xj")
            {
                List<string> keys = infos.Where(info => code.Compile()(info).LeftLike(key) && code.Compile()(info).Length == key.Length + 2).Select(info => code.Compile()(info)).ToList();
                if (keys.IsEmpty())
                    return key + "01";
                else
                {
                    return _NewKey(keys);
                }
            }
            else
            {
                return key;
            }
        }

        public static string _NewKey(List<string> keys)
        {
            if (keys.IsEmpty())
                return "";
            else
            {
                string left = keys.FirstOrDefault().CutRight(2);
                int k = 1;
                keys.Select(a => a.Substring(a.Length - 2).ToInt()).OrderBy(a => a).ForEachWithBreak(a =>
                {
                    if (a == k)
                    {
                        k++;
                        return true;
                    }
                    else
                        return false;
                });
                return left + k.ToString().FillLeft('0', 2);
            }
        }
    }

}
