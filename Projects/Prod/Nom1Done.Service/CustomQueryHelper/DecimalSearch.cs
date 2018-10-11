using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Service
{
   public class DecimalSearch : AbstractSearch
    {
        public decimal SearchTerm { get; set; }

        public string Comparator { get; set; }

        protected override Expression BuildExpression(MemberExpression property)
        {          

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
                //        return Expression.LessThan(property, Expression.Constant(this.SearchTerm));
                //    case "<=":
                //        return Expression.LessThanOrEqual(property, Expression.Constant(this.SearchTerm));
                //    case "==":
                //        return Expression.Equal(property, Expression.Constant(this.SearchTerm));
                //    case ">=":
                //        return Expression.GreaterThanOrEqual(property, Expression.Constant(this.SearchTerm));
                //    case ">":
                //        return Expression.GreaterThan(property, Expression.Constant(this.SearchTerm));
                //    default:
                //        throw new InvalidOperationException("Comparator not supported.");
                //}

                ConstantExpression constant = Expression.Constant(this.SearchTerm);
                switch (this.Comparator)
                {
                    case "<":
                        return Expression.LessThan(property, Expression.Convert(constant, typeof(decimal)));
                    case "<=":
                        return Expression.LessThanOrEqual(property, Expression.Convert(constant, typeof(decimal)));
                    case "==":
                        return Expression.Equal(property, Expression.Convert(constant, typeof(decimal)));
                    case ">=":
                        return Expression.GreaterThanOrEqual(property, Expression.Convert(constant, typeof(decimal)));
                    case ">":
                        return Expression.GreaterThan(property, Expression.Convert(constant, typeof(decimal)));
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
}
