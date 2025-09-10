using Auora.ConDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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
                Address = user.Address,
                Country = user.Country,
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
                existingUser.Address = Input.Address;
                existingUser.imgPath = Input.imgPath;
                existingUser.Country = Input.Country;
                existingUser.PhoneNumber = Input.PhoneNumber;
                existingUser.BirthDate = Input.BirthDate;
                await _userService.UpdateProfileAsync(userId, existingUser, 0);

                TempData["SuccessMessage"] = "Perfil atualizado com sucesso.";
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
            public string imgPath { get; set; }
        }
        public async Task<IActionResult> OnPostUpdateImageAsync([FromBody] ImageRequest request)
        {



            try
            {
                var userId = Request.Cookies["UserId"];
                if (string.IsNullOrEmpty(userId))
                    return new JsonResult(new { success = false, message = "Usuário não encontrado." });

                var existingUser = await _userService.GetByIdAsync(userId);
                if (existingUser == null)
                    return new JsonResult(new { success = false, message = "Usuário inválido." });

                if (string.IsNullOrEmpty(request.imgPath))
                    return new JsonResult(new { success = false, message = "Nenhuma imagem selecionada." });

                existingUser.imgPath = request.imgPath;
                await _userService.UpdateProfileAsync(userId, existingUser, 1);

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar imagem");
                return new JsonResult(new { success = false, message = "Erro no servidor." });
            }



        }
        public async Task<IActionResult> OnPostUploadImageAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return new JsonResult(new { success = false, message = "Nenhum arquivo enviado." });

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine("wwwroot/media/img-profile/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = "/media/img-profile/uploads/" + fileName;

                var userId = Request.Cookies["UserId"];
                if (string.IsNullOrEmpty(userId))
                    return new JsonResult(new { success = false, message = "Usuário não encontrado." });

                var existingUser = await _userService.GetByIdAsync(userId);
                if (existingUser == null)
                    return new JsonResult(new { success = false, message = "Usuário inválido." });

                existingUser.imgPath = relativePath;
                await _userService.UpdateProfileAsync(userId, existingUser, 1);

                return new JsonResult(new { success = true, path = relativePath });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no upload da imagem");
                return new JsonResult(new { success = false, message = "Erro no servidor." });
            }
        }
    }
}
