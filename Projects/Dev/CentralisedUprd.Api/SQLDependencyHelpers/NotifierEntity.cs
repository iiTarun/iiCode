using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CentralisedUprd.Api
{
    public class NotifierEntity
    {
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

            // return new JavaScriptSerializer().Deserialize<NotifierEntity>(value);
            return JsonConvert.DeserializeObject<NotifierEntity>(value);
        }
    }



    public static class NotifierEntityExtentions
    {
        public static String ToJson(this NotifierEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("NotifierEntity can not be null!");
            return JsonConvert.SerializeObject(entity); //new JavaScriptSerializer().Serialize(entity);
        }
    }
}
