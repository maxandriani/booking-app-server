using Booking.Core.Commons.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Commons.Handlers;

public abstract class DeleteCmdHandlerBase<TDbContext, TEntity, TRequest, TBusinessEvent> : IRequestHandler<TRequest>
    where TDbContext : DbContext
    where TEntity : class
    where TRequest : IRequest
    where TBusinessEvent : INotification
{
    protected readonly TDbContext _dbContext;
    protected readonly IValidator<TRequest> _validator;
    protected readonly IMediator _mediator;

    protected DeleteCmdHandlerBase(
        TDbContext dbContext,
        IValidator<TRequest> validator,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _validator = validator;
        _mediator = mediator;
    }

    protected abstract Task<TEntity?> GetByKeyAsync(TRequest request);
    protected abstract TBusinessEvent MapToEvent(TEntity entity);

    protected async virtual Task DeleteEntityAsync(TEntity entity, TRequest request, CancellationToken cancellationToken)
    {
        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        var entity = await GetByKeyAsync(request);
        if (entity == null) throw new ResourceNotFoundException($"Recurso {typeof(TEntity).Name} não encontrado.");
        await _mediator.Publish(MapToEvent(entity));
        await DeleteEntityAsync(entity, request, cancellationToken);
        return Unit.Value;
    }
}