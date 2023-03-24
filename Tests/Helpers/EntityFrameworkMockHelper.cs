using Microsoft.EntityFrameworkCore;
using SV.Demo.Application.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.Helpers;

public static class EntityFrameworkMockHelper
{
    public static Mock<IApplicationContext> GetMockContext<TEntity>(
        IQueryable<TEntity> data,
        Expression<Func<IApplicationContext, DbSet<TEntity>>> expressionDbSet)
        where TEntity : class
    {
        var mockSet = MockedDbSet(data);

        var mockContext = new Mock<IApplicationContext>();
        mockContext.Setup(expressionDbSet).Returns(mockSet.Object);

        return mockContext;
    }

    public static void MockDbSet<TEntity>(this Mock<IApplicationContext> mockContext, IQueryable<TEntity> data, Expression<Func<IApplicationContext, DbSet<TEntity>>> expressionDbSet)
        where TEntity : class, new()
    {
        var mockSet = MockedDbSet(data);
        mockContext.Setup(expressionDbSet).Returns(mockSet.Object);
    }

    private static Mock<DbSet<TEntity>> MockedDbSet<TEntity>(IQueryable<TEntity> data) where TEntity : class
    {
        var mockSet = new Mock<DbSet<TEntity>>();
        mockSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] ids) => MockFind(ids[0], data));
        
        mockSet.Setup(m => m.Remove(It.IsAny<TEntity>())).Callback<TEntity>(t =>
        {
            var temp = data.ToList();
            temp.Remove(t);

            data = temp.AsQueryable();
        });

        mockSet.Setup(m => m.AddAsync(It.IsAny<TEntity>(), default))
            .Callback<TEntity, CancellationToken>((t, _) =>
            {
                var temp = data.ToList();
                temp.Add(t);

                data = temp.AsQueryable();
            });

        //mockSet.Setup(m => m.ToList()).Returns(data.ToList());

        //mockSet.Setup(set => set.Add(It.IsAny<TEntity>())).Callback<TEntity>((x) =>
        //{
        //    var temp = data.ToList();
        //    temp.Add(x);

        //    data = temp.AsQueryable();
        //});
        //mockSet.Setup(set => set.Attach(It.IsAny<TEntity>())).Callback<TEntity>((x) =>
        //{
        //    var temp = data.ToList();
        //    var pkProperty = GetPrimaryKey(x);
        //    var t = MockFind(pkProperty.GetValue(x), temp);

        //    temp.Remove(t);
        //    temp.Add(x);

        //    data = temp.AsQueryable();
        //});
        //mockSet.Setup(set => set.AddRange(It.IsAny<IEnumerable<TEntity>>())).Callback<IEnumerable<TEntity>>((x) =>
        //{
        //    var temp = data.ToList();

        //    foreach (var entity in x)
        //    {
        //        temp.Add(entity);
        //    }

        //    data = temp.AsQueryable();
        //});
        //mockSet.Setup(set => set.RemoveRange(It.IsAny<IEnumerable<TEntity>>())).Callback<IEnumerable<TEntity>>(ts =>
        //{
        //    var temp = data.ToList();
        //    foreach (var t in ts.ToList()) { temp.Remove(t); }

        //    data = temp.AsQueryable();
        //});

        var queryableMock = mockSet.As<IQueryable<TEntity>>();
        queryableMock.Setup(m => m.Provider).Returns(data.Provider);
        queryableMock.Setup(m => m.Expression).Returns(data.Expression);
        queryableMock.Setup(m => m.ElementType).Returns(data.ElementType);
        queryableMock.Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

        return mockSet;
    }

    /// <summary>
    /// This method assumes there is an Id property that acts as a key
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="id"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    private static TEntity MockFind<TEntity>(object id, IEnumerable<TEntity> table)
    {
        TEntity result = default(TEntity);
        foreach (var e in table)
        {
            var pkProperty = GetPrimaryKey(e);
            var value = pkProperty.GetValue(e);
            var convertedValue = Convert.ChangeType(value, id.GetType());
            if (convertedValue.Equals(id))
            {
                result = (TEntity)e;
                break;
            }
        }
        return result;
    }

    private static PropertyInfo GetPrimaryKey<TEntity>(TEntity entity)
    {
        Type type = entity.GetType();
        return type.GetProperties().FirstOrDefault(x => x.CustomAttributes.Any(c => c.AttributeType.Name == "KeyAttribute"));
    }
}
