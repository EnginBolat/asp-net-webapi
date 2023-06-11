using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(DosyaYonetimPortalı.Auth.Startup))]
namespace DosyaYonetimPortalı.Auth
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseCors(CorsOptions.AllowAll);
            HttpConfiguration httpConfiguration = new HttpConfiguration();

            ConfigureOAuth(appBuilder);

            WebApiConfig.Register(httpConfiguration);
            appBuilder.UseWebApi(httpConfiguration);
        }


        private void ConfigureOAuth(IAppBuilder appBuilder)
        {
            OAuthAuthorizationServerOptions oAuthAuthorizationServerOptions = new OAuthAuthorizationServerOptions()
            {

                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                AllowInsecureHttp = true,
                Provider = new AuthProvider(),
            };
            appBuilder.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);

            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}