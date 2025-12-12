namespace ArturRios.Extensions.Tests.Mock;

public class NotEmptyCollections : TheoryData<ITestEnumerable>
{
    private static readonly TestEnumerable TestData = new(1, 2, 3);

    public NotEmptyCollections() => Add(TestData);
}
