using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using z.DbHelper.DbDomain;
using z.DBHelper.Helper;
using z.Extensions;
using z.Extensiont;
using z.SuperLambda;

namespace z.Verify
{
    public class EntityVerify<TEntity> : VerifyBase where TEntity : TableEntityBase
    {
        DbHelperBase _dbHelper;
        TEntity _entity;
        public EntityVerify(TEntity e)
        {
            _entity = e;
        }
        public void SetDb(DbHelperBase db)
        {
            _dbHelper = db;
        }

        /// <summary>
        /// 必输项
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ErrorModel"></param>
        public void Require(Expression<Func<TEntity, string>> p, string ErrorModel = "字段[{0}]不能为空")
        {
            //var aa = new
            //{
            //    a = 1,
            //    b = 2,
            //    c = new int[] { 1, 2, 3 }
            //};
            //aa.c.First();
            //Expression<Func<dynamic, int>> exp = LambdaParser.Parse<Func<dynamic, int>>("a=>a.c.First()");
            //exp.Compile()(aa);



            CommonString(p, StringExtension.IsNotEmpty, a => string.Format(ErrorModel, a));
        }

        /// <summary>
        /// 必须是数字,可以是空
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ErrorModel"></param>
        public void IsNumber(Expression<Func<TEntity, string>> p, string ErrorModel = "字段[{0}]必须是数字")
        {
            CommonString(p, a => a.IsEmpty() || a.IsNumber(), a => string.Format(ErrorModel, a));
        }

        /// <summary>
        /// 必须是数字,可以是空
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ErrorModel"></param>
        public void IsInt(Expression<Func<TEntity, string>> p, string ErrorModel = "字段[{0}]必须是整数")
        {

            CommonString(p, a => a.IsEmpty() || a.IsInt(), a => string.Format(ErrorModel, a));
        }

        /// <summary>
        /// 在表里必须是唯一的
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ErrorModel"></param>
        public void IsUnique(Expression<Func<TEntity, string>> p, int limit = 0, string ErrorModel = "[{0}]表中字段[{1}]的值[{2}]已存在")
        {
            if (p.Body is MemberExpression)
            {
                MemberExpression me = p.Body as MemberExpression;
                PropertyInfo prop = me.Member as PropertyInfo;
                string str = prop.GetValue(_entity)?.ToString().Trim();
                //if (_entity.HasAllPrimaryKey() && _dbHelper.Select(_entity) != null)
                //{
                //    TEntity rest = _dbHelper.Select(_entity);
                //    if (rest != null)
                //    {
                //        if (prop.GetValue(rest)?.ToString().Trim() == str)
                //        {
                //            limit++;
                //        }
                //    }
                //}
                string sql = $"select {_entity.GetPrimaryKey().Select(s => s.Name).ToArray().SuperJoin(",")} from {_entity.GetTableName()} where {me.Member.Name}='{str}'";
                List<TEntity> res = _dbHelper.ExecuteTable(sql).ToList<TEntity>();
                if (res.Where(a => !a.EqualWithPrimary(_entity)).Count() > limit)
                {
                    SetError(string.Format(ErrorModel, _entity.GetComments(), _entity.GetFieldName(p), str));
                }
            }
            else
                throw new Exception("此校验只对字段属性生效");
        }

        /// <summary>
        /// 外键约束
        /// </summary>
        /// <typeparam name="TTarget">目标表</typeparam>
        /// <param name="p">本表字段</param>
        /// <param name="t">目标表字段</param>
        /// <param name="ErrorModel">错误信息</param>
        public void IsForeignKey<TTarget>(Expression<Func<TEntity, string>> p, Expression<Func<TTarget, string>> t, string ErrorModel = "[{0}]表中字段[{1}]的值[{2}]在[{3}]表中的字段[{4}]已存在") where TTarget : TableEntityBase, new()
        {
            if (p.Body is MemberExpression && t.Body is MemberExpression)
            {
                MemberExpression tme = t.Body as MemberExpression;
                MemberExpression me = p.Body as MemberExpression;
                PropertyInfo prop = me.Member as PropertyInfo;
                string str = prop.GetValue(_entity)?.ToString().Trim();
                TTarget tar = (TTarget)Activator.CreateInstance(
                                 typeof(TTarget),
                                 BindingFlags.Instance | BindingFlags.Public,
                                 null,
                                 new object[] { },
                                 null);
                tar.SetPropertyValue(tme.Member.Name, prop.GetValue(_entity));
                if (!_dbHelper.SelectList(tar).IsEmpty())
                {
                    SetError(string.Format(ErrorModel, _entity.GetComments(), _entity.GetFieldName(p), str, tar.GetTableName(), tar.GetFieldName(t)));
                }
            }
            else
                throw new Exception("此校验只对字段属性生效");
        }

        #region 基础方法
        void CommonString(Expression<Func<TEntity, string>> p, Func<string, bool> verify, Func<string, string> geterror)
        {
            if (p.Body is MemberExpression)
            {
                MemberExpression me = p.Body as MemberExpression;
                PropertyInfo prop = me.Member as PropertyInfo;
                FieldAttribute f = prop.GetAttribute<FieldAttribute>();
                string fieldname = me.Member.Name;
                if (f != null)
                    fieldname = f.Fieldname;
                if (!verify(p.Compile()(_entity)?.Trim()))
                {
                    SetError(geterror(fieldname));
                }
            }
            else
                throw new Exception("此校验只对字段属性生效");
        }
        #endregion


    }
}
