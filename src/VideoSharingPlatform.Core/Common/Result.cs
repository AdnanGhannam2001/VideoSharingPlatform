namespace VideoSharingPlatform.Core.Common;

public class Result<V, E> : IEquatable<Result<V, E>> {
    public V? Value { get; init; }
    public E? Error { get; init; }

    public Result(V value) => Value = value;
    public Result(E error) => Error = error;

    public bool Equals(Result<V, E>? other) => Equals(other);
    public override bool Equals(object? obj) {
        return obj is not null
            && obj is Result<V, E> result
            && ((Value is null && result.Value is null)
                || (Value is not null && result.Value is not null && Value.Equals(result.Value)))
            && ((Error is null && result.Error is null)
                || (Error is not null && result.Error is not null && Error.Equals(result.Error)));
    }

    public static bool operator ==(Result<V, E> a, Result<V, E> b) => a.Equals(b);
    public static bool operator !=(Result<V, E> a, Result<V, E> b) => !a.Equals(b);

    public override int GetHashCode() => GetHashCode();

    public bool IsSuccess => Error is null;

    public override string ToString()
    {
        return Value is not null
            ? Value?.ToString() ?? ""
            : Error?.ToString() ?? "";
    }
}
