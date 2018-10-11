using Nom1Done.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nom1Done.DTO;
using Nom1Done.Data.Repositories;

namespace Nom1Done.Service
{
    public class metadataFileStatusService : ImetadataFileStatusService
    {
        ImetadataFileStatusRepositpry ImetadataFileStatusRepositpry;

        public metadataFileStatusService(ImetadataFileStatusRepositpry ImetadataFileStatusRepositpry) {
            this.ImetadataFileStatusRepositpry = ImetadataFileStatusRepositpry;
        }
        public List<NomStatusDTO> GetNomStatus()
        {
            List<NomStatusDTO> model = new List<NomStatusDTO>();
            //model = ImetadataFileStatusRepositpry.GetAll().Select(a => new NomStatusDTO
            //{
            //    ID = a.ID,
            //    Name = a.Name
            //}).ToList();
            model = GetMainNomStatus().OrderBy(a=>a.Name).ToList();
            return model;
        }

        public List<NomStatusDTO> GetMainNomStatus()
        {
            List<NomStatusDTO> model = new List<NomStatusDTO>() {
                new NomStatusDTO() { ID=1,Name="In-Process"},
                new NomStatusDTO() { ID=5, Name="Submitted"},
                new NomStatusDTO() { ID=7, Name="Accepted"},
                new NomStatusDTO() { ID=10, Name="Rejected"},
                new NomStatusDTO() { ID=8, Name="Exception occured"},
                new NomStatusDTO() { ID=11, Name="Draft"},
                new NomStatusDTO() { ID=0, Name="GISB Unprocessed"}
                //new NomStatusDTO() { ID=12, Name="Replaced"},
            };           
            return model;
        }

    }
}
