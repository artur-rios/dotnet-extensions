using System.Collections;
using Xunit.Abstractions;

namespace ArturRios.Extensions.Tests.Mock;

public class TestEnumerable : ITestEnumerable
{
    private List<object?> _items = [];
    
    public TestEnumerable()
    {
    }
    
    public static TestEnumerable Empty => new();
    
    public TestEnumerable(IEnumerable<object?> items)
    {
        _items = items.ToList();
    }
    
    public TestEnumerable(params object?[] items)
    {
        _items = items.ToList();
    }

    public IEnumerator GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    public void Deserialize(IXunitSerializationInfo info)
    {
        var arr = info.GetValue<object?[]>("Items");
        
        _items = arr?.ToList() ?? [];
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue("Items", _items.ToArray());
    }
}