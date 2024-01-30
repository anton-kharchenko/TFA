namespace TFA.Domain;

public class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}