namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types
{
    public class SignAgreementRequest
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public int AgreementVersion { get; set; }
    }
}
