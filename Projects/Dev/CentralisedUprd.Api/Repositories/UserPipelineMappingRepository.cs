using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.UPRD.DTO;
using Nom1Done.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api.Repositories
{
    public class UserPipelineMappingRepository
    {
        readonly UprdDbEntities1 dbcontext = new UprdDbEntities1();
        PipelineRepository repo = new PipelineRepository();
        ModalFactory factory = new ModalFactory();
        #region Get All Permissions By User
        public List<UserPipelineMappingDTO> GetAllPermissionsByUser(string userID)
        {
            var Permissions = (from p in dbcontext.Pipelines
                               join u in dbcontext.UserPipelineMappings.Where(a => a.userId == userID)
                               on p.DUNSNo equals u.PipeDuns into left
                               from r in left.DefaultIfEmpty()
                               join sc in dbcontext.ShipperCompanies on r.shipperId equals sc.ID into PipeAndMappingAllleft
                               from s in PipeAndMappingAllleft.DefaultIfEmpty()
                               select new UserPipelineMappingDTO
                               {
                                   UserId = r.userId,
                                   PipeDuns = r.PipeDuns ?? p.DUNSNo,
                                   IsNoms = r.IsNoms,
                                   IsUPRD = r.IsUPRD,
                                   CreatedBy = r.createdBy,
                                   CreatedDate = r.createdDate,
                                   ModifiedBy = r.modifiedBy,
                                   ModifiedDate = r.modifiedDate,
                                   PipeName = p.Name,
                                   ShipperId = r.shipperId,
                                   ShipperName = s.Name
                               }).Distinct().ToList();
            if (Permissions != null)
                return Permissions;
            else
                return null;
        }
        #endregion

        #region Save/Update UserPermission
        public bool SaveUserPermissions(UserPipelineDTO userMapping)
        {
            string userID = (userMapping.userPipelineMappingDTO.Count > 0 && !string.IsNullOrEmpty(userMapping.userPipelineMappingDTO.Select(u=>u.UserId).FirstOrDefault()) ? userMapping.userPipelineMappingDTO[0].UserId : userMapping.UserId);


            var userPipelineMapping = factory.Parse(userMapping);
            if (userMapping!=null)
            {
                var deleteUser = dbcontext.UserPipelineMappings.Where(a => a.userId == userID).Select(a => a).ToList();
                var Delete=dbcontext.UserPipelineMappings.RemoveRange(deleteUser);
                dbcontext.SaveChanges();
                var OnlyNomsOrUprd = userPipelineMapping
                    .Where(a => a.IsNoms == true || a.IsUPRD == true)
                    .Select(a => a).ToList();
                var result = dbcontext.UserPipelineMappings.AddRange(OnlyNomsOrUprd);
                var Success=dbcontext.SaveChanges();
                if (Success > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }
        #endregion

        #region Has Pipelines
        public bool HasPipeline(string UserID, int ShipperID)
        {
            int IsPipeAssigned = dbcontext.UserPipelineMappings
                .Where(a => a.userId == UserID && a.shipperId == ShipperID)
                .Select(a=>a)
                .Count();
            if (IsPipeAssigned > 0)
                return true;
            else
                return false;
        }
        #endregion
    }
}