using Nom1Done.DTO;

namespace Nom1Done.Service.Interface
{
    public interface IPipelineEDISettingService
    {
        PipelineEDISettingDTO GetPipelineSetting(int DatasetId, string pipeDuns,string shipperDuns);
        bool UpdatePipelineEDISetting(PipelineEDISettingDTO customUPRDReqDTO);
    }
}
