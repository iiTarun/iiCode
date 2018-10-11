using Nom.ViewModel;
using UPRD.DTO;
using System.Collections.Generic;
using UPRD.Model;
using Nom1Done.DTO;

namespace UPRD.Service.Interface
{
    public interface IModalFactory
    {

        UnscPerTransactionDTO Parse(UnscPerTransaction item);
        OACYPerTransactionDTO Parse(OACYPerTransaction item);
        //TransactionalReportDTO Parse(TransactionalReport model);
        //TransactionalReport Create(TransactionalReportDTO model);
        //LocationsDTO Parse(Location model);
        //Location Create(LocationsDTO dto);
        SwntPerTransactionDTO Parse(SwntPerTransaction model);
        //BatchDetailDTO Parse(V4_Batch batch, IEnumerable<V4_Nomination> nomList);
        //ContractsDTO Parse(Contract item);
        //CounterPartiesDTO Parse(CounterParty itemObj);
        //TransactionTypesDTO Parse(metadataTransactionType tt);
        //UploadFileDTO Parse(UploadedFile a);
        //BatchDTO Parse(V4_Batch batch);
        SettingDTO Parse(Setting a);
        PipelineEDISettingDTO Parse(PipelineEDISetting a);
        //List<V4_NominationDTO> Parse(List<V4_Nomination> a);
        //V4_NominationDTO Parse(V4_Nomination a);
        PipelineEDISetting Create(PipelineEDISettingDTO a);
        //List<NMQRPerTransaction> Create(List<NMQRPerTransactionDTO> nmqrList);
        List<OACYPerTransaction> Create(List<EDIOacyWrapperDTO> oacyList);
        List<SwntPerTransaction> Create(List<SwntPerTransactionDTO> swntList);
        List<UnscPerTransaction> Create(List<EDIUnscWrapperDTO> unscList);
        List<UPRDStatus> Create(List<UPRDStatusDTO> uprdStatusList);
        //List<SQTSPerTransaction> Create(List<SQTSPerTransactionDTO> sqtsList);
        List<UPRDStatusDTO> Parse(List<UPRDStatus> uprdStatusList);

        //V4_Nomination Create(NonPathedRecieptNom rec);
        //Contract Create(ContractsDTO conDTO);

        UPRD.DTO.DashNominationStatusDTO Parse(DashNominationStatus item);
        UPRD.DTO.CounterPartyDTO Parse(CounterParty item);
         CounterParty Create(UPRD.DTO.CounterPartyDTO item);
        UPRD.DTO.PipelineDTO Parse(Pipeline item);
        Pipeline Create(UPRD.DTO.PipelineDTO item);


        UPRD.DTO.LocationsDTO Parse(Location item);
        Location Create(UPRD.DTO.LocationsDTO item);
        DTO.MetaDataTransactionTypesDTO Parse(metadataTransactionType Trans);
        metadataTransactionType Create(DTO.MetaDataTransactionTypesDTO dto);

        DTO.Pipeline_TransactionType_MapDTO Parse(Pipeline_TransactionType_Map Trans);
        Pipeline_TransactionType_Map Create(DTO.Pipeline_TransactionType_MapDTO dto);
        //List<UPRD.DTO.CounterPartiesDTO> Create(List<CounterParty> uprdCounterList);



    }
}
