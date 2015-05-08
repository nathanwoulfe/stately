using System.Web.Hosting;
using System.Xml;

namespace Stately
{
    public static class Install
    {
        /// <summary>
        /// 
        /// </summary>
        public static void AddSectionDashboard()
        {
            bool saveFile = false;

            //Open up dashboard config
            var dashboardPath = "~/config/dashboard.config";

            //Path to the file resolved
            var dashboardFilePath = HostingEnvironment.MapPath(dashboardPath);

            //Load settings.config XML file
            XmlDocument dashboardXml = new XmlDocument();
            dashboardXml.Load(dashboardFilePath);

            // Section Node
            XmlNode findSection = dashboardXml.SelectSingleNode("//section [areas/area = 'settings']");

            // Found settings section
            if (findSection != null)
            {
                XmlNode statelySection = dashboardXml.SelectSingleNode("//tab [@caption = 'Stately']");

                if (statelySection == null)
                {

                    //Let's add the xml
                    var xmlToAdd = "<tab caption='Stately'>" +
                                        "<control addPanel='true' panelCaption=''>/App_Plugins/Stately/Views/view.html</control>" +
                                    "</tab>";

                    //Load in the XML string above
                    XmlDocumentFragment xmlNodeToAdd = dashboardXml.CreateDocumentFragment();
                    xmlNodeToAdd.InnerXml = xmlToAdd;

                    XmlElement mapElement = (XmlElement)findSection;
                    mapElement.AppendChild(xmlNodeToAdd);

                    //Save the file flag to true
                    saveFile = true;
                }
            }

            //If saveFile flag is true then save the file
            if (saveFile)
            {
                //Save the XML file
                dashboardXml.Save(dashboardFilePath);
            }
        }

    }
}
