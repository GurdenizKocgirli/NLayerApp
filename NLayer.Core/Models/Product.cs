namespace NLayer.Core
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } //Bir Product nesnesinin sadece bir Category'e bağlı olabileceğini belirtir
        public ProductFeature ProductFeature { get; set; } //Bir Product nesnesinin sadece bir adet ProductFeature'a sahip olabileceğini belirtir
    }
}
