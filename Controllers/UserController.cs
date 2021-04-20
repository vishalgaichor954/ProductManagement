using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductManagement.Controllers
{
    public class UserController : ApiController
    {
        ProductManagementEntities managementEntities = new ProductManagementEntities();
        // GET api/<controller>
        public IEnumerable<string> Index()
        {
            return new string[] { "value1", "value2" };
        }
    }
}