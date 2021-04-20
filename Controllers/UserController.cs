using ProductManagement.Filters;
using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace ProductManagement.Controllers
{
    public class UserController : ApiController
    {
        ProductManagementEntities managementEntities = new ProductManagementEntities();
        // GET api/<controller>
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/GetUserAuthenticate")]
        public IHttpActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;            
            return Ok(identity.IsAuthenticated);
        }
    }
}