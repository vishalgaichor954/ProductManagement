using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductManagement.Controllers
{
    public class ProductController : ApiController
    {
        ProductManagementEntities managementEntities = new ProductManagementEntities();
        // GET api/<controller>
        public IEnumerable<Usermaster> Get()
        {
            IEnumerable<Usermaster> usermaster = managementEntities.Usermasters.ToList();
            foreach (var itm in usermaster)
            {
                itm.CityMaster = null;
                itm.CoutryMaster = null;
                itm.StateMaster = null;
            }
            return usermaster;
        }

        // GET api/<controller>/5
        public Usermaster Get(int id)
        {
            Usermaster usermaster = managementEntities.Usermasters.Where(x => x.ID == id).FirstOrDefault();
            return usermaster;
        }

        // POST api/<controller>
        public void Post([FromBody] Usermaster usermaster)
        {
            managementEntities.Usermasters.Add(usermaster);
            managementEntities.SaveChanges();
            //return usermaster;
        }

        // PUT api/<controller>/5
        public void Put(int id, Usermaster usermaster)
        {
            managementEntities.Usermasters.Attach(usermaster);
            managementEntities.Entry(usermaster).State = EntityState.Modified;
            managementEntities.SaveChanges();
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            var itemToRemove = managementEntities.Usermasters.SingleOrDefault(x => x.id == 1); //returns a single item.

            if (itemToRemove != null)
            {
                managementEntities.Usermasters.Remove(itemToRemove);
                managementEntities.SaveChanges();
            }
        }
    }
}