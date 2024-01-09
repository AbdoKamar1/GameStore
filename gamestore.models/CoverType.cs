using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamestore.models
{
    public class CoverType
    {
        [key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("Cover Type")]
        public string Name { get; set; }
    }
}
