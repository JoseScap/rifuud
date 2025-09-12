namespace Api.Services.Interfaces;

public interface IStartupService
{
    Task InitializeRootUserAsync();
    Task WatchUpServerConfig();
    Task StartupServerConfiguration();
}
