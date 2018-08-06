﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using z.ERP.WebService.Model;
using z.SSO;
using z.SSO.Model;

namespace z.ERP.WebService.Controllers
{
    public class CommonController : BaseController
    {
        public LoginResponseDTO Login(LoginRequestDTO dto)
        {
            var user = service.HomeService.GetUserByCode(dto.UserName, dto.UserPassword);
            string LoginStr = ServiceUserHelper.GetSrc(new ServiceUser()
            {
                PlatformId = dto.PlatformId,
                Id = user.Id,
                Name = user.Name
            });
            UserApplication.Login(LoginStr, null);
            return new LoginResponseDTO()
            {
                Success = true,
                SecretKey = LoginStr,
                UserName = employee.Name
            };
        }
    }
}