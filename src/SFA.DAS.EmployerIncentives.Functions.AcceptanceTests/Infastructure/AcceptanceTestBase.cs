using NServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Infastructure
{
    public abstract class AcceptanceTestBase
    {
        private IEndpointInstance _endpointInstance;

        protected async Task Start()
        {
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.EmployerIncentives.Functions.TestMessageBus");
            var storageDirectory = Path.Combine(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().IndexOf("bin")), ".learningtransport");

            endpointConfiguration
                .UseNewtonsoftJsonSerializer()
                .UseTransport<LearningTransport>()
                .StorageDirectory(storageDirectory);

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningEventsAs(type => type.Namespace == "SFA.DAS.EmployerIncentives.Messages.Events");
            // TODO: add any external message namespaces here


            _endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

        }

        protected async Task Stop()
        {
            await _endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        protected async Task Publish(object message)
        {
            await _endpointInstance.Publish(message);
        }
    }
}
