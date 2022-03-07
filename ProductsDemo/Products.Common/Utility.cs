using System.Web.Script.Serialization;

namespace Products.Common
{
    public static class Utility
    {
        public static string ToJson(object src)
        {
            if (src == null) return string.Empty;

            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            return serializer.Serialize(src);
        }
    }
}
