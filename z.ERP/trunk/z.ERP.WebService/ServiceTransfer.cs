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
                types.ForEachWithBreak(t =>
                {
                    t.GetMethods().ForEachWithBreak(a =>
                      {
                          ServiceAbleAttribute attr = a.GetAttribute<ServiceAbleAttribute>();
                          if (attr.Key == dto.ServiceName)
                          {
                              thistype = t;
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
                //thisMethod.Invoke (thistype,)
                return res;
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