using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Category.Components.CreateUpdateForm {
    public class CreateUpdateFormViewComponent : ViewComponent {
        public IViewComponentResult Invoke() {
            return View();
        }
    }
}
