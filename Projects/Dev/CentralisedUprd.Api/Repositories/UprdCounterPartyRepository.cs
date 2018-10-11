using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CentralisedUprd.Api.Models;
using CentralisedUprd.Api.UPRD.DTO;
using CentralisedUprd.Api.Helpers;
using System;

namespace CentralisedUprd.Api.Repositories
{
    public class UprdCounterPartyRepository
    {
        UprdDbEntities1 DbContext = new UprdDbEntities1();
        ModalFactory modalFactory = new ModalFactory();
        SortingPagingInfo sortingPagingInfo = new SortingPagingInfo();
        public CounterParty GetCounterPartyByPropCode(string propCode)
        {
            return this.DbContext.CounterParties.Where(a => a.PropCode == propCode).FirstOrDefault();
        }

        public CounterParty GetCounterPartyByIdentifier(string identifier)
        {
            return this.DbContext.CounterParties.Where(a => a.Identifier == identifier).FirstOrDefault();
        }

        public List<CounterPartiesDTO> GetCounterParties(string Keyword, string PipeDuns)
        {
            List<CounterParty> data = new List<CounterParty>();
            if (string.IsNullOrEmpty(Keyword))
            {
                data = DbContext.CounterParties.Where(a =>
                a.IsActive
                 ).ToList();
            }
            else
            {
                data = DbContext.CounterParties.Where(a =>
                (a.Identifier.Contains(Keyword)
                || a.PropCode.Contains(Keyword)
                || a.Name.Contains(Keyword))
                //&& (a.PipeDuns == PipeDuns)
                && a.IsActive
                 ).ToList();
            }

            return data.Select(a => modalFactory.Parse(a)).ToList();
        }

        public List<CounterPartiesDTO> GetCounterParty()
        {
            try
            {
                var data = DbContext.CounterParties.Where(a => a.IsActive).Distinct().ToList();
                List<CounterPartiesDTO> list = new List<CounterPartiesDTO>();
                foreach (var item in data)
                {
                    list.Add(modalFactory.Parse(item));
                }
                return list;
                //var data= DbContext.CounterParties.Where(a => a.IsActive).ToList().Select(a => modalFactory.Parse(a)).ToList();
                //return data;
            }
            catch (Exception ex)
            {
                return null;
            }
            
            //return new List<CounterPartiesDTO>
            //{
            //    new CounterPartiesDTO
            //    {
            //        Identifier="000000001",
            //        Name="Name1"
            //    },new CounterPartiesDTO
            //    {
            //        Identifier="000000002",
            //        Name="Name2"
            //    },
            //    new CounterPartiesDTO
            //    {
            //        Identifier="000000003",
            //        Name="Name3"
            //    },
            //    new CounterPartiesDTO
            //    {
            //        Identifier="000000004",
            //        Name="Name4"
            //    }
            //};
        }
        // PageNo  = 0,1,2,3...so on. 
        public CounterPartiesResultDTO GetCounterPartiesUsingPaging(string Keyword, string PipeDuns, SortingPagingInfo sortingPagingInfo)
        {
            CounterPartiesResultDTO counterPartiesResultDTO = new CounterPartiesResultDTO();
            List<CounterParty> Result = new List<CounterParty>();

            int PageNo = sortingPagingInfo.CurrentPageIndex;
            int PageSize = sortingPagingInfo.PageSize;
            string order = sortingPagingInfo.SortField;
            string orderDir = sortingPagingInfo.SortDirection;

             
            if (string.IsNullOrEmpty(Keyword))
            {
                var QueryData = DbContext.CounterParties.Where(a =>
                 a.IsActive);                    //.Skip(PageNo * PageSize).Take(PageSize);
                var QueryDataWithOrder = GetCounterPartiesWithOrder(QueryData, sortingPagingInfo);
                sortingPagingInfo.PageCount = QueryDataWithOrder.Count();
                Result = QueryDataWithOrder.Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                var QueryDataWithKeyword = DbContext.CounterParties.Where(a =>
                (a.Identifier.Contains(Keyword)
                || a.PropCode.Contains(Keyword)
                || a.Name.Contains(Keyword))
                && a.IsActive);           //.OrderBy(a => a.Identifier).Skip(PageNo * PageSize).Take(PageSize).ToList();
                var QueryDataWithOrder = GetCounterPartiesWithOrder(QueryDataWithKeyword, sortingPagingInfo);
                sortingPagingInfo.PageCount = QueryDataWithOrder.Count();
                Result = QueryDataWithOrder.Skip((PageNo -1) * PageSize).Take(PageSize).ToList();
            }
            counterPartiesResultDTO.RecordCount = sortingPagingInfo.PageCount;
            counterPartiesResultDTO.CounterParties = Result.Select(a => modalFactory.Parse(a)).ToList();
            return counterPartiesResultDTO;
        }

        private IQueryable<CounterParty> GetCounterPartiesWithOrder(IQueryable<CounterParty> queryData, SortingPagingInfo sortingPagingInfo)
        {
            if (sortingPagingInfo != null)
            {
                // Sorting
                string orderDir = sortingPagingInfo.SortDirection;
                switch (sortingPagingInfo.SortField)
                {
                    case "Name":
                        queryData = orderDir.Equals("desc") ? queryData.OrderByDescending(a => a.Name) : queryData.OrderBy(a => a.Name);
                        break;
                    case "Identifier":
                        queryData = orderDir.Equals("desc") ? queryData.OrderByDescending(a => a.Identifier) : queryData.OrderBy(a => a.Identifier);
                        break;
                    case "PropCode":
                        queryData = orderDir.Equals("desc") ? queryData.OrderByDescending(a => a.PropCode) : queryData.OrderBy(a => a.PropCode);
                        break;
                    default:
                        queryData = queryData.OrderByDescending(p => p.CreatedDate);
                        break;
                }
            }
            else
            {
                return queryData;
            }
            return queryData;
        }


        public int GetTotalCounterParties(string Keyword, string PipeDuns)
        {
            int data = 0;
            if (string.IsNullOrEmpty(Keyword))
            {
                data = DbContext.CounterParties.Where(a =>
                a.IsActive
                 ).Count();
            }
            else
            {
                data = DbContext.CounterParties.Where(a =>
                 (a.Identifier.Contains(Keyword)
                 || a.PropCode.Contains(Keyword)
                 || a.Name.Contains(Keyword))
                 && a.IsActive
                  ).Count();
            }
            return data;
        }
    }
}