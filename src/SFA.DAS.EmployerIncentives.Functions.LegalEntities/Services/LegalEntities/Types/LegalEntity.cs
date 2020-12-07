using System;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types
{
    public class LegalEntity
    {
        public long Id { get; set; }
        public string HashedLegalEntityId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string Name { get; set; }
        public bool HasSignedAgreementTerms { get; set; }
        public string VrfVendorId { get; set; }
        public string VrfCaseId { get; set; }
        public string VrfCaseStatus { get; set; }
        public DateTime? VrfCaseStatusLastUpdatedDateTime { get; set; }
    }
}
