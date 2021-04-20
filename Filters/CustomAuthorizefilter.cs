using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ProductManagement.Filters
{
    public class CustomAuthorizefilter : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (!IsAuthorized(filterContext) && !HttpContext.Current.User.IsInRole(Roles))
            {
                ShowAuthenticationError(filterContext);
                return;
            }
            base.OnAuthorization(filterContext);
        }

        private static void ShowAuthenticationError(HttpActionContext filterContext)
        {
            var responseDTO = new { Code = 401, Message = "Unable to access service, Please login again" };
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, responseDTO);
        }
    }
}