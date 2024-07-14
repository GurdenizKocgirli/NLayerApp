namespace NLayer.Core
{
    //Code first yaklaşımıyla kodlarımızı yazdığımız gözden kaçmamalı
    public abstract class BaseEntity //Entity'lerimiz için base bir sınıfın oluşturulması. Ortak ve her entity'nin kullandığı bu değerleri tekrar tekrar yazmak zorunda değiliz
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
