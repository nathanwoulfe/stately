using Stately.Models;
using Stately.Services;
using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;

namespace Stately.Startup
{
    public class StatelyComponent : IComponent
    {
        private readonly ISettingsService _settingsService;

        public StatelyComponent(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public void Initialize()
        {
            TreeControllerBase.TreeNodesRendering += new TypedEventHandler<TreeControllerBase, TreeNodesRenderingEventArgs>(TreeControllerBase_TreeNodesRendering);
        }

        private void TreeControllerBase_TreeNodesRendering(TreeControllerBase sender, TreeNodesRenderingEventArgs e)
        {
            if (!(sender.TreeAlias == Constants.Applications.Content))
                return;

            var settings = _settingsService.Get();

            foreach (TreeNode treeNode in e.Nodes)
            {
                int id = Convert.ToInt32(treeNode.Id);
                IPublishedContent node = null;

                if (id > 0)
                {
                    node = Umbraco.Web.Composing.Current.UmbracoContext.ContentCache.GetById(id);
                }

                if (node != null)
                {
                    AddClassesToNode(treeNode, node, settings);
                }
            }
        }

        /// <summary>
        /// Do the magic - adds the first matching class to the node
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="node"></param>
        private static void AddClassesToNode(TreeNode treeNode, IPublishedContent node, List<Settings> settings)
        {
            bool flag = false;

            foreach (Settings setting in settings)
            {
                if (flag)
                    break;

                if (node.HasProperty(setting.PropertyAlias))
                {

                    var hasValue = node.HasValue(setting.PropertyAlias);
                    var statelyBool = Convert.ToBoolean(setting.Value);
                    var propString = node.Value<string>(setting.PropertyAlias);

                    bool propCanParse = bool.TryParse(propString, out bool propBool);

                    // match cases
                    // statelyBool == false, show icon if
                    //      node doesn't have a value for the property
                    //      or node has a value, and the value is false
                    // statelyBool == true, show icon if
                    //      propAsBool is true
                    //      node has a value which is not bool

                    if ((statelyBool == false && (hasValue == false || (hasValue == true && propCanParse == true && propBool == false)))
                    || statelyBool == true && (propBool == true || (hasValue == true && propCanParse == false)))
                    {

                        treeNode.CssClasses.Add($"stately-icon {setting.CssClass}");

                        if (!string.IsNullOrEmpty(setting.CssColor))
                        {
                            treeNode.CssClasses.Add($"stately-{setting.CssColor}");
                        }

                        flag = true;
                    }
                }
            }
        }

        public void Terminate()
        {

        }
    }
}
