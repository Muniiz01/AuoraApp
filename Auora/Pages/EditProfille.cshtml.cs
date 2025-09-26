using Auora.ConDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Auora.Pages
{
    public class EditProfilleModel : PageModel
    {
        [BindProperty]
        public EditProfileViewModel Input { get; set; } = default!;

        private readonly ILogger<EditProfilleModel> _logger;
        private readonly UserService _userService;


        public EditProfilleModel(ILogger<EditProfilleModel> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {



            var userId = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Error");

            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return NotFound();


            Input = new EditProfileViewModel
            {
                Name = user.Name,
                Street = user.Street,
                Country = user.Country,
                PostalCode = user.PostalCode,
                City = user.City,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate
            };

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateUserAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userId = Request.Cookies["UserId"];
                if (string.IsNullOrEmpty(userId))
                    return RedirectToPage("/Error");

                var existingUser = await _userService.GetByIdAsync(userId);
                if (existingUser == null)
                    return NotFound();


                existingUser.Name = Input.Name;
                existingUser.Street = Input.Street;
                existingUser.imgPath = Input.imgPath;
                existingUser.Country = Input.Country;
                existingUser.PostalCode = Input.PostalCode;
                existingUser.City = Input.City;
                existingUser.PhoneNumber = Input.PhoneNumber;
                existingUser.BirthDate = Input.BirthDate;
                await _userService.UpdateProfileAsync(userId, existingUser, 0);

                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the profile. Please try again.");
                return RedirectToPage("/Error");
            }
        }
        public class ImageRequest
        {
            public string? imgPath { get; set; }
        }
        public async Task<IActionResult> OnPostUpdateImageAsync([FromBody] ImageRequest request)
        {



            try
            {
                var userId = Request.Cookies["UserId"];
                if (string.IsNullOrEmpty(userId))
                    return new JsonResult(new { success = false, message = "User not found." });

                var existingUser = await _userService.GetByIdAsync(userId);
                if (existingUser == null)
                    return new JsonResult(new { success = false, message = "Invalid user." });

                if (string.IsNullOrEmpty(request.imgPath))
                    return new JsonResult(new { success = false, message = "No images selected." });

                existingUser.imgPath = request.imgPath;
                await _userService.UpdateProfileAsync(userId, existingUser, 1);

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating image");
                return new JsonResult(new { success = false, message = "Server error." });
            }



        }
        public async Task<IActionResult> OnPostUploadImageAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return new JsonResult(new { success = false, message = "No files sent." });

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine("wwwroot/media/img-profile/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = "/media/img-profile/uploads/" + fileName;

                var userId = Request.Cookies["UserId"];
                if (string.IsNullOrEmpty(userId))
                    return new JsonResult(new { success = false, message = "User not found." });

                var existingUser = await _userService.GetByIdAsync(userId);
                if (existingUser == null)
                    return new JsonResult(new { success = false, message = "Invalid user." });

                //funcao que deleta imagem anterior... desabilitada (temporario)
                /*
                var oldImg = existingUser.imgPath;
                if (System.IO.File.Exists($"wwwroot{oldImg}"))
                {
                    System.IO.File.Delete($"wwwroot{oldImg}");
                }
                */

                existingUser.imgPath = relativePath;
                await _userService.UpdateProfileAsync(userId, existingUser, 1);

                return new JsonResult(new { success = true, path = relativePath });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Image upload error");
                return new JsonResult(new { success = false, message = "Server error." });
            }
        }
    }
}
