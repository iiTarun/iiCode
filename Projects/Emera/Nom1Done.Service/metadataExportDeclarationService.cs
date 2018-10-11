using Nom1Done.Data.Repositories;
using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.DTO;

namespace Nom1Done.Service
{
    public class metadataExportDeclarationService: ImetadataExportDeclarationService
    {
        private readonly ImetadataExportDeclarationRepository metadataExportDeclarationRepository;
        public metadataExportDeclarationService(ImetadataExportDeclarationRepository metadataExportDeclarationRepository)
        {
            this.metadataExportDeclarationRepository = metadataExportDeclarationRepository;
        }

        public List<ExportDeclarationDTO> GetExportDeclarations()
        {
            List<ExportDeclarationDTO> model = new List<ExportDeclarationDTO>();
            model = metadataExportDeclarationRepository.GetAll().Where(a => a.IsActive == true).Select(a => new ExportDeclarationDTO
            {
                Code = a.Code,
                Name = a.Name
            }).ToList();
            return model;
        }
    }
}
