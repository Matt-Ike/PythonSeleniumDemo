using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TastyTreats.Model.Entities
{
    public class Review : BaseEntity
    {
        [Display(Name = "ID")]
        public int ReviewId { get; set; }

        [Display(Name = "Comment")]
        [Required(ErrorMessage = "The comment field is required")]
        public string Comment { get; set; }

        [Display(Name = "Stars")]
        [Required(ErrorMessage = "The stars field is required")]
        [Range(0, 5, ErrorMessage = "Stars can only be numbers between 0 and 5")]
        public int Stars { get; set; }

        [Display(Name = "Review Date")]
        [Required(ErrorMessage = "The review date field is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ReviewDate { get; set; }

        public int RecipeId { get; set; }
    }
}
