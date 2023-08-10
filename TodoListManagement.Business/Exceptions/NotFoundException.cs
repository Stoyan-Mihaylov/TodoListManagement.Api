using System;

namespace TodoListManagement.Business.Exceptions
{
    public class NotFoundException : Exception
    {
        private const string NotFound = "Not Found";

        public NotFoundException(string message) : base(message) { }
        public NotFoundException() : base(NotFound) { }
    }
}
