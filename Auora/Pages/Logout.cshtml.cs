using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Auora.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<JsonResult> OnPostLogoutAsync()
        {
            try
            {
                Response.Cookies.Delete("UserId");
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new 
                {
                    success = false,
                    message = "An error occurred while logging out."
                });
            }
        }
    }
}