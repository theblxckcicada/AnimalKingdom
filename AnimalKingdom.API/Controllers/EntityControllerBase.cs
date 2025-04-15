using AnimalKingdom.API.Handlers;
using AnimalKingdom.API.Models;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimalKingdom.API.Controllers;

//[Authorize]
public abstract class EntityControllerBase<TModel, TKey>(IMediator mediator) : ControllerBase
    where TModel : class, IEntityBase<TKey>
{
    protected readonly IMediator mediator = mediator;

    [HttpPost]
    public virtual async Task<IActionResult> Add(
        [FromBody] TModel entity,
        CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(
            new AddEntityRequest<TModel, TKey> { Entity = entity },
            cancellationToken
        );

        response.ValidationResult.AddToModelState(ModelState);
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (response.Entity == null)
        {
            return Problem();
        }

        return CreatedAtAction(nameof(Get), new { id = response.Entity.Id }, response.Entity);
    }

    [HttpPost("bulk")]
    public virtual async Task<IActionResult> AddBulkAsync(
        [FromBody] List<TModel> entities,
        CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(
            new AddEntityRequest<TModel, TKey> { Entities = entities },
            cancellationToken
        );

        response.ValidationResult.AddToModelState(ModelState);
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (response.Entities is null)
        {
            return Problem();
        }

        return CreatedAtAction(nameof(Get), response.Entities);
    }
    [HttpPost("bulk")]
    public virtual async Task<IActionResult> UpdateBulkAsync(
        [FromBody] List<TModel> entities,
        CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(
            new UpdateEntityRequest<TModel, TKey> { Entities = entities },
            cancellationToken
        );

        response.ValidationResult.AddToModelState(ModelState);
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (response.Entities is null)
        {
            return Problem();
        }

        return CreatedAtAction(nameof(Get), response.Entities);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var entities = await mediator.Send(
            new GetEntitiesRequest<TModel, TKey>(),
            cancellationToken
        );
        return Ok(entities);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(TKey id, CancellationToken cancellationToken)
    {
        var entity = await mediator.Send(
            new GetEntityRequest<TModel, TKey> { Id = id },
            cancellationToken
        );

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(entity);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(
        TKey id,
        [FromBody] TModel entity,
        CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(
            new UpdateEntityRequest<TModel, TKey> { Id = id, Entity = entity },
            cancellationToken
        );

        response.ValidationResult.AddToModelState(ModelState);
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(response.Entity);
    }

    [HttpDelete(template: "{id}")]
    public virtual async Task<IActionResult> Delete(TKey id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new RemoveEntityRequest<TModel, TKey> { Id = id },
            cancellationToken
        );

        response.ValidationResult.AddToModelState(ModelState);
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (response.Entity == null)
        {
            return NotFound();
        }

        return Ok(response.Entity);
    }

    [HttpPost("query")]
    public virtual async Task<IActionResult> Query(
        [FromBody] EntityQuery query,
        CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(BuildQuery(query), cancellationToken);

        return Ok(response);
    }

    protected abstract QueryEntitiesRequest<TModel, TKey> BuildQuery(EntityQuery query);
}
