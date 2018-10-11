using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CentralisedUprd.Api.CustomQueryHelper
{
    public class DateSearch : AbstractSearch
    {
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? SearchTerm { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? SearchTerm2 { get; set; }

        // public DateComparators Comparator { get; set; }
        public string Comparator { get; set; }
        protected override Expression BuildExpression(MemberExpression property)
        {
            Expression searchExpression1 = null;
            Expression searchExpression2 = null;

            if (this.SearchTerm.HasValue)
            {
                searchExpression1 = this.GetFilterExpression(property);
            }

            if (this.SearchTerm2.HasValue)
            {
                searchExpression2 = Expression.LessThanOrEqual(property, Expression.Constant(this.SearchTerm2.Value));
            }

            if (searchExpression1 == null && searchExpression2 == null)
            {
                return null;
            }
            else if (searchExpression1 != null && searchExpression2 != null)
            {
                var combinedExpression = Expression.AndAlso(searchExpression1, searchExpression2);
                return combinedExpression;
            }
            else if (searchExpression1 != null)
            {
                return searchExpression1;
            }
            else
            {
                return searchExpression2;
            }
        }
        private Expression GetFilterExpression(MemberExpression property)
        {
           
            try
            {    
                MethodCallExpression left = Expression.Call(null, typeof(DbFunctions).GetMethod("TruncateTime", new Type[] { typeof(DateTime) }), property.Expression);
                ConstantExpression right = Expression.Constant(this.SearchTerm.Value, property.Expression.Type);
                       
            switch (this.Comparator)
            {
                case "<":
                    return Expression.LessThan(left, right);
                case "<=":
                    return Expression.LessThanOrEqual(left, right);
                case "==":
                    return Expression.Equal(left, right);
                case "!=":
                    return Expression.NotEqual(left, right);
                case ">=":               
                    return Expression.GreaterThanOrEqual(left, right);
                case ">":
                    return Expression.GreaterThan(left, right);
                default:
                    throw new InvalidOperationException("Comparator not supported.");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public enum DateComparators
    {
        [Display(Name = "<")]
        Less,

        [Display(Name = "<=")]
        LessOrEqual,

        [Display(Name = "==")]
        Equal,

        [Display(Name = ">=")]
        GreaterOrEqual,

        [Display(Name = ">")]
        Greater,

        [Display(Name = "Range")]
        InRange
    }
}
