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
    internal class AddressSeed : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasData(new Address
            {
                Id = 1,
                UserId = 1,
                Name = "Kartal/İSTANBUL",
                CreatedDate = DateTime.Now
            },
            new Address
            {
                Id = 2,
                UserId = 2,
                Name = "Maltepe/İSTANBUL",
                CreatedDate = DateTime.Now
            },
            new Address
            {
                Id = 3,
                UserId = 3,
                Name = "Kadıköy/İSTANBUL",
                CreatedDate = DateTime.Now
            }
            );
        }
    }
}
