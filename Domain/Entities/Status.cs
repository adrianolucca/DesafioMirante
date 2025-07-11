using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;

        public ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
    }
}
