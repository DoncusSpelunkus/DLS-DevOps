using FeatureHubSDK;
public interface IFeatureHubContext
{
    Task<IClientContext> GetFeatureHubContextAsync();
}