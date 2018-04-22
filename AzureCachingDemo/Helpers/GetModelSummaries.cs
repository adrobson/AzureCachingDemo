using AzureCachingDemo.EFModel;
using AzureCachingDemo.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCachingDemo.Helpers
{
    public class GetModelSummaries
    {
        private readonly CachingDemoContext cachingDemoContext;

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect("instercloudcachingdemo.redis.cache.windows.net,abortConnect=false,ssl=true,password=44dOq6snH+CTS1d4+zqmKqNhMMgGqE+kwQZ0hC0yUpI=");
        });
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        const string MODELKEYNAME = "ModelKeyName";
        public List<ModelSummary> ModelSummaryList { get; set; }
        public TimeSpan TimeToGetModels { get; private set; }

        public GetModelSummaries(CachingDemoContext _cachingDemoContext)
        {
            DateTime beforeCreateModels = System.DateTime.UtcNow;

            cachingDemoContext = _cachingDemoContext;
            IDatabase cache = Connection.GetDatabase();
            ModelSummaryList = new List<ModelSummary>();

            foreach (Model ms in cachingDemoContext.Model)
            {
                if(!string.IsNullOrEmpty(cache.StringGet(MODELKEYNAME + ms.Id)))
                {
                    ModelSummaryList.Add(JsonConvert.DeserializeObject<ModelSummary>(cache.StringGet(MODELKEYNAME + ms.Id)));
                }
            }

            TimeToGetModels = System.DateTime.UtcNow - beforeCreateModels;
        }
    }
}
