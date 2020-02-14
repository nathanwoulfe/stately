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
    [RoutePrefix("umbraco/backoffice/stately/settings")]
    [PluginController("stately")]
    public class SettingsApiController : UmbracoAuthorizedApiController
    {
        private readonly ISettingsService _settingsService;

        public SettingsApiController(ISettingsService settingsService)
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

        [HttpGet]
        public IHttpActionResult GetAliases()
        {
            try
            {
                return Ok(new
                {
                    status = HttpStatusCode.OK,
                    data = _settingsService.GetAliases()
                });
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Post(List<StatelySettings> settings)
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
