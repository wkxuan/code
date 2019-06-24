using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Extensions;

namespace z.ATR.WinService.Model
{
    public class Settings
    {
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 循环类型
        /// 1:每多少秒执行一次,秒数输在Pram参数中   ,2:每天执行一次,格式HHMMSS,例如要每天早上九点半执行就输093000  输在Pram中
        /// </summary>
        public LoopType Type
        {
            get;
            set;
        }
        public string Pram
        {
            get;
            set;
        }

        /// <summary>
        /// 要执行的方法名称
        /// </summary>
        public string MethodName
        {
            get;
            set;
        }
        public DateTime LastRunTime
        {
            get; set;
        }

        internal bool CanDo()
        {
            switch (Type)
            {
                case LoopType.Seconds:
                    {
                        return (DateTime.Now - LastRunTime).TotalSeconds > Pram.ToInt();
                    }
                case LoopType.Day:
                    {
                        if (Pram.Length != 6)
                            throw new Exception($"参数{Pram}非法,必须是HHMMSS结构");
                        int h = Pram.Substring(0, 2).ToInt();
                        int m = Pram.Substring(2, 2).ToInt();
                        int s = Pram.Substring(4, 2).ToInt();
                        if (LastRunTime.Date < DateTime.Now.Date)
                        {
                            if (DateTime.Now.Hour >= h && DateTime.Now.Minute >= m && DateTime.Now.Second >= s)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                case LoopType.Cron:
                    throw new Exception("还没写");
                default:
                    throw new Exception($"未知的循环类型{Type}");
            }
        }
    }
}
