using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TastyTreats.Model.Entities;
using TastyTreats.Types;
using TatyTreats.Repository;

namespace TastyTreats.Service
{
    public class ReviewService
    {
        #region Fields
        private readonly ReviewRepo repo = new();
        #endregion

        #region Public Methods
        public Review AddReview(Review review)
        {
            if (Validate(review))
                repo.Add(review);

            return review;
        }
        #endregion

        #region Private Methods
        private bool Validate(Review review)
        {
            List<ValidationResult> results = new();

            Validator.TryValidateObject(review, new ValidationContext(review), results, true);

            foreach (ValidationResult result in results)
            {
                review.AddError(new(result.ErrorMessage, ErrorType.Model));
            }

            return review.Errors.Count == 0;
        }
        #endregion
    }
}
