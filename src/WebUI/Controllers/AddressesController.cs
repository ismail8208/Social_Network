using MediaLink.Application.Addresses.Commands.CreateAddress;
using MediaLink.Application.Addresses.Commands.DeleteAddress;
using MediaLink.Application.Addresses.Commands.UpdateAddress;
using MediaLink.Application.Addresses.Queries;
using MediaLink.Application.Addresses.Queries.GetAddressByUserId;
using MediaLink.Application.Addresses.Queries.SearchAddress;
using MediaLink.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaLink.WebUI.Controllers;

[Authorize]
public class AddressesController : ApiControllerBase
{
    [HttpGet("{address}/Search")]
    public async Task<ActionResult<PaginatedList<AddressDto>>> Search(string address)
    {
        var query = new SearchAddressQuery();
        query.Query = address;
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AddressDto>> Get(int id)
    {
        return await Mediator.Send(new GetAddressQuery(id));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateAddressCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateAddressCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteAddressCommand(id));

        return NoContent();
    }
}
