using Newtonsoft.Json;
using Stately.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Umbraco.Core.Cache;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;

namespace Stately.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly string _configPath = HttpContext.Current.Server.MapPath("~/app_plugins/stately/backoffice/settings.json");

        private readonly IContentTypeService _contentTypeService;
        private readonly ILogger _logger;

        public SettingsService(ILogger logger, IContentTypeService contentTypeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _contentTypeService = contentTypeService ?? throw new ArgumentNullException(nameof(contentTypeService));
        }

        private List<StatelySettings> GetSettings()
        {
            List<StatelySettings> settings;

            using (var file = new StreamReader(_configPath))
            {
                string json = file.ReadToEnd();
                settings = JsonConvert.DeserializeObject<List<StatelySettings>>(json);
            }

            return settings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<StatelySettings> Get()
        {
            List<StatelySettings> fromCache = Current.AppCaches.RuntimeCache.GetCacheItem("StatelySettings", () => GetSettings(), new TimeSpan(24, 0, 0), false);

            if (fromCache != null)
            {
                return fromCache;
            }

            _logger.Error<SettingsService>(new NullReferenceException("Could not get Stately settings"));

            return null;           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<StatelySettings> GetActiveSettings()
        {
            return Get().Where(x => !x.Disabled).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public bool Set(List<StatelySettings> settings)
        {
            try
            {
                Current.AppCaches.RuntimeCache.InsertCacheItem("StatelySettings", () => settings, new TimeSpan(24, 0, 0), false);

                using (var file = new StreamWriter(_configPath, false))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(file, settings);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error<SettingsService>(ex, "Could not save Stately settings");
                return false;
            }
        }

        /// <summary>
        /// Get all the property editor aliases
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAliases()
        {
            try
            {
                return _contentTypeService.GetAllPropertyTypeAliases();
            }
            catch (Exception ex)
            {
                _logger.Error<SettingsService>(ex, "Could not get property aliases");
            }

            return null;
        }
    }
}
