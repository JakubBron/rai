using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Reservation
{
    public class ActionNotAllowedModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Reason { get; set; }
        public void OnGet()
        {
        }
    }
}
