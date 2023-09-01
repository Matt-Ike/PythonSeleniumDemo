using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TastyTreats.Types;

namespace TastyTreats.Model.Entities
{
    public class ValidationError
    {
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }


        public ValidationError(string desc, ErrorType type)
        {
            Description = desc;
            ErrorType = type;
        }
    }
}
