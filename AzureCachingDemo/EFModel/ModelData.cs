using System;
using System.Collections.Generic;

namespace AzureCachingDemo.EFModel
{
    public partial class ModelData
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public int? PreviousId { get; set; }
        public decimal Amount { get; set; }

        public Model Model { get; set; }
    }
}
