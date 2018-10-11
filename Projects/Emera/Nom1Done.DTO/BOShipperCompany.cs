using System;

namespace Nom1Done.DTO
{

    /// <summary>
    /// The BOShipperCompany class. [Please feel free to enter extra information]
    /// </summary>
    public class BOShipperCompany
    {

        #region Public Properties
        public Int32 ID { get; set; }

        public String Name { get; set; }

        public String DUNS { get; set; }

        public Boolean IsActive { get; set; }

        public String CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public String ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        #endregion
    }

}
