using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using z.ERP.WebService.Controllers;
using z.ERP.WebService.Model;
using z.Extensions;
using z.Extensions;
using z.SSO;

namespace z.ERP.WebService
{
    public class ServiceTransfer
    {
        public ResponseDTO Do(RequestDTO dto)
        {
            ResponseDTO res = new ResponseDTO();

                UserApplication.Login(dto.SecretKey, null);
                List<Type> types = Assembly.GetExecutingAssembly().FindAllType(a => a.BaseOn<BaseController>()).ToList();
                Type thistype = null;
                MethodInfo thisMethod = null;
                types.ForEachWithBreak(tt =>
                {
                    tt.GetMethods().ForEachWithBreak(a =>
                      {
                          ServiceAbleAttribute attr = a.GetAttribute<ServiceAbleAttribute>();
                          if (attr != null && attr.Key == dto.ServiceName)
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
                BaseController cb = new BaseController();
                var t = cb.Create(thistype);
                ParameterInfo[] pinfo = thisMethod.GetParameters();
                if (pinfo.Count() > 1)
                {
                    throw new Exception($"方法{thisMethod.Name}最多能有一个参数");
                }
                object pram = null;
                if (pinfo.Count() == 1)
                    if (!dto.Context.TryToObj(pinfo.First().ParameterType, out pram))
                    {
                        throw new Exception("参数格式不正确");
                    }
                object obj;
                if (pram != null)
                    obj = thisMethod.Invoke(t, new object[] { pram });
                else
                    obj = thisMethod.Invoke(t, null);
                return new ResponseDTO()
                {
                    Success = true,
                    Context = obj.ToJson()
                };
         
        }
    }
}