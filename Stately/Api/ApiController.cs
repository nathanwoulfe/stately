using Stately.Models;
using Stately.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Stately.Api
{
    [PluginController("stately")]
    public class ApiController : UmbracoAuthorizedApiController
    {
        private readonly ISettingsService _settingsService;

        public ApiController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(new
                {
                    status = HttpStatusCode.OK,
                    data = _settingsService.Get()
                });
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }            
        }

        [HttpPost]
        public IHttpActionResult Set(List<Settings> settings)
        {
            try
            {
                return Ok(new
                {
                    status = HttpStatusCode.OK,
                    data = _settingsService.Set(settings)
                });
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public string GetUmbracoVersion()
        {
            return "8"; //UmbracoVersion.Current.ToString();
        }

        private IHttpActionResult Error(string message)
        {
            return Ok(new
            {
                status = HttpStatusCode.InternalServerError,
                data = message
            });
        }
    }
}
