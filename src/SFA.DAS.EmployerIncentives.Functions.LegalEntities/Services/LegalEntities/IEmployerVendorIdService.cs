﻿using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public interface IEmployerVendorIdService
    {
        Task AddEmployerVendorId(string hashedLegalEntityId);
    }
}