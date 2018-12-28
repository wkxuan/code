using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace z.Cron
{
    public class CronExpression
    {
        List<_CronExpression> clist;
        /// <summary>
        /// 创建一个CronExpression实例,服从cron的各项规范,额外的
        /// 使用;分开多个字符串,计算时间时,返回用多个字符串计算的时间的最小值
        /// </summary>
        /// <param name="cronExpression"></param>
        public CronExpression(string cronExpression)
        {
            if (string.IsNullOrWhiteSpace(cronExpression))
            {
                throw new Exception("无效的时间字符串");
            }
            clist = cronExpression.Split(';').Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => new _CronExpression(a.Trim())).ToList();
        }

        public DateTimeOffset? GetTimeAfter(DateTimeOffset afterTimeUtc)
        {
            var times = clist.Select(a => a.GetTimeAfter(afterTimeUtc))?.Where(a => a.HasValue);
            if (times != null && times.Count() > 0)
                return times.Min();
            else
                return null;
        }
        public List<DateTimeOffset> GetTimeAfter(DateTimeOffset afterTimeUtc, int count)
        {
            List<DateTimeOffset> list = new List<DateTimeOffset>();
            DateTimeOffset offset = afterTimeUtc;
            for (int i = 0; i < count; i++)
            {
                DateTimeOffset? timeAfter = this.GetTimeAfter(offset);
                if (timeAfter.HasValue)
                {
                    list.Add(timeAfter.Value);
                    offset = timeAfter.Value;
                }
            }
            return list;
        }
    }
}
