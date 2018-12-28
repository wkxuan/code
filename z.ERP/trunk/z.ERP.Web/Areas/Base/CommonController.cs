using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using z.DBHelper.DBDomain;
using z.ERP.Services;
using z.Extensions;
using z.MVC5.Results;
using z.WebPage;

namespace z.ERP.Web.Areas.Base
{
    public class CommonController : BaseController
    {
        public CommonController()
        {
        }

        public DataGridResult Search(string Service, string Method, SearchItem Data)
        {
            Type type = service.GetType();
            PropertyInfo propertyInfo = type.GetProperty(Service);
            if (propertyInfo == null)
                throw new Exception($"无效的Service:{Service}");
            if (!propertyInfo.PropertyType.BaseOn<ServiceBase>())
                throw new Exception($"Service:{Service}不继承于ServiceBase");
            ServiceBase list = propertyInfo.GetValue(service, null) as ServiceBase;
            MethodInfo mi = propertyInfo.PropertyType.GetMethod(Method);
            if (mi == null)
                throw new Exception($"无效的Method:{Method}");
            if (!mi.ReturnType.BaseOn<UIResult>())
                throw new Exception($"Method:{Method}返回值错误,必须返回UIResult");
            ParameterInfo[] info = mi.GetParameters();
            if (info == null || info.Count() != 1 || !info[0].ParameterType.BaseOn<SearchItem>())
                throw new Exception($"Method:{Method}参数错误,必须只有一个参数SearchItem");

           // Data.PageInfo.PageIndex=
            var d = mi.Invoke(list, new object[] { Data }) as DataGridResult;
            return d;
        }


        public UIResult SearchNoQuery(string Service, string Method)
        {
            Type type = service.GetType();
            PropertyInfo propertyInfo = type.GetProperty(Service);
            if (propertyInfo == null)
                throw new Exception($"无效的Service:{Service}");
            if (!propertyInfo.PropertyType.BaseOn<ServiceBase>())
                throw new Exception($"Service:{Service}不继承于ServiceBase");
            ServiceBase list = propertyInfo.GetValue(service, null) as ServiceBase;
            MethodInfo mi = propertyInfo.PropertyType.GetMethod(Method);
            if (mi == null)
                throw new Exception($"无效的Method:{Method}");
            if (!mi.ReturnType.BaseOn<UIResult>())
                throw new Exception($"Method:{Method}返回值错误,必须返回UIResult");
            ParameterInfo[] info = mi.GetParameters();
            if (info.Count() != 0)
                throw new Exception($"Method:{Method}不能有参数");
            var d = mi.Invoke(list, null) as UIResult;
            return d;
        }
    }
}