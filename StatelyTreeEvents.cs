using log4net;
using Stately.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Hosting;
using System.Xml;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;

namespace Stately
{
    public class StatelyTreeEvents : ApplicationEventHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string ConfigPath = "~/App_Plugins/Stately/settings.config";
        private static List<Settings> StatelySettings = new List<Settings>();

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            Install.AddSectionDashboard();

            TreeControllerBase.TreeNodesRendering += TreeControllerBase_TreeNodesRendering;
            TreeControllerBase.RootNodeRendering += TreeControllerBase_RootNodeRendering;
        }

        void TreeControllerBase_RootNodeRendering(TreeControllerBase sender, TreeNodeRenderingEventArgs e)
        {
            if (sender.TreeAlias == "content")
            {
                var helper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);

                StatelySettings = GetSettings();

                var nodeId = Convert.ToInt32(e.Node.Id);
                if (nodeId > 0)
                {
                    IPublishedContent _node = helper.TypedContent(nodeId);
                    AddClassesToNode(e.Node, _node);
                }
            }
        }

        void TreeControllerBase_TreeNodesRendering(TreeControllerBase sender, TreeNodesRenderingEventArgs e)
        {
            if (sender.TreeAlias == "content")
            {
                var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);

                foreach (var node in e.Nodes)
                {
                    var nodeId = Convert.ToInt32(node.Id);
                    IPublishedContent _node = null;

                    if (nodeId > 0)
                    {
                        _node = umbracoHelper.TypedContent(nodeId);

                        if (_node == null)
                        {
                            ContentService cs = new ContentService();
                            _node = (IPublishedContent)cs.GetPublishedVersion(nodeId);
                        }
                    }

                    if (_node != null)
                    {
                        AddClassesToNode(node, _node);
                    }
                }
            }
        }

        private List<Settings> GetSettings()
        {
            string filename = HostingEnvironment.MapPath(ConfigPath);
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
                        if (Convert.ToBoolean(xmlNode2.Attributes["disabled"].Value) == false) {
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
            }
            return list;
        }

        /// <summary>
        /// Checks node for presence of property values and adds appropriate classes
        /// </summary>
        /// <param name="node">The TreeNode to manipulate</param>
        /// <returns></returns>
        private static void AddClassesToNode(TreeNode node, IPublishedContent _node)
        {
            var stateSet = false;

            foreach (var s in StatelySettings)
            {
                if (stateSet)
                {
                    break;
                }                
                else if (_node.HasProperty(s.PropertyAlias) // node must have the property
                    && (_node.HasValue(s.PropertyAlias) == Convert.ToBoolean(s.Value) // property value presence == true/false
                    || _node.GetPropertyValue<bool>(s.PropertyAlias) == Convert.ToBoolean(s.Value))) // property value is true/false
                {
                    node.CssClasses.Add("stately-" + s.CssClass);

                    if (!String.IsNullOrEmpty(s.CssColor))
                    {
                        node.CssClasses.Add("stately_" + s.CssColor);
                    }

                    node.CssClasses.Add("stately");
                    stateSet = true;
                }
            }
        }
    }
}