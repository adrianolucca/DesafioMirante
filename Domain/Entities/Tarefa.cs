using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Tarefa
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public DateTime DataVencimento { get; set; }
        public int StatusId { get; set; }

        public Status Status { get; set; } = null!;
    }
}
