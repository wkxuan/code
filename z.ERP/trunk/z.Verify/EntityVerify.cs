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

namespace z.Verify
{
    public class EntityVerify<TEntity> : VerifyBase where TEntity : EntityBase
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
        public void IsUnique(Expression<Func<TEntity, string>> p, string ErrorModel = "[{0}]表中字段[{1}]的值[{2}]已存在")
        {
            if (p.Body is MemberExpression)
            {
                MemberExpression me = p.Body as MemberExpression;
                PropertyInfo prop = me.Member as PropertyInfo;
                FieldAttribute fa = prop.GetAttribute<FieldAttribute>();
                string str = prop.GetValue(_entity)?.ToString().Trim();
                string sql = $"select 1 from {_entity.GetTableName()} where {me.Member.Name}='{str}'";
                if (_dbHelper.ExecuteTable(sql).Rows.Count != 0)
                {
                    SetError(string.Format(ErrorModel, _entity.GetComments(), fa == null ? me.Member.Name : fa.Fieldname, str));
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
                string str = prop.GetValue(_entity)?.ToString().Trim();
                FieldAttribute f = prop.GetAttribute<FieldAttribute>();
                string fieldname = me.Member.Name;
                if (f != null)
                    fieldname = f.Fieldname;
                if (!verify(str))
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
