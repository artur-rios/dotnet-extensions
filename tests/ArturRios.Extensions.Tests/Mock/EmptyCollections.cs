using System.Collections;

namespace ArturRios.Extensions.Tests.Mock;

public class EmptyCollections : TheoryData<ITestEnumerable?>
{
    public EmptyCollections()
    {
        Add(null);
        Add(TestEnumerable.Empty);
        Add(new TestEnumerable());
    }
}