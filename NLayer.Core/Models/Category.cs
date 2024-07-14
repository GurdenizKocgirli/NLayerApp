namespace NLayer.Core
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; } //IEnumerable'dan miras alan ICollection, koleksiyon içerisinde gezinmenin dışında crud operasyonlarının da gerçekleşebilmesini sağlar
        //Her category nesnesinin birden fazla product'a sahip olabileceğini belirtir
    }
}
