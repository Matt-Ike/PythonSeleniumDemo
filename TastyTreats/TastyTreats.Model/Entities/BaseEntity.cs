using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TastyTreats.Model.Entities
{
    public abstract class BaseEntity
    {
        public List<ValidationError> Errors = new();

        public void AddError(ValidationError error)
        {
            Errors.Add(error);
        }
    }
}
