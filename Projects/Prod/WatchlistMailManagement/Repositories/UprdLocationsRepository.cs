using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System;
using UPRD.Data;
using UPRD.Model;
using WatchlistMailManagement.UPRD.DTO;
using WatchlistMailManagement.Uprd.DTO;

namespace WatchlistMailManagement.Repositories
{
    public class UprdLocationsRepository
    { 
        UPRDEntities DbContext = new UPRDEntities();
        ModalFactory modalFactory = new ModalFactory();
        SortingPagingInfo sortingPagingInfo = new SortingPagingInfo();
        public IQueryable<Location> GetLocations(string Keyword, string PipelineDuns)
        {

            IQueryable<Location> data;
            if (string.IsNullOrEmpty(Keyword))
            {
                data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true);
            }
            else
            {
                data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
                && a.IsActive == true
                && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword)));
            }

            return data;  //.Select(a => modalFactory.Parse(a)).OrderBy(a=>a.Name).ToList();
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
                    var QueryableData = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB);
                    var QueryableDataWithOrder = GetQueryableDataWithOrder(QueryableData, sortingPagingInfo);
                    sortingPagingInfo.PageCount= QueryableDataWithOrder.Count();
                    Result = QueryableDataWithOrder.Skip((PageNo - 1) * PageSize).Take(PageSize).ToList();

                }
                else
                {
                    var QueryableDataWithKeyword = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
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
                            Result = DbContext.Location.Where(a => a.PropCode == "103565" || a.PropCode == "103702").OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Location.Where(a => (a.PropCode == "103565" || a.PropCode == "103702")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                    }
                    else if (PipelineDuns == "007933047") //Transwestern Pipeline Company, LLC
                    {
                        // locationList = locationList.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            Result = DbContext.Location.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").OrderBy(a => a.Name).Skip((PageNo-1) * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Location.Where(a => (a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                    }
                    else if (PipelineDuns == "829416002")  // FAYETTEVILLE EXPRESS PIPELINE LLC
                    {
                        //locationList = locationList.Where(a => a.PropCode == "58744").ToList();
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            Result = DbContext.Location.Where(a => a.PropCode == "58744").OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Location.Where(a => (a.PropCode == "58744")
                            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Keyword))
                        {
                            Result = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB).OrderBy(a => a.Name).Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                        }
                        else
                        {
                            Result = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
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
                        var QueriableDelLocData = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB);
                        var QueryableDataWithOrderDelLoc = GetQueryableDataWithOrder(QueriableDelLocData, sortingPagingInfo);
                        sortingPagingInfo.PageCount = QueryableDataWithOrderDelLoc.Count();
                        Result = QueryableDataWithOrderDelLoc.Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
                    }
                    else
                    {
                        var QueriableDelLocDataWithData = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
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

            }

            return data;
        }

        public List<LocationsDTO> GetLocationList()
        {
            try
            {
                var data= DbContext.Location.Where(a => a.IsActive).Distinct().ToList();
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
                data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true).ToList();
            }
            else
            {
                data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
                && a.IsActive == true
               && a.Identifier.Contains(Identifier)).ToList();
            }
            return data.Select(a => modalFactory.Parse(a)).OrderBy(a => a.Name).ToList();
        }
    }
}