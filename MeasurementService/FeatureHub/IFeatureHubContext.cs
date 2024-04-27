using FeatureHubSDK;
public interface IFeatureHubContext
{
    Task<EdgeFeatureHubConfig> GetFeatureHubContextAsync();
}