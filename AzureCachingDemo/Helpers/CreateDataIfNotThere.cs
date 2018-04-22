using AzureCachingDemo.EFModel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCachingDemo.Helpers
{
    public class CreateDataIfNotThere
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

        public TimeSpan TimeToCreateData;

        public CreateDataIfNotThere(CachingDemoContext _cachingDemoContext, int modelCount, int modelItemCount)
        {
            cachingDemoContext = _cachingDemoContext;
            TimeToCreateData = new TimeSpan();

            DateTime beforeCreateData = System.DateTime.UtcNow;

            CheckModelData(modelCount);

            CheckModelItemData(modelItemCount);

            TimeToCreateData = System.DateTime.UtcNow - beforeCreateData;
        }

        private void CheckModelData(int modelCount)
        {
            if (cachingDemoContext.Model.ToList().Count() != modelCount)
            {
                ClearModelData();

                for (int i = 0; i < modelCount; i++)
                {
                    Model m = new Model() { Name = "" };
                    cachingDemoContext.Model.Add(m);
                    cachingDemoContext.SaveChanges();
                    cachingDemoContext.Model.Where(x => x.Id == m.Id).First().Name = m.Id.ToString();
                    cachingDemoContext.SaveChanges();
                }
            }
        }

        private void ClearModelData()
        {
            foreach (Model m in cachingDemoContext.Model.ToList())
            {
                RemoveItemFromCache(m.Id);

                ClearModelItemData(m.Id);

                cachingDemoContext.Model.Remove(m);
                cachingDemoContext.SaveChanges();
            }
        }

        private void ClearModelItemData(int modelId)
        {
            cachingDemoContext.ModelData.RemoveRange(cachingDemoContext.ModelData.Where(md => md.ModelId == modelId));
            cachingDemoContext.SaveChanges();
        }

        private void CheckModelItemData(int obsCount)
        { 
            foreach (Model m in cachingDemoContext.Model.ToList())
            {
                if (cachingDemoContext.ModelData.Where(md => md.ModelId == m.Id).Count() != obsCount)
                {
                    //the model data is going to change so we need to clear the cache for that model
                    RemoveItemFromCache(m.Id);
                    ClearModelItemData(m.Id);

                    int previousId = 0;
                    for (int j = 0; j < obsCount; j++)
                    {
                        var newData = new ModelData() { ModelId = m.Id };
                        var rnd = new Random();
                        newData.Amount = rnd.Next();
                        if (j > 0)
                        {
                            newData.PreviousId = previousId;
                        }

                        cachingDemoContext.ModelData.Add(newData);
                        cachingDemoContext.SaveChanges();
                        previousId = newData.Id;
                    }
                }
            }
        }

        private void RemoveItemFromCache(int id)
        {
            IDatabase cache = Connection.GetDatabase();
            cache.KeyDelete(MODELKEYNAME + id);
        }
    }
}
