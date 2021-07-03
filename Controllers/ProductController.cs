using ProductManagement.Filters;
using ProductManagement.Models;
using ProductManagement.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ProductManagement.Controllers
{
   
    public class ProductController : ApiController
    {
        ProductManagementEntities managementEntities = new ProductManagementEntities();

        #region Product List
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
        #endregion

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

        #region // GET api/<controller>/5
        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            ProductDataModel dataModel = new ProductDataModel();
            ProductMaster product = managementEntities.ProductMasters.Where(x => x.ID == id).FirstOrDefault();

            if (product != null)
            {
                var productImage = managementEntities.ProductImageMappings.Where(x => x.ID == product.ID).FirstOrDefault();
                dataModel.ID = product.ID;
                dataModel.ProductName = product.ProductName;
                dataModel.ProductDescription = product.ProductDescription;
                dataModel.productImageName = productImage.ImageTitle;
                dataModel.ProductKeyword = product.ProductKeyword;
                dataModel.ProductMetadata = product.ProductMetadata;
                dataModel.productImageFile = GetFileArray(productImage.ImagePath);
                dataModel.ProductPrice = product.ProductPrice;

                dataModel.StatusCode = 1;
                dataModel.StatusMessage = "Success";

                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
            else
            {
                dataModel.StatusCode = 0;
                dataModel.StatusMessage = "Invalid Product Request"; ;

                return Request.CreateResponse(HttpStatusCode.BadRequest, dataModel);
            }
        }
        #endregion

        #region // POST api/<controller>

        // POST api/<controller>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] ProductDataModel productData)
        {
            if (ModelState.IsValid)
            {
                ProductMaster product = new ProductMaster();

                product.ProductCategory = productData.ProductCategory;
                product.ProductType = productData.ProductType;

                product.ProductName = productData.ProductName;
                product.ProductDescription = productData.ProductDescription;
                product.ProductKeyword = productData.ProductKeyword;
                product.ProductMetadata = productData.ProductMetadata;
                product.ProductPrice = productData.ProductPrice;
                product.IsActive = true;
                product.CreatedBy = productData.CreatedBy;
                product.CreatedDate = DateTime.Now;

                managementEntities.ProductMasters.Add(product);
                managementEntities.SaveChanges();

                if (product.ID != 0)
                {
                    ProductImageMapping productImage = new ProductImageMapping();

                    productImage.SequenceNo = 1;
                    productImage.CreatedBy = productData.CreatedBy;
                    productImage.CreatedDate = DateTime.Now;
                    productImage.ImageTitle = productImage.ImageTitle;
                    productImage.ImageDescription = productImage.ImageDescription;
                    productImage.ImagePath = DateTime.Now.ToLongDateString() + ".jpg";
                    SaveByteArrayAsImage(productImage.ImagePath, productData.productImageFile);


                    managementEntities.ProductImageMappings.Add(productImage);
                    managementEntities.SaveChanges();

                }
                productData.StatusCode = 1;
                productData.StatusMessage = "Product Added Successfully";

                return Request.CreateResponse(HttpStatusCode.OK, productData);
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                           .SelectMany(x => x.Errors)
                                           .Select(x => x.ErrorMessage));
                productData.StatusCode = 0;
                productData.StatusMessage = messages;

                return Request.CreateResponse(HttpStatusCode.BadRequest, productData);
            }

            //return usermaster;
        }

        #endregion

        #region Save Byte Array As Image
        private void SaveByteArrayAsImage(string filename, string base64String)
        {
            string path = "~/Images/ProductImages/" + filename;
            string finalPath = HttpContext.Current.Server.MapPath(path);
            if (!string.IsNullOrEmpty(base64String))
            {
                byte[] bytes = Convert.FromBase64String(base64String);

                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }

                image.Save(finalPath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        #endregion


        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, ProductDataModel productData)
        {
            if (ModelState.IsValid)
            {
                if (id != 0)
                {
                    var product = managementEntities.ProductMasters.Where(x => x.ID == id).FirstOrDefault();
                    if (product != null)
                    {
                        product.ProductName = productData.ProductName;
                        product.ProductDescription = productData.ProductDescription;
                        product.ProductKeyword = productData.ProductKeyword;
                        product.ProductMetadata = productData.ProductMetadata;
                        product.ProductPrice = productData.ProductPrice;
                        product.ProductMetadata = productData.ProductMetadata;

                        managementEntities.ProductMasters.Attach(product);
                        managementEntities.Entry(product).State = EntityState.Modified;
                        managementEntities.SaveChanges();

                        var productImage = managementEntities.ProductImageMappings.Where(x => x.ProductID == id).FirstOrDefault();

                        if (productData.IsImageEdited == 1)
                        {
                            productImage.SequenceNo = 1;
                            productImage.UpdatedBy = productData.CreatedBy;
                            productImage.UpdatedDate = DateTime.Now;
                            productImage.ImageTitle = productData.productImageName;
                            productImage.ImageDescription = productData.ProductDescription;
                            productImage.ImagePath = DateTime.Now.ToLongDateString() + ".jpg";
                            SaveByteArrayAsImage(productImage.ImagePath, productData.productImageFile);
                        }
                        else
                        {
                            productImage.SequenceNo = 1;
                            productImage.UpdatedBy = productData.CreatedBy;
                            productImage.UpdatedDate = DateTime.Now;
                            productImage.ImageTitle = productData.productImageName;
                            productImage.ImageDescription = productData.ProductDescription;
                            productImage.ImagePath = productData.OldImageName;
                        }

                        managementEntities.ProductImageMappings.Attach(productImage);
                        managementEntities.Entry(productImage).State = EntityState.Modified;
                        managementEntities.SaveChanges();


                        productData.StatusCode = 1;
                        productData.StatusMessage = "Product updated Successfully";

                        return Request.CreateResponse(HttpStatusCode.OK, productData);
                    }
                    else
                    {
                        productData.StatusCode = 0;
                        productData.StatusMessage = "Invalid request : Product ID is missing.";

                        return Request.CreateResponse(HttpStatusCode.BadRequest, productData);
                    }
                }
                else
                {
                    productData.StatusCode = 0;
                    productData.StatusMessage = "Invalid request : Product ID is missing.";

                    return Request.CreateResponse(HttpStatusCode.BadRequest, productData);
                }
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                           .SelectMany(x => x.Errors)
                                           .Select(x => x.ErrorMessage));
                productData.StatusCode = 0;
                productData.StatusMessage = messages;

                return Request.CreateResponse(HttpStatusCode.BadRequest, productData);
            }
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            ProductDataModel productData = new ProductDataModel();
            var product = managementEntities.ProductMasters.Where(x => x.ID == id).FirstOrDefault();

            if (product != null)
            {
                product.IsActive = false;

                managementEntities.ProductMasters.Attach(product);
                managementEntities.Entry(product).State = EntityState.Modified;
                managementEntities.SaveChanges();

                productData.StatusCode = 1;
                productData.StatusMessage = "Product deleted Successfully";

                return Request.CreateResponse(HttpStatusCode.BadRequest, productData);
            }
            else
            {
                productData.StatusCode = 0;
                productData.StatusMessage = "Invalid request : Product ID is missing.";

                return Request.CreateResponse(HttpStatusCode.BadRequest, productData);
            }
        }
    }
}