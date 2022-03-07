using ProductsUI.Utility;
using ProductWebAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ProductsUI.ServiceLayer
{
    public class ProductServiceProvider : IProductServiceProvider
    {
        readonly string svcURI = ConfigurationManager.AppSettings["ProductsWebAPIBaseURL"];
        public bool AddProduct(ProductViewModel product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(svcURI);

                //HTTP POST                    
                var result = client.PostAsync("Products/Create",
                    new StringContent(new JavaScriptSerializer().Serialize(product), Encoding.UTF8, "application/json")).Result;

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else //web api sent error response 
                {
                    //log response status here..
                    return false;
                }
            }
        }

        public IList<ProductCategoryViewModel> GetCategoriesList()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(svcURI);
                //HTTP GET
                var result = client.GetAsync("ProductCategory").Result;

                if (result.IsSuccessStatusCode)
                {
                    var jsonResponse = result.Content.ReadAsStringAsync().Result;

                    return CommonUtility.GetObjectFromJson<List<ProductCategoryViewModel>>(jsonResponse);
                }
                else //web api sent error response 
                {
                    //log response status here..
                    return null;
                }
            }
        }

        public ProductViewModel GetProductDetails(long id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(svcURI);
                //HTTP GET
                var result = client.GetAsync("Products/Details?id=" + id.ToString()).Result;

                if (result.IsSuccessStatusCode)
                {
                    var jsonResponse = result.Content.ReadAsStringAsync().Result;

                    return CommonUtility.GetObjectFromJson<ProductViewModel>(jsonResponse);

                }
                else //web api sent error response 
                {
                    //log response status here..
                    return null;
                }
            }
        }

        public IList<ProductViewModel> GetProductsList()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(svcURI);
                //HTTP GET
                var result = client.GetAsync("Products").Result;

                if (result.IsSuccessStatusCode)
                {
                    var jsonResponse = result.Content.ReadAsStringAsync().Result;

                    return CommonUtility.GetObjectFromJson<List<ProductViewModel>>(jsonResponse);
                    
                }
                else //web api sent error response 
                {
                    //log response status here..
                    return null;
                }
            }
        }
        public ProductViewModel UpdateProduct(ProductViewModel product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(svcURI);

                //HTTP POST                    
                var result = client.PostAsync("Products/Edit",
                    new StringContent(new JavaScriptSerializer().Serialize(product), Encoding.UTF8, "application/json")).Result;

                if (result.IsSuccessStatusCode)
                {
                    return product;
                }
                else //web api sent error response 
                {
                    //log response status here..
                    return null;
                }
            }
        }
    }
}
