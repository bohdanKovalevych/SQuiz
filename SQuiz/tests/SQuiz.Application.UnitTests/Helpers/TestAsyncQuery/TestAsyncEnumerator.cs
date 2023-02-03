using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQuiz.Application.UnitTests.Helpers.TestAsyncQuery
{
    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public T Current
        {
            get
            {
                return _inner.Current;
            }
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public ValueTask<bool> MoveNextAsync()
        {
            try
            {
                var result = _inner.MoveNext();
                return new ValueTask<bool>(Task.FromResult(result));
            }
            catch (NullReferenceException)
            {
                return new ValueTask<bool>(Task.FromResult(false));
            }
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask();
        }
    }
}
