using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApi.Application.Common.Result
{
    public class FailureResult : OperationResult
    {

        private readonly Exception exception;
        private readonly string? message;

        public FailureResult(Exception exception, string? message = null)
        {
            this.exception = exception;
            this.message = message;
        }

        public override Exception? Exception => exception;
        public override string? Messages
        {
            get
            {
                if (message == null)
                {
                    if (exception.Message != null)
                    {
                        return exception.Message;
                    }
                    return default;
                };
                return message;
            }
        }
        public override bool Success => false;

    }
    public class FailureResult<TData> : OperationResult<TData>
    {

        private readonly Exception exception;
        private readonly string? message;

        public FailureResult(Exception exception, string? message = null)
        {
            this.exception = exception;
            this.message = message;
        }

        public override Exception? Exception => exception;
        public override string? Messages
        {
            get
            {
                if (message == null)
                {
                    if (exception.Message != null)
                    {
                        return exception.Message;
                    }
                    return default;
                };
                return message;
            }
        }
        public override TData? Result => default;
        public override bool Success => false;

    }
}
