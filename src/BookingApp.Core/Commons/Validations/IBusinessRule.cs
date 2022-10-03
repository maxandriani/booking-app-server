using MediatR;

namespace BookingApp.Core.Commons.Validators;

public interface IBusinessRule<TEntity>
    where TEntity : class
{
    string RuleName { get; }
    Task ValidateAndThrowAsync(TEntity entity, CancellationToken cancellationToken = default);
}

public interface ICreateBusinessRule<TEntity> : IBusinessRule<TEntity>
    where TEntity : class
{ }

public interface IUpdateBusinessRule<TEntity> : IBusinessRule<TEntity>
    where TEntity : class
{ }

public interface IDeleteBusinessRule<TEntity> : IBusinessRule<TEntity>
    where TEntity : class
{ }