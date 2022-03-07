using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsUI.ServiceLayer
{
    /// <summary>
    /// Placeholder class for providing Authorization functionality
    /// </summary>
    public class Authorize : IAuthorize
    {
        public bool IsUserAuthorized(string userId, string methodName)
        {
            return true;
            //returned true for demo purpose. Please provide your real implmentation here for user authorization
        }
    }
}
