using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace gamestore.models
{
    public class Category
    {
        [key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order Must Be Between 1 AND 100 Only !!")]
        public int DisplayOreder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }

}
