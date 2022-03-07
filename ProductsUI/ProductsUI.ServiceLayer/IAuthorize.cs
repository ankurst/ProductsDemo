using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsUI.ServiceLayer
{
    public interface IAuthorize
    {
        bool IsUserAuthorized(string userId, string methodName);
    }
}
