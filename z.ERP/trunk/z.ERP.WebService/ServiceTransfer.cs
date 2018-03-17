using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using z.ERP.WebService.Controllers;
using z.ERP.WebService.Model;
using z.Extensions;
using z.Extensiont;

namespace z.ERP.WebService
{
    public class ServiceTransfer
    {
        public ResponseDTO Do(RequestDTO dto)
        {
            ResponseDTO res = new ResponseDTO();
            try
            {
                List<Type> types = Assembly.GetExecutingAssembly().FindAllType(a => a.BaseOn<ControllerBase>()).ToList();
                Type thistype = null;
                MethodInfo thisMethod = null;
                types.ForEachWithBreak(tt =>
                {
                    tt.GetMethods().ForEachWithBreak(a =>
                      {
                          ServiceAbleAttribute attr = a.GetAttribute<ServiceAbleAttribute>();
                          if (attr.Key == dto.ServiceName)
                          {
                              thistype = tt;
                              thisMethod = a;
                              return false;
                          }
                          return true;
                      });
                    return thisMethod == null;
                });
                if (thistype == null || thisMethod == null)
                {
                    throw new Exception($"找不到接口方法{dto.ServiceName}");
                }
                ControllerBase cb = new ControllerBase();
                var t = cb.Create(thistype);
                ParameterInfo[] pinfo = thisMethod.GetParameters();
                if (pinfo == null || pinfo.Count() != 1)
                {
                    throw new Exception($"方法{thisMethod.Name}有且只能有一个参数");
                }
                object obj = thisMethod.Invoke(t, new object[] { dto.Context.ToObj(pinfo.First().ParameterType) });
                return new WebService.ResponseDTO()
                {
                    Success = true,
                    Context = obj.ToJson()
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO()
                {
                    Success = false,
                    Msg = ex.Message,
                    Context = ""
                };
            }
        }
    }
}