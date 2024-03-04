using System.Text;

namespace VideoSharingPlatform.Core.Common;

public class Result<V, E> : IEquatable<Result<V, E>>
    where V : notnull
    where E : ExceptionBase
{
    private readonly V? _value;
    private readonly E[]? _exceptions;

    public V Value {
        get {
            if (_value is null)
                throw new InvalidOperationException($"Can't access '{nameof(Value)}' when {nameof(IsSuccess)} is false");

            return _value;
        }
        init {
            _value = value;
        }
    }

    public E[] Exceptions {
        get {
            if (_exceptions is null)
                throw new InvalidOperationException($"Can't access '{nameof(Exceptions)}' when {nameof(IsSuccess)} is true");

            return _exceptions;
        }
        init {
            _exceptions = value;
        }
    }

    public Result(V value) => Value = value;
    public Result(params E[] exceptions) => Exceptions = exceptions;

    public override bool Equals(object? obj) {
        if (obj is Result<V, E> result) {
            bool valueEquality = (_value is null && result._value is null)
                || (result._value is not null
                    && _value?.Equals(result._value) == true);
            bool exceptionsEquality = (_exceptions is null && result._exceptions is null)
                || (result._exceptions is not null
                    && _exceptions?.SequenceEqual(result._exceptions) == true);

            return valueEquality && exceptionsEquality;
        }

        return false;
    }

    public bool Equals(Result<V, E>? other) => other is not null && Equals(other as object);

    public static bool operator ==(Result<V, E> a, Result<V, E> b) => a.Equals(b);
    public static bool operator !=(Result<V, E> a, Result<V, E> b) => !a.Equals(b);

    public override int GetHashCode() => GetHashCode();

    public bool IsSuccess => _value is not null && _exceptions is null;

    public override string ToString()
    {
        var builder = new StringBuilder();

        if (IsSuccess) {
            builder.Append(Value.ToString()).AppendLine();
        } else {
            builder.AppendLine("List of Exceptions:");

            foreach (var exception in Exceptions) {
                builder.Append(exception.ToString()).AppendLine();
            }
        }

        return builder.ToString();
    }
}
