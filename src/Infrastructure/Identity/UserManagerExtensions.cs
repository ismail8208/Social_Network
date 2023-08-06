using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Users.Commands.UpdateUserCommand;
using MediaLink.Application.Users.Queries.FindUser;
using MediaLink.Domain.Entities;
using MediaLink.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public static class UserManagerExtensions
{
    /* public static async Task<bool> IsUserActiveAsync(this UserManager<ApplicationUser> userManager, string userId)
     {


     }*/
   /* public async  Task<bool> UpdateInnerUser( InnerUser user)
    {
        var appUser = await userManager.FindByNameAsync(user.UserName!);

        if (appUser == null)
        {
            throw new NotFoundException(nameof(user));
        }

        appUser.FirstName = user.FirstName;
        appUser.LastName = user.LastName;
        var result = await userManager.UpdateAsync(appUser);

        if (result.Succeeded)
        {
            UpdateUserCommand updateCommand = new UpdateUserCommand();
            updateCommand.Summary = user.Summary;
            updateCommand.FirstName = user.FirstName;
            updateCommand.LastName = user.LastName;
            updateCommand.ProfileImage = user.ProfileImage;
            await _mediator.Send(updateCommand);

        }
        return result.Succeeded;
    }*/
    public static async Task<bool> UpdateUserAsync(this UserManager<IdentityUser> userManager, InnerUser user)
    {
        var existingUser = await userManager.FindByNameAsync(user.UserName!);
        if (existingUser == null)
        {
            return false;
        }

  /*      existingUser.UserName = user.UserName;
        existingUser.Email = user.Email;*/
        // Add more properties as needed...

        var result = await userManager.UpdateAsync(existingUser);
        return result.Succeeded;
    }

/*    public static async Task<UserDto> GetInnerUser(this UserManager<IdentityUser> userManager, string username)
    {
        return await 
    }*/

    // Add more custom extension methods as needed...
}