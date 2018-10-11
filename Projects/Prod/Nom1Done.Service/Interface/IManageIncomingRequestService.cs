using System.Web;

namespace Nom1Done.Service.Interface
{
    public interface IManageIncomingRequestService
    {
        string ProcessRequest(HttpRequestBase request, bool isTest, bool separateFiles);
    }
}
