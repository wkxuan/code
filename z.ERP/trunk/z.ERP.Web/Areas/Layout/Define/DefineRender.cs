using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace z.ERP.Web.Areas.Layout.Define
{
    public class DefineRender
    {
        public string Permission_Add
        {
            get;
            set;
        }

    /*    public string Permission_Save
        {
            get;
            set;
        } */

        public string Permission_Mod
        {
            get;
            set;
        }
        public string Permission_Chk
        {
            get;
            set;
        }
        public bool Invisible_Srch          //查询按扭是否不可见
        {
            get;
            set;
        }

        public bool Invisible_Add         //新增按扭是否不可见
        {
            get;
            set;
        }
        public bool Invisible_Chk         //新增按扭是否不可见
        {
            get;
            set;
        }
    }
}