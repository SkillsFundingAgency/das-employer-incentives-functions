using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types
{
    public class Account
    {
        public long Id { get; set; }
        public IEnumerable<LegalEntity> LegalEntities { get; set; }

    }
}
