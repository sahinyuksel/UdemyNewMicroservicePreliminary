using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UdemyMicroservices.Web.Constants;
using UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages.Instructor.Course;

[Authorize(Roles = RoleConstants.Instructor)]
public class UpdateCourseModel(CatalogService catalogService) : BasePageModel
{
    [BindProperty] public UpdateCourseViewModel ViewModel { get; set; } = UpdateCourseViewModel.Empty;

    public async Task OnGet(Guid id)
    {
        var categoryListAsResult = await catalogService.GetCategoryList();

        if (categoryListAsResult.IsFail)
        {
            // ModelState.AddModelError(string.Empty, categoryListAsResult.Error!);
        }

        var courseResult = await catalogService.GetCourse(id);

        if (courseResult.IsFail)
        {
            // ModelState.AddModelError(string.Empty, courseResult.Error!);
        }

        var course = courseResult.Data!;


        ViewModel.CategoryDropdownList =
            new SelectList(categoryListAsResult.Data!, "Id", "Name", course.Category.Id);
        ViewModel.PictureUrl = course.PictureUrl;
        ViewModel.Name = course.Name;
        ViewModel.Description = course.Description;
        ViewModel.Price = course.Price;
        ViewModel.CategoryId = course.Category.Id;
        ViewModel.Id = course.Id;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var categoriesAsResult = await catalogService.GetCategoryList();
            if (categoriesAsResult.IsFail) return ErrorPage(categoriesAsResult);
            ViewModel.SetCategoryDropdownList(categoriesAsResult.Data!);
            return Page();
        }


        var response = await catalogService.UpdateCourseAsync(ViewModel);

        if (response.IsFail)
            // ModelState.AddModelError(string.Empty, response.Error!);
            return Page();

        return RedirectToPage("GetAll");
    }
}