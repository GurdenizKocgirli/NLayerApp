using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Seeds
{
    internal class CreditCardSeed : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.HasData(new CreditCard
            {
                Id = 1,
                UserId = 1,
                Name = "Akbank",
                CreatedDate = DateTime.Now
            },
            new CreditCard
            {
                Id = 2,
                UserId = 2,
                Name = "FinansBank",
                CreatedDate = DateTime.Now
            },
            new CreditCard
            {
                Id = 3,
                UserId = 3,
                Name = "VakıfBank",
                CreatedDate = DateTime.Now
            });
                
        }
    }
}
