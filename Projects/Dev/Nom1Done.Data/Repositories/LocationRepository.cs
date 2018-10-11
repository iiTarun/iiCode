using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System.Linq;
using Nom.ViewModel;
using System.Collections.Generic;
using System;

namespace Nom1Done.Data.Repositories
{
    public class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public List<LocationsDTO> GetLocations(string Keyword, string PipelineDuns)
        {
            List<LocationsDTO> resultantList=new List<LocationsDTO>();
            List<Location> data = new List<Location>();
            if (string.IsNullOrEmpty(Keyword)) {
                data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true).ToList();
            } else {
                data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
                && a.IsActive == true
                && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).ToList();
            }

            foreach (var item in data) {
                var dtoObj = new LocationsDTO
                {
                    CreatedBy = item.CreatedBy,
                    ID = item.ID,
                    CreatedDate = item.CreatedDate,
                    Identifier = item.Identifier,
                    IsActive = item.IsActive,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    Name = item.Name,
                    PipelineID = item.PipelineID,
                    PropCode = item.PropCode,
                    RDUsageID = item.RDUsageID,
                };
                if (dtoObj.RDUsageID == 1)
                    dtoObj.RDB = "R";
                else if (dtoObj.RDUsageID == 2)
                    dtoObj.RDB = "D";
                else
                    dtoObj.RDB = "B";
                resultantList.Add(dtoObj);
            }
            return resultantList.OrderBy(a=>a.Name).ToList();
        }

        // PageNo  = 0,1,2,3...so on.
        public List<LocationsDTO> GetLocationsWithPaging(string Keyword, string PipelineDuns, int PageNo, int PageSize, string PopupFor, bool IsSpecialDelCase,string order,string orderDir)
        {
            List<LocationsDTO> resultantList = new List<LocationsDTO>();
            List<Location> Result = new List<Location>();
            int NotRDB = 1; // for not Receipt locs.
            if (PopupFor == "RecLoc" || PopupFor == "Receipt" || PopupFor == "ContractPath" || PopupFor == "Supply")
            {
                NotRDB = 2;  // for not delivery locs. 
                if (string.IsNullOrEmpty(Keyword))
                {
                   var QueryableData = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB);
                   var QueryableDataWithOrder = GetQueryableDataWithOrder(QueryableData, order, orderDir);
                   Result=QueryableDataWithOrder.Skip(PageNo * PageSize).Take(PageSize).ToList();

                }
                else
                {
                    var QueryableDataWithKeyword = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
                    && a.IsActive == true
                    && a.RDUsageID != NotRDB
                    && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword)));
                    var QueryableDataWithOrder = GetQueryableDataWithOrder(QueryableDataWithKeyword, order, orderDir);
                    Result = QueryableDataWithOrder.Skip(PageNo * PageSize).Take(PageSize).ToList();
                }
            }
            else {
                NotRDB = 1;  // not-Receipt 
                if (IsSpecialDelCase)
                {

                    if (PipelineDuns == "006958581")  // ANR Pipeline Company
                    {
                        // locationList = locationList.Where(a => a.PropCode == "103565" || a.PropCode == "103702").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            Result = DbContext.Location.Where(a => a.PropCode == "103565" || a.PropCode == "103702").OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Location.Where(a => (a.PropCode == "103565" || a.PropCode == "103702")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
                        }
                    }
                    else if (PipelineDuns == "007933047") //Transwestern Pipeline Company, LLC
                    {
                        // locationList = locationList.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            Result = DbContext.Location.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Location.Where(a => (a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
                        }
                    }
                    else if (PipelineDuns == "829416002")  // FAYETTEVILLE EXPRESS PIPELINE LLC
                    {
                        //locationList = locationList.Where(a => a.PropCode == "58744").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            Result = DbContext.Location.Where(a => a.PropCode == "58744").OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Location.Where(a => (a.PropCode == "58744")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            Result = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB).OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
                            && a.IsActive == true
                            && a.RDUsageID != NotRDB
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
                        }
                    }
                }
                else//If location is Delivery
                {
                    if (string.IsNullOrEmpty(Keyword))
                    {
                        var QueriableDelLocData = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB);
                        var QueryableDataWithOrderDelLoc = GetQueryableDataWithOrder(QueriableDelLocData, order, orderDir);
                        Result = QueryableDataWithOrderDelLoc.Skip(PageNo * PageSize).Take(PageSize).ToList();
                    }
                    else
                    {
                        var QueriableDelLocDataWithData = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
                        && a.IsActive == true
                        && a.RDUsageID != NotRDB
                        && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword)));
                        var QueryableDataWithOrderDelLoc = GetQueryableDataWithOrder(QueriableDelLocDataWithData, order, orderDir);
                        Result = QueryableDataWithOrderDelLoc.Skip(PageNo * PageSize).Take(PageSize).ToList();
                    }
                }
            }            
           

            foreach (var item in Result)
            {
                var dtoObj = new LocationsDTO
                {
                    CreatedBy = item.CreatedBy,
                    ID = item.ID,
                    CreatedDate = item.CreatedDate,
                    Identifier = item.Identifier,
                    IsActive = item.IsActive,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    Name = item.Name,
                    PipelineID = item.PipelineID,
                    PropCode = item.PropCode,
                    RDUsageID = item.RDUsageID,
                };
                if (dtoObj.RDUsageID == 1)
                    dtoObj.RDB = "R";
                else if (dtoObj.RDUsageID == 2)
                    dtoObj.RDB = "D";
                else
                    dtoObj.RDB = "B";
                resultantList.Add(dtoObj);
            }
            return resultantList;
        }

        private IQueryable<Location> GetQueryableDataWithOrder(IQueryable<Location> queryableData, string order, string orderDir)
        {
            switch(order)
            {
                case "1":
                    queryableData = orderDir.Equals("desc") ? queryableData.OrderByDescending(a => a.Name) : queryableData.OrderBy(a => a.Name);
                    break;
                case "2":
                    queryableData = orderDir.Equals("desc") ? queryableData.OrderByDescending(a => a.Identifier) : queryableData.OrderBy(a => a.Identifier);
                    break;
                case "3":
                    queryableData = orderDir.Equals("desc") ? queryableData.OrderByDescending(a => a.PropCode) : queryableData.OrderBy(a => a.PropCode);
                    break;
                case "4":
                    queryableData = orderDir.Equals("desc") ? queryableData.OrderByDescending(a => a.RDUsageID) : queryableData.OrderBy(a => a.RDUsageID);
                    break;
            }
            return queryableData;
        }

        public int GetTotalLocationCount(string Keyword, string PipelineDuns, string PopupFor, bool IsSpecialDelCase)
        {
           
            int data = 0;
            int NotRDB = 1; // for not Receipt locs.
            if (PopupFor == "RecLoc" || PopupFor == "Receipt" || PopupFor == "ContractPath" || PopupFor == "Supply")
            {
                NotRDB = 2;  // for not delivery locs.
                if (string.IsNullOrEmpty(Keyword))
                {
                    data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB).Count();
                }
                else
                {
                    data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
                    && a.IsActive == true
                    && a.RDUsageID != NotRDB
                    && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                }
            }
            else {
                NotRDB = 1; // for not Receipt locs.
                if (IsSpecialDelCase) {
                    if (PipelineDuns == "006958581")  // ANR Pipeline Company
                    {
                        // locationList = locationList.Where(a => a.PropCode == "103565" || a.PropCode == "103702").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            data = DbContext.Location.Where(a => a.PropCode == "103565" || a.PropCode == "103702").Count();
                         }
                        else
                        {
                            data = DbContext.Location.Where(a => (a.PropCode == "103565" || a.PropCode == "103702")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                        }
                    }
                    else if (PipelineDuns == "007933047") //Transwestern Pipeline Company, LLC
                    {
                        // locationList = locationList.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            data = DbContext.Location.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").Count();
                        }
                        else
                        {
                            data = DbContext.Location.Where(a => (a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                        }
                    }
                    else if (PipelineDuns == "829416002")  // FAYETTEVILLE EXPRESS PIPELINE LLC
                    {
                        //locationList = locationList.Where(a => a.PropCode == "58744").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            data = DbContext.Location.Where(a => a.PropCode == "58744").Count();
                        }
                        else
                        {
                            data = DbContext.Location.Where(a => (a.PropCode == "58744")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB).Count();
                        }
                        else
                        {
                            data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
                            && a.IsActive == true
                            && a.RDUsageID != NotRDB
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                        }
                    }


                } else {
                    if (string.IsNullOrEmpty(Keyword))
                    {
                        data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB).Count();
                    }
                    else
                    {
                        data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
                        && a.IsActive == true
                        && a.RDUsageID != NotRDB
                        && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                    }
                }

            }
           
            return data;
        }
    }

    public interface ILocationRepository : IRepository<Location>
    {
        List<LocationsDTO> GetLocations(string Keyword, string PipelineDuns);
        List<LocationsDTO> GetLocationsWithPaging(string Keyword, string PipelineDuns, int PageNo, int PageSize, string PopupFor, bool IsSpecialDelCase,string order,string orderDir);
        int GetTotalLocationCount(string Keyword, string PipelineDuns,string PopupFor, bool IsSpecialDelCase);



    }
}
