using Apps.Voiseed.Api;
using Apps.Voiseed.Models.AudioProduction;
using Apps.Voiseed.Models.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Voiseed.Actions;

[ActionList("Project")]
public class ProjectActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Search projects", Description = "Searches projects")]
    public async Task<ProjectsListResponse> SearchProjects([ActionParameter] SearchProjectsRequest input)
    {
        var request = new RestRequest("/projects", Method.Get);
        if (!string.IsNullOrWhiteSpace(input.ProjectType))
        {
            request.AddQueryParameter("type", input.ProjectType);
        }

        var response = await Client.ExecuteWithErrorHandling<List<ProjectDto>>(request);

        return new ProjectsListResponse { Projects = response };
    }

    [Action("Get project", Description = "Gets project by ID")]
    public async Task<ProjectDto> GetProject([ActionParameter][Display("Project ID")] string projectId)
    {
        var endpoint = $"/projects/{projectId}";

        var request = new RestRequest(endpoint, Method.Get);

        return await Client.ExecuteWithErrorHandling<ProjectDto>(request);
    }

    [Action("Create project", Description = "Creates project")]
    public async Task<ProjectDto> CreateProject([ActionParameter] CreateAudioProductionProjectRequest input)
    {
        var client = new VoiseedClient(invocationContext.AuthenticationCredentialsProviders);

        var body = new Dictionary<string, object>
        {
            ["name"] = input.Name,
            ["targetLanguages"] = input.TargetLanguages,
            ["type"] = "audio-production",
            ["model"] = input.Model
        };

        if (!string.IsNullOrWhiteSpace(input.SourceLanguage))
        {
            body["sourceLanguage"] = input.SourceLanguage;
        }

        var req = new RestRequest("/projects", Method.Post).AddJsonBody(body);
        return await client.ExecuteWithErrorHandling<ProjectDto>(req);
    }
}
