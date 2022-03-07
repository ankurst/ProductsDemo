namespace ProductWebAPI.ViewModel
{
    /// <summary>
    /// mapped to ProductAttributeLookup db table
    /// </summary>
    public class CategoryAttributeViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public int AttributeId { get; set; }
    }
}