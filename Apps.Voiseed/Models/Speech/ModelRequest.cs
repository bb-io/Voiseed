using Apps.Voiseed.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Voiseed.Models.Speech
{
    public class ModelRequest
    {

        [Display("Model")]
        [DataSource(typeof(ModelsDataHandler))]
        public string Model { get; set; }
    }
}
