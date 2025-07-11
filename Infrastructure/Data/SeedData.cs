using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioMirante.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Apply(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Nome = "Pendente" },
                new Status { Id = 2, Nome = "Em Andamento" },
                new Status { Id = 3, Nome = "Concluída" }
            );
        }
    }
}
