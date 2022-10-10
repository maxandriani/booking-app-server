using BookingApp.Core.Commons.Queries;
using BookingApp.Core.Commons.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Linq.Dynamic.Core;

namespace BookingApp.Core.Commons.Handlers;

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
    protected abstract IQueryable<TResponse> MapToResponse(IQueryable<TEntity> query);
    protected abstract IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query);

    protected virtual IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, IPageableQuery request)
    {
        if (request.Skip != null)
        {
            if (request.Skip < 0) throw new ArgumentOutOfRangeException($"O parâmetro Skip não pode ser inferior a Zero.");
            query = query.Skip(request.Skip.Value);
        }

        if (request.Take != null)
        {
            if (request.Take < 1) throw new ArgumentOutOfRangeException($"O parâmetro Take não pode ser inferior a 1.");
            query = query.Take(request.Take.Value);
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
        var query = CreateSearchQuery(request).AsNoTracking();
        var totalCount = await query.CountAsync(cancellationToken);

        if (request is ISortableQuery sorteableRequest)
            query = ApplySorting(query, sorteableRequest);

        if (request is IPageableQuery pageableRequest)
            query = ApplyPagination(query, pageableRequest);

        var items = MapToResponse(query).AsAsyncEnumerable();
        return new CollectionResponse<TResponse>(items, totalCount);
    }
}