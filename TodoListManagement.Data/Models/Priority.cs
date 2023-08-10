using System.Collections.Generic;

namespace TodoListManagement.Data.Models
{
    public class Priority
    {
        public Priority()
        {
            Items = new HashSet<Item>();
        }
        public int Id { get; set; }

        public string Title { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
