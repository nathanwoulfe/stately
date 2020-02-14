using Stately.Api;
using Stately.Models;
using Stately.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.JavaScript;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;

namespace Stately.Startup
{
    public class StatelyComponent : IComponent
    {
        private readonly ISettingsService _settingsService;
        private readonly IContentService _contentService;

        public StatelyComponent(ISettingsService settingsService, IContentService contentService)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _contentService = contentService ?? throw new ArgumentNullException(nameof(contentService));
        }

        public void Initialize()
        {
            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;
            TreeControllerBase.TreeNodesRendering += new TypedEventHandler<TreeControllerBase, TreeNodesRenderingEventArgs>(TreeControllerBase_TreeNodesRendering);
        }

        /// <summary>
        /// Add preflight-specific values to the servervariables dictionary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dictionary"></param>
        private static void ServerVariablesParser_Parsing(object sender, Dictionary<string, object> dictionary)
        {
            var urlHelper = new System.Web.Mvc.UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));

            dictionary.Add("Stately", new Dictionary<string, object>
            {
                { "ApiPath", urlHelper.GetUmbracoApiServiceBaseUrl<SettingsApiController>(controller => controller.Get()) }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeControllerBase_TreeNodesRendering(TreeControllerBase sender, TreeNodesRenderingEventArgs e)
        {
            if (!(sender.TreeAlias == Umbraco.Core.Constants.Applications.Content))
                return;

            var settings = _settingsService.GetActiveSettings();

            foreach (TreeNode treeNode in e.Nodes)
            {
                int id = Convert.ToInt32(treeNode.Id);
                IPublishedContent node = null;

                if (id > 0)
                {
                    node = sender.UmbracoContext.Content.GetById(id);
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
        private void AddClassesToNode(TreeNode treeNode, IPublishedContent node, List<StatelySettings> settings)
        {
            // need the IContent item for access to the current property values
            // however, we should only fetch that if it has properties existing in settings.
            var currentSettingsAliases = settings.Select(x => x.PropertyAlias).ToList();
            var validProperties = node.ContentType.PropertyTypes.Where(x => currentSettingsAliases.Contains(x.Alias));

            if (!validProperties.Any())
                return;

            var content = _contentService.GetById(node.Id);

            foreach (StatelySettings setting in settings)
            {
                if (content.HasProperty(setting.PropertyAlias))
                {
                    var property = validProperties.First(x => x.Alias == setting.PropertyAlias);
                    var propAsBool = property.ClrType.FullName == "System.Boolean"
                            ? content.GetValue<bool>(setting.PropertyAlias)
                            : content.GetValue(setting.PropertyAlias) != null;                

                    if (setting.Value == propAsBool)
                    {
                        if (setting.Replace || setting.Recolor)
                        {
                            if (setting.Replace && setting.CssClass != null)
                            {
                                treeNode.Icon = setting.CssClass;
                            }
                            if (setting.Recolor)
                            {
                                treeNode.CssClasses.Add($"stately-re{setting.CssColor}");
                            }
                        }
                        else
                        {
                            treeNode.CssClasses.Add($"stately-icon {setting.CssClass}");

                            if (!string.IsNullOrEmpty(setting.CssColor))
                            {
                                treeNode.CssClasses.Add($"stately-{setting.CssColor}");
                            }
                        }

                        break;
                    }
                }
            }
        }

        public void Terminate()
        {

        }
    }
}
