using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace UPRD.Data.SQLServerNotifier
{
    public class NotifierEntity
    {
        public String SqlQuery { get; set; }

        public String SqlConnectionString { get; set; }

        public string SqlParamVal { get; set; }

        public string SqlParam { get; set; }

        public ICollection<SqlParameter> SqlParameters { get; set; } = new List<SqlParameter>();


        public static NotifierEntity FromJson(String value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException("NotifierEntity Value can not be null!");

            return new JavaScriptSerializer().Deserialize<NotifierEntity>(value);
        }
    }
}