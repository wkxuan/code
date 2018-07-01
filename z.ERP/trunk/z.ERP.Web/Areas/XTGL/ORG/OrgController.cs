using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using z.ERP.Entities;
using z.ERP.Entities.Enum;
using z.ERP.Model.Vue;
using z.ERP.Web.Areas.Base;
using z.Exceptions;

namespace z.ERP.Web.Areas.XTGL.ORG
{
    public class OrgController: BaseController
    {
        public ActionResult Org()
        {
            ViewBag.Tiele = "组织架构维护";
            return View();
        }

        public string Save(string Tar, string Key, ORGEntity DefineSave)
        {
 
            var allenum = SelectList(new ORGEntity());
            string newkey = TreeModel.GetNewKey(allenum, a => a.ORGCODE, Key, Tar);
            DefineSave.ORGCODE = newkey;
            //当前数据有下级,并且当前数据要修改为末级,那么提示不让修改保存成功
            if (((Tar == null)||(Tar==" "))&&(DefineSave.LEVEL_LAST == ((int)末级标记.末级).ToString()))
            {
                foreach (var data in allenum) {
                    if (data.ORGCODE.Length > DefineSave.ORGCODE.Length) {
                        var dataOrgcode = data.ORGCODE.Substring(0, DefineSave.ORGCODE.Length);
                        if ((data.ORGCODE != DefineSave.ORGCODE)
                            && (dataOrgcode == DefineSave.ORGCODE))
                        {
                            throw new LogicException("当前组织已经有下级数据不能修改为末级!");
                        }
                    }
                }
            };
            //同一级次已经有核算部门不能在设置为核算部门
            foreach (var data in allenum)
            {
                if ((data.ORGCODE.Length < DefineSave.ORGCODE.Length) 
                    &&
                    (data.ORG_TYPE== ((int)部门类型.核算部门).ToString())) 
                    //代码长度小于当前长度并且其是核算部门
                {
                    var dataOrgcode = DefineSave.ORGCODE.Substring(0, data.ORGCODE.Length);

                    if ((data.ORGCODE != DefineSave.ORGCODE)   
                        && (dataOrgcode == data.ORGCODE) 
                        && (DefineSave.ORG_TYPE == ((int)部门类型.核算部门).ToString())
                        )
                    {
                        throw new LogicException($"当前级次组织上({data.ORGCODE})已经拥有了核算类型的组织!");
                    }
                };

                if ((data.ORGCODE.Length > DefineSave.ORGCODE.Length)
                    &&
                    (data.ORG_TYPE == ((int)部门类型.核算部门).ToString()))
                //代码长度大于当前长度并且其是核算部门
                {
                    var dataOrgcode = data.ORGCODE.Substring(0, DefineSave.ORGCODE.Length);

                    if ((data.ORGCODE != DefineSave.ORGCODE)
                        && (dataOrgcode == DefineSave.ORGCODE)
                        && (DefineSave.ORG_TYPE == ((int)部门类型.核算部门).ToString())
                        )
                    {
                        throw new LogicException($"当前级次组织上({data.ORGCODE})已经拥有了核算类型的组织!");
                    }
                };
            }

            if (DefineSave.ORGID.IsEmpty())
            {
                DefineSave.ORGID = service.CommonService.NewINC("ORG");
            }
            var v = GetVerify(DefineSave);

            v.Require(a => a.ORGNAME);
            v.IsUnique(a => a.ORGNAME);
            v.Require(a => a.ORG_TYPE);
            v.Require(a => a.LEVEL_LAST);
            v.Require(a => a.VOID_FLAG);
            v.Verify();
            if (( Tar == "xj") &&(Key!=null))  {
                DefineSave.BRANCHID = service.XtglService.Org_BRANCHID(Key);
            }
      
            CommonSave(DefineSave);
            return newkey;
        }
        public void Delete(ORGEntity DefineDelete)
        {
            var allenum = SelectList(new ORGEntity());
            //已经有下级的部门不能删除
            foreach (var data in allenum)
            {
                if (data.ORGCODE.Length > DefineDelete.ORGCODE.Length)
                {
                    var dataOrgcode = data.ORGCODE.Substring(0, DefineDelete.ORGCODE.Length);

                    if ((data.ORGCODE != DefineDelete.ORGCODE) && (dataOrgcode == DefineDelete.ORGCODE))
                    {
                        throw new LogicException($"当前部门级次已经有下级不能删除!");
                    }
                };
            };
            //已经使用的部门通过外键控制
            

            var v = GetVerify(DefineDelete);
            v.Require(a => a.ORGID);
            v.Verify();
            CommenDelete(DefineDelete);
        }
    }
}