using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPRD.Model;


using UPRD.Infrastructure;
using System.Data.Entity;
using UPRD.DTO;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace UPRD.Data.Repositories
{
    public class UPRDLocationRepository : RepositoryBase<Location>, IUPRDLocationRepository

    {
        public UPRDLocationRepository(IDbFactory dbfactory) : base(dbfactory)
        {

        }

        public List<LocationsDTO> GetLocations(string Keyword, string PipelineDuns)
        {
            List<LocationsDTO> resultantList = new List<LocationsDTO>();
            List<Location> data = new List<Location>();
            if (string.IsNullOrEmpty(Keyword))
            {
                data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true).ToList();
            }
            else
            {
                data = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
                && a.IsActive == true
                && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).ToList();
            }

            foreach (var item in data)
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
            return resultantList.OrderBy(a => a.Name).ToList();
        }

        // PageNo  = 0,1,2,3...so on.
        //public List<Location> GetLocationsWithPaging(string Keyword, string PipelineDuns, int PageNo, int PageSize, string PopupFor, bool IsSpecialDelCase, string order, string orderDir)
        //{
        //    List<LocationsDTO> resultantList = new List<LocationsDTO>();
        //    List<Location> Result = new List<Location>();
        //    int NotRDB = 1; // for not Receipt locs.
        //    if (PopupFor == "RecLoc" || PopupFor == "Receipt" || PopupFor == "ContractPath" || PopupFor == "Supply")
        //    {
        //        NotRDB = 2;  // for not delivery locs. 
        //        if (string.IsNullOrEmpty(Keyword))
        //        {
        //            var QueryableData = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB);
        //            var QueryableDataWithOrder = GetQueryableDataWithOrder(QueryableData, order, orderDir);
        //            Result = QueryableDataWithOrder.Skip(PageNo * PageSize).Take(PageSize).ToList();

        //        }
        //        else
        //        {
        //            var QueryableDataWithKeyword = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
        //            && a.IsActive == true
        //            && a.RDUsageID != NotRDB
        //            && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword)));
        //            var QueryableDataWithOrder = GetQueryableDataWithOrder(QueryableDataWithKeyword, order, orderDir);
        //            Result = QueryableDataWithOrder.Skip(PageNo * PageSize).Take(PageSize).ToList();
        //        }
        //    }
        //    else
        //    {
        //        NotRDB = 1;  // not-Receipt 
        //        if (IsSpecialDelCase)
        //        {

        //            if (PipelineDuns == "006958581")  // ANR Pipeline Company
        //            {
        //                // locationList = locationList.Where(a => a.PropCode == "103565" || a.PropCode == "103702").ToList();
        //                if (string.IsNullOrEmpty(Keyword))
        //                {
        //                    Result = DbContext.Location.Where(a => a.PropCode == "103565" || a.PropCode == "103702").OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
        //                }
        //                else
        //                {
        //                    Result = DbContext.Location.Where(a => (a.PropCode == "103565" || a.PropCode == "103702")
        //                    && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
        //                }
        //            }
        //            else if (PipelineDuns == "007933047") //Transwestern Pipeline Company, LLC
        //            {
        //                // locationList = locationList.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").ToList();
        //                if (string.IsNullOrEmpty(Keyword))
        //                {
        //                    Result = DbContext.Location.Where(a => a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543").OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
        //                }
        //                else
        //                {
        //                    Result = DbContext.Location.Where(a => (a.PropCode == "58646" || a.PropCode == "58647" || a.PropCode == "58648" || a.PropCode == "58649" || a.PropCode == "500543")
        //                    && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
        //                }
        //            }
        //            else if (PipelineDuns == "829416002")  // FAYETTEVILLE EXPRESS PIPELINE LLC
        //            {
        //                //locationList = locationList.Where(a => a.PropCode == "58744").ToList();
        //                if (string.IsNullOrEmpty(Keyword))
        //                {
        //                    Result = DbContext.Location.Where(a => a.PropCode == "58744").OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
        //                }
        //                else
        //                {
        //                    Result = DbContext.Location.Where(a => (a.PropCode == "58744")
        //                    && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
        //                }
        //            }
        //            else
        //            {
        //                if (string.IsNullOrEmpty(Keyword))
        //                {
        //                    Result = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB).OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
        //                }
        //                else
        //                {
        //                    Result = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
        //                    && a.IsActive == true
        //                    && a.RDUsageID != NotRDB
        //                    && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword))).OrderBy(a => a.Name).Skip(PageNo * PageSize).Take(PageSize).ToList();
        //                }
        //            }
        //        }
        //        else//If location is Delivery
        //        {
        //            if (string.IsNullOrEmpty(Keyword))
        //            {
        //                var QueriableDelLocData = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns && a.IsActive == true && a.RDUsageID != NotRDB);
        //                var QueryableDataWithOrderDelLoc = GetQueryableDataWithOrder(QueriableDelLocData, order, orderDir);
        //                Result = QueryableDataWithOrderDelLoc.Skip(PageNo * PageSize).Take(PageSize).ToList();
        //            }
        //            else
        //            {
        //                var QueriableDelLocDataWithData = DbContext.Location.Where(a => a.PipeDuns == PipelineDuns
        //                && a.IsActive == true
        //                && a.RDUsageID != NotRDB
        //                && (a.Identifier.Contains(Keyword) || a.PropCode.Contains(Keyword) || a.Name.Contains(Keyword)));
        //                var QueryableDataWithOrderDelLoc = GetQueryableDataWithOrder(QueriableDelLocDataWithData, order, orderDir);
        //                Result = QueryableDataWithOrderDelLoc.Skip(PageNo * PageSize).Take(PageSize).ToList();
        //            }
        //        }
        //    }


        //    foreach (var item in Result)
        //    {
        //        var dtoObj = new LocationsDTO
        //        {
        //            CreatedBy = item.CreatedBy,
        //            ID = item.ID,
        //            CreatedDate = item.CreatedDate,
        //            Identifier = item.Identifier,
        //            IsActive = item.IsActive,
        //            ModifiedBy = item.ModifiedBy,
        //            ModifiedDate = item.ModifiedDate,
        //            Name = item.Name,
        //            PipelineID = item.PipelineID,
        //            PropCode = item.PropCode,
        //            RDUsageID = item.RDUsageID,
        //        };
        //        if (dtoObj.RDUsageID == 1)
        //            dtoObj.RDB = "R";
        //        else if (dtoObj.RDUsageID == 2)
        //            dtoObj.RDB = "D";
        //        else
        //            dtoObj.RDB = "B";
        //        resultantList.Add(dtoObj);
        //    }
        //    return resultantList;
        //}

        private IQueryable<Location> GetQueryableDataWithOrder(IQueryable<Location> queryableData, string order, string orderDir)
        {
            switch (order)
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
        public List<Location> GetLocationByPipeline(string pipelineDuns)
        {

            //var locationResult = DbContext.Location.Where(a => a.PipeDuns == pipelineDuns && a.IsActive == true).OrderBy(a => a.Name).ToList();

            var locationResult = DbContext.Location.Where(a => a.PipeDuns == pipelineDuns).OrderBy(a => a.Name).ToList();
            //locationResult = locationResult.Select(a => modalFactory.Parse(a)).OrderBy(a => a.Name).ToList();
            //locResult.RecordCount = locResult.locationsDTO.Count();

            return locationResult;

        }
        public Location GetLocationById(int id)
        {

            var data = DbContext.Location.Where(a => a.ID == id).FirstOrDefault();


            return data;

        }

        List<Location> IUPRDLocationRepository.GetLocations(string Keyword, string PipelineDuns)
        {
            throw new NotImplementedException();
        }



        public bool AddLocationByPipeline(List<Location> locList, string pipeDuns)
        {
            foreach (var item in locList)
            {
                Location loc = new Location()
                {

                    Name = item.Name,
                    Identifier = item.Identifier,
                    PropCode = item.PropCode,
                    PipelineID = item.PipelineID,
                    RDUsageID = item.RDUsageID,
                    CreatedBy = item.CreatedBy,
                    ModifiedBy = item.ModifiedBy,
                    PipeDuns = pipeDuns,
                    CreatedDate = DateTime.Now,//item.CreatedDate,
                    IsActive = true,
                    ModifiedDate = DateTime.Now,//item.ModifiedDate,
                    TransactionTypeId = 0
                };
                DbContext.Location.Add(loc);
            }

            DbContext.SaveChanges();

            return true;
        }

        public bool DeleteLocationByID(int ID)
        {

            var objLoc = DbContext.Location.Where(a => a.ID == ID).FirstOrDefault();

            objLoc.IsActive = false;
            objLoc.ModifiedDate = DateTime.Now;
            DbContext.Entry(objLoc).State = EntityState.Modified;
            DbContext.SaveChanges();

            return true;
        }

        public bool UpdateLocationByID(Location loc)
        {
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
                location.PipeDuns = loc.PipeDuns;
                location.TransactionTypeId = 0;

                DbContext.Location.Add(location);
                DbContext.SaveChanges();

            }
            else
            {
                //update location in location table 
                var objLoc = DbContext.Location.Where(a => a.ID == loc.ID).FirstOrDefault();
                objLoc.Name = loc.Name;
                objLoc.Identifier = loc.Identifier;
                objLoc.PropCode = loc.PropCode;
                objLoc.RDUsageID = loc.RDUsageID;

                objLoc.ModifiedDate = DateTime.Now;

                DbContext.Entry(objLoc).State = EntityState.Modified;
                DbContext.SaveChanges();
            }


            return true;
        }


        public LocationsResultDTO GetLocFromOacyUnsc(string Pipelineduns)
        {

            LocationsResultDTO locationsResultDTO = new LocationsResultDTO();
            var pipeId = DbContext.Pipeline.Where(a => a.DUNSNo == Pipelineduns).Select(p => p.ID);

            var Res = ((from unsc in DbContext.UnscPerTransaction
                        join l in DbContext.Location
                        on new { unsc.Loc, Column1 = unsc.Loc, unsc.LocName, unsc.TransactionServiceProvider }
                        equals new { Loc = l.Identifier, Column1 = l.PropCode, LocName = l.Name, TransactionServiceProvider = l.PipeDuns } into l_join
                        from l in l_join.DefaultIfEmpty()
                        where (l.Identifier == null || l.PropCode == null) && l.Name == null && l.PropCode == null && l.PipeDuns == null && unsc.TransactionServiceProvider == Pipelineduns
                        group unsc by new { unsc.Loc, unsc.LocName, unsc.TransactionServiceProvider } into g
                        select new { g.Key.LocName, g.Key.Loc, g.Key.TransactionServiceProvider }).Union
                       (from oacy in DbContext.OACYPerTransaction
                        join l in DbContext.Location
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
                              //RDUsageID = 0,
                              PipelineDuns = r.TransactionServiceProvider
                          }).ToList();
            foreach (var item in Result)
            {

                if (item.RDUsageID == 1)
                {
                    item.RDB = "R";
                }
                else if (item.RDUsageID == 2)
                {
                    item.RDB = "D";
                }
                else
                {
                    item.RDB = "B";
                }

            }

            locationsResultDTO.locationsDTO = (Result != null || Result.Count > 0) ? Result : new List<LocationsDTO>();
            locationsResultDTO.RecordCount = Result.Count();


            return locationsResultDTO;
        }

        public bool ClientEnvironmentsetting()
        {

            //List<Location> SourceList = DbContext.Location.ToList();
            //var clientList = DbContext.ClientEnvironmentSetting.Where(a => a.Enginestatus == true).ToList();
            //foreach (var item in clientList)
            //{
            //    using (var DestContext = new UPRDEntities(item.ConnectionString))
            //    {
            //        DestContext.Database.ExecuteSqlCommand("Truncate table Locations");
            //        foreach (var Locations in SourceList)
            //        {
            //            DestContext.Location.Add(Locations);
            //        }
            //        DestContext.SaveChanges();
            //    }
            var clientList = DbContext.ClientEnvironmentSetting.Where(a => a.Enginestatus == true).ToList();
            foreach (var item in clientList)
            {
                string sourceCon = ConfigurationManager.ConnectionStrings["UPRDEntities"].ConnectionString;
                using (SqlConnection Sourcecon = new SqlConnection(sourceCon))
                {
                    SqlCommand com = new SqlCommand("select * from Locations", Sourcecon);
                    Sourcecon.Open();
                    using (SqlDataReader rdr = com.ExecuteReader())
                    {
                        using (SqlConnection Descon = new SqlConnection(item.ConnectionString))
                        {
                            SqlCommand com1 = new SqlCommand("Truncate table Locations", Descon);
                            Descon.Open();
                            com1.ExecuteNonQuery();
                            using (SqlBulkCopy bc = new SqlBulkCopy(Descon))
                            {
                                bc.DestinationTableName = "Locations";
                                bc.WriteToServer(rdr);
                            }

                        }

                    }


                }
            }

            return true;
        }

        public bool ActivateLocation(int ID)
        {
            var objCounter = DbContext.Location.Where(a => a.ID == ID).FirstOrDefault();

            objCounter.IsActive = true;
            objCounter.ModifiedDate = DateTime.Now;
            DbContext.Entry(objCounter).State = EntityState.Modified;
            DbContext.SaveChanges();

            return true;
        }
    }
    public interface IUPRDLocationRepository : IRepository<Location>
    {
        List<Location> GetLocations(string Keyword, string PipelineDuns);
        //List<Location> GetLocationsWithPaging(string Keyword, string PipelineDuns, int PageNo, int PageSize, string PopupFor, bool IsSpecialDelCase, string order, string orderDir);
        int GetTotalLocationCount(string Keyword, string PipelineDuns, string PopupFor, bool IsSpecialDelCase);
        List<Location> GetLocationByPipeline(string pipelineDuns);
        bool UpdateLocationByID(Location loc);
        Location GetLocationById(int id);
        bool DeleteLocationByID(int ID);
        //bool AddLocationByPipeline(List<Location> locList);
        bool AddLocationByPipeline(List<Location> locList, string pipeDuns);
        LocationsResultDTO GetLocFromOacyUnsc(string Pipelineduns);
        bool ClientEnvironmentsetting();
        bool ActivateLocation(int id);
    }




}