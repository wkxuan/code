using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using z.Extensions;
using z.Extensiont;

/// <summary>
/// 数学集合
/// </summary>
namespace z.MathTools
{
    /// <summary>
    /// 财务工具
    /// </summary>
    public static class FinanceTools
    {
        /// <summary>
        /// 按一定的规则,分配一个值到集合中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="settings"></param>
        public static void Allocation<T>(this List<T> list, AllocationSettings<T> settings)
        {
            settings.Allocation.SetValue = settings.SetValue;
            settings.Allocation.GetValue = settings.GetValue;
            settings.Tail.SetValue = settings.SetValue;
            settings.Tail.GetValue = settings.GetValue;

            settings.Allocation.Rounding = settings.Rounding;
            settings.Allocation.RoundCent = settings.RoundCent;
            settings.Tail.Rounding = settings.Rounding;
            settings.Tail.RoundCent = settings.RoundCent;

            settings.Allocation.Verify();
            double fp = settings.Allocation.Do(list, settings.AllQty);
            if (settings.AllQty != fp)
            {
                settings.Tail.Verify();
                settings.Tail.Do(list, settings.AllQty - fp);
            }
        }

        /// <summary>
        /// 舍入
        /// </summary>
        /// <param name="d"></param>
        /// <param name="decimals">保留小数位数</param>
        /// <param name="Rounding">舍入方式</param>
        /// <returns></returns>
        public static double Round(this double d, int decimals = 0, RoundingType Rounding = RoundingType.Normal)
        {
            switch (Rounding)
            {
                case RoundingType.Normal:
                    return Math.Round(d, decimals, MidpointRounding.AwayFromZero);
                case RoundingType.Banker:
                    return Math.Round(d, decimals, MidpointRounding.ToEven);
                default:
                    throw new Exception("舍入法未知");
            }
        }
    }

    #region 舍入法
    /// <summary>
    /// 舍入类型
    /// </summary>
    public enum RoundingType
    {
        /// <summary>
        /// 普通舍入法
        /// 四舍五入
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 银行家舍入法
        /// 四舍六入五取偶
        /// </summary>
        Banker = 1
    }
    #endregion

    #region 分配

    /// <summary>
    /// 分配方式的设置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AllocationSettings<T>
    {
        /// <summary>
        /// 要分配的字段
        /// </summary>
        public Action<T, double> SetValue
        {
            get;
            set;
        }
        /// <summary>
        /// 取值
        /// </summary>
        public Func<T, double> GetValue
        {
            get;
            set;
        }
        /// <summary>
        /// 要分配的总值
        /// </summary>
        public double AllQty = 0d;

        /// <summary>
        /// 舍入位数
        /// </summary>
        public int RoundCent = 2;

        /// <summary>
        /// 舍入方式
        /// </summary>
        public RoundingType Rounding = RoundingType.Normal;

        /// <summary>
        /// 基础分配方式
        /// </summary>
        public AllocationMatchBase<T> Allocation
        {
            get;
            set;
        }
        /// <summary>
        /// 尾差分配方式
        /// </summary>
        public TailMatchBase<T> Tail
        {
            get;
            set;
        }
    }

    #region 分配方式
    /// <summary>
    /// 分配方式的基础
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MatchBase<T>
    {
        /// <summary>
        /// 给字段赋值
        /// </summary>
        public Action<T, double> SetValue
        {
            get;
            set;
        }
        /// <summary>
        /// 取值
        /// </summary>
        public Func<T, double> GetValue
        {
            get;
            set;
        }

        /// <summary>
        /// 舍入位数
        /// </summary>
        public int RoundCent = 2;

        /// <summary>
        /// 舍入方式
        /// </summary>
        public RoundingType Rounding = RoundingType.Normal;

        /// <summary>
        /// 验证数据合法性
        /// </summary>
        public void Verify()
        {
            _Verify();
        }
        /// <summary>
        /// 验证数据合法性
        /// </summary>
        protected virtual void _Verify()
        {
        }
    }
    #region 初步分配
    /// <summary>
    /// 基础分配方式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AllocationMatchBase<T> : MatchBase<T>
    {
        protected override void _Verify()
        {
            if (SetValue == null)
                throw new Exception("SetValue方法不能为空");
            base._Verify();
        }
        /// <summary>
        /// 初步分配
        /// </summary>
        /// <param name="list"></param>
        /// <param name="allqty"></param>
        /// <returns>分配数量</returns>
        public abstract double Do(List<T> list, double allqty);
    }

    /// <summary>
    /// 平均分配
    /// </summary>
    public class EqualAllocationMatch<T> : AllocationMatchBase<T>
    {
        public override double Do(List<T> list, double allqty)
        {
            double fp = 0;
            list.ForEach(l =>
           {
               double i = (allqty / list.Count).Round(RoundCent, Rounding);
               fp += i;
               SetValue(l, i);
           });
            return fp;
        }
    }

    /// <summary>
    /// 加权分配
    /// </summary>
    public class WeightingAllocationMatch<T> : AllocationMatchBase<T>
    {
        /// <summary>
        /// 获取权重值
        /// </summary>
        public Func<T, double> WeightingValue
        {
            get;
            set;
        }

        protected override void _Verify()
        {
            if (WeightingValue == null)
                throw new Exception("权重值没有设置");
            base._Verify();
        }

        public override double Do(List<T> list, double allqty)
        {
            double fp = 0;
            double allWei = list.Sum(a => WeightingValue(a)).Round(RoundCent, Rounding);
            list.ForEach(l =>
            {
                double i = (allqty / allWei * WeightingValue(l)).Round(RoundCent, Rounding);
                fp += i;
                SetValue(l, i);
            });
            return fp;
        }
    }

    #endregion
    #region 尾差分配
    /// <summary>
    /// 尾差分配方式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TailMatchBase<T> : MatchBase<T>
    {
        protected override void _Verify()
        {
            if (GetValue == null)
                throw new Exception("GetValue方法不能为空");
            base._Verify();
        }
        /// <summary>
        /// 分配尾差
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Tail"></param>
        /// <returns></returns>
        public abstract void Do(List<T> list, double Tail);
    }

    /// <summary>
    /// 分配在最后一个上
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LastTailMatch<T> : TailMatchBase<T>
    {
        /// <summary>
        /// 分配在第一个上
        /// </summary>
        public bool First = false;
        public override void Do(List<T> list, double Tail)
        {
            T t = First ? list.First() : list.Last();
            SetValue(t, GetValue(t) + Tail);
        }
    }

    /// <summary>
    /// 分配在最大的上
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaxTailMatch<T> : TailMatchBase<T>
    {
        /// <summary>
        /// 分配在最小的上
        /// </summary>
        public bool Min = false;
        public override void Do(List<T> list, double Tail)
        {
            T t = Min ? list.Min2(a => GetValue(a)) : list.Max2(a => GetValue(a));
            SetValue(t, GetValue(t) + Tail);
        }
    }

    #endregion
    #endregion

    #endregion
}
