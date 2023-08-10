using TodoListManagement.Data.Enums;

namespace TodoListManagement.Data.Models
{
    public class Item
    {
        public Item(string title, string description, Status status, int priorityId)
        {
            Title = title;
            Description = description;
            Status = status;
            PriorityId = priorityId;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Status Status { get; set; }

        public int PriorityId { get; set; }

        public Priority Priority { get; set; }
    }
}
