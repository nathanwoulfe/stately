using System.Web.Hosting;
using System.Xml;

namespace Stately
{
    public static class Install
    {
        public static void AddSectionDashboard()
        {
            bool flag = false;
            string filename = HostingEnvironment.MapPath("~/config/dashboard.config");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("//section [areas/area = 'settings']");
            if (xmlNode != null && xmlDocument.SelectSingleNode("//tab [@caption = 'Stately']") == null)
            {
                string str = "<tab caption='Stately'><control addPanel='true' panelCaption=''>/App_Plugins/Stately/Views/view.html</control></tab>";
                XmlDocumentFragment documentFragment = xmlDocument.CreateDocumentFragment();
                documentFragment.InnerXml = str;
                xmlNode.AppendChild((XmlNode)documentFragment);
                flag = true;
            }
            if (!flag)
            {
                return;
            }
            xmlDocument.Save(filename);
        }
    }
}
