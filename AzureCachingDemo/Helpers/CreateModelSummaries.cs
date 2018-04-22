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
    public class CreateModelSummaries
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

        public TimeSpan TimeToCreateModels { get; set; }

        public CreateModelSummaries(CachingDemoContext _cachingDemoContext)
        {
            DateTime startCreateModels = System.DateTime.UtcNow;
            cachingDemoContext = _cachingDemoContext;
            IDatabase cache = Connection.GetDatabase();

            List<Model> modelList = cachingDemoContext.Model.ToList();
            foreach(Model m in modelList)
            {
                CreateModelSummary(m, cache);
            }

            TimeToCreateModels = System.DateTime.UtcNow - startCreateModels;
        }

        public void CreateModelSummary(Model m, IDatabase cache)
        {
            ModelSummary modelSummary = new ModelSummary() { ModelId = m.Id, ModelName = m.Name };
            decimal modelSummaryAmount = 0;

            foreach (ModelData d in cachingDemoContext.ModelData.Where(x => x.ModelId == m.Id))
            {
                var thisAmount = d.Amount;
                decimal plusAmount = 0;
                if (d.PreviousId != null)
                {
                    plusAmount = cachingDemoContext.ModelData.Where(x => x.Id == d.PreviousId).First().Amount;
                }

                modelSummaryAmount += thisAmount + plusAmount;
            }

            modelSummary.ModelValue = modelSummaryAmount;
            cache.StringSet(MODELKEYNAME + m.Id, JsonConvert.SerializeObject(modelSummary));
        }
    }
}
