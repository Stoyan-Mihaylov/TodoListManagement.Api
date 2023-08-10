using System.ComponentModel.DataAnnotations;
using TodoListManagement.Data.Enums;

namespace TodoListManagement.Business.Dtos.ImportDtos
{
    public class TodoItemDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public Status? Status { get; set; }

        public int PriorityId { get; set; }
    }
}
