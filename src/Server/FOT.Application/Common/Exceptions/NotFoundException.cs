namespace FOT.Application.Common.Exceptions;

/// <summary>
/// Not found exception.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/>.
    /// </summary>
    public NotFoundException() : base("Not found")
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/>.
    /// </summary>
    public NotFoundException(string message) : base(message)
    {
    }
}