using Booking.Core.Commons.Queries;
using Booking.Core.Commons.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Booking.Core.Commons.Handlers;

public abstract class SearchEntityQueryHandlerBase<TDbContext, TEntity, TRequest, TResponse> : IRequestHandler<TRequest, CollectionResponse<TResponse>>
    where TDbContext : DbContext
    where TEntity : class
    where TRequest : IRequest<CollectionResponse<TResponse>>
{
    protected readonly TDbContext _dbContext;
    protected readonly IValidator<TRequest> _validator;

    protected SearchEntityQueryHandlerBase(
        TDbContext dbContext,
        IValidator<TRequest> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    protected abstract IQueryable<TEntity> CreateSearchQuery(TRequest request);
    protected abstract TResponse MapToResponse(TEntity entity);
    protected abstract IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query);

    protected virtual IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, IPageableQuery request)
    {
        if (request.Take != null)
        {
            if (request?.Take < 1) throw new ArgumentOutOfRangeException($"O parâmetro Take não pode ser inferior a 1.");
            var take = request?.Take ?? 0;
            query = query.Take(take);
        }

        if (request.Skip != null)
        {
            if (request?.Skip < 0) throw new ArgumentOutOfRangeException($"O parâmetro Skip não pode ser inferior a Zero.");
            var skip = request?.Skip ?? 0;
            query = query.Skip(skip);
        }

        return query;
    }

    protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, ISortableQuery request)
    {
        if (string.IsNullOrEmpty(request?.SortBy)) return ApplyDefaultSorting(query);
        return query.OrderBy(request.SortBy);
    }

    public virtual async Task<CollectionResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        var query = CreateSearchQuery(request);
        var totalCount = await query.CountAsync(cancellationToken);

        if (request is ISortableQuery sorteableRequest)
            query = ApplySorting(query, sorteableRequest);
        
        if (request is IPageableQuery pageableRequest)
            query = ApplyPagination(query, pageableRequest);
        
        var items = query.Select(x => MapToResponse(x)).AsAsyncEnumerable();
        return new CollectionResponse<TResponse>(items, totalCount);
    }
}