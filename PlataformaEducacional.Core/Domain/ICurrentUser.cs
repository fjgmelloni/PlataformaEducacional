namespace PlataformaEducacao.Core.Domain
{
    public interface ICurrentUser
    {
        string GetUserId();
        bool IsAuthenticated();
    }
}
