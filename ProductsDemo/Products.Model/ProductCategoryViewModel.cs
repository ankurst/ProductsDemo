using System.Collections.Generic;

namespace ProductWebAPI.Models
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CategoryAttributeViewModel> Attributes { get; set; }
    }
}