using System.Collections;
using Xunit.Abstractions;

namespace ArturRios.Extensions.Tests.Mock;

public interface ITestEnumerable : IEnumerable, IXunitSerializable;
