using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApi.Application.Common.Result
{
    public abstract class OperationResult
    {
        public abstract bool Success { get; }
        public abstract string? Messages { get; }
        public abstract Exception? Exception { get; }
    }
    public abstract class OperationResult<T> : OperationResult
    {
        public abstract T? Result { get; }
    }
}
