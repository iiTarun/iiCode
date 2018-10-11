using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service
{
   public class NumericSearch : AbstractSearch
    {
        public long SearchTerm { get; set; }

        public string Comparator { get; set; }

        protected override Expression BuildExpression(MemberExpression property)
        {
            //if (!this.SearchTerm.HasValue)
            //{
            //    return null;
            //}

            Expression searchExpression = this.GetFilterExpression(property);

            return searchExpression;
        }

        private Expression GetFilterExpression(MemberExpression property)
        {
            try
            {
                //switch (this.Comparator)
                //{
                //    case "<":
                //        return Expression.LessThan(property, Expression.Constant(this.SearchTerm.Value));
                //    case "<=":
                //        return Expression.LessThanOrEqual(property, Expression.Constant(this.SearchTerm.Value));
                //    case "==":
                //        return Expression.Equal(property, Expression.Constant(this.SearchTerm.Value));
                //    case ">=":
                //        return Expression.GreaterThanOrEqual(property, Expression.Constant(this.SearchTerm.Value));
                //    case ">":
                //        return Expression.GreaterThan(property, Expression.Constant(this.SearchTerm.Value));
                //    default:
                //        throw new InvalidOperationException("Comparator not supported.");
                //}

                ConstantExpression constant = Expression.Constant(this.SearchTerm);
                switch (this.Comparator)
                {
                    case "<":
                        return Expression.LessThan(property, Expression.Convert(constant, typeof(long)));
                    case "<=":
                        return Expression.LessThanOrEqual(property, Expression.Convert(constant, typeof(long)));
                    case "==":
                        return Expression.Equal(property, Expression.Convert(constant, typeof(long)));
                    case ">=":
                        return Expression.GreaterThanOrEqual(property, Expression.Convert(constant, typeof(long)));
                    case ">":
                        return Expression.GreaterThan(property, Expression.Convert(constant, typeof(long)));
                    default:
                        throw new InvalidOperationException("Comparator not supported.");
                }

            }
            catch (Exception ex)
            {
                return Expression.Empty();
            }
        }
    }

    public enum NumericComparators
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
        Greater
    }
}
