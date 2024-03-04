namespace VideoSharingPlatform.Core.Common;

public class Result<V, E> : IEquatable<Result<V, E>>
    where E : ExceptionBase
{
    private readonly V? _value;
    private readonly E[]? _exceptions;

    public V Value {
        get {
            if (_value is null)
                throw new InvalidOperationException($"Can't access '{nameof(Value)}' when {nameof(IsSuccess)} is false");

            return _value!;
        }
        init {
            _value = value;
        }
    }

    public E[] Exceptions {
        get {
            if (_exceptions is null)
                throw new InvalidOperationException($"Can't access '{nameof(Exceptions)}' when {nameof(IsSuccess)} is true");

            return _exceptions!;
        }
        init {
            _exceptions = value;
        }
    }

    public Result(V value) => Value = value;
    public Result(params E[] exceptions) => Exceptions = exceptions;

    public bool Equals(Result<V, E>? other) => Equals(other);
    public override bool Equals(object? obj) {
        return obj is not null
            && obj is Result<V, E> result
            && ((Value is null && result.Value is null)
                || (Value is not null && result.Value is not null && Value.Equals(result.Value)))
            && ((Exceptions is null && result.Exceptions is null)
                || (Exceptions is not null && result.Exceptions is not null && Exceptions.Equals(result.Exceptions)));
    }

    public static bool operator ==(Result<V, E> a, Result<V, E> b) => a.Equals(b);
    public static bool operator !=(Result<V, E> a, Result<V, E> b) => !a.Equals(b);

    public override int GetHashCode() => GetHashCode();

    public bool IsSuccess => Value is not null && Exceptions is null;

    public override string ToString()
    {
        // TODO: Change Exceptions.ToString()
        return Value is not null
            ? Value.ToString() ?? ""
            : Exceptions.ToString() ?? "";
    }
}
