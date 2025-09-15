using Auora.ConDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;

namespace Auora.Pages
{
    public class RegisteUserModel : PageModel
    {
        private readonly UserService _userService;

        public RegisteUserModel(UserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public User User { get; set; } = new();
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (!UserService.IsPasswordValid(User.Password))
            {
                ModelState.AddModelError("User.Password", "The password must contain at least 6 characters, one uppercase letter, one lowercase letter, one number and one special character.");
                return Page();
            }

            Random rnd = new Random();

            User.Password = UserService.HashPassword(User.Password);
            User.imgPath = $"/media/img-profile/default-img/default{rnd.Next(1, 3)}.jpg";
            User.CreatedAt = DateTime.UtcNow;
            User.UpdatedAt = DateTime.UtcNow;
            User.IsActive = true;

            await _userService.CreateAsync(User);

            SetCookie("UserId", User.Id);

            return RedirectToPage("/Index");
        }

        public void SetCookie(string key, string value)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict
            };
            Response.Cookies.Append(key, value, cookieOptions);
        }

    }
}
