// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Comp1640.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Comp1640.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;

        private readonly IWebHostEnvironment _webHostEnvironment;


 private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        public string ProfileImagePath { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Profile picture")]
            public IFormFile ProfilePicture { get; set; }
        }

        private async Task LoadAsync(CustomUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            // Lấy thông tin người dùng từ HttpContext.User thay vì _signInManager.Context.User
        var claimsPrincipal = _httpContextAccessor.HttpContext.User;

        // Truy cập đối tượng CustomUser từ ClaimsPrincipal
        var customUser = await _userManager.GetUserAsync(claimsPrincipal);

        // Lấy đường dẫn ảnh đại diện từ CustomUser
        var profileImagePath = customUser.ProfileImagePath;

            Username = userName;
            ProfileImagePath = profileImagePath;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            // Xử lý và lưu ảnh đại diện
            if (Input.ProfilePicture != null)
            {
                // Tạo đường dẫn duy nhất cho tệp ảnh
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.ProfilePicture.FileName;
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "avatar");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Lưu tệp ảnh vào thư mục wwwroot/avatar
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfilePicture.CopyToAsync(fileStream);
                }

                // Lưu đường dẫn tệp ảnh vào đối tượng user
                user.ProfileImagePath = uniqueFileName;
            }

            // Cập nhật thông tin người dùng trong cơ sở dữ liệu
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
