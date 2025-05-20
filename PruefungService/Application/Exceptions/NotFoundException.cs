namespace PruefungService.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity '{name}' ({key}) wurde nicht gefunden.")
        {
        }
    }
}