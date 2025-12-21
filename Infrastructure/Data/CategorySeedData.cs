using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class CategorySeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryTypes>().HasData
             (
               new CategoryTypes { Id = 1, Name = "InformationTechnology",Code=(int)CategoryEnum.InformationTechnology },
               new CategoryTypes { Id = 2, Name = "FullstackDev",Code=(int)CategoryEnum.FullstackDev },
               new CategoryTypes { Id = 3, Name = "Sales",Code=(int)CategoryEnum.Sales },
               new CategoryTypes { Id = 4, Name = "HumanResources",Code=(int)CategoryEnum.HumanResources },
               new CategoryTypes { Id = 5, Name = "Marketing" ,Code = (int)CategoryEnum.Marketing}

             );
        }
    }
}
