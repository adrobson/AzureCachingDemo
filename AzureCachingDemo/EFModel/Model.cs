using System;
using System.Collections.Generic;

namespace AzureCachingDemo.EFModel
{
    public partial class Model
    {
        public Model()
        {
            ModelData = new HashSet<ModelData>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ModelData> ModelData { get; set; }
    }
}
