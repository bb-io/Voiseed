using Apps.Voiseed.Actions;
using Tests.Voiseed.Base;

namespace Tests.Voiseed
{
    [TestClass]
    public class ProjectActionsTests : TestBase
    {
        [TestMethod]
        public async Task CreateProject_IsSuccess()
        {
            var action = new ProjectActions(InvocationContext);

            var response = await action.CreateProject(new()
            {
                Name = "Test Project local call 511",    
                Model = "xpressive",
                TargetLanguages = ["fr-fr"],
                SourceLanguage = "en-us"
            });

        }

        [TestMethod]
        public async Task GetProject_IsSuccess()
        {
            var action = new ProjectActions(InvocationContext);

            var response = await action.GetProject("2990163e-3c84-4cc8-a743-24ebbaf6afa7");

        }

        [TestMethod]
        public async Task SearchProjects_IsSuccess()
        {
            var action = new ProjectActions(InvocationContext);

            var response = await action.SearchProjects(new()
            {
            });

        }
    }
}
