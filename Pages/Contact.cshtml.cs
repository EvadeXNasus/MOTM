using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MOTM.Pages
{
    [AllowAnonymous]
    public class ContactModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
