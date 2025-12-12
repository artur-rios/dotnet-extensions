using System.Collections;
using Xunit.Abstractions;

namespace ArturRios.Extensions.Tests.Mock;

public class TestEnumerable : ITestEnumerable
{
    private List<object?> _items = [];

    public TestEnumerable()
    {
    }

    public TestEnumerable(params object?[] items) => _items = items.ToList();

    public static TestEnumerable Empty => new();

    public IEnumerator GetEnumerator() => _items.GetEnumerator();

    public void Deserialize(IXunitSerializationInfo info)
    {
        var arr = info.GetValue<object?[]>("Items");

        _items = arr?.ToList() ?? [];
    }

    public void Serialize(IXunitSerializationInfo info) => info.AddValue("Items", _items.ToArray());
}
