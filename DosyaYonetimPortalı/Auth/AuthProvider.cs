using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace DosyaYonetimPortalı.Auth
{
    public class AuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return base.ValidateClientAuthentication(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var memberService = new MemberService();
            var member = memberService.MemberLogin(context.UserName, context.Password);

            if (member != null)
            {
                string role = member.userAuthority.authorityName;

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
                identity.AddClaim(new Claim(ClaimTypes.PrimarySid, member.Id.ToString()));
                //context.Validated(identity);

                AuthenticationProperties propert = new AuthenticationProperties(new Dictionary<string, string> {

                    {"Id", member.Id.ToString()},
                    {"userNameSurname", member.userNameSurname},
                    {"userEmail", member.userEmail},
                    {"userAuthorityName", member.userAuthority.authorityName},

                });


                AuthenticationTicket ticket = new AuthenticationTicket(identity, propert);
                context.Validated(ticket);


            }
            else
            {
                context.SetError("Geçersiz İstek", "Hatalı Kullanıcı Bilgisi");
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
    }
}