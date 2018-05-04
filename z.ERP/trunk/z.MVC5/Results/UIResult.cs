using NewtonsoftCode.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using z.Extensions;

namespace z.MVC5.Results
{
    /// <summary>
    /// 页面通用返回值
    /// </summary>
    public class UIResult : JsonResult
    {
        public static int Unknown = -1;
        public UIResult()
        {
        }
        public UIResult(object o)
        {
            Obj = o;
        }
        public UIResult(Exception ex)
        {
            Flag = UIResult.Unknown;
            Msg = ex.Message;
        }

        public object GetData()
        {
            return new
            {
                Flag = Flag,
                Obj = Obj,
                Msg = Msg
            };
        }

        public int Flag = 0;
        public object Obj;
        public string Msg;

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            if (this.Data != null)
            {
                JsonSerializerSettings setting = new JsonSerializerSettings();
                // 设置日期序列化的格式  
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                response.Write(JsonConvert.SerializeObject(Data, setting));
            }
        }
    }
}
