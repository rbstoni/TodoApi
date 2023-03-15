using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApi.Application.Common.Result
{
    public class SuccessResult : OperationResult
    {

        private readonly string? message;

        public SuccessResult(string? message = null)
        {
            this.message = message;
        }

        public override bool Success => true;
        public override string? Messages => message ?? default;
        public override Exception? Exception => default;

    }

    public class SuccessResult<TData> : OperationResult<TData>
    {
        private readonly TData data;
        private readonly string? message;

        public SuccessResult(TData data, string? message = null)
        {
            this.data = data;
            this.message = message;
        }
        public override TData Result => data;
        public override bool Success => true;
        public override string? Messages => message ?? default;
        public override Exception? Exception => default;

    }
}
