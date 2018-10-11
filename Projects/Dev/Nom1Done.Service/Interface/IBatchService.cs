using Nom.ViewModel;
using System;

namespace Nom1Done.Service.Interface
{
    public interface IBatchService
    {
        BatchDTO GetBatch(Guid transactionId);
        bool ValidateNomination(Guid transactioId, string pipelineDuns);
    }
}
