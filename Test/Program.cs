using FeatureHubSDK;

var config = new EdgeFeatureHubConfig("http://featurehub:8085", "1a112027-e3c7-4836-b79e-09afd4efce0e/6CG0MX01lQAZgtGLxXDOY4mRAz13nBloTSLleV4A");
var fh = await config.NewContext().Build();

await Task.Run(() =>
{
   while(true)
   {
      Console.WriteLine(fh["Test"].IsEnabled);
      Thread.Sleep(1000);
   }
});