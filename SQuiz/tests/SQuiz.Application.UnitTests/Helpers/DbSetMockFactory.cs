using LanguageExt;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using SQuiz.Application.UnitTests.Helpers.TestAsyncQuery;
using SQuiz.Shared.Models.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace SQuiz.Application.UnitTests.Helpers
{
    public static class DbSetMockFactory
    {
        public static DbSet<TEntity> GetDbSetAsyncMock<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            var queryable = entities.AsQueryable();
            var mockSet = Substitute.For<DbSet<TEntity>, IQueryable<TEntity>, IAsyncEnumerable<TEntity>>();
            
            if (entities is List<TEntity> list)
            {
                mockSet.When(s => s.AddRangeAsync(Arg.Any<IEnumerable<TEntity>>()))
                    .Do(callInfo =>
                    {
                        list.AddRange((IEnumerable<TEntity>)callInfo.Args()[0]);
                    });

                mockSet.When(s => s.AddRange(Arg.Any<IEnumerable<TEntity>>()))
                    .Do(callInfo =>
                    {
                        list.AddRange((IEnumerable<TEntity>)callInfo.Args()[0]);
                    });

                mockSet.When(s => s.AddAsync(Arg.Any<TEntity>()))
                    .Do(callInfo =>
                    {
                        list.Add((TEntity)callInfo.Args()[0]);
                    });

                mockSet.When(s => s.Add(Arg.Any<TEntity>()))
                    .Do(callInfo =>
                    {
                        list.Add((TEntity)callInfo.Args()[0]);
                    });

                mockSet.When(s => s.Remove(Arg.Any<TEntity>()))
                    .Do(callInfo =>
                    {
                        list.Remove((TEntity)callInfo.Args()[0]);
                    });

                mockSet.When(s => s.RemoveRange(Arg.Any<IEnumerable<TEntity>>()))
                    .Do(callInfo =>
                    {
                        foreach (var ent in (IEnumerable<TEntity>)callInfo.Args()[0])
                        {
                            list.Remove(ent);
                        }
                    });
            }
            
            mockSet.AsQueryable<TEntity>().Provider.Returns(new TestAsyncQueryProvider<TEntity>(queryable.Provider));
            mockSet.AsQueryable<TEntity>().Expression.Returns(queryable.Expression);
            mockSet.AsQueryable<TEntity>().ElementType.Returns(queryable.ElementType);
            mockSet.AsQueryable<TEntity>().GetEnumerator().Returns(_ => queryable.GetEnumerator());
            mockSet.AsAsyncEnumerable<TEntity>().GetAsyncEnumerator().Returns(new TestAsyncEnumerator<TEntity>(queryable.GetEnumerator()));

            if (typeof(IResourceItem).IsAssignableFrom(typeof(TEntity)))
            {
                mockSet.FindAsync(Arg.Any<string>()).Returns(call =>
                {
                    var id = ((call[0] as object[])[0]) as string;

                    var entitiesList = (entities as IEnumerable<IResourceItem>).ToList();
                    var ret = entitiesList.Find(ti => ti.Id == id);

                    return new ValueTask<TEntity>(ret as TEntity);
                });
            }

            return mockSet;
        }
    }
}
