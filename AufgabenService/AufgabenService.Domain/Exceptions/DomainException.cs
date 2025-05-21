namespace AufgabenService.Domain.Exceptions
{
    /// <summary>
    /// Domänenspezifische Exception
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
        
        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}