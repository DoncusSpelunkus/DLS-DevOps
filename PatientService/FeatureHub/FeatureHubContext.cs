using FeatureHubSDK;

public class FeatureHubContextService : IFeatureHubContext
{
    private EdgeFeatureHubConfig _edgeFeatureHubConfig;
    private IClientContext _featureHubContext;

    public FeatureHubContextService()
    {
        _edgeFeatureHubConfig = new EdgeFeatureHubConfig("http://featurehub:8085", "1a112027-e3c7-4836-b79e-09afd4efce0e/6CG0MX01lQAZgtGLxXDOY4mRAz13nBloTSLleV4A");
    }

    public async Task<IClientContext> GetFeatureHubContextAsync()
    {
        if (_featureHubContext == null)
        {
            _featureHubContext = await _edgeFeatureHubConfig.NewContext().Build();
        }
        return _featureHubContext;
    }
}