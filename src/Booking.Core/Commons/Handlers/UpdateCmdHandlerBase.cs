using Booking.Core.Commons.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Commons.Handlers;

public abstract class UpdateCmdHandlerBase<TDbContext, TEntity, TRequest, TResponse, TBusinessEvent> : IRequestHandler<TRequest, TResponse>
    where TDbContext : DbContext
    where TEntity : class
    where TRequest : IRequest<TResponse>
    where TBusinessEvent : INotification
{
    protected readonly TDbContext _dbContext;
    protected readonly IValidator<TRequest> _validator;
    protected readonly IMediator _mediator;

    protected UpdateCmdHandlerBase(
        TDbContext dbContext,
        IValidator<TRequest> validator,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _validator = validator;
        _mediator = mediator;
    }

    protected abstract Task<TEntity?> GetByKeyAsync(TRequest request);
    protected abstract void UpdateEntity(TRequest request, TEntity entity);
    protected abstract TResponse MapToResponse(TEntity entity);
    protected abstract TBusinessEvent MapToEvent(TEntity entity);

    protected async virtual Task UpdateEntityAsync(TEntity entity, TRequest request, CancellationToken cancellationToken)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        var entity = await GetByKeyAsync(request);
        if (entity == null) throw new ResourceNotFoundException($"Recurso {typeof(TEntity).Name} não encontrado.");
        UpdateEntity(request, entity);
        await _mediator.Publish(MapToEvent(entity));
        await UpdateEntityAsync(entity, request, cancellationToken);
        return MapToResponse(entity);
    }
}