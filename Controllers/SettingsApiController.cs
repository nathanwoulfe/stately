using Stately.Models;
using System;
using System.Collections.Generic;
using System.Web.Hosting;
using System.Xml;
using Umbraco.Core.Configuration;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Stately.Controllers
{
    [PluginController("Stately")]
    public class SettingsApiController : UmbracoAuthorizedApiController
    {
        private const string ConfigPath = "~/App_Plugins/Stately/settings.config";

        public List<Settings> GetSettings()
        {
            List<Settings> list = new List<Settings>();
            string filename = HostingEnvironment.MapPath("~/App_Plugins/Stately/settings.config");
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
                foreach (XmlNode xmlNode in xmlDocument.SelectNodes("//Settings"))
                {
                    xmlNode.RemoveAll();
                    foreach (Settings settings1 in settings)
                    {
                        XmlNode node = xmlDocument.CreateNode("element", "Setting", "");
                        XmlAttribute attribute1 = xmlDocument.CreateAttribute("propertyAlias");
                        attribute1.Value = settings1.PropertyAlias;
                        node.Attributes.SetNamedItem((XmlNode)attribute1);
                        XmlAttribute attribute2 = xmlDocument.CreateAttribute("value");
                        attribute2.Value = settings1.Value;
                        node.Attributes.SetNamedItem((XmlNode)attribute2);
                        XmlAttribute attribute3 = xmlDocument.CreateAttribute("cssClass");
                        attribute3.Value = settings1.CssClass;
                        node.Attributes.SetNamedItem((XmlNode)attribute3);
                        XmlAttribute attribute4 = xmlDocument.CreateAttribute("cssColor");
                        attribute4.Value = settings1.CssColor;
                        node.Attributes.SetNamedItem((XmlNode)attribute4);
                        XmlAttribute attribute5 = xmlDocument.CreateAttribute("disabled");
                        attribute5.Value = settings1.Disabled.ToString();
                        node.Attributes.SetNamedItem((XmlNode)attribute5);
                        xmlNode.AppendChild(node);
                    }
                }
                xmlDocument.Save(filename);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetUmbracoVersion()
        {
            return UmbracoVersion.Current.ToString();
        }
    }
}
