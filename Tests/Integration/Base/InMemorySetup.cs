using System;

namespace Tests.Integration.Base
{
    public class InMemorySetup
    {
        public readonly string DbName = $"{Guid.NewGuid()}_test_database";
    }
}
