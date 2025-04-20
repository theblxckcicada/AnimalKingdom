namespace AnimalKingdom.Mobile.MSALClient
{
    public class DownstreamApiHelper
    {
        private string[] DownstreamApiScopes;
        public DownStreamApiConfig DownstreamApiConfig;
        private MSALClientHelper MSALClient;

        public DownstreamApiHelper(DownStreamApiConfig downstreamApiConfig, MSALClientHelper msalClientHelper)
        {
            if (msalClientHelper == null)
            {
                throw new ArgumentNullException(nameof(msalClientHelper));
            }

            DownstreamApiConfig = downstreamApiConfig;
            MSALClient = msalClientHelper;
            DownstreamApiScopes = DownstreamApiConfig.ScopesArray;
        }
    }
}
