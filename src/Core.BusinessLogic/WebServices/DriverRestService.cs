using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.BusinessLogic.WebServices.UrlBuilders;
using Core.Models;
using Core.Models.Settings;
using Flurl.Http;

namespace Core.BusinessLogic.WebServices
{
    public class DriverRestService
    {
        private readonly DriverRestServiceUrlBuilder _urlBuilder;

        public DriverRestService(DriverRestServiceUrlBuilder urlBuilder)
        {
            _urlBuilder = urlBuilder;
        }

        public Task<List<Driver>> GetVacantDriversAsync(DateTime onDateTime)
        {
            var url = _urlBuilder.GetVacantDriversUrl(onDateTime);
            return url.GetJsonAsync<List<Driver>>();
        }
    }
}