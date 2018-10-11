using Nom.ViewModel;
using Nom1Done.DTO;
using Nom1Done.Model;
using Nom1Done.Model.Models;
using Nom1Done.Nom.ViewModel;
using System.Collections.Generic;

namespace Nom1Done.Service.Interface
{
    public interface IModalFactory
    {
        DTO.Route Parse(Model.Route model);
        
       
        TransactionalReportDTO Parse(TransactionalReport model);
        TransactionalReport Create(TransactionalReportDTO model);
        LocationsDTO Parse(Location model);
        Location Create(LocationsDTO dto);
        BatchDetailDTO Parse(V4_Batch batch, IEnumerable<V4_Nomination> nomList);
        ContractsDTO Parse(Contract item);
        CounterPartiesDTO Parse(CounterParty itemObj);
        TransactionTypesDTO Parse(metadataTransactionType tt);
        UploadFileDTO Parse(UploadedFile a);
        BatchDTO Parse(V4_Batch batch);
        SettingDTO Parse(Setting a);
        PipelineEDISettingDTO Parse(PipelineEDISetting a);
        List<V4_NominationDTO> Parse(List<V4_Nomination> a);
        V4_NominationDTO Parse(V4_Nomination a);
        PipelineEDISetting Create(PipelineEDISettingDTO a);
        List<NMQRPerTransaction> Create(List<NMQRPerTransactionDTO> nmqrList);       
        List<UPRDStatu> Create(List<UPRDStatusDTO> uprdStatusList);
        List<SQTSPerTransaction> Create(List<SQTSPerTransactionDTO> sqtsList);
        List<UPRDStatusDTO> Parse(List<UPRDStatu> uprdStatusList);
        PipelineDTO Parse(Pipeline pipe);
        V4_Nomination Create(NonPathedRecieptNom rec);
        Contract Create(ContractsDTO conDTO);
        List<SQTSOPPerTransaction> Create(List<SQTSOPPerTransactionDTO> list);
    }
}
