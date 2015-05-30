namespace ceenq.com.Core.Infrastructure.Compute
{
    public interface IServerCommand
    {
        void Initialize(params string[] parameters);
        string CommandText { get; }
    }
}