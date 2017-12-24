using z.ERP.Web.Areas.Base;
using System.Web.Mvc;
using z.ERP.Entities;
using z.Extensions;
using System;

namespace z.ERP.Web.Areas.XTGL.ENERGYFILES
{
    public class EnergyFilesController: BaseController
    {
        public ActionResult EnergyFiles()
        {
            return View();
        }

        public string Save(ENERGY_FILESEntity DefineSave)
        {
            var v = GetVerify(DefineSave);
            if (DefineSave.FILEID.IsEmpty())
            {
                DefineSave.FILEID = service.CommonService.NewINC("ENERGYFILES");
            }
            v.IsUnique(a => a.FILEID);
            v.IsUnique(a => a.FILECODE);
            v.Require(a => a.FILENAME);
            v.Require(a => a.SHOPID);
            v.Require(a => a.AREAID);            
            v.Require(a => a.PRICE);
            v.Require(a => a.VALUE_LAST);
            v.Require(a => a.DATE_LAST);
            v.Require(a => a.MULTIPLE);
            v.Require(a => a.DESCRIPTION);
            v.Require(a => a.FACTORY_DATE);
            v.Verify();
            return CommonSave(DefineSave);
        }

        public void Delete(ENERGY_FILESEntity DefineDelete)
        {
            var v = GetVerify(DefineDelete);
            CommenDelete(DefineDelete);
        }
    }
}