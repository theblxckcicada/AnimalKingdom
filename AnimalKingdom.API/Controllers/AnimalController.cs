namespace api.Controllers;

using System.Linq.Expressions;
using AnimalKingdom.API.Controllers;
using AnimalKingdom.API.Handlers;
using AnimalKingdom.API.Models;
// using AnimalKingdom.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AnimalController(IMediator mediator) : EntityControllerBase<Animal, Guid>(mediator)
{
    [HttpGet]
    public override async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await base.Get(cancellationToken);
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        return await base.Get(id, cancellationToken);
    }

    [HttpPost]
    public override async Task<IActionResult> Add(
        [FromBody] Animal entity,
        CancellationToken cancellationToken
    )
    {
        return await base.Add(entity, cancellationToken);
    }

    [HttpPut("{id}")]
    public override async Task<IActionResult> Update(
        Guid id,
        [FromBody] Animal entity,
        CancellationToken cancellationToken
    )
    {
        return await base.Update(id, entity, cancellationToken);
    }

    [HttpDelete(template: "{id}")]
    public override async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        return await base.Delete(id, cancellationToken);
    }

    [HttpPost("query")]
    protected override QueryEntitiesRequest<Animal, Guid> BuildQuery(EntityQuery query)
    {
        var sort = query.Sort ?? nameof(Animal.Name);
        var desc = string.Equals(
            query.SortDirection,
            "desc",
            StringComparison.InvariantCultureIgnoreCase
        );
        var filter = query.Filter ?? string.Empty;

        return new QueryEntitiesRequest<Animal, Guid>
        {
            PageSize = query.PageSize,
            PageIndex = query.PageIndex,
            Sort = q =>
            {
                var param = Expression.Parameter(typeof(Animal), "x");
                var property = Expression.Property(param, sort);
                var lambda = Expression.Lambda<Func<Animal, object>>(
                    Expression.Convert(property, typeof(object)),
                    param
                );

                if (desc)
                    return q.OrderByDescending(lambda);
                else
                    return q.OrderBy(lambda);
            },
            Filter = x => x.Name.ToString().Contains(filter),
        };
    }
}
