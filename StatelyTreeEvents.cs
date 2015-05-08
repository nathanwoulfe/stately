// Decompiled with JetBrains decompiler
// Type: Stately.StatelyTreeEvents
// Assembly: Stately, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33018894-CB80-4D79-988E-1E4D73E9CDAE
// Assembly location: D:\uStats\uStats.Site\bin\Stately.dll

using log4net;
using Stately.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Hosting;
using System.Xml;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;

namespace Stately
{
  public class StatelyTreeEvents : ApplicationEventHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static List<Settings> StatelySettings = new List<Settings>();
    private const string ConfigPath = "~/App_Plugins/Stately/settings.config";

    protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
      Install.AddSectionDashboard();
      TreeControllerBase.TreeNodesRendering += new TypedEventHandler<TreeControllerBase, TreeNodesRenderingEventArgs>(this.TreeControllerBase_TreeNodesRendering);
      TreeControllerBase.RootNodeRendering += new TypedEventHandler<TreeControllerBase, TreeNodeRenderingEventArgs>(this.TreeControllerBase_RootNodeRendering);
    }

    private void TreeControllerBase_RootNodeRendering(TreeControllerBase sender, TreeNodeRenderingEventArgs e)
    {
      if (!(sender.TreeAlias == "content"))
        return;
      UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
      StatelyTreeEvents.StatelySettings = this.GetSettings();
      int id = Convert.ToInt32(e.Node.Id);
      if (id > 0)
      {
        IPublishedContent _node = umbracoHelper.TypedContent(id);
        StatelyTreeEvents.AddClassesToNode(e.Node, _node);
      }
    }

    private void TreeControllerBase_TreeNodesRendering(TreeControllerBase sender, TreeNodesRenderingEventArgs e)
    {
      if (!(sender.TreeAlias == "content"))
        return;
      UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
      foreach (TreeNode node in (List<TreeNode>) e.Nodes)
      {
        int id = Convert.ToInt32(node.Id);
        IPublishedContent _node = (IPublishedContent) null;
        if (id > 0)
          _node = umbracoHelper.TypedContent(id) ?? (IPublishedContent) new ContentService().GetPublishedVersion(id);
        if (_node != null)
          StatelyTreeEvents.AddClassesToNode(node, _node);
      }
    }

    private List<Settings> GetSettings()
    {
      string filename = HostingEnvironment.MapPath("~/App_Plugins/Stately/settings.config");
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(filename);
      XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//Settings");
      List<Settings> list = new List<Settings>();
      if (xmlNodeList != null)
      {
        foreach (XmlNode xmlNode1 in xmlNodeList)
        {
          foreach (XmlNode xmlNode2 in xmlNode1.ChildNodes)
          {
            if (!Convert.ToBoolean(xmlNode2.Attributes["disabled"].Value))
              list.Add(new Settings()
              {
                PropertyAlias = xmlNode2.Attributes["propertyAlias"].Value,
                Value = xmlNode2.Attributes["value"].Value,
                CssClass = xmlNode2.Attributes["cssClass"].Value,
                CssColor = xmlNode2.Attributes["cssColor"].Value
              });
          }
        }
      }
      return list;
    }

    private static void AddClassesToNode(TreeNode node, IPublishedContent _node)
    {
      bool flag = false;
      foreach (Settings settings in StatelyTreeEvents.StatelySettings)
      {
        if (flag)
          break;
        if (PublishedContentExtensions.HasProperty(_node, settings.PropertyAlias) && (PublishedContentExtensions.HasValue(_node, settings.PropertyAlias) == Convert.ToBoolean(settings.Value) || PublishedContentExtensions.GetPropertyValue<bool>(_node, settings.PropertyAlias) == Convert.ToBoolean(settings.Value)))
        {
          node.CssClasses.Add("stately-" + settings.CssClass);
          if (!string.IsNullOrEmpty(settings.CssColor))
            node.CssClasses.Add("stately_" + settings.CssColor);
          node.CssClasses.Add("stately");
          flag = true;
        }
      }
    }
  }
}
