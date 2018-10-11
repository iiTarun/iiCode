namespace Nom1Done.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataTypesChangesofUPRD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OACYPerTransactions", "PostingDateTime", c => c.DateTime());
            AddColumn("dbo.OACYPerTransactions", "EffectiveGasDayTime", c => c.DateTime());
            AddColumn("dbo.OACYPerTransactions", "AvailablePercentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PipelineEDISettings", "StartDate", c => c.DateTime());
            AddColumn("dbo.PipelineEDISettings", "EndDate", c => c.DateTime());
            AddColumn("dbo.PipelineEDISettings", "SendManually", c => c.Boolean(nullable: false));
            AddColumn("dbo.PipelineEDISettings", "ForOacy", c => c.Boolean(nullable: false));
            AddColumn("dbo.PipelineEDISettings", "ForUnsc", c => c.Boolean(nullable: false));
            AddColumn("dbo.PipelineEDISettings", "ForSwnt", c => c.Boolean(nullable: false));
            AddColumn("dbo.UnscPerTransactions", "PostingDateTime", c => c.DateTime());
            AddColumn("dbo.UnscPerTransactions", "EffectiveGasDayTime", c => c.DateTime());
            AddColumn("dbo.UnscPerTransactions", "AvailablePercentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.OACYPerTransactions", "DesignCapacity", c => c.Long(nullable: false));
            AlterColumn("dbo.OACYPerTransactions", "OperatingCapacity", c => c.Long(nullable: false));
            AlterColumn("dbo.OACYPerTransactions", "TotalScheduleQty", c => c.Long(nullable: false));
            AlterColumn("dbo.OACYPerTransactions", "OperationallyAvailableQty", c => c.Long(nullable: false));
            AlterColumn("dbo.UnscPerTransactions", "TotalDesignCapacity", c => c.Long(nullable: false));
            AlterColumn("dbo.UnscPerTransactions", "UnsubscribeCapacity", c => c.Long(nullable: false));
            DropColumn("dbo.OACYPerTransactions", "PostingDate");
            DropColumn("dbo.OACYPerTransactions", "PostingTime");
            DropColumn("dbo.OACYPerTransactions", "EffectiveGasDay");
            DropColumn("dbo.OACYPerTransactions", "EffectiveTime");
            DropColumn("dbo.UnscPerTransactions", "PostingDate");
            DropColumn("dbo.UnscPerTransactions", "PostingTime");
            DropColumn("dbo.UnscPerTransactions", "EffectiveGasDay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UnscPerTransactions", "EffectiveGasDay", c => c.DateTime());
            AddColumn("dbo.UnscPerTransactions", "PostingTime", c => c.String());
            AddColumn("dbo.UnscPerTransactions", "PostingDate", c => c.DateTime());
            AddColumn("dbo.OACYPerTransactions", "EffectiveTime", c => c.String());
            AddColumn("dbo.OACYPerTransactions", "EffectiveGasDay", c => c.DateTime());
            AddColumn("dbo.OACYPerTransactions", "PostingTime", c => c.String());
            AddColumn("dbo.OACYPerTransactions", "PostingDate", c => c.DateTime());
            AlterColumn("dbo.UnscPerTransactions", "UnsubscribeCapacity", c => c.String());
            AlterColumn("dbo.UnscPerTransactions", "TotalDesignCapacity", c => c.String());
            AlterColumn("dbo.OACYPerTransactions", "OperationallyAvailableQty", c => c.String());
            AlterColumn("dbo.OACYPerTransactions", "TotalScheduleQty", c => c.String());
            AlterColumn("dbo.OACYPerTransactions", "OperatingCapacity", c => c.String());
            AlterColumn("dbo.OACYPerTransactions", "DesignCapacity", c => c.String());
            DropColumn("dbo.UnscPerTransactions", "AvailablePercentage");
            DropColumn("dbo.UnscPerTransactions", "EffectiveGasDayTime");
            DropColumn("dbo.UnscPerTransactions", "PostingDateTime");
            DropColumn("dbo.PipelineEDISettings", "ForSwnt");
            DropColumn("dbo.PipelineEDISettings", "ForUnsc");
            DropColumn("dbo.PipelineEDISettings", "ForOacy");
            DropColumn("dbo.PipelineEDISettings", "SendManually");
            DropColumn("dbo.PipelineEDISettings", "EndDate");
            DropColumn("dbo.PipelineEDISettings", "StartDate");
            DropColumn("dbo.OACYPerTransactions", "AvailablePercentage");
            DropColumn("dbo.OACYPerTransactions", "EffectiveGasDayTime");
            DropColumn("dbo.OACYPerTransactions", "PostingDateTime");
        }
    }
}
