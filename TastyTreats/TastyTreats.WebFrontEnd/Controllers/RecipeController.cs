using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TastyTreats.Model.DTO;
using TastyTreats.Model.Entities;
using TastyTreats.Service;
using TastyTreats.WebFrontEnd.Models;

namespace TastyTreats.WebFrontEnd.Controllers
{
    public class RecipeController : Controller
    {
        private readonly RecipeService service = new RecipeService();

        // GET: RecipeController
        public ActionResult Index()
        {
            try
            {
                return View(service.GetAllWithDetails());
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        // GET: RecipeController/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                    return new BadRequestResult();

                RecipeReviewsDTO recipesWithReviewsDTO = service.GetRecipeWithReviews(id.Value);

                if (recipesWithReviewsDTO == null)
                    return new NotFoundResult();

                RecipeDetailsVM recipesWithListings = new()
                {
                    RecipeId = recipesWithReviewsDTO.Details.RecipeId,
                    Title = recipesWithReviewsDTO.Details.Title,
                    Chef = recipesWithReviewsDTO.Details.ChefFullName,
                    Yield = recipesWithReviewsDTO.Details.Yield,
                    Archived = recipesWithReviewsDTO.Details.Archived,
                    Reviews = recipesWithReviewsDTO.Reviews

                };

                return View(recipesWithListings);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        // GET: RecipeController/Create
        public ActionResult Create()
        {
            try
            {
                RecipeCreateVM vm = new RecipeCreateVM()
                {
                    Chefs = GetChefs(),
                    RecipeTypes = GetRecipeTypes(),
                };

                return View(vm);
            }
            catch(Exception ex)
            {
                return ShowError(ex);
            }
        }

        // POST: RecipeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Recipe recipe = new Recipe()
                {
                    Title = collection["Title"].ToString(),
                    ChefId = Convert.ToInt32(collection["ChefId"]),
                    Yield = Convert.ToInt32(collection["Yield"]),
                    DateAdded = DateTime.Now,
                    RecipeTypeId = Convert.ToInt32(collection["RecipeTypeId"]),
                    Archived = (collection["Archived"].ToString() == "true,false") ? true : false 
                };

                service.AddRecipe(recipe);

                if (recipe.Errors.Count > 0 )
                {
                    RecipeCreateVM vm = new()
                    {
                        Title = collection["Title"].ToString(),
                        ChefId = Convert.ToInt32(collection["ChefId"]),
                        Yield = Convert.ToInt32(collection["Yield"]),
                        DateAdded = DateTime.Now,
                        RecipeTypeId = Convert.ToInt32(collection["RecipeTypeId"]),
                        Archived = Convert.ToBoolean(collection["Archived"]),
                        Chefs = GetChefs(),
                        RecipeTypes = GetRecipeTypes(),
                        Errors = recipe.Errors
                    };

                    return View(vm);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        // GET: RecipeController/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                    return new BadRequestResult();

                Recipe r = service.Get(id.Value);

                if (r == null)
                    return new NotFoundResult();

                RecipeEditVM vm = new RecipeEditVM()
                {
                    Recipe = r,
                    RecipeTypes = GetRecipeTypes(),
                    Chefs = GetChefs()
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        // POST: RecipeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecipeEditVM vm)
        {
            try
            {
                vm.Recipe = service.UpdateRecipe(vm.Recipe);

                if (vm.Recipe.Errors.Count == 0)
                    return RedirectToAction(nameof(Index));

                vm.RecipeTypes = GetRecipeTypes();
                vm.Chefs = GetChefs();

                return View(vm);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        // GET: RecipeController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                RecipeReviewsDTO recipesWithReviewsDTO = service.GetRecipeWithReviews(id);

                if (recipesWithReviewsDTO == null)
                    return new NotFoundResult();

                RecipeDetailsVM vm = new()
                {
                    RecipeId = recipesWithReviewsDTO.Details.RecipeId,
                    Title = recipesWithReviewsDTO.Details.Title,
                    Chef = recipesWithReviewsDTO.Details.ChefFullName,
                    Yield = recipesWithReviewsDTO.Details.Yield,
                    Archived = recipesWithReviewsDTO.Details.Archived,
                    Reviews = recipesWithReviewsDTO.Reviews

                };

                return View(vm);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        // POST: RecipeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                service.DeleteRecipe(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        #region Private Methods
        private List<SelectListItem> GetRecipeTypes()
        {
            return new LookupsService().GetRecipeTypes().Select(rt => new SelectListItem
            {
                Value = rt.RecipeTypeId.ToString(),
                Text = rt.RecipeTypeName.ToString(),
            }).ToList();
        }

        private List<SelectListItem> GetChefs()
        {
            return new LookupsService().GetChefs().Select(c => new SelectListItem
            {
                Value = c.ChefId.ToString(),
                Text = c.ChefName.ToString(),
            }).ToList();
        }

        private ActionResult ShowError(Exception ex)
        {
            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Exception = ex
            });
        }
        #endregion
    }
}
