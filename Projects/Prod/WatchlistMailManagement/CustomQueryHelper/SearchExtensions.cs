using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace WatchlistMailManagement
{ 
    public static class SearchExtensions
    {

        public static IQueryable<T> ApplySearchCriterias<T>(this IQueryable<T> query, IEnumerable<AbstractSearch> searchCriterias)
        {
            foreach (var criteria in searchCriterias)
            {
                query = criteria.ApplyToQuery(query);
            }

            var result = query.ToArray();

            return query;
        }       


        public static AbstractSearch CreateSearchCriteria(Type targetType, Type propertyType, string property,string operatr, string value)
        {
            AbstractSearch result = null;
            try { 
            if (propertyType.Equals(typeof(string))|| propertyType.Equals(typeof(String)))
            {
                result = new StringSearch() { Comparator = operatr,SearchTerm=value};
            }
            else if (propertyType.Equals(typeof(long)))
            {
               result = new NumericSearch() { Comparator = operatr, SearchTerm =long.Parse(value) };
            }
            else if (propertyType.Equals(typeof(decimal)) || propertyType.Equals(typeof(Decimal)))
            {
                 result = new DecimalSearch() { Comparator = operatr, SearchTerm = decimal.Parse(value) };
            }
            else if (propertyType.Equals(typeof(DateTime)) || propertyType.Equals(typeof(DateTime?)))
            {
                result = new DateSearch() { Comparator=operatr , SearchTerm= DateTime.ParseExact(value, "MM/dd/yyyy", CultureInfo.InvariantCulture) };
               // result = new DateSearch() { Comparator = operatr, SearchTerm = Convert.ToDateTime(value) };
            }
            else if (propertyType.IsEnum)
            {
               result = new EnumSearch(propertyType) {  SearchTerm = value };
            }

            if (result != null)
            {
                result.Property = property;
                result.TargetTypeName = targetType.AssemblyQualifiedName;
            }
            }
            catch (Exception ex)
             {
                throw ex;
            }
            return result;
        }


    }
}
