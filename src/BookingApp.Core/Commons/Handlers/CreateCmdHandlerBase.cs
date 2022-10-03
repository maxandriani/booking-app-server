using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Commons.Handlers;

public abstract class CreateCmdHandlerBase<TDbContext, TEntity, TRequest, TResponse, TBusinessEvent> : IRequestHandler<TRequest, TResponse>
    where TDbContext : DbContext
    where TEntity : class
    where TRequest : IRequest<TResponse>
    where TBusinessEvent : INotification
{
    protected readonly TDbContext _dbContext;
    protected readonly IValidator<TRequest> _validator;
    protected readonly IMediator _mediator;

    protected CreateCmdHandlerBase(
        TDbContext dbContext,
        IValidator<TRequest> validator,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _validator = validator;
        _mediator = mediator;
    }

    protected abstract TEntity MapToEntity(TRequest request);
    protected abstract TResponse MapToResponse(TEntity entity);
    protected abstract TBusinessEvent MapToEvent(TEntity entity);

    protected async virtual Task CreateEntityAsync(TEntity entity, TRequest request, CancellationToken cancellationToken)
    {
        _dbContext.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        var entity = MapToEntity(request);
        await _mediator.Publish(MapToEvent(entity));
        await CreateEntityAsync(entity, request, cancellationToken);
        return MapToResponse(entity);
    }
}