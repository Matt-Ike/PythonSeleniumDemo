using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TastyTreats.Model.Entities;
using TastyTreats.Service;
using TastyTreats.WebFrontEnd.Models;

namespace TastyTreats.WebFrontEnd.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ReviewService service = new ReviewService();

        // GET: ReviewsController/Create
        public ActionResult Create(int? recipeId)
        {
            try
            {
                if (recipeId == null)
                    return new BadRequestResult();

                Review review = new()
                {
                    RecipeId = recipeId.Value,
                    ReviewDate = DateTime.Now,
                };

                return View(review);
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        // POST: ReviewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Review review)
        {
            try
            {
                service.AddReview(review);

                if(review.Errors.Count > 0)
                {
                    return View(review);
                }

                return RedirectToAction("Details", "Recipe", new { id = review.RecipeId});
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }
    }
}
