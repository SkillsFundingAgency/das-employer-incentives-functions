using SFA.DAS.EmployerIncentives.Infrastructure.Commands;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.Commands.AddLegalEntity
{
    // TODO: implement version that calls outer API

    public class AddLegalEntityCommandHandler //: ICommandHandler<AddLegalEntityCommand>
    {
        //private readonly IAccountDomainRepository _domainRepository;

        //public AddLegalEntityCommandHandler(IAccountDomainRepository domainRepository)
        //{
        //    _domainRepository = domainRepository;
        //}

        //public async Task Handle(AddLegalEntityCommand command)
        //{
        //    var account = await _domainRepository.Find(command.AccountId);
        //    if (account != null)
        //    {
        //        if (account.ContainsAccountLegalEntityId(command.AccountLegalEntityId))
        //        {
        //            return; // already created
        //        }
        //    }
        //    else
        //    {
        //        account = Account.New(command.AccountId);
        //    }

        //    account.AddLegalEntity(command.AccountLegalEntityId, LegalEntity.New(command.LegalEntityId, command.Name));

        //    await _domainRepository.Save(account);
        //}
    }
}
