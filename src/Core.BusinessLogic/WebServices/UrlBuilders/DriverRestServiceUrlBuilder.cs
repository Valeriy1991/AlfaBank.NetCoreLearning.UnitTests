using System;
using Core.Models.Settings;
using Flurl;

namespace Core.BusinessLogic.WebServices.UrlBuilders
{
    public class DriverRestServiceUrlBuilder
    {
        private readonly RestServiceSettings _restServiceSettings;

        public DriverRestServiceUrlBuilder(RestServiceSettings restServiceSettings)
        {
            _restServiceSettings = restServiceSettings;
        }

        public string GetVacantDriversUrl(DateTime onDateTime)
        {
            var driverApiSettings = _restServiceSettings.DriverApi;

            var host = driverApiSettings.Host;
            var version = driverApiSettings.Version;
            var url = $"{host}/{version}/driver/vacant"
                .SetQueryParams(new {onDateTime = onDateTime});

            return url;
        }
    }
}