using System.Diagnostics.CodeAnalysis;
using WhatsAppBusinessPlatform.Domain.Common;

namespace WhatsAppBusinessPlatform.Application.Common;
public class Result 
{
    public Result(bool isSuccess, DomainError error)
    {
        if (isSuccess && error != DomainError.None ||
            !isSuccess && error == DomainError.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public DomainError Error { get; }
    public static Result Success() => new(true, DomainError.None);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, DomainError.None);
    public static Result Failure(DomainError error) => new(false, error);
    public static Result<TValue> Failure<TValue>(DomainError error) => new(default, false, error);

    public static implicit operator Result(DomainError error) => Failure(error);
}
public class Result<TValue> : Result
{
    public Result(TValue? value, bool isSuccess, DomainError error) : base(isSuccess, error) 
        => Value = value;

    [NotNull]
    public TValue? Value
    {
        get
        {
            if (IsSuccess)
            {
                return field!;
            }
            throw new InvalidOperationException("The value of a failure result can't be accessed.");
        }
        private set => field = value;
    } 

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(DomainError.NullValue);

    public static implicit operator Result<TValue>(DomainError error) => Failure<TValue>(error);
    public static Result<TValue> ValidationFailure(DomainError error) => Failure<TValue>(error);
}
