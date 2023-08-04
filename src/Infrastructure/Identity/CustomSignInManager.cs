using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MediaLink.Infrastructure.Identity;
public class CustomSignInManager : SignInManager<ApplicationUser> 
{
    public CustomSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<ApplicationUser> confirmation)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }

    public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        // Custom check for username
        var user = await UserManager.FindByNameAsync(userName);
        if (user == null)
        {
            return SignInResult.Failed;
        }
        if (user.IsDeleted)
        {
            //_pageModel.ModelState.AddModelError(user.UserName + ",", "This account has been deleted due to violating MediaLink community standards. If you think an error has occurred, you can contact us via this number { +62 859-7173-0924 }");
            return SignInResult.Failed;
        }
        // Call the base method if custom check passed
        return await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
    }
}

