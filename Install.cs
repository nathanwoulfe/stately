// Decompiled with JetBrains decompiler
// Type: Stately.Install
// Assembly: Stately, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33018894-CB80-4D79-988E-1E4D73E9CDAE
// Assembly location: D:\uStats\uStats.Site\bin\Stately.dll

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
        xmlNode.AppendChild((XmlNode) documentFragment);
        flag = true;
      }
      if (!flag)
        return;
      xmlDocument.Save(filename);
    }
  }
}
