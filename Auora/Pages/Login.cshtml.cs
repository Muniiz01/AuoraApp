using Auora.ConDB;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Auora.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;
        public LoginModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public async Task<JsonResult> OnPostLoginAsync([FromBody] LoginRequest request)
        {
            var user = await _userService.ValidateUser(request.Email, request.Password);
            if (user == null)
            {
                return new JsonResult(new { success = false, message = "Invalid email or password." });
            }

            SetCookie("UserId", user.Id.ToString());
            return new JsonResult(new { success = true,
                user = new {
                    id = user.Id,
                    name = user.Name
                }
               });
        }

        public class LoginRequest
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        public void SetCookie(string key, string value)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append(key, value, cookieOptions);
        }
    }
}
