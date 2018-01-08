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
    }
}
