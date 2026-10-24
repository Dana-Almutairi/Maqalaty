using System;
using System.ComponentModel.DataAnnotations;

namespace Maqalaty.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = false;

        // Foreign Key
        public int? CategoryId { get; set; }

        // Navigation Property
        public Category? Category { get; set; }
    }
}