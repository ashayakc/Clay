using Lock.Integration.Test.Drivers;
using Lock.Integration.Test.Model;
using Newtonsoft.Json;
using System.Net;

namespace Lock.Integration.Test.StepDefinitions
{
    [Binding]
    public class LockStepDefinitions
    {
        private readonly LockDriver _lockDriver;
        private string token;
        private string doorOpenResponse;
        private HttpResponseMessage httpResponseMessage;
        public LockStepDefinitions()
        {
            _lockDriver = new LockDriver();
            token = String.Empty;
            doorOpenResponse = String.Empty;
            httpResponseMessage = new HttpResponseMessage();
        }

        [Given(@"we have a valid user")]
        public void GivenWeHaveAValidUser()
        {
            //Seeded
        }

        [Given(@"a door created for an office")]
        public void GivenADoorCreatedForAnOffice()
        {
            //Seeded
        }

        [Given(@"the user is authorized to access the door")]
        public async Task GivenTheUserIsAuthorizedToAccessTheDoor()
        {
            httpResponseMessage = await _lockDriver.LoginAsync("sheldon", "sheldon");
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            token = await httpResponseMessage.Content.ReadAsStringAsync();
        }

        [When(@"the user tries to open the door")]
        public async Task WhenTheUserTriesToOpenTheDoor()
        {
            httpResponseMessage = await _lockDriver.OpenDoorAsync("Opening door for ann", token);
            doorOpenResponse = await httpResponseMessage.Content.ReadAsStringAsync();
        }

        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(int p0)
        {
            Assert.Equal(p0, (int)httpResponseMessage.StatusCode);
        }

        [Then(@"the door should be opened successfully")]
        public void ThenTheDoorShouldBeOpenedSuccessfully()
        {
            Assert.Equal("Door opened successfully", doorOpenResponse);
        }

        [Given(@"the user is not authorized to access the door")]
        public async Task GivenTheUserIsNotAuthorizedToAccessTheDoor()
        {
            token = string.Empty;
        }
    }
}
