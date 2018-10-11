using System;
using System.Web.Script.Serialization;

namespace Nom1Done.Data
{
    public static class NotifierEntityExtentions
    {
        public static String ToJson(this NotifierEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("NotifierEntity can not be null!");
            return new JavaScriptSerializer().Serialize(entity);
        }
    }
}