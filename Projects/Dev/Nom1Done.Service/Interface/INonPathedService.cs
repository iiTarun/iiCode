using Nom1Done.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service.Interface
{
    public interface INonPathedService
    {
        Guid SaveNonPathedNomination(NonPathedDTO model);
        bool UpdateNonPathedNomination(NonPathedDTO model);
        NonPathedDTO GetNonPathedNominationOnTransactionId(Guid TransactionId);

        Guid? SaveAllNonPathedNominations(NonPathedDTO NonpathedNom);
        NonPathedDTO GetNonPathedNominations(string pipelineDuns, int Status, DateTime StartDate, DateTime EndDate, string shipperDuns, Guid loginUser);
    }
}
