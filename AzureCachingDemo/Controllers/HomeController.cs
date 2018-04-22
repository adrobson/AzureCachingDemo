extern alias signed;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AzureCachingDemo.Models;
using AzureCachingDemo.EFModel;
using Microsoft.EntityFrameworkCore;
using AzureCachingDemo.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;

namespace AzureCachingDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly CachingDemoContext cachingDemoContext;
        private readonly IDistributedCache distributedCache;

        private ViewDataModel viewDataModel;

        public HomeController(CachingDemoContext _cachingDemoContext, IDistributedCache _distributedCache)
        {
            cachingDemoContext = _cachingDemoContext;
            distributedCache = _distributedCache;
        }

        public IActionResult Index()
        {
            if(cachingDemoContext.Model.Count() > 0)
            {
                viewDataModel = new ViewDataModel(cachingDemoContext.Model.Count(), cachingDemoContext.ModelData.Count() / cachingDemoContext.Model.Count() );
                GetModelsFromCache();
            }
            else
            {
                viewDataModel = new ViewDataModel(0,0);
            }
            return View(viewDataModel);
        }
        
        public IActionResult ProcessModels([Bind(Prefix = "ModelInput")]ModelInput modelInput)
        {
            var numModels = modelInput.NumModels;
            var numVariables = modelInput.NumVariables;
            viewDataModel = new ViewDataModel(numModels, numVariables);
            if (cachingDemoContext.Model.Count() != numModels)
            {
                ProcessModelData(numModels, numVariables);
            }
            else
            {
                if (cachingDemoContext.ModelData.Count() / cachingDemoContext.Model.Count() != numVariables)
                {
                    ProcessModelData(numModels, numVariables);
                }
            }

            GetModelsFromCache();

            return View("Index", viewDataModel);
        }

        private void GetModelsFromCache()
        {
            GetModelSummaries getModelSummaries = new GetModelSummaries(cachingDemoContext);
            viewDataModel.ModelSummaryResults = getModelSummaries.ModelSummaryList;
            viewDataModel.TimeToGetModels = getModelSummaries.TimeToGetModels;
        }


        private void ProcessModelData(int numModels, int numVariables)
        {
            CreateDataIfNotThere createData = new CreateDataIfNotThere(cachingDemoContext, numModels, numVariables);
            viewDataModel.TimeToCreateData = createData.TimeToCreateData;

            var createModelSummaries = new CreateModelSummaries(cachingDemoContext);
            viewDataModel.TimeToCreateModels = createModelSummaries.TimeToCreateModels;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
