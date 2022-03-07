using System.Collections.Generic;

namespace ProductWebAPI.Models
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ProductCategoryViewModel Category { get; set; }

        public List<ProductAttributeViewModel> Attributes { get; set; }
    }
}