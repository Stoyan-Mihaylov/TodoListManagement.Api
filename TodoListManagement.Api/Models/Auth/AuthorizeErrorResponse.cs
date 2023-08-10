namespace TodoListManagement.Api.Models.Auth;

public class AuthorizeErrorResponse
{
    private readonly string _error;
    public AuthorizeErrorResponse() { }

    public AuthorizeErrorResponse(string error)
    {
        _error = error;
    }
}
