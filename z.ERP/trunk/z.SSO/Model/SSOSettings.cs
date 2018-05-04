using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.SSO.Model
{
    public class SSOSettings : ConfigurationSection
    {
        public static string Name = "SSOSettings";

        [ConfigurationProperty("WcfUrl")]
        public string WcfUrl
        {
            get
            {
                return this["WcfUrl"] as string;
            }
            set
            {
                this["WcfUrl"] = value;
            }
        }
        [ConfigurationProperty("User")]
        public string User
        {
            get
            {
                return this["User"] as string;
            }
            set
            {
                this["User"] = value;
            }
        }
        [ConfigurationProperty("Password")]
        public string Password
        {
            get
            {
                return this["Password"] as string;
            }
            set
            {
                this["Password"] = value;
            }
        }
        [ConfigurationProperty("Type")]
        public string Type
        {
            get
            {
                return this["Type"] as string;
            }
            set
            {
                this["Type"] = value;
            }
        }
    }

}
