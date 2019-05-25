using Newtonsoft.Json;
using Stately.Models;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Stately.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly string _configPath = HttpContext.Current.Server.MapPath("~/app_plugins/stately/backoffice/settings.json");
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Settings> Get()
        {
            var settings = new List<Settings>();
            using (var file = new StreamReader(_configPath))
            {
                string json = file.ReadToEnd();
                settings = JsonConvert.DeserializeObject<List<Settings>>(json);
            }

            return settings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public bool Set(List<Settings> settings)
        {
            using (var file = new StreamWriter(_configPath, false))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, settings);
            }

            return true;
        }
    }
}
