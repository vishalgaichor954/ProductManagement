using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ProductManagement.Providers
{
    public class OauthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() => context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            using (var db = new ProductManagementEntities())
            {
                if (db != null)
                {
                    var user = db.Usermasters.Where(o => o.EmailID == context.UserName && o.Password == context.Password).FirstOrDefault();
                    if (user != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, user.UserRolemaster.Role));
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                        identity.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));

                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                        keyValuePairs.Add("username", user.Username);
                        keyValuePairs.Add("roles", user.UserRolemaster.Role);

                        var props = new AuthenticationProperties(keyValuePairs);
                        var ticket = new AuthenticationTicket(identity, props);

                        await Task.Run(() => context.Validated(ticket));
                        //await Task.Run(() => context.Validated(identity));
                    }
                    else
                    {
                        context.SetError("Wrong Crendtials", "Provided username and password is incorrect");
                    }
                }
                else
                {
                    context.SetError("Wrong Crendtials", "Provided username and password is incorrect");
                }
                return;
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