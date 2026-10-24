using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maqalaty.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Article> Articles { get; set; }
            = new List<Article>();
    }
}