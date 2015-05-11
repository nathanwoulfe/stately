using System;
using System.Web.Hosting;
using System.Xml;
using umbraco.cms.businesslogic.packager;
using Umbraco.Core;

namespace Stately
{
    public class Uninstall : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //Add OLD Style Package Event
            InstalledPackage.BeforeDelete += InstalledPackage_BeforeDelete;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void InstalledPackage_BeforeDelete(InstalledPackage sender, EventArgs e)
        {
            // make sure it's stately...
            if (sender.Data.Name == "Stately")
            {
                // clean the dashboard config.
                Uninstall.RemoveSectionDashboard();
            }
        }

        /// <summary>
        /// Remove the reference to Stately from the settings dashboard on uninstalling package
        /// </summary>
        public static void RemoveSectionDashboard()
        {

            bool flag = false;
            string filename = HostingEnvironment.MapPath("~/config/dashboard.config");

            // load the dashboard xml, find the stately node, then remove it
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("//section [areas/area = 'settings']");

            if (xmlNode != null)
            {
                XmlNode statelyNode = xmlNode.SelectSingleNode("//tab [@caption = 'Stately']");

                if (statelyNode != null)
                {
                    xmlNode.RemoveChild(statelyNode);
                }

                flag = true;
            }

            // removed the section? save it.
            if (flag)
            {
                xmlDocument.Save(filename);                
            }

        }
    }
}
