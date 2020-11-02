using NServiceBus;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Commands;
using SFA.DAS.EmployerIncentives.Infrastructure;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public static class RoutingSettingsExtensions
    {
        public static void AddRouting(this RoutingSettings routingSettings)
        {
            routingSettings.RouteToEndpoint(typeof(AddEmployerVendorIdCommand), QueueNames.AddEmployerVendorId);
        }
    }
}
