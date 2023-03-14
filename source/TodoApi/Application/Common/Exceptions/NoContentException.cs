using System.Net;

namespace TodoApi.Application.Common.Exceptions
{
    public class NoContentException : CustomException
    {

        public NoContentException(string message) : base(message, null, HttpStatusCode.NotFound)
        {
        }

    }
}
