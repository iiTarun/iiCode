using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.Model.Models
{
    public class TransactionalReport
    {

        [Key]
        public long id { get; set; }
        public int PipelineId { get; set; }
        public string PipeLineDuns { get; set; }
        public string TSP { get; set; }
        public string TSPName { get; set; }
        public DateTime PostingDateTime { get; set; }
        public string ContractHolderName { get; set; }
        public string ContractHolderIdentifier { get; set; }
        public string ContractHolderProp { get; set; }
        public string AffiliateIndicatorDesc { get; set; }
        public string RateSchedule { get; set; }
        public string ServiceRequesterContract { get; set; }
        public string ContactStatus { get; set; }
        public string AmendmentReporting { get; set; }
        public DateTime ContractBeginDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public DateTime ContractEntitlementBeginDate { get; set; }
        public DateTime ContractEntitlementEndDate { get; set; }
        public int SurchargeIndicator { get; set; }
        public string RateIdentificationCode { get; set; }
        public double RateCharged { get; set; }
        public string RateChargedReference { get; set; }
        public double MaximumTariffRate { get; set; }
        public string MaximumTariffRateReference { get; set; }
       public string  MarketBasedRateIndicator { get; set; }
       public string ReservationRateBasis { get; set; }
       public long ContractualQuantityContract { get; set; }
       public string NegotiatedRateIndicator { get; set; }
       public string TermsNotesIndicator { get; set; }

       public string LocQTIPurpDesc1 { get; set; }
       public long Location1 { get; set; }
       public string Location1Name { get; set; }
       public string LocationZone1 { get; set; }

      public string LocQTIPurpDesc2 { get; set; }
      public long Location2 { get; set; }
      public string Location2Name { get; set; }
      public string LocationZone2 { get; set; }
      public string CapacityTypeIndicator { get; set; }     
        public string Comments { get; set; }

        //For KinderMorgon-NGPL
        public DateTime PeakSeasonalStartDate { get; set; }
        public DateTime PeakSeasonalEndDate { get; set; }
        public DateTime OffPeakSeasonalStartDate { get; set; }
        public DateTime OffPeakSeasonalEndDate { get; set; }

        public long NegotId { get; set; }    // Only in ANR, but not showing in Screen

    }
}
