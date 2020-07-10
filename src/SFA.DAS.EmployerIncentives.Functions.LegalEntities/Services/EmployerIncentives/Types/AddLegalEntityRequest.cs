namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmployerIncentives.Types
{
    public class AddLegalEntityRequest
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long LegalEntityId { get; set; }
        public string OrganisationName { get; set; }
    }
}
