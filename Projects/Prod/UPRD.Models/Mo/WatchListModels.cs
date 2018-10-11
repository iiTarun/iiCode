using Nom1Done.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using UPRD.Model.Enums;

namespace UPRD.Model
{

    public class DataTypeGrouping
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class LogicalOperator
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Name { get; set; }
        [ForeignKey("DataType")]
        public int DataTypeId { get; set; }
        public virtual DataTypeGrouping DataType { get; set; }
    }

    public class MasterColumn
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public UprdDataSet DataSetId { get; set; }
        [ForeignKey("DataType")]
        public int DataTypeId { get; set; }
        public virtual DataTypeGrouping DataType { get; set; }
    }
    public class Watchlist
    {
        
        public int Id { get; set; }     
        public string Name { get; set; }
        public UprdDataSet DataSetId { get; set; }    
        
      
        public string UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? ExecutionDateTime { get; set; }

        public string Email { get; set; }

        public string MoreDetailURLinAlert { get; set; }

    }

    public class WatchlistRule
    {
        public int Id { get; set; }
     //   [ForeignKey("MasterColumn")]
        public int ColumnId { get; set; }
      //  [ForeignKey("LogicalOperator")]
        public int OperatorId { get; set; }
        public string RuleValue { get; set; }
        [ForeignKey("Watchlist")]
        public int WatchlistId { get; set; } 
       // public virtual MasterColumn MasterColumn { get; set; }
       // public virtual LogicalOperator LogicalOperator { get; set; }
        public virtual Watchlist Watchlist { get; set; }
        public string PipelineDuns { get; set; }
        public string LocationIdentifier { get; set; }
        public bool AlertSent { get; set; } = false;
        public WatchlistAlertFrequency AlertFrequency { get; set; }
        public bool IsCriticalNotice { get; set; } = false;    //Use only for SWNT Dataset
        public string UpperRuleValue { get; set; }
    }


}
