using ProductsUI.Models;
using ProductWebAPI.ViewModel;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using ProductsUI.ServiceLayer;


namespace ProductsUI.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IAuthorize _authorizeObj = null;
        private readonly IProductServiceProvider _productSvcProvider = null;
        private string userId = string.Empty;

        public HomeController() { }

        public HomeController(IAuthorize authorizeObj = null, IProductServiceProvider _productSvcObj = null)
        {
            //caller who wants its own implementation can pass it in this constructor (e.g. test proj passing mock)
            //else this constructor will instanciate it
            _authorizeObj = authorizeObj ?? new Authorize();
            _productSvcProvider = _productSvcObj ?? new ProductServiceProvider();
        }
        public ActionResult Index()
        {
            userId = User.Identity.Name;

            if (!_authorizeObj.IsUserAuthorized(userId,"Index"))
            {
                return View("Error");
            }
            
            IList<ProductViewModel> products = null;
            var gridProducts = new List<ProductListForGrid>();
            string attrSeparator = ConfigurationManager.AppSettings["AttributeSeparator"];

            try
            {
                products = _productSvcProvider.GetProductsList();

                if (products != null)
                {
                    foreach (var p in products)
                    {
                        var gridProduct = new ProductListForGrid()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Category = p.Category
                        };
                        var tmp = string.Empty;
                        foreach (var prodAttr in p.Attributes)
                        {
                            foreach (var catAttr in p.Category.Attributes)
                            {
                                if (prodAttr.Id == catAttr.AttributeId && p.Category.Id == catAttr.CategoryId)
                                {
                                    tmp += catAttr.Name + " - " + prodAttr.Value + attrSeparator;
                                }
                            }
                        }
                        gridProduct.DisplayAttributes = tmp.Remove(tmp.LastIndexOf("|")).TrimEnd();
                        gridProducts.Add(gridProduct);
                    }
                }
                else //web api sent error response 
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error with null response. Please contact helpdesk.");
                }                
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error. Please ensure that WebAPI is up and running OR contact helpdesk.");
                return View(new List<ProductListForGrid>());
            }
            return View(gridProducts);
        }
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Fetch all categories alongwith all attributes name for each category (from Web API)
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCategoriesDetails()
        {
            IList<ProductCategoryViewModel> productWithCategory = null;

            try
            {
                productWithCategory  = _productSvcProvider.GetCategoriesList();

                if (productWithCategory == null)                 
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error with null response. Please contact helpdesk.");
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error. Please ensure that WebAPI is up and running OR contact helpdesk.");
                return Json(new List<ProductCategoryViewModel>(), JsonRequestBehavior.AllowGet);
            }
            return Json(productWithCategory, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(ProductViewModel productObj)
        {
            try
            {
                bool result = _productSvcProvider.AddProduct(productObj);

                if (result)
                {
                    return Json(productObj);
                }
                else //web api sent error response 
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact helpdesk.");
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error. Please ensure that WebAPI is up and running OR contact helpdesk.");
                return Json(new ProductViewModel());
            }
            return Json(new ProductViewModel());
        }

        // GET: Products/Edit/1
        public ActionResult Edit(long id)
        {
            ViewBag.pId = id;
            return View("Create");            
        }

        [HttpPost]
        public JsonResult Edit(ProductViewModel productObj)
        {
            try
            {
                productObj =  _productSvcProvider.UpdateProduct(productObj);

                if (productObj != null)
                    return Json(productObj);
                else
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact helpdesk.");
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error. Please ensure that WebAPI is up and running OR contact helpdesk.");
                return Json(new ProductViewModel());
            }
            return Json(new ProductViewModel());
        }

        public JsonResult GetProductDetails(long id)
        {
            ProductViewModel product = null;

            try
            {
                product = _productSvcProvider.GetProductDetails(id);

                if (product == null)
                {
                    //log response status here..
                    ModelState.AddModelError(string.Empty, "Server error. Please contact helpdesk.");
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error. Please ensure that WebAPI is up and running OR contact helpdesk.");
                return Json(new ProductViewModel(), JsonRequestBehavior.AllowGet);
            }
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Products Demo page.";

            return View();
        }
    }
}