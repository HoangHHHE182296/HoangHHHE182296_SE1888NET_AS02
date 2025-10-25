using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Account.Components.DeleteForm {
    public class DeleteFormViewComponent : ViewComponent {
        public IViewComponentResult Invoke(string name) {
            return View("Default", name);
        }
    }
}
