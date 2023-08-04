using MediaLink.Application.Comments.Queries.GetCommentsWithPagination;
using MediaLink.Application.Common.Models;
using MediaLink.Application.Users.Queries.FindUser;
using MediaLink.Application.Users.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaLink.WebUI.Controllers;
/*[Authorize]
*/public class UsersController : ApiControllerBase
{
    [HttpGet("{name}/Search")]
    public async Task<ActionResult<PaginatedList<UserDto>>> Search(string name)
    {
        var query = new SearchUserQuery();
        query.Query = name.ToString();
        return await Mediator.Send(query);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserDto>> Get(string username)
    {
        return await Mediator.Send(new GetUserQuery(username));
    }

}
