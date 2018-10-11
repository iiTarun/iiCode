﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CentralisedUprd.Api.CustomQueryHelper
{
   public class EnumSearch : AbstractSearch
    {
        public string SearchTerm { get; set; }

        public Type EnumType
        {
            get
            {
                return Type.GetType(this.EnumTypeName);
            }
        }

        public string EnumTypeName { get; set; }

        public EnumSearch()
        {
        }

        public EnumSearch(Type enumType)
        {
            this.EnumTypeName = enumType.AssemblyQualifiedName;
        }

        protected override System.Linq.Expressions.Expression BuildExpression(System.Linq.Expressions.MemberExpression property)
        {
            if (this.SearchTerm == null)
            {
                return null;
            }

            var enumValue = System.Enum.Parse(this.EnumType, this.SearchTerm);

            Expression searchExpression = Expression.Equal(property, Expression.Constant(enumValue));

            return searchExpression;
        }
    }
}
