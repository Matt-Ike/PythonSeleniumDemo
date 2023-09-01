using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TastyTreats.Model.DTO
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public int Stars { get; set; }

        [Display(Name = "Date Reviewed")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ReviewDate { get; set; }
    }
}
