using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Nom1Done.Data
{
    public class NotifierEntity
    {
       // ICollection<SqlParameter> sqlParameter = new List<SqlParameter>();

        public String SqlQuery { get; set; }

        public String SqlConnectionString { get; set; }

        public string SqlParamVal { get; set; }

        public string SqlParam { get; set; }

        public ICollection<SqlParameter> SqlParameters { get; set; } = new List<SqlParameter>();
        //{
        //    get
        //    {
        //        return sqlParameter;
        //    }
        //    set
        //    {
        //        sqlParameter = value;
        //    }
        //}

        public static NotifierEntity FromJson(String value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException("NotifierEntity Value can not be null!");

            return new JavaScriptSerializer().Deserialize<NotifierEntity>(value);
           // return JsonConvert.DeserializeObject<NotifierEntity>(value);
        }
    }
}