using Booking.Core.Commons.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Commons.Handlers;

public abstract class GetEntityByKeyQueryHandlerBase<TDbContext, TEntity, TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TDbContext : DbContext
    where TEntity : class
    where TRequest : IRequest<TResponse>
{
    protected readonly TDbContext _dbContext;
    protected readonly IValidator<TRequest> _validator;

    protected GetEntityByKeyQueryHandlerBase(
        TDbContext dbContext,
        IValidator<TRequest> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    protected abstract Task<TEntity?> GetByKeyAsync(TRequest request);
    protected abstract TResponse MapToResponse(TEntity entity);

    public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        var entity = await GetByKeyAsync(request);
        if (entity == null) throw new ResourceNotFoundException($"Recurso {typeof(TEntity).Name} não encontrado.");
        return MapToResponse(entity);
    }
}