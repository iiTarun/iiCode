using Nom1Done.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service.Interface
{
    public interface ITransactionalReportingService
    {
        List<TransactionalReportDTO> GetAllTransactionalReport(string PipelineDuns);

        bool AddReport(TransactionalReportDTO Item);
        bool DeleteAll();
    }
}
