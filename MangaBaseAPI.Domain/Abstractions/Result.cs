﻿using System.Text.Json.Serialization;

namespace MangaBaseAPI.Domain.Abstractions
{
    public class Result
    {
        protected internal Result(bool isSuccess, Error error)
        {
            if ((isSuccess && error != Error.None) && (isSuccess && error != Error.Null))
            {
                throw new InvalidOperationException("Cannot create success result with error 1");
            }

            if ((!isSuccess && error == Error.None) && (!isSuccess && error == Error.Null))
            {
                throw new InvalidOperationException("Cannot create failed result with no error 2");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Error Error { get; }

        public static Result Success() => new(true, Error.None);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        public static Result SuccessNullError() => new(true, Error.Null);

        public static Result<TValue> SuccessNullError<TValue>(TValue value) => new(value, true, Error.Null);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

        public static Result Create(bool condition) =>
            condition ? Success() : Failure(Error.ConditionNotMet);

        public static Result<TValue> Create<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error) =>
            _value = value;

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result can not be accessed");

        public static implicit operator Result<TValue>(TValue value) => Create(value);
    }
}
