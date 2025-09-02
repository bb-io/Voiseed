using Blackbird.Applications.Sdk.Common;

namespace Apps.Voiseed.Models.Project
{
    public class ProjectsListResponse
    {
        [Display("Projects")]
        public List<ProjectDto> Projects { get; set; }
    }
}
