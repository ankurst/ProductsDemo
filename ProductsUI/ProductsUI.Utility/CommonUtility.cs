using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ProductsUI.Utility
{
    public class CommonUtility
    {
        public static T GetObjectFromJson<T>(string json)
        {
            T temp = Activator.CreateInstance<T>();

            JavaScriptSerializer serializer = new JavaScriptSerializer
            {
                MaxJsonLength = Int32.MaxValue
            };
            temp = serializer.Deserialize<T>(json);

            return temp;
        }
    }
}
