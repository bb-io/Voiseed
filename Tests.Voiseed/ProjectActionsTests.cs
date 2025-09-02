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
                Name = "Test Project local call",    
                Model = "xpressive",
                TargetLanguages = ["fr-fr"]
            });

        }
    }
}
