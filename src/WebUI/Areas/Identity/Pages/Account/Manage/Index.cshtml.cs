// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Azure.Core;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.FilesHandling;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.CVService.DTOs;
using MediaLink.Application.Users.Commands.UpdateUserCommand;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Enums;
using MediaLink.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebUI.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IApplicationDbContext _context;
        private readonly ISender _mediator;
        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IApplicationDbContext context,
            ISender mediator
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _mediator = mediator;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }
        public string ImageUrl { get; private set; }


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

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Summary")]
            public string Summary { get; set; }

            [Display(Name = "specialization")]
            public string specialization { get; set; }

            [Display(Name = "Profile Image")]
            public IFormFile ProfileImage { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var innerUser = await _context.InnerUsers.FirstOrDefaultAsync(u => u.UserName == userName && u.IsDeleted == false);

            Username = userName;
            ImageUrl = innerUser.ProfileImage;


            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = innerUser.FirstName,
                LastName = innerUser.LastName,
                Summary = innerUser.Summary,
                specialization = innerUser.specialization
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
            try
            {
                var innerUser = await _context.InnerUsers.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.IsDeleted == false);
                innerUser.FirstName = Input.FirstName;
                innerUser.LastName = Input.LastName;
                innerUser.Summary = Input.Summary;
                innerUser.specialization = Input.specialization;
                if (Input.ProfileImage != null)
                {
                    innerUser.ProfileImage = await SaveFile.Save(FileType.image, Input.ProfileImage);
                }
                var res = await UpdateInnerUser(innerUser);
            }
            catch (Exception)
            {

                throw;
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<bool> UpdateInnerUser(InnerUser user)
        {
            var appUser = await _userManager.FindByNameAsync(user.UserName!);

            if (appUser == null)
            {
                throw new NotFoundException(nameof(user));
            }

            appUser.FirstName = user.FirstName;
            appUser.LastName = user.LastName;
            var result = await _userManager.UpdateAsync(appUser);

            if (result.Succeeded)
            {
                UpdateUserCommand updateCommand = new UpdateUserCommand();
                updateCommand.Username= user.UserName;
                updateCommand.Summary = user.Summary;
                updateCommand.FirstName = user.FirstName;
                updateCommand.LastName = user.LastName;
                updateCommand.ProfileImage = user.ProfileImage;
                updateCommand.specialization = user.specialization;
                await _mediator.Send(updateCommand);

            }
            return result.Succeeded;
        }
    }
}
