using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCachingDemo.Models
{
    public class ViewDataModel
    {
        public ModelInput ModelInput { get; set; }
        public List<ModelSummary> ModelSummaryResults { get; set; }

        public TimeSpan TimeToCreateData;
        public TimeSpan TimeToCreateModels;
        public TimeSpan TimeToGetModels;

        public ViewDataModel(int numModels, int numVariables)
        {
            ModelInput = new ModelInput() { NumModels = numModels, NumVariables = numVariables };
            ModelSummaryResults = new List<ModelSummary>();
        }
    }
}
