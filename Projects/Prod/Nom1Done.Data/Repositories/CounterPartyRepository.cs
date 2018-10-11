using Nom1Done.Model;
using Nom1Done.Infrastructure;
using System;
using System.Linq;
using System.Collections.Generic;
using Nom.ViewModel;

namespace Nom1Done.Data.Repositories
{
    public class CounterPartyRepository : RepositoryBase<CounterParty>, ICounterPartyRepository
    {
        public CounterPartyRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public CounterParty GetCounterPartyByPropCode(string propCode)
        {
            return this.DbContext.CounterParty.Where(a => a.PropCode == propCode).FirstOrDefault();
        }


        public CounterParty GetCounterPartyByIdentifier(string identifier)
        {
            return this.DbContext.CounterParty.Where(a => a.Identifier == identifier).FirstOrDefault();
        }

        public List<CounterPartiesDTO> GetCounterParties(string Keyword, int PipelineID)
        {
            List<CounterPartiesDTO> items = new List<CounterPartiesDTO>();
            List<CounterParty> data = new List<CounterParty>();
            if (string.IsNullOrEmpty(Keyword))
            {
                data = DbContext.CounterParty.Where(a =>                
                (a.PipelineID == PipelineID || a.PipelineID == 0)
                && a.IsActive
                 ).ToList();
            }
            else { 
                data = DbContext.CounterParty.Where(a=>
                (a.Identifier.Contains(Keyword)
                || a.PropCode.Contains(Keyword)
                || a.Name.Contains(Keyword))
                && (a.PipelineID == PipelineID || a.PipelineID == 0)
                && a.IsActive
                 ).ToList();
            }

            foreach (var item in data)
            {
                CounterPartiesDTO itemObj = new CounterPartiesDTO();
                itemObj.ID = item.ID;
                itemObj.Name = item.Name;
                itemObj.Identifier = item.Identifier;
                itemObj.PropCode = item.PropCode;
                itemObj.PipelineID = item.PipelineID;
                itemObj.IsActive = item.IsActive;
                itemObj.CreatedBy = item.CreatedBy;
                itemObj.CreatedDate = item.CreatedDate;
                itemObj.ModifiedBy = item.ModifiedBy;
                itemObj.ModifiedDate = item.ModifiedDate;
                items.Add(itemObj);
            }

            return items;
        }


        // PageNo  = 0,1,2,3...so on. 
        public List<CounterPartiesDTO> GetCounterPartiesUsingPaging(string Keyword, int PipelineID, int PageNo, int PageSize,string order,string orderDir)
        {
            List<CounterPartiesDTO> items = new List<CounterPartiesDTO>();
            List<CounterParty> Result = new List<CounterParty>();
            if (string.IsNullOrEmpty(Keyword))
            {
                var QueryData = DbContext.CounterParty.Where(a =>
                (a.PipelineID == PipelineID || a.PipelineID == 0)
                && a.IsActive);                    //.Skip(PageNo * PageSize).Take(PageSize);
                var QueryDataWithOrder = GetCounterPartiesWithOrder(QueryData, orderDir,order);
                Result = QueryDataWithOrder.Skip(PageNo * PageSize).Take(PageSize).ToList();                
            }
            else
            {
                var QueryDataWithKeyword = DbContext.CounterParty.Where(a =>
                (a.Identifier.Contains(Keyword)
                || a.PropCode.Contains(Keyword)
                || a.Name.Contains(Keyword))
                && (a.PipelineID == PipelineID || a.PipelineID == 0)
                && a.IsActive);           //.OrderBy(a => a.Identifier).Skip(PageNo * PageSize).Take(PageSize).ToList();
                var QueryDataWithOrder = GetCounterPartiesWithOrder(QueryDataWithKeyword, orderDir, order);
                Result = QueryDataWithOrder.Skip(PageNo * PageSize).Take(PageSize).ToList();
            }

            foreach (var item in Result)
            {
                CounterPartiesDTO itemObj = new CounterPartiesDTO();
                itemObj.ID = item.ID;
                itemObj.Name = item.Name;
                itemObj.Identifier = item.Identifier;
                itemObj.PropCode = item.PropCode;
                itemObj.PipelineID = item.PipelineID;
                itemObj.IsActive = item.IsActive;
                itemObj.CreatedBy = item.CreatedBy;
                itemObj.CreatedDate = item.CreatedDate;
                itemObj.ModifiedBy = item.ModifiedBy;
                itemObj.ModifiedDate = item.ModifiedDate;
                items.Add(itemObj);
            }

            return items;
        }

        private IQueryable<CounterParty> GetCounterPartiesWithOrder(IQueryable<CounterParty> queryData,string sortingDir,string order)
        {
            switch(order)
            {
                case "1":
                    queryData = sortingDir.Equals("desc") ? queryData.OrderByDescending(a => a.Name) : queryData.OrderBy(a => a.Name);
                    break;
                case "2":
                    queryData = sortingDir.Equals("desc") ? queryData.OrderByDescending(a => a.Identifier) : queryData.OrderBy(a => a.Identifier);
                    break;
                case "3":
                    queryData = sortingDir.Equals("desc") ? queryData.OrderByDescending(a => a.PropCode) : queryData.OrderBy(a => a.PropCode);
                    break;
            }
            return queryData;
        }

        public int GetTotalCounterParties(string Keyword, int PipelineID)
        {
            if (string.IsNullOrEmpty(Keyword))
            {
                return DbContext.CounterParty.Where(a =>
                (a.PipelineID == PipelineID || a.PipelineID == 0)
                && a.IsActive
                 ).Count();
            }
            else
            {
               return DbContext.CounterParty.Where(a =>
                (a.Identifier.Contains(Keyword)
                || a.PropCode.Contains(Keyword)
                || a.Name.Contains(Keyword))
                && (a.PipelineID == PipelineID || a.PipelineID == 0)
                && a.IsActive
                 ).Count();
            }
        }


     }
    public interface ICounterPartyRepository:IRepository<CounterParty>
    {
        CounterParty GetCounterPartyByPropCode(string propCode);
        List<CounterPartiesDTO> GetCounterParties(string Keyword, int PipelineID);
        CounterParty GetCounterPartyByIdentifier(string identifier);
        int GetTotalCounterParties(string Keyword, int PipelineID);

        List<CounterPartiesDTO> GetCounterPartiesUsingPaging(string Keyword, int PipelineID, int PageNo, int PageSize,string order,string orderDir);

    }
}
