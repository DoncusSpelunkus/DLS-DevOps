using FeatureHubSDK;

public class FeatureHubContextService : IFeatureHubContext
{
    private EdgeFeatureHubConfig _edgeFeatureHubConfig;
    private IClientContext _featureHubContext;

    public FeatureHubContextService()
    {
        _edgeFeatureHubConfig = new EdgeFeatureHubConfig("http://localhost:8085", "5cc62fb9-3d9e-4f73-a80d-1dbcfca21b2e/6dq92LV7N9xn1meXY0TpwR88xAhUvSfn2sjPbaOp");
    }

    public async Task<EdgeFeatureHubConfig> GetFeatureHubContextAsync()
    {
        
        return _edgeFeatureHubConfig;
    }
}