namespace NLayer.Core
{
    public class ProductFeature //Product nesnelerinin özelliklerini tanımlamak için kullanılan bir entity
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } //Her bir ProductFeature nesnesinin sadece bir adet product ile ilişkili olacağını belirtir
    }
}
