using System.Web.Hosting;
using System.Web.Http;
using System.Xml;
using Umbraco.Core.Configuration;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Stately.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Stately.Controllers
{
    [PluginController("Stately")]
    public class SettingsApiController : UmbracoAuthorizedApiController
    {
        private const string ConfigPath = "~/App_Plugins/Stately/settings.config";

        public List<Settings> GetSettings()
        {
            List<Settings> list = new List<Settings>();
            string filename = HostingEnvironment.MapPath(ConfigPath);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//Settings");
            if (xmlNodeList != null)
            {
                foreach (XmlNode xmlNode1 in xmlNodeList)
                {
                    foreach (XmlNode xmlNode2 in xmlNode1.ChildNodes)
                    {
                        list.Add(new Settings()
                        {
                            PropertyAlias = xmlNode2.Attributes["propertyAlias"].Value,
                            Value = xmlNode2.Attributes["value"].Value,
                            CssClass = xmlNode2.Attributes["cssClass"].Value,
                            CssColor = xmlNode2.Attributes["cssColor"].Value,
                            Disabled = Convert.ToBoolean(xmlNode2.Attributes["disabled"].Value)
                        });
                    }
                }
            }
            return list;
        }

        public bool PostSettings(List<Settings> settings)
        {
            try
            {
                string filename = HostingEnvironment.MapPath("~/App_Plugins/Stately/settings.config");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filename);
                foreach (XmlNode settingsNode in xmlDocument.SelectNodes("//Settings"))
                {
                    settingsNode.RemoveAll();
                    foreach (var setting in settings)
                    {
                        XmlNode newSetting = xmlDocument.CreateNode("element", "Setting", "");

                        XmlAttribute pa = xmlDocument.CreateAttribute("propertyAlias");
                        pa.Value = setting.PropertyAlias;
                        newSetting.Attributes.SetNamedItem(pa);

                        XmlAttribute v = xmlDocument.CreateAttribute("value");
                        v.Value = setting.Value;
                        newSetting.Attributes.SetNamedItem(v);

                        XmlAttribute cc = xmlDocument.CreateAttribute("cssClass");
                        cc.Value = setting.CssClass;
                        newSetting.Attributes.SetNamedItem(cc);

                        XmlAttribute color = xmlDocument.CreateAttribute("cssColor");
                        color.Value = setting.CssColor;
                        newSetting.Attributes.SetNamedItem(color);

                        XmlAttribute d = xmlDocument.CreateAttribute("disabled");
                        d.Value = setting.Disabled.ToString();
                        newSetting.Attributes.SetNamedItem(d);

                        settingsNode.AppendChild(newSetting);
                    }
                }
                xmlDocument.Save(filename);
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public string GetUmbracoVersion()
        {
            return UmbracoVersion.Current.ToString();
        }
    }
}
