using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.UPRD.DTO;
using CentralisedUprd.Api.Helpers;
using System;

namespace CentralisedUprd.Api.Repositories
{
    public class UprdLocationsRepository
    { 
        UprdDbEntities1 DbContext = new UprdDbEntities1();
        ModalFactory modalFactory = new ModalFactory();
        SortingPagingInfo sortingPagingInfo = new SortingPagingInfo();
        public IQueryable<Location> GetLocations(string Keyword, string PipelineDuns)
        {

            IQueryable<Location> data;
            if (string.IsNullOrEmpty(Keyword))
            {
                data = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true);
            }
            else
            {
                data = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns
                && a.IsActive == true
                && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword)));
            }

            return data;  //.Select(a => modalFactory.Parse(a)).OrderBy(a=>a.Name).ToList();
        }
        public LocationsResultDTO GetLocationByPipeline(string pipelineDuns)
        {
            LocationsResultDTO locResult = new LocationsResultDTO();
            var locationResult = DbContext.Locations.Where(a => a.PipeDuns == pipelineDuns).ToList();
            locResult.locationsDTO = locationResult.Select(a => modalFactory.Parse(a)).OrderBy(a => a.Name).ToList();
            locResult.RecordCount = locResult.locationsDTO.Count();
            return locResult;

        }

        // PageNo  = 0,1,2,3...so on.
        public LocationsResultDTO GetLocationsWithPaging(string Keyword, string PipelineDuns, string PopupFor, bool IsSpecialDelCase, SortingPagingInfo sortingPagingInfo)
        {
            LocationsResultDTO locationsResultDTO = new LocationsResultDTO();
            List<Location> Result = new List<Location>();
            int PageNo = sortingPagingInfo.CurrentPageIndex;
            int PageSize = sortingPagingInfo.PageSize;

            int NotRDB = 1; // for not Receipt locs.
            if (PopupFor == "RecLoc" || PopupFor == "Receipt" || PopupFor == "ContractPath" || PopupFor == "Supply")
            {
                NotRDB = 2;  // for not delivery locs. 
                if (string.IsNullOrEmpty(Keyword))
                {
                    var QueryableData = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB);
                    var QueryableDataWithOrder = GetQueryableDataWithOrder(QueryableData, sortingPagingInfo);
                    sortingPagingInfo.PageCount= QueryableDataWithOrder.Count();
                    Result = QueryableDataWithOrder.Skip((PageNo - 1) * PageSize).Take(PageSize).ToList();

                }
                else
                {
                    var QueryableDataWithKeyword = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns
                    && a.IsActive == true
                    && a.RDUsageID != NotRDB
                    && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword)));                  
                    var QueryableDataWithOrder = GetQueryableDataWithOrder(QueryableDataWithKeyword, sortingPagingInfo);
                    sortingPagingInfo.PageCount = QueryableDataWithOrder.Count();
                    Result = QueryableDataWithOrder.Skip((PageNo - 1) * PageSize).Take(PageSize).ToList();
                }
            }
            else
            {
                NotRDB = 1;  // not-Receipt 
                if (IsSpecialDelCase)
                {

                    if (PipelineDuns == "006958581")  // ANR Pipeline Company
                    {
                        // locationList = locationList.Where(a => a.PropCode == "103565" || a.PropCode == "103702").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            Result = DbContext.Locations.Where(a => a.PropCode == "103565" || a.PropCode == "103702").OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Locations.Where(a => (a.PropCode == "103565" || a.PropCode == "103702")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                    }
                    else if (PipelineDuns == "007933047") //Transwestern Pipeline Company, LLC
                    {
                        // locationList = locationList.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            Result = DbContext.Locations.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").OrderBy(a => a.Name).Skip((PageNo-1) * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Locations.Where(a => (a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                    }
                    else if (PipelineDuns == "829416002")  // FAYETTEVILLE EXPRESS PIPELINE LLC
                    {
                        //locationList = locationList.Where(a => a.PropCode == "58744").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            Result = DbContext.Locations.Where(a => a.PropCode == "58744").OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Locations.Where(a => (a.PropCode == "58744")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            Result = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB).OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns
                            && a.IsActive == true
                            && a.RDUsageID != NotRDB
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                    }
                }
                else//If location is Delivery
                {
                    if (string.IsNullOrEmpty(Keyword))
                    {
                        var QueriableDelLocData = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB);
                        var QueryableDataWithOrderDelLoc = GetQueryableDataWithOrder(QueriableDelLocData, sortingPagingInfo);
                        sortingPagingInfo.PageCount = QueryableDataWithOrderDelLoc.Count();
                        Result = QueryableDataWithOrderDelLoc.Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                    }
                    else
                    {
                        var QueriableDelLocDataWithData = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns
                        && a.IsActive == true
                        && a.RDUsageID != NotRDB
                        && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword)));
                        var QueryableDataWithOrderDelLoc = GetQueryableDataWithOrder(QueriableDelLocDataWithData, sortingPagingInfo);
                        sortingPagingInfo.PageCount = QueryableDataWithOrderDelLoc.Count();
                        Result = QueryableDataWithOrderDelLoc.Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                    }
                }
            }
            locationsResultDTO.RecordCount = sortingPagingInfo.PageCount;
            locationsResultDTO.locationsDTO =  Result.Select(a => modalFactory.Parse(a)).OrderBy(a => a.Name).ToList();
       
            return locationsResultDTO;
        }

        private IQueryable<Location> GetQueryableDataWithOrder(IQueryable<Location> queryableData, SortingPagingInfo sortingPagingInfo)
        {

            if (sortingPagingInfo != null)
            {
                // Sorting
                string orderDir = sortingPagingInfo.SortDirection;
                switch (sortingPagingInfo.SortField)
                {
                    case "Name":
                        queryableData = orderDir.Equals("desc") ? queryableData.OrderByDescending(a => a.Name) : queryableData.OrderBy(a => a.Name);
                        break;
                    case "Identifier":
                        queryableData = orderDir.Equals("desc") ? queryableData.OrderByDescending(a => a.Identifier) : queryableData.OrderBy(a => a.Identifier);
                        break;
                    case "PropCode":
                        queryableData = orderDir.Equals("desc") ? queryableData.OrderByDescending(a => a.PropCode) : queryableData.OrderBy(a => a.PropCode);
                        break;
                    case "RDUsageID":
                        queryableData = orderDir.Equals("desc") ? queryableData.OrderByDescending(a => a.RDUsageID) : queryableData.OrderBy(a => a.RDUsageID);
                        break;
                    default:
                        queryableData = queryableData.OrderByDescending(p => p.CreatedDate);
                        break;

                }
            }
            else
            {
                return queryableData;
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
                    data = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB).Count();
                }
                else
                {
                    data = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns
                    && a.IsActive == true
                    && a.RDUsageID != NotRDB
                    && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                }
            }
            else
            {
                NotRDB = 1; // for not Receipt locs.
                if (IsSpecialDelCase)
                {
                    if (PipelineDuns == "006958581")  // ANR Pipeline Company
                    {
                        // locationList = locationList.Where(a => a.PropCode == "103565" || a.PropCode == "103702").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            data = DbContext.Locations.Where(a => a.PropCode == "103565" || a.PropCode == "103702").Count();
                        }
                        else
                        {
                            data = DbContext.Locations.Where(a => (a.PropCode == "103565" || a.PropCode == "103702")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                        }
                    }
                    else if (PipelineDuns == "007933047") //Transwestern Pipeline Company, LLC
                    {
                        // locationList = locationList.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            data = DbContext.Locations.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").Count();
                        }
                        else
                        {
                            data = DbContext.Locations.Where(a => (a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                        }
                    }
                    else if (PipelineDuns == "829416002")  // FAYETTEVILLE EXPRESS PIPELINE LLC
                    {
                        //locationList = locationList.Where(a => a.PropCode == "58744").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            data = DbContext.Locations.Where(a => a.PropCode == "58744").Count();
                        }
                        else
                        {
                            data = DbContext.Locations.Where(a => (a.PropCode == "58744")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            data = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB).Count();
                        }
                        else
                        {
                            data = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns
                            && a.IsActive == true
                            && a.RDUsageID != NotRDB
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                        }
                    }


                }
                else
                {
                    if (string.IsNullOrEmpty(Keyword))
                    {
                        data = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB).Count();
                    }
                    else
                    {
                        data = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns
                        && a.IsActive == true
                        && a.RDUsageID != NotRDB
                        && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).Count();
                    }
                }

            }

            return data;
        }

        public List<LocationsDTO> GetLocationList()
        {
            try
            {
                var data= DbContext.Locations.Where(a => a.IsActive).Distinct().ToList();
                List<LocationsDTO> list = new List<LocationsDTO>();
                foreach (var item in data)
                {
                    list.Add(modalFactory.Parse(item));
                }
                return list;
                //return DbContext.Locations.Where(a => a.IsActive).Distinct().Select(a => modalFactory.Parse(a)).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<LocationsDTO> GetLocationListByPipelineDuns(string pipelineDuns)
        {
            try
            {
                var data = DbContext.Locations.Where(a =>a.PipeDuns==pipelineDuns && a.IsActive).Distinct().ToList();
                List<LocationsDTO> list = new List<LocationsDTO>();
                foreach (var item in data)
                {
                    list.Add(modalFactory.Parse(item));
                }
                return list;
                //return DbContext.Locations.Where(a => a.IsActive).Distinct().Select(a => modalFactory.Parse(a)).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
            //return new List<LocationsDTO>
            //{
            //    new LocationsDTO
            //    {
            //        Identifier="000000001",
            //        Name="Name1"
            //    },
            //    new LocationsDTO
            //    {
            //        Identifier="000000002",
            //        Name="Name2"
            //    },
            //    new LocationsDTO
            //    {
            //        Identifier="000000003",
            //        Name="Name3"
            //    },
            //    new LocationsDTO
            //    {
            //        Identifier="000000004",
            //        Name="Name4"
            //    },
            //    new LocationsDTO
            //    {
            //        Identifier="000000005",
            //        Name="Name5"
            //    }
            //};
        }

        public List<LocationsDTO> GetLocationByIdentifier(string Identifier,string PipelineDuns)
        {
            List<Location> data = new List<Location>();
            if (string.IsNullOrEmpty(Identifier))
            {
                data = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true).ToList();
            }
            else
            {
                data = DbContext.Locations.Where(a => a.PipeDuns == PipelineDuns
                && a.IsActive == true
               && a.Identifier.Contains(Identifier)).ToList();
            }
            return data.Select(a => modalFactory.Parse(a)).OrderBy(a => a.Name).ToList();
        }

        public bool AddLocationByPipeline(List<LocationsDTO> locList)
        {
            foreach (var item in locList)
            {
                Location loc = new Location() {
                    CreatedDate = DateTime.Now,//item.CreatedDate,
                    Name = item.Name,
                    Identifier = item.Identifier,
                    PropCode = item.PropCode,
                    RDUsageID = item.RDUsageID,
                    PipelineID = item.PipelineID,
                    IsActive = true,
                    CreatedBy = item.CreatedBy,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = DateTime.Now,//item.ModifiedDate,
                    PipeDuns = item.PipelineDuns, // adding pipeduns is pending
                    TransactionTypeId = 0
               };
                DbContext.Locations.Add(loc);
            }
         
       DbContext.SaveChanges();

            return true;
        }

        public LocationsDTO GetLocationById(int id)
        {
            LocationsDTO result = new LocationsDTO();

            Location data = new Location();
            if (id != 0)
            {
                data = DbContext.Locations.Where(a => a.ID == id).FirstOrDefault();
            }
                result.CreatedBy = data.CreatedBy;
                result.ID = data.ID;
                result.CreatedDate = data.CreatedDate;
                result.Identifier = data.Identifier;
                result.IsActive = data.IsActive;
                result.ModifiedBy = data.ModifiedBy;
                result.ModifiedDate = data.ModifiedDate;
                result.Name = data.Name;
                result.PipelineID = data.PipelineID;
                result.PropCode = data.PropCode;
                result.RDUsageID = data.RDUsageID;

                if (result.RDUsageID == 1)
                    result.RDB = "R";
                else if (result.RDUsageID == 2)
                    result.RDB = "D";
                else
                    result.RDB = "B";

                    return result;

        }

        public bool UpdateLocationByID(LocationsDTO loc)
        {

            // save location which are in Oacy/Unsc but not in table 
            Location location = new Location();
            if (loc.ID == 0)
            {
                location.CreatedDate = DateTime.Now;
                location.Name = loc.Name;
                location.Identifier = loc.Identifier;
                location.PropCode = loc.PropCode;
                location.RDUsageID = loc.RDUsageID;
                location.PipelineID = loc.PipelineID;
                location.IsActive = true;
                location.CreatedBy = loc.CreatedBy;
                location.ModifiedBy = loc.ModifiedBy;
                location.ModifiedDate = DateTime.Now;
                location.PipeDuns = loc.PipelineDuns;
                location.TransactionTypeId = 0;

            DbContext.Locations.Add(location);
            DbContext.SaveChanges();

             }
             else {
                //update location in location table 
                  var objLoc = DbContext.Locations.Where(a => a.ID == loc.ID).FirstOrDefault();
                  objLoc.Name = loc.Name;
                  objLoc.Identifier = loc.Identifier;
                  objLoc.PropCode = loc.PropCode;
                  objLoc.RDUsageID = loc.RDUsageID;
                  // objLoc.PipelineID = loc.PipelineID;
                // objLoc.IsActive = loc.IsActive;                
                  //objLoc.ModifiedBy = loc.ModifiedBy;
                  objLoc.ModifiedDate = DateTime.Now;
                  // objLoc.PipeDuns = loc.PipeDuns; // adding pipeduns is pending
                  //objLoc.TransactionTypeId = 0;                
                  DbContext.Entry(objLoc).State = EntityState.Modified;
                  DbContext.SaveChanges();
            }
            return true;
        }
        public bool DeleteLocationByID(int ID)
        {
           
            var objLoc = DbContext.Locations.Where(a => a.ID == ID).FirstOrDefault();

            objLoc.IsActive = false;
            objLoc.ModifiedDate = DateTime.Now;
            DbContext.Entry(objLoc).State = EntityState.Modified;
            DbContext.SaveChanges();

            return true;
        }
       public LocationsResultDTO GetLocFromOacyUnsc(string Pipelineduns)
       {
           
            LocationsResultDTO locationsResultDTO = new LocationsResultDTO();
            var pipeId = DbContext.Pipelines.Where(a => a.DUNSNo == Pipelineduns).Select(p => p.ID);
            try
            {
                var Res = ((from unsc in DbContext.UnscPerTransactions
                            join l in DbContext.Locations
                            on new { unsc.Loc, Column1 = unsc.Loc, unsc.LocName, unsc.TransactionServiceProvider }
                            equals new { Loc = l.Identifier, Column1 = l.PropCode, LocName = l.Name, TransactionServiceProvider = l.PipeDuns } into l_join
                            from l in l_join.DefaultIfEmpty()
                            where (l.Identifier == null || l.PropCode == null) && l.Name == null && l.PropCode == null && l.PipeDuns == null && unsc.TransactionServiceProvider == Pipelineduns
                            group unsc by new { unsc.Loc, unsc.LocName, unsc.TransactionServiceProvider } into g
                            select new { g.Key.LocName, g.Key.Loc, g.Key.TransactionServiceProvider }).Union
                           (from oacy in DbContext.OACYPerTransactions
                            join l in DbContext.Locations
                            on new { oacy.Loc, Column1 = oacy.Loc, oacy.LocName, oacy.TransactionServiceProvider }
                            equals new { Loc = l.Identifier, Column1 = l.PropCode, LocName = l.Name, TransactionServiceProvider = l.PipeDuns } into l_join
                            from l in l_join.DefaultIfEmpty()
                            where (l.Identifier == null || l.PropCode == null) && l.Name == null && l.PropCode == null && l.PipeDuns == null && oacy.TransactionServiceProvider == Pipelineduns
                            group oacy by new { oacy.Loc, oacy.LocName, oacy.TransactionServiceProvider } into g
                            select new { LocName = g.Key.LocName, Loc = g.Key.Loc, TransactionServiceProvider = g.Key.TransactionServiceProvider }));


                var Result = (from r in Res
                              select new LocationsDTO()
                              {
                                  Name = r.LocName,
                                  RDB = null,
                                  Identifier = r.Loc,
                                  PropCode = r.Loc,
                                  RDUsageID = 0,
                                  PipelineDuns = r.TransactionServiceProvider                                 
                              }).ToList();
                     
                locationsResultDTO.locationsDTO = (Result != null || Result.Count > 0) ? Result : new List<LocationsDTO>();
                locationsResultDTO.RecordCount = Result.Count();
            }
            catch (Exception ex)
            {
            }

             return locationsResultDTO;
        }
        
    }
}


