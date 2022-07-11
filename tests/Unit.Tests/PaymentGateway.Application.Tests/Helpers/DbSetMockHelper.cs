namespace PaymentGateway.Application.Tests.Helpers
{
    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using MockQueryable.Moq;
    using System.Linq;

    public static class DbSetMockHelper
    {
        public static DbSet<T> BuildMockDbSet<T> (List<T> entities) where T : class
        {
            return entities.AsQueryable().BuildMockDbSet().Object;
        }
    }
}
