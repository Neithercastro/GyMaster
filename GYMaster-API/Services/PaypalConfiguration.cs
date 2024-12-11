using System.Collections.Generic;
using PayPal.Api;

namespace GYMaster_API.Services
{
    public static class PaypalConfiguration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        // Static constructor for setting the readonly static members.
        static PaypalConfiguration()
        {
            
        }

        // Create the configuration map that contains mode and other optional configuration details.
        public static Dictionary<string, string> GetConfig(string mode)
        {
            return new Dictionary<string, string>() 
            {
                { "Mode", mode}
            };
        }

        // Create accessToken
        private static string GetAccessToken(string ClientId, string ClientSecret)
        {    
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret).GetAccessToken();

            return accessToken;
        }

        // Returns APIContext object
        public static APIContext GetAPIContext(string ClientId, string ClientSecret)
        {
            var apiContext = new APIContext(GetAccessToken(ClientId, ClientSecret));
            apiContext.Config = GetConfig("sandbox");

            return apiContext;
        }

    }
}
