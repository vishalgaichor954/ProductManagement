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
                        await Task.Run(() => context.Validated(identity));
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
    }
}