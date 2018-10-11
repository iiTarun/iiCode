using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CentralisedUprd.Api.CustomQueryHelper
{
   public class StringSearch : AbstractSearch
    {
        public string SearchTerm { get; set; }

       // public TextComparators Comparator { get; set; }
        public string Comparator { get; set; }
        protected override Expression BuildExpression(MemberExpression property)
        {
            try { 
            if (this.SearchTerm == null)
            {
                return null;
            }
                if (this.Comparator == "==") { this.Comparator = "Equals"; }
                var searchExpression = Expression.Call(
                property,
                typeof(string).GetMethod(this.Comparator.ToString(), new[] { typeof(string) }),
                Expression.Constant(this.SearchTerm));
                return searchExpression;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public enum TextComparators
    {
        [Display(Name = "Contains")]
        Contains,

        [Display(Name = "==")]
        Equals
    }
}
