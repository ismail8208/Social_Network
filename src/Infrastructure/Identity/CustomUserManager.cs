using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using IdentityModel;
using IdentityModel.Client;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Users.Commands.CreateUserCommand;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MediaLink.Infrastructure.Identity;
public class CustomUserManager : UserManager<ApplicationUser>
{
    private readonly ISender _mediator;
    private readonly IApplicationDbContext _context;

    public CustomUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger,
        ISender mediator, IApplicationDbContext context)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _mediator = mediator;
        _context = context;
    }

    public override async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        var result = await base.CreateAsync(user, password);

        if (result.Succeeded)
        {
            CreateUserCommand command = new CreateUserCommand();
            command.Email = user.Email;
            command.UserName = user.UserName;
            command.Password = password;
            command.FirstName = user.FirstName;
            command.LastName = user.LastName;
            user.User = await _mediator.Send(command);
            await UpdateAsync(user);
        }

        return result;
    }

    public async override Task<IdentityResult> AddToRolesAsync(ApplicationUser user, IEnumerable<string> roles)
    {
        return await base.AddToRolesAsync(user, roles);
    }

    public override Task<IdentityResult> UpdateAsync(ApplicationUser user)
    {
        return base.UpdateAsync(user);
    }
    public override async Task<IdentityResult> DeleteAsync(ApplicationUser user)
    {
        var innerUser = await _context.InnerUsers.FirstOrDefaultAsync(x => x.UserName == user.UserName && x.IsDeleted == false);

        if (innerUser == null)
        {
            throw new NotFoundException(nameof(user));
        }

        innerUser.IsDeleted = true;
        user.IsDeleted = true;

        await _context.SaveChangesAsync(CancellationToken.None);

        return await base.UpdateAsync(user);
    }

    public override async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
    {
        var innerUser = await _context.InnerUsers.FirstOrDefaultAsync(x => x.UserName == user.UserName && x.IsDeleted == false);

        if (innerUser == null)
        {
            throw new DirectoryNotFoundException(nameof(user));
        }

        innerUser!.Password = newPassword;
        _context.InnerUsers.Update(innerUser);

        await _context.SaveChangesAsync(CancellationToken.None);
        return await base.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    public override async Task<IdentityResult> ChangeEmailAsync(ApplicationUser user, string newEmail, string token)
    {
        var innerUser = await _context.InnerUsers.FirstOrDefaultAsync(x => x.UserName == user.UserName);
      
        if (innerUser == null)
        {
            throw new DirectoryNotFoundException(nameof(user));
        }

        innerUser!.Email = newEmail;
        _context.InnerUsers.Update(innerUser);

        await _context.SaveChangesAsync(CancellationToken.None);
        return await base.ChangeEmailAsync(user, newEmail, token);
    }

    public override Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
    {

        return base.AddToRoleAsync(user, role);
    }
}
