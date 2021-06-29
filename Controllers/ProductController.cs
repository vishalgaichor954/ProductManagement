using ProductManagement.Filters;
using ProductManagement.Models;
using ProductManagement.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ApiController
    {
        ProductManagementEntities managementEntities = new ProductManagementEntities();
        // GET api/<controller>
        //This method is For all types of role  
        [HttpGet]
        public List<ProductDataModel> Get()
        {

            List<ProductDataModel> prodDataModelList = new List<ProductDataModel>();
            var productMasters = (from m in managementEntities.ProductMasters
                                  join n in managementEntities.ProductImageMappings on m.ID equals n.ProductID into ns
                                  from n in ns.DefaultIfEmpty()
                                  where n.SequenceNo == 1
                                  select new { m, n }).ToList();

            if (productMasters != null & productMasters.Count > 0)
            {
                foreach (var itm in productMasters)
                {
                    ProductDataModel productData = new ProductDataModel();
                    productData.ID = itm.m.ID;
                    productData.ProductName = itm.m.ProductName;
                    productData.ProductDescription = itm.m.ProductDescription;
                    productData.productImageName = itm.n.ImageTitle;
                    productData.ProductKeyword = itm.m.ProductKeyword;
                    productData.ProductMetadata = itm.m.ProductMetadata;
                    productData.productImageFile = GetFileArray(itm.n.ImagePath);
                    productData.ProductPrice = itm.m.ProductPrice;
                    prodDataModelList.Add(productData);
                }
            }

            return prodDataModelList;
        }


        #region Get File Array
        private string GetFileArray(string path)
        {
            string imgPath = "";

            if (!string.IsNullOrEmpty(path))
            {
                string finalPath = "";
                path = "~/Images/ProductImages/" + path;
                finalPath = HttpContext.Current.Server.MapPath(path);
                byte[] readText = File.ReadAllBytes(finalPath);

                if (readText != null && readText.Count() > 0)
                {
                    imgPath = Convert.ToBase64String(readText);
                }

            }
            return imgPath;
        } 
        #endregion


        // GET api/<controller>/5
        public ProductMaster Get(int id)
        {
            ProductMaster product = new ProductMaster(); //managementEntities.Usermasters.Where(x => x.ID == id).FirstOrDefault();
            return product;
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
            var itemToRemove = managementEntities.Usermasters.SingleOrDefault(x => x.ID == id); //returns a single item.

            if (itemToRemove != null)
            {
                managementEntities.Usermasters.Remove(itemToRemove);
                managementEntities.SaveChanges();
            }
        }
    }
}