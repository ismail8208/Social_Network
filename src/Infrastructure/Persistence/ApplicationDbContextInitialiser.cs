using MediaLink.Domain.Entities;
using MediaLink.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MediaLink.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        var memberRole = new IdentityRole("member");

        if (_roleManager.Roles.All(r => r.Name != memberRole.Name))
        {
            await _roleManager.CreateAsync(memberRole);
        }

        var visitorRol = new IdentityRole("visitor");

        if (_roleManager.Roles.All(r => r.Name != visitorRol.Name))
        {
            await _roleManager.CreateAsync(visitorRol);
        }

        var companyRole = new IdentityRole("company");

        if (_roleManager.Roles.All(r => r.Name != companyRole.Name))
        {
            await _roleManager.CreateAsync(companyRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };
        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }

        /*        // Default innerUser
                // Seed, if necessary
                if (!_context.InnerUsers.Any())
                {
                    _context.InnerUsers.AddRange(
                        new InnerUser
                        {
                            FirstName = "F_user 1",
                            LastName = "L_user 1",
                            Email = "email user 1",
                            Password = "Password user 1",
                            DateOfBirth = DateTime.Now,
                            Gender = "Gender 1"
                        },
                        new InnerUser
                        {
                            FirstName = "F_user 2",
                            LastName = "L_user 2",
                            Email = "email user 2",
                            Password = "Password user 2",
                            DateOfBirth = DateTime.Now,
                            Gender = "Gender 2"
                        },
                        new InnerUser
                        {
                            FirstName = "F_user 3",
                            LastName = "L_user 3",
                            Email = "email user 3",
                            Password = "Password user 3",
                            DateOfBirth = DateTime.Now,
                            Gender = "Gender 3"
                        });
                    await _context.SaveChangesAsync();
                }

                // Default Posts
                // Seed, if necessary
                if (!_context.Posts.Any())
                {
                    _context.Posts.AddRange(
                        new Post
                        {
                            Content = " Content of 1 Post",
                            ImageURL = "Image 1",
                            NumberOfComments= 2,
                            NumberOfLikes= 2,
                            UserId = 1

                        },
                         new Post
                         {
                             Content = " Content of 2 Post",
                             ImageURL = "Image 2",
                             NumberOfComments = 5,
                             NumberOfLikes = 7,
                             UserId = 1

                         },
                          new Post
                          {
                              Content = " Content of 3 Post",
                              ImageURL = "Image 3",
                              NumberOfComments = 12,
                              NumberOfLikes = 8,
                              UserId = 1

                          },
                           new Post
                           {
                               Content = " Content of 4 Post",
                               VideoURL = "Video 1",
                               NumberOfComments = 6,
                               NumberOfLikes = 52,
                               UserId = 2

                           },
                            new Post
                            {
                                Content = " Content of 5 Post",
                                ImageURL = "Image 4",
                                NumberOfComments = 2,
                                NumberOfLikes = 2,
                                UserId = 2

                            },
                             new Post
                             {
                                 Content = " Content of 6 Post",
                                 ImageURL = "Image 5",
                                 NumberOfComments = 2,
                                 NumberOfLikes = 2,
                                 UserId = 3

                             }
                       );
                    await _context.SaveChangesAsync();
                }*/


    }
}
