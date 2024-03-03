using System.Collections;

namespace VideoSharingPlatform.Core.Common;

public class Page<T> : IEnumerable<T> {
    public Page(IEnumerable<T> items, int total) {
        Items = items;
        Total = total;
    }

    public IEnumerable<T> Items { get; init; }

    public int Total { get; init; }

    public IEnumerator<T> GetEnumerator() {
        foreach(var item in Items) {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString()
        => $"{Items.Count()} items out of {Total}";
}