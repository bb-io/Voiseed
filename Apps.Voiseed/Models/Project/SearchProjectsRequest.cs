using Apps.Voiseed.Handlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Voiseed.Models.Project
{
    public class SearchProjectsRequest
    {
        [StaticDataSource(typeof(ProjectTypeHandler))]
        [Display("Project type")]
        public string? ProjectType { get; set; }
    }
}
