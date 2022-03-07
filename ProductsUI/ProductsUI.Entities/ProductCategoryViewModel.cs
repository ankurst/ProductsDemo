using System.Collections.Generic;

namespace ProductWebAPI.ViewModel
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CategoryAttributeViewModel> Attributes { get; set; }
    }
}