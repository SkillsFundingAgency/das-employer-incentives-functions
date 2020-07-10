﻿using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using SFA.DAS.EmployerIncentives.Infrastructure.DistributedLock;

namespace SFA.DAS.EmployerIncentives.Functions.Commands.AddLegalEntity
{
    public class AddLegalEntityCommand : ICommand, ILockIdentifier
    {
        public long AccountId { get; private set; }
        public long LegalEntityId { get; private set; }
        public string Name { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string LockId { get => $"{GetType().Name}_{AccountId}"; }

        public AddLegalEntityCommand(
            long accountId,
            long legalEntityId,
            string name,
            long accountLegalEntityId)
        {
            AccountId = accountId;
            LegalEntityId = legalEntityId;
            Name = name;
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}
