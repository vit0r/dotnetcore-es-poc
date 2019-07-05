using System;
using Elasticsearch.Net;
using Nest;

namespace elasticsearch_poc
{
  class Program
  {
    static void Main(string[] args)
    {
      var nodes = new Uri[]
            {
            new Uri("https://elasticsearch01:9200"),
            new Uri("https://elasticsearch01:9200"),
            new Uri("https://elasticsearch01:9200")
            };

      var pool = new StaticConnectionPool(nodes);
      var settings = new ConnectionSettings(pool);
      settings.BasicAuthentication("es_user", "test1");
      var idx = "testindex-" + DateTime.Now.ToString("yyyy.MM.dd");
      settings.DefaultIndex(idx);
      var client = new ElasticClient(settings);
      var request = new IndexExistsRequest(idx);
      var result = client.IndexExists(request);
      if (!result.Exists)
      {
        var taskCreate = client.CreateIndexAsync(idx).GetAwaiter();
        while (!taskCreate.IsCompleted)
        {
          Console.WriteLine(taskCreate.IsCompleted);
        }
      }
      Console.ReadLine();
    }
  }
}
