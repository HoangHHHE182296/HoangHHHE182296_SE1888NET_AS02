using BusinessLogic.Services;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.Models.Data;
using Presentation.Models.Params;
using static Presentation.Models.Params.CategoryParams;

namespace Presentation.Pages.Category {
    public class IndexModel : PageModel {

        private readonly CategoryService _categoryService;

        public IndexModel(CategoryService categoryService) {
            _categoryService = categoryService;
        }

        [BindProperty(SupportsGet = true)]
        public SearchCategoryParams SearchParams { get; set; } = new();

        public List<CategoryData> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetSearchCategoryAsync() {
            var keyword = string.IsNullOrWhiteSpace(SearchParams.Keyword) ? null : SearchParams.Keyword;
            Status? status = SearchParams.Status == null ? null : (Status)Enum.Parse(typeof(Status), SearchParams.Status);

            var categories = await _categoryService.SearchCategoryAsync(keyword, status);

            var list = categories.Select(c => new CategoryData {
                Id = c.CategoryId,
                Name = c.CategoryName,
                Description = c.CategoryDescription,
                Status = c.Status,
                ArticleQuantity = c.ArticleCount,
                Parent = c.ParentCategory == null ? null : new CategoryData {
                    Id = c.ParentCategory.CategoryId,
                    Name = c.ParentCategory.CategoryName,
                    Status = c.ParentCategory.Status,
                    Description = c.ParentCategory.CategoryDescription,
                }
            }).ToList();

            return ViewComponent("CategoryList", list);
        }
    }
}
