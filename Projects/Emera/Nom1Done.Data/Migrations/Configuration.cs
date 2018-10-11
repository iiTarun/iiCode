namespace Nom1Done.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Nom1Done.Data.SeedData;
    using Nom1Done.Model;
    using Nom1Done.Model.Models;
    using NPOI.HSSF.UserModel;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Nom1Done.Data.NomEntities>
    {
        // private readonly bool _pendingMigrations;
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            //AutomaticMigrationDataLossAllowed = false;
            ContextKey = "Nom1Done.Data.NomEntities";
            // Seed(new NomEntities());
        }

        protected override void Seed(Nom1Done.Data.NomEntities context)
        {
            // var Path = SeedPathHelper.MapPath("~/SeedFiles/NewContracts.xls");
            //bool demoTest = false;

            //GetDatSetConfigSettings().ForEach(c => context.DataSetConfigurationSettings.AddOrUpdate(g => g.DataSet, c));
            
            //#region Seed Data for WatchList
            //List<DataTypeGrouping> dataTypeGrouping = new List<DataTypeGrouping>()
            //{
            //    new DataTypeGrouping{Name = "string" },
            //    new DataTypeGrouping{Name = "long" },
            //    new DataTypeGrouping{Name = "DateTime" },
            //    new DataTypeGrouping{Name = "decimal" }
            //};
            //dataTypeGrouping.ForEach(c => context.DataTypeGroupings.AddOrUpdate(g => g.Name, c));

            //List<LogicalOperator> logicalOperators = new List<LogicalOperator>()
            //{
            //    new LogicalOperator{Name = "Contains",Title="Contains", DataTypeId=1},
            //    new LogicalOperator{Name = "==",Title="Equal",DataTypeId=1},
            //    new LogicalOperator{Name = "<",Title="Less",DataTypeId=2},
            //    new LogicalOperator{Name = "<=",Title="LessOrEqual",DataTypeId=2},
            //    new LogicalOperator{Name = "==",Title="Equal",DataTypeId=2},
            //    new LogicalOperator{Name = ">=",Title="GreaterOrEqual",DataTypeId=2},
            //    new LogicalOperator{Name = ">",Title="Greater",DataTypeId=2},
            //     new LogicalOperator{Name = "<",Title="Less",DataTypeId=3},
            //    new LogicalOperator{Name = "<=",Title="LessOrEqual",DataTypeId=3},
            //    new LogicalOperator{Name = "==",Title="Equal",DataTypeId=3},
            //    new LogicalOperator{Name = "!=",Title="NotEqual",DataTypeId=3},
            //    new LogicalOperator{Name = ">=",Title="GreaterOrEqual",DataTypeId=3},
            //    new LogicalOperator{Name = ">",Title="Greater",DataTypeId=3},
            //    new LogicalOperator{Name = "<",Title="Less",DataTypeId=4},
            //    new LogicalOperator{Name = "<=",Title="LessOrEqual",DataTypeId=4},
            //    new LogicalOperator{Name = "==",Title="Equal",DataTypeId=4},
            //    new LogicalOperator{Name = ">=",Title="GreaterOrEqual",DataTypeId=4},
            //    new LogicalOperator{Name = ">",Title="Greater",DataTypeId=4}
            //};
            //logicalOperators.ForEach(c => context.LogicalOperators.AddOrUpdate(g => new { g.Name, g.DataTypeId }));

            //List<MasterColumn> masterColumns = new List<MasterColumn>()
            //{
            //    new MasterColumn{Name = "LocName",DataSetId = Enums.EnercrossDataSets.OACY,Title = "LocName",DataTypeId = 1},
            //    new MasterColumn{Name = "Loc",DataSetId = Enums.EnercrossDataSets.OACY,Title = "LocPropCode",DataTypeId = 1},
            //    //new MasterColumn{Name = "DesignCapacity",DataSetId = Enums.EnercrossDataSets.OACY,Title = "DesignCapacity",DataTypeId = 2},
            //    new MasterColumn{Name = "EffectiveGasDayTime",DataSetId = Enums.EnercrossDataSets.OACY,Title = "Effective Date",DataTypeId = 3},
            //    new MasterColumn{Name = "PostingDateTime",DataSetId = Enums.EnercrossDataSets.OACY,Title = "Posting Date",DataTypeId = 3},
            //    new MasterColumn{Name = "AvailablePercentage",DataSetId = Enums.EnercrossDataSets.OACY,Title = "%available",DataTypeId = 4},


            //    new MasterColumn{Name = "Loc",DataSetId = Enums.EnercrossDataSets.UNSC,Title = "LocPropCode",DataTypeId = 1},
            //    new MasterColumn{Name = "LocName",DataSetId = Enums.EnercrossDataSets.UNSC,Title = "LocName",DataTypeId = 1},
            //    new MasterColumn{Name = "UnsubscribeCapacity",DataSetId = Enums.EnercrossDataSets.UNSC,Title = "UnsubscribeCapacity",DataTypeId = 2},
            //    new MasterColumn{Name = "EffectiveGasDayTime",DataSetId = Enums.EnercrossDataSets.UNSC,Title = "Effective Gas Date",DataTypeId = 3},
            //    new MasterColumn{Name = "PostingDateTime",DataSetId = Enums.EnercrossDataSets.UNSC,Title = "Posting Date",DataTypeId = 3},


            //    new MasterColumn{Name = "Subject",DataSetId = Enums.EnercrossDataSets.SWNT,Title = "Subject",DataTypeId = 1},
            //    new MasterColumn{Name = "Message",DataSetId = Enums.EnercrossDataSets.SWNT,Title = "Description",DataTypeId = 1},
            //    new MasterColumn{Name = "NoticeEffectiveDateTime",DataSetId = Enums.EnercrossDataSets.SWNT,Title = "Effective Start Date",DataTypeId = 3},
            //    new MasterColumn{Name = "PostingDateTime",DataSetId = Enums.EnercrossDataSets.SWNT,Title = "Posting Date",DataTypeId = 3}

            //};
            //masterColumns.ForEach(c => context.MasterColumns.AddOrUpdate(g => new { g.DataSetId, g.Name }));

            //#endregion

            //#region Routes

            //GetRoutes().ForEach(c => context.Routes.AddOrUpdate(g => g.DirectionId, c));

            //#endregion


            //#region Add Seed data for PipelineEdiSetting
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "006931794") && (u.ShipperCompDuns == "078711334") && (u.DatasetId == 11))))//NGPL
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "006931794",
            //        DatasetId = 11,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "U",
            //        ISA12_Segment = "00304",
            //        ISA16_Segment = ">",
            //        GS01_Segment = "IB",
            //        GS03_Segment = "006931794UPRD",
            //        GS07_Segment = "X",
            //        GS08_Segment = "003040",
            //        ST01_Segment = "846",
            //        ShipperCompDuns = "078711334",
            //        GS02_Segment = "078711334"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "006900518") && (u.ShipperCompDuns == "078711334") && (u.DatasetId == 11))))//SNG
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "006900518",
            //        DatasetId = 11,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "U",
            //        ISA12_Segment = "00304",
            //        ISA16_Segment = "^",
            //        GS01_Segment = "IB",
            //        GS03_Segment = "006900518UPRD",
            //        GS07_Segment = "X",
            //        GS08_Segment = "003040",
            //        ST01_Segment = "846",
            //        ShipperCompDuns = "078711334",
            //        GS02_Segment = "078711334"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "006958581") && (u.ShipperCompDuns == "078711334") && (u.DatasetId == 11))))//ANR
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "006958581",
            //        DatasetId = 11,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "U",
            //        ISA12_Segment = "00304",
            //        ISA16_Segment = ":",
            //        GS01_Segment = "IB",
            //        GS03_Segment = "078711334UPR",
            //        GS07_Segment = "X",
            //        GS08_Segment = "003040",
            //        ST01_Segment = "846",
            //        ShipperCompDuns = "078711334",
            //        GS02_Segment = "078711334"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "007932908") && (u.ShipperCompDuns == "078711334") && (u.DatasetId == 11))))//Tetco
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "007932908",
            //        DatasetId = 11,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "U",
            //        ISA12_Segment = "00304",
            //        ISA16_Segment = ">",
            //        GS01_Segment = "IB",
            //        GS03_Segment = "846RD",
            //        GS07_Segment = "X",
            //        GS08_Segment = "003040",
            //        ST01_Segment = "846",
            //        ShipperCompDuns = "078711334",
            //        GS02_Segment = "078711334"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "001939164") && (u.ShipperCompDuns == "078711334") && (u.DatasetId == 11))))//TGP
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "001939164",
            //        DatasetId = 11,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "U",
            //        ISA12_Segment = "00304",
            //        ISA16_Segment = "^",
            //        GS01_Segment = "IB",
            //        GS03_Segment = "001939164UPRD",
            //        GS07_Segment = "X",
            //        GS08_Segment = "003040",
            //        ST01_Segment = "846",
            //        ShipperCompDuns = "078711334",
            //        GS02_Segment = "078711334"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            ////Nomination EDI settings for Enercross (078711334)
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "006931794") && (u.ShipperCompDuns == "078711334") && (u.DatasetId == 35))))//NGPL
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "006931794",
            //        DatasetId = 35,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "+",
            //        ISA12_Segment = "00602",
            //        ISA16_Segment = ">",
            //        GS01_Segment = "CU",
            //        GS03_Segment = "006931794NMST",
            //        GS07_Segment = "X",
            //        GS08_Segment = "006020",
            //        ST01_Segment = "873",
            //        ShipperCompDuns = "078711334",
            //        GS02_Segment = "078711334NMST"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "006900518") && (u.ShipperCompDuns == "078711334") && (u.DatasetId == 35))))//SNG
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "006900518",
            //        DatasetId = 35,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "+",
            //        ISA12_Segment = "00602",
            //        ISA16_Segment = "^",
            //        GS01_Segment = "CU",
            //        GS03_Segment = "006900518NMST",
            //        GS07_Segment = "X",
            //        GS08_Segment = "006020",
            //        ST01_Segment = "873",
            //        ShipperCompDuns = "078711334",
            //        GS02_Segment = "078711334NMST"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "006958581") && (u.ShipperCompDuns == "078711334") && (u.DatasetId == 29))))//ANR
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "006958581",
            //        DatasetId = 29,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "+",
            //        ISA12_Segment = "00602",
            //        ISA16_Segment = ":",
            //        GS01_Segment = "CU",
            //        GS03_Segment = "078711334NMS",
            //        GS07_Segment = "T",
            //        GS08_Segment = "006020",
            //        ST01_Segment = "873",
            //        ShipperCompDuns = "078711334",
            //        GS02_Segment = "078711334"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "007932908") && (u.ShipperCompDuns == "078711334") && (u.DatasetId == 29))))//Tetco
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "007932908",
            //        DatasetId = 29,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "+",
            //        ISA12_Segment = "00602",
            //        ISA16_Segment = ">",
            //        GS01_Segment = "CU",
            //        GS03_Segment = "873NM",
            //        GS07_Segment = "X",
            //        GS08_Segment = "006020",
            //        ST01_Segment = "873",
            //        ShipperCompDuns = "078711334",
            //        GS02_Segment = "078711334"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "001939164") && (u.ShipperCompDuns == "078711334") && (u.DatasetId == 35))))//TGP
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "001939164",
            //        DatasetId = 35,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "+",
            //        ISA12_Segment = "00602",
            //        ISA16_Segment = "^",
            //        GS01_Segment = "CU",
            //        GS03_Segment = "001939164NMST",
            //        GS07_Segment = "X",
            //        GS08_Segment = "006020",
            //        ST01_Segment = "873",
            //        ShipperCompDuns = "078711334",
            //        GS02_Segment = "078711334NMST"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            ////Nomination EDI settings for Emera (101069263)
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "006951446") && (u.ShipperCompDuns == "101069263") && (u.DatasetId == 29))))
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "006951446",
            //        DatasetId = 29,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "+",
            //        ISA12_Segment = "00602",
            //        ISA16_Segment = ">",
            //        GS01_Segment = "CU",
            //        GS03_Segment = "006951446NMST",
            //        GS07_Segment = "X",
            //        GS08_Segment = "006020",
            //        ST01_Segment = "873",
            //        ShipperCompDuns = "101069263",
            //        GS02_Segment = "101069263NMST",
            //        ISA06_Segment= "101069263",
            //        ISA08_segment= "006951446"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "116025180") && (u.ShipperCompDuns == "101069263") && (u.DatasetId == 29))))
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "116025180",
            //        DatasetId = 29,
            //        SegmentSeperator = "",
            //        DataSeparator = "~",
            //        ISA11_Segment = "+",
            //        ISA12_Segment = "00602",
            //        ISA16_Segment = "^",
            //        GS01_Segment = "CU",
            //        GS03_Segment = "116025180NMST",
            //        GS07_Segment = "X",
            //        GS08_Segment = "006020",
            //        ST01_Segment = "873",
            //        ShipperCompDuns = "101069263",
            //        GS02_Segment = "101069263NMST",
            //        ISA06_Segment = "101069263",
            //        ISA08_segment = "116025180"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "045256641") && (u.ShipperCompDuns == "101069263") && (u.DatasetId == 29))))
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "045256641",
            //        DatasetId = 29,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "+",
            //        ISA12_Segment = "00602",
            //        ISA16_Segment = ">",
            //        GS01_Segment = "CU",
            //        GS03_Segment = "045256641NMST",
            //        GS07_Segment = "X",
            //        GS08_Segment = "006020",
            //        ST01_Segment = "873",
            //        ShipperCompDuns = "101069263",
            //        GS02_Segment = "101069263NMST",
            //        ISA06_Segment = "101069263",
            //        ISA08_segment = "045256641"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //if (!(context.PipelineEDISetting.Any(u => (u.PipeDuns == "001939164") && (u.ShipperCompDuns == "101069263") && (u.DatasetId == 35))))
            //{
            //    var pipelineEdiSetting = new PipelineEDISetting
            //    {
            //        PipeDuns = "001939164",
            //        DatasetId = 35,
            //        SegmentSeperator = "~",
            //        DataSeparator = "*",
            //        ISA11_Segment = "+",
            //        ISA12_Segment = "00602",
            //        ISA16_Segment = "^",
            //        GS01_Segment = "CU",
            //        GS03_Segment = "001939164NMST",
            //        GS07_Segment = "X",
            //        GS08_Segment = "006020",
            //        ST01_Segment = "873",
            //        ShipperCompDuns = "101069263",
            //        GS02_Segment = "101069263NMST",
            //        ISA06_Segment = "101069263",
            //        ISA08_segment = "001939164"
            //    };
            //    context.PipelineEDISetting.AddOrUpdate(pipelineEdiSetting);
            //}
            //#endregion

            //#region Add Emera Shipper if not exist and Emera users
            //if (!(context.ShipperCompany.Any(u => u.DUNS == "101069263")))
            //{
            //    var shipperCompany = new ShipperCompany
            //    {
            //        Name = "Emera",
            //        DUNS = "101069263",
            //        IsActive = true,
            //        CreatedBy = "",
            //        CreatedDate = DateTime.Now,
            //        ModifiedBy = "",
            //        ModifiedDate = DateTime.Now,
            //        SubscriptionID = 0,
            //        ShipperAddress = ""
            //    };
            //    context.ShipperCompany.AddOrUpdate(a => a.DUNS, shipperCompany);
            //    if (!(context.Users.Any(u => u.UserName == "Kathryn.Richardson@emeraenergy.com")))
            //    {
            //        var userStore = new UserStore<ApplicationUser>(context);
            //        var userManager = new UserManager<ApplicationUser>(userStore);
            //        var userToInsert = new ApplicationUser
            //        {
            //            UserName = "Kathryn.Richardson@emeraenergy.com",
            //            PhoneNumber = "",
            //            EmailConfirmed = true,
            //            Email = "Kathryn.Richardson@emeraenergy.com",
            //            PhoneNumberConfirmed = true
            //        };

            //        userManager.Create(userToInsert, "Monday01__");
            //        userManager.AddToRole(userToInsert.Id, "Admin");

            //        var shipper = new Shipper
            //        {
            //            UserId = userToInsert.Id,
            //            FirstName = "Kathryn",
            //            LastName = "Richardson",
            //            ShipperCompanyID = shipperCompany.ID,
            //            IsActive = true,
            //            CreatedBy = "",
            //            CreatedDate = DateTime.Now,
            //            ModifiedBy = "",
            //            ModifiedDate = DateTime.Now
            //        };
            //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


            //    }
            //    if (!(context.Users.Any(u => u.UserName == "Scott.Durling@emeraenergy.com")))
            //    {
            //        var userStore = new UserStore<ApplicationUser>(context);
            //        var userManager = new UserManager<ApplicationUser>(userStore);
            //        var userToInsert = new ApplicationUser
            //        {
            //            UserName = "Scott.Durling@emeraenergy.com",
            //            PhoneNumber = "",
            //            EmailConfirmed = true,
            //            Email = "Scott.Durling@emeraenergy.com",
            //            PhoneNumberConfirmed = true
            //        };

            //        userManager.Create(userToInsert, "Monday02__");
            //        userManager.AddToRole(userToInsert.Id, "Admin");

            //        var shipper = new Shipper
            //        {
            //            UserId = userToInsert.Id,
            //            FirstName = "Scott",
            //            LastName = "Durling",
            //            ShipperCompanyID = shipperCompany.ID,
            //            IsActive = true,
            //            CreatedBy = "",
            //            CreatedDate = DateTime.Now,
            //            ModifiedBy = "",
            //            ModifiedDate = DateTime.Now
            //        };
            //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


            //    }
            //    if (!(context.Users.Any(u => u.UserName == "Andy.Fraser@emeraenergy.com")))
            //    {
            //        var userStore = new UserStore<ApplicationUser>(context);
            //        var userManager = new UserManager<ApplicationUser>(userStore);
            //        var userToInsert = new ApplicationUser
            //        {
            //            UserName = "Andy.Fraser@emeraenergy.com",
            //            PhoneNumber = "",
            //            EmailConfirmed = true,
            //            Email = "Andy.Fraser@emeraenergy.com",
            //            PhoneNumberConfirmed = true
            //        };

            //        userManager.Create(userToInsert, "Monday03__");
            //        userManager.AddToRole(userToInsert.Id, "Admin");

            //        var shipper = new Shipper
            //        {
            //            UserId = userToInsert.Id,
            //            FirstName = "Andy",
            //            LastName = "Fraser",
            //            ShipperCompanyID = shipperCompany.ID,
            //            IsActive = true,
            //            CreatedBy = "",
            //            CreatedDate = DateTime.Now,
            //            ModifiedBy = "",
            //            ModifiedDate = DateTime.Now
            //        };
            //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


            //    }
            //    if (!(context.Users.Any(u => u.UserName == "Matthew.North@emeraenergy.com")))
            //    {
            //        var userStore = new UserStore<ApplicationUser>(context);
            //        var userManager = new UserManager<ApplicationUser>(userStore);
            //        var userToInsert = new ApplicationUser
            //        {
            //            UserName = "Matthew.North@emeraenergy.com",
            //            PhoneNumber = "",
            //            EmailConfirmed = true,
            //            Email = "Matthew.North@emeraenergy.com",
            //            PhoneNumberConfirmed = true
            //        };

            //        userManager.Create(userToInsert, "Monday04__");
            //        userManager.AddToRole(userToInsert.Id, "Admin");

            //        var shipper = new Shipper
            //        {
            //            UserId = userToInsert.Id,
            //            FirstName = "Matthew",
            //            LastName = "North",
            //            ShipperCompanyID = shipperCompany.ID,
            //            IsActive = true,
            //            CreatedBy = "",
            //            CreatedDate = DateTime.Now,
            //            ModifiedBy = "",
            //            ModifiedDate = DateTime.Now
            //        };
            //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


            //    }
            //    if (!(context.Users.Any(u => u.UserName == "James.Harvey@emeraenergy.com")))
            //    {
            //        var userStore = new UserStore<ApplicationUser>(context);
            //        var userManager = new UserManager<ApplicationUser>(userStore);
            //        var userToInsert = new ApplicationUser
            //        {
            //            UserName = "James.Harvey@emeraenergy.com",
            //            PhoneNumber = "",
            //            EmailConfirmed = true,
            //            Email = "James.Harvey@emeraenergy.com",
            //            PhoneNumberConfirmed = true
            //        };

            //        userManager.Create(userToInsert, "Monday05__");
            //        userManager.AddToRole(userToInsert.Id, "Admin");

            //        var shipper = new Shipper
            //        {
            //            UserId = userToInsert.Id,
            //            FirstName = "James",
            //            LastName = "Harvey",
            //            ShipperCompanyID = shipperCompany.ID,
            //            IsActive = true,
            //            CreatedBy = "",
            //            CreatedDate = DateTime.Now,
            //            ModifiedBy = "",
            //            ModifiedDate = DateTime.Now
            //        };
            //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


            //    }
            //    if (!(context.Users.Any(u => u.UserName == "Robert.Foster@emeraenergy.com")))
            //    {
            //        var userStore = new UserStore<ApplicationUser>(context);
            //        var userManager = new UserManager<ApplicationUser>(userStore);
            //        var userToInsert = new ApplicationUser
            //        {
            //            UserName = "Robert.Foster@emeraenergy.com",
            //            PhoneNumber = "",
            //            EmailConfirmed = true,
            //            Email = "Robert.Foster@emeraenergy.com",
            //            PhoneNumberConfirmed = true
            //        };

            //        userManager.Create(userToInsert, "Monday06__");
            //        userManager.AddToRole(userToInsert.Id, "Admin");

            //        var shipper = new Shipper
            //        {
            //            UserId = userToInsert.Id,
            //            FirstName = "Robert",
            //            LastName = "Foster",
            //            ShipperCompanyID = shipperCompany.ID,
            //            IsActive = true,
            //            CreatedBy = "",
            //            CreatedDate = DateTime.Now,
            //            ModifiedBy = "",
            //            ModifiedDate = DateTime.Now
            //        };
            //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


            //    }
            //    if (!(context.Users.Any(u => u.UserName == "Shawn.Neily@emeraenergy.com")))
            //    {
            //        var userStore = new UserStore<ApplicationUser>(context);
            //        var userManager = new UserManager<ApplicationUser>(userStore);
            //        var userToInsert = new ApplicationUser
            //        {
            //            UserName = "Shawn.Neily@emeraenergy.com",
            //            PhoneNumber = "",
            //            EmailConfirmed = true,
            //            Email = "Shawn.Neily@emeraenergy.com",
            //            PhoneNumberConfirmed = true
            //        };

            //        userManager.Create(userToInsert, "Monday07__");
            //        userManager.AddToRole(userToInsert.Id, "Admin");

            //        var shipper = new Shipper
            //        {
            //            UserId = userToInsert.Id,
            //            FirstName = "Shawn",
            //            LastName = "Neily",
            //            ShipperCompanyID = shipperCompany.ID,
            //            IsActive = true,
            //            CreatedBy = "",
            //            CreatedDate = DateTime.Now,
            //            ModifiedBy = "",
            //            ModifiedDate = DateTime.Now
            //        };
            //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
            //    }
            //}
            //#endregion

            //#region Emera Pipeline Mapping Seed
            //if (context.ShipperCompany.Any(u => u.DUNS == "101069263"))
            //{
            //    var shipperID = context.ShipperCompany.Where(a => a.DUNS == "101069263").Select(a => a.ID).FirstOrDefault();
            //    if (!(context.ShipperCompany_Pipeline_Map.Any(c => c.CompanyID == shipperID)))
            //    {
            //        var ShipperPipeMapAlgon = new ShipperCompany_Pipeline_Map
            //        {
            //            CompanyID = shipperID,
            //            PipelineID = context.Pipeline.Where(d => d.DUNSNo == "006951446").Select(a => a.ID).FirstOrDefault(),
            //            IsActive = true,
            //            CreatedBy = "",
            //            CreatedDate = DateTime.Now,
            //            LastModifiedBy = "",
            //            LastModifiedDate = DateTime.Now
            //        };

            //        context.ShipperCompany_Pipeline_Map.Add(ShipperPipeMapAlgon);

            //        var ShipperPipeMapDominion = new ShipperCompany_Pipeline_Map
            //        {
            //            CompanyID = shipperID,
            //            PipelineID = context.Pipeline.Where(d => d.DUNSNo == "116025180").Select(a => a.ID).FirstOrDefault(),
            //            IsActive = true,
            //            CreatedBy = "",
            //            CreatedDate = DateTime.Now,
            //            LastModifiedBy = "",
            //            LastModifiedDate = DateTime.Now
            //        };

            //        context.ShipperCompany_Pipeline_Map.Add(ShipperPipeMapDominion);

            //        var ShipperPipeMapPanhandle = new ShipperCompany_Pipeline_Map
            //        {
            //            CompanyID = shipperID,
            //            PipelineID = context.Pipeline.Where(d => d.DUNSNo == "045256641").Select(a => a.ID).FirstOrDefault(),
            //            IsActive = true,
            //            CreatedBy = "",
            //            CreatedDate = DateTime.Now,
            //            LastModifiedBy = "",
            //            LastModifiedDate = DateTime.Now
            //        };

            //        context.ShipperCompany_Pipeline_Map.Add(ShipperPipeMapPanhandle);
            //        var ShipperPipeMapTennessee = new ShipperCompany_Pipeline_Map
            //        {
            //            CompanyID = shipperID,
            //            PipelineID = context.Pipeline.Where(d => d.DUNSNo == "001939164").Select(a => a.ID).FirstOrDefault(),
            //            IsActive = true,
            //            CreatedBy = "",
            //            CreatedDate = DateTime.Now,
            //            LastModifiedBy = "",
            //            LastModifiedDate = DateTime.Now
            //        };

            //        context.ShipperCompany_Pipeline_Map.Add(ShipperPipeMapTennessee);
            //    }
            //}
            //#endregion

            //if (demoTest)//!Path.Contains("ScrappingService") && !Path.Contains("TaskManager") && !Path.Contains("EDIEngine.Send") && !Path.Contains("ReceiveUI") && !Path.Contains("EDIEngine.Receive"))
            //{
            //    GetBidUpImdicator().ForEach(c => context.BidUpIndicators.AddOrUpdate(g => g.Code, c));
            //    GetCapacityTypeIndicator().ForEach(c => context.metadataCapacityTypeIndicator.AddOrUpdate(g => g.Code, c));
            //    GetCycle().ForEach(c => context.metadataCycle.AddOrUpdate(g => g.Code, c));
            //    GetExportDeclaration().ForEach(c => context.metadataExportDeclaration.AddOrUpdate(g => g.Code, c));
            //    GetQuantityTypeIndicator().ForEach(c => context.metadataQuantityTypeIndicator.AddOrUpdate(s => s.Code, c));
            //    GetRequestType().ForEach(c => context.metadataRequestType.AddOrUpdate(s => s.Name, c));
            //    ContractSeed.GetContract().ForEach(c => context.Contract.AddOrUpdate(s => new { s.RequestNo, s.RequestTypeID, s.PipeDuns, s.ShipperID, s.ValidUpto, s.FuelPercentage }, c));
            //    EmailTemplateSeed.GetEmailSeed().ForEach(c => context.EmailTemplates.AddOrUpdate(s => s.Name, c));
            //    FileStatusSeed.GetFileStatus().ForEach(c => context.metadataFileStatu.AddOrUpdate(s => s.Name, c));
            //    MetadataDataSetSeed.GetDataSet().ForEach(c => context.metadataDataset.AddOrUpdate(s => new { s.Code, s.CategoryID, s.Name }, c));

            //    SettingSeed.GetSettingData().ForEach(c => context.Setting.AddOrUpdate(s => new { s.ID, s.Name, s.Value, s.CreatedBy, s.CreatedDate }, c));
            //    metadataErrorCodeSeed.GetErrorCode().ForEach(c => context.metadataErrorCode.AddOrUpdate(s => new { s.Code, s.DataElement, s.Description }, c));
            //    GetSendingStatus().ForEach(c => context.SendingStages.AddOrUpdate(s => s.Name, c));
            //    GetReceivingStages().ForEach(c => context.ReceivingStages.AddOrUpdate(s => s.Name, c));


                //#region Identity Seed
                //// add roles
                //if (!context.Roles.Any(r => r.Name == "Admin"))
                //{
                //    var store = new RoleStore<IdentityRole>(context);
                //    var manager = new RoleManager<IdentityRole>(store);
                //    var role = new IdentityRole { Name = "Admin" };
                //    manager.Create(role);
                //}
                ////add users
                //if (!(context.ShipperCompany.Any(u => u.DUNS == "837565548")))
                //{
                //    var shipperCompany = new ShipperCompany
                //    {
                //        Name = "Shell",
                //        DUNS = "837565548",
                //        IsActive = true,
                //        CreatedBy = "",
                //        CreatedDate = DateTime.Now,
                //        ModifiedBy = "",
                //        ModifiedDate = DateTime.Now,
                //        SubscriptionID = 0,
                //        ShipperAddress = ""
                //    };
                //    context.ShipperCompany.AddOrUpdate(a => a.DUNS, shipperCompany);
                //    if (!(context.Users.Any(u => u.UserName == "tiffanyP.on@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser
                //        {
                //            UserName = "tiffanyP.on@sena.com",
                //            PhoneNumber = "0797697898",
                //            EmailConfirmed = true,
                //            Email = "tiffanyP.on@sena.com",
                //            PhoneNumberConfirmed = true,
                //        };
                //        userManager.Create(userToInsert, "Monday02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper
                //        {
                //            UserId = userToInsert.Id,
                //            FirstName = "Tiffany P",
                //            LastName = "On",
                //            ShipperCompanyID = shipperCompany.ID,
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            ModifiedBy = "",
                //            ModifiedDate = DateTime.Now
                //        };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "rebeccaJ.newson@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "rebeccaJ.newson@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "rebeccaJ.newson@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Tuesday02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Rebecca J", LastName = "Newson", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "terryA.mcmillin@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "terryA.mcmillin@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "terryA.mcmillin@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Wednesday02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Terry A", LastName = "Mcmillin", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "amanda.boettcher@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "amanda.boettcher@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "amanda.boettcher@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Thursday02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Amanda", LastName = "Boettcher", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "jasonJ.babin@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "jasonJ.babin@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "jasonJ.babin@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Friday02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Jason J", LastName = "Babin", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "deepthi.bollu@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "deepthi.bollu@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "deepthi.bollu@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Saturday02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Deepthi", LastName = "Bollu", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "carlyM.billington@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "carlyM.billington@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "carlyM.billington@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Sunday02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Carly M", LastName = "Billington", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "jaymeC.dagnolo@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "jaymeC.dagnolo@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "jaymeC.dagnolo@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Jan02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Jayme C", LastName = "D'Agnolo", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "brandon.johnson@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "brandon.johnson@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "brandon.johnson@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Feb02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Brandon", LastName = "Johnson", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "tamiL.covington@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "tamiL.covington@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "tamiL.covington@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Mar02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Tami L", LastName = "Covington", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "danielle.foster@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "danielle.foster@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "danielle.foster@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "April02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Danielle", LastName = "Foster", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "christen.schaffer@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "christen.schaffer@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "christen.schaffer@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "June02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Christen", LastName = "Schaffer", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "john.powell@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "john.powell@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "john.powell@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "July02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "John", LastName = "Powell", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "leslie.may@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "leslie.may@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "leslie.may@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Aug02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Leslie", LastName = "May", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //    if (!(context.Users.Any(u => u.UserName == "justin.babb@sena.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "justin.babb@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "justin.babb@sena.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Sept02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Justin", LastName = "Babb", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //}
                //if (!(context.ShipperCompany.Any(u => u.DUNS == "078711334")))
                //{
                //    var shipperCompany = new ShipperCompany
                //    {
                //        Name = "Enercross",
                //        DUNS = "078711334",
                //        IsActive = true,
                //        CreatedBy = "",
                //        CreatedDate = DateTime.Now,
                //        ModifiedBy = "",
                //        ModifiedDate = DateTime.Now,
                //        SubscriptionID = 0,
                //        ShipperAddress = ""
                //    };
                //    context.ShipperCompany.AddOrUpdate(a => a.DUNS, shipperCompany);
                //    if (!(context.Users.Any(u => u.UserName == "support@invertedi.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "support@invertedi.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "support@invertedi.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Monday02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Gagan", LastName = "Deep", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }

                //    if (!(context.Users.Any(u => u.UserName == "Jay.Singh@enercross.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser { UserName = "Jay.Singh@enercross.com", PhoneNumber = "0000000000", EmailConfirmed = true, Email = "Jay.Singh@enercross.com", PhoneNumberConfirmed = true, };
                //        userManager.Create(userToInsert, "Monday02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Gagan", LastName = "Deep", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //}
                ////add users and shipper for Emera
                //if (!(context.ShipperCompany.Any(u => u.DUNS == "101069263")))
                //{
                //    var shipperCompany = new ShipperCompany
                //    {
                //        Name = "Emera",
                //        DUNS = "101069263",
                //        IsActive = true,
                //        CreatedBy = "",
                //        CreatedDate = DateTime.Now,
                //        ModifiedBy = "",
                //        ModifiedDate = DateTime.Now,
                //        SubscriptionID = 0,
                //        ShipperAddress = ""
                //    };
                //    context.ShipperCompany.AddOrUpdate(a => a.DUNS, shipperCompany);
                //    if (!(context.Users.Any(u => u.UserName == "Kathryn.Richardson@emeraenergy.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser
                //        {
                //            UserName = "Kathryn.Richardson@emeraenergy.com",
                //            PhoneNumber = "",
                //            EmailConfirmed = true,
                //            Email = "Kathryn.Richardson@emeraenergy.com",
                //            PhoneNumberConfirmed = true
                //        };

                //        userManager.Create(userToInsert, "Monday01__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper
                //        {
                //            UserId = userToInsert.Id,
                //            FirstName = "Kathryn",
                //            LastName = "Richardson",
                //            ShipperCompanyID = shipperCompany.ID,
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            ModifiedBy = "",
                //            ModifiedDate = DateTime.Now
                //        };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


                //    }
                //    if (!(context.Users.Any(u => u.UserName == "Scott.Durling@emeraenergy.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser
                //        {
                //            UserName = "Scott.Durling@emeraenergy.com",
                //            PhoneNumber = "",
                //            EmailConfirmed = true,
                //            Email = "Scott.Durling@emeraenergy.com",
                //            PhoneNumberConfirmed = true
                //        };

                //        userManager.Create(userToInsert, "Monday02__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper
                //        {
                //            UserId = userToInsert.Id,
                //            FirstName = "Scott",
                //            LastName = "Durling",
                //            ShipperCompanyID = shipperCompany.ID,
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            ModifiedBy = "",
                //            ModifiedDate = DateTime.Now
                //        };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


                //    }
                //    if (!(context.Users.Any(u => u.UserName == "Andy.Fraser@emeraenergy.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser
                //        {
                //            UserName = "Andy.Fraser@emeraenergy.com",
                //            PhoneNumber = "",
                //            EmailConfirmed = true,
                //            Email = "Andy.Fraser@emeraenergy.com",
                //            PhoneNumberConfirmed = true
                //        };

                //        userManager.Create(userToInsert, "Monday03__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper
                //        {
                //            UserId = userToInsert.Id,
                //            FirstName = "Andy",
                //            LastName = "Fraser",
                //            ShipperCompanyID = shipperCompany.ID,
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            ModifiedBy = "",
                //            ModifiedDate = DateTime.Now
                //        };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


                //    }
                //    if (!(context.Users.Any(u => u.UserName == "Matthew.North@emeraenergy.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser
                //        {
                //            UserName = "Matthew.North@emeraenergy.com",
                //            PhoneNumber = "",
                //            EmailConfirmed = true,
                //            Email = "Matthew.North@emeraenergy.com",
                //            PhoneNumberConfirmed = true
                //        };

                //        userManager.Create(userToInsert, "Monday04__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper
                //        {
                //            UserId = userToInsert.Id,
                //            FirstName = "Matthew",
                //            LastName = "North",
                //            ShipperCompanyID = shipperCompany.ID,
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            ModifiedBy = "",
                //            ModifiedDate = DateTime.Now
                //        };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


                //    }
                //    if (!(context.Users.Any(u => u.UserName == "James.Harvey@emeraenergy.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser
                //        {
                //            UserName = "James.Harvey@emeraenergy.com",
                //            PhoneNumber = "",
                //            EmailConfirmed = true,
                //            Email = "James.Harvey@emeraenergy.com",
                //            PhoneNumberConfirmed = true
                //        };

                //        userManager.Create(userToInsert, "Monday05__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper
                //        {
                //            UserId = userToInsert.Id,
                //            FirstName = "James",
                //            LastName = "Harvey",
                //            ShipperCompanyID = shipperCompany.ID,
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            ModifiedBy = "",
                //            ModifiedDate = DateTime.Now
                //        };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


                //    }
                //    if (!(context.Users.Any(u => u.UserName == "Robert.Foster@emeraenergy.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser
                //        {
                //            UserName = "Robert.Foster@emeraenergy.com",
                //            PhoneNumber = "",
                //            EmailConfirmed = true,
                //            Email = "Robert.Foster@emeraenergy.com",
                //            PhoneNumberConfirmed = true
                //        };

                //        userManager.Create(userToInsert, "Monday06__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper
                //        {
                //            UserId = userToInsert.Id,
                //            FirstName = "Robert",
                //            LastName = "Foster",
                //            ShipperCompanyID = shipperCompany.ID,
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            ModifiedBy = "",
                //            ModifiedDate = DateTime.Now
                //        };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);


                //    }
                //    if (!(context.Users.Any(u => u.UserName == "Shawn.Neily@emeraenergy.com")))
                //    {
                //        var userStore = new UserStore<ApplicationUser>(context);
                //        var userManager = new UserManager<ApplicationUser>(userStore);
                //        var userToInsert = new ApplicationUser
                //        {
                //            UserName = "Shawn.Neily@emeraenergy.com",
                //            PhoneNumber = "",
                //            EmailConfirmed = true,
                //            Email = "Shawn.Neily@emeraenergy.com",
                //            PhoneNumberConfirmed = true
                //        };

                //        userManager.Create(userToInsert, "Monday07__");
                //        userManager.AddToRole(userToInsert.Id, "Admin");

                //        var shipper = new Shipper
                //        {
                //            UserId = userToInsert.Id,
                //            FirstName = "Shawn",
                //            LastName = "Neily",
                //            ShipperCompanyID = shipperCompany.ID,
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            ModifiedBy = "",
                //            ModifiedDate = DateTime.Now
                //        };
                //        context.Shipper.AddOrUpdate(a => new { a.UserId, a.FirstName }, shipper);
                //    }
                //}
                //#endregion

                //#region Emera Pipeline Mapping Seed
                //if (context.ShipperCompany.Any(u => u.DUNS == "101069263"))
                //{
                //    var shipperID = context.ShipperCompany.Where(a => a.DUNS == "101069263").Select(a => a.ID).FirstOrDefault();
                //    if (!(context.ShipperCompany_Pipeline_Map.Any(c => c.CompanyID == shipperID)))
                //    {
                //        var ShipperPipeMapAlgon = new ShipperCompany_Pipeline_Map
                //        {
                //            CompanyID = shipperID,
                //            PipelineID = context.Pipeline.Where(d => d.DUNSNo == "006951446").Select(a => a.ID).FirstOrDefault(),
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            LastModifiedBy = "",
                //            LastModifiedDate = DateTime.Now
                //        };

                //        context.ShipperCompany_Pipeline_Map.Add(ShipperPipeMapAlgon);

                //        var ShipperPipeMapDominion = new ShipperCompany_Pipeline_Map
                //        {
                //            CompanyID = shipperID,
                //            PipelineID = context.Pipeline.Where(d => d.DUNSNo == "116025180").Select(a => a.ID).FirstOrDefault(),
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            LastModifiedBy = "",
                //            LastModifiedDate = DateTime.Now
                //        };

                //        context.ShipperCompany_Pipeline_Map.Add(ShipperPipeMapDominion);

                //        var ShipperPipeMapPanhandle = new ShipperCompany_Pipeline_Map
                //        {
                //            CompanyID = shipperID,
                //            PipelineID = context.Pipeline.Where(d => d.DUNSNo == "045256641").Select(a => a.ID).FirstOrDefault(),
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            LastModifiedBy = "",
                //            LastModifiedDate = DateTime.Now
                //        };

                //        context.ShipperCompany_Pipeline_Map.Add(ShipperPipeMapPanhandle);
                //        var ShipperPipeMapTennessee = new ShipperCompany_Pipeline_Map
                //        {
                //            CompanyID = shipperID,
                //            PipelineID = context.Pipeline.Where(d => d.DUNSNo == "001939164").Select(a => a.ID).FirstOrDefault(),
                //            IsActive = true,
                //            CreatedBy = "",
                //            CreatedDate = DateTime.Now,
                //            LastModifiedBy = "",
                //            LastModifiedDate = DateTime.Now
                //        };

                //        context.ShipperCompany_Pipeline_Map.Add(ShipperPipeMapTennessee);
                //    }
                //}
                //#endregion


                //#region Add Pipeline and locations
                // LocationsSeed.GetLocations().ForEach(c => context.Location.Add(c));
                //  PipelineEncKeyInfoSeed.GetPipelineEncKeyInfo().ForEach(c => context.metadataPipelineEncKeyInfo.AddOrUpdate(s=> new { s.KeyName,s.PipelineId}, c));
                //try
                //{
                //    var pipelineList = PipelineSeed.GetPipelines();
                //    var transactionTypeList = TransactionTypesSeed.GetTransactionTypes();
                //    var tspList = TransportationServiceProviderSeed.GetTransportationServiceProvider();
                //    var pipelineTranTypeMapList = PipelineTransactionTypeMapSeed.GetPipelineTransactionTypeMap();
                //    var tpwList = TradingPartnerWorksheetSeed.GetTradingPartnerWorksheet();
                //    var counterPartyList = CounterPartySeed.GetCounterParty();
                //    var pipeEncKeyInfoList = PipelineEncKeyInfoSeed.GetPipelineEncKeyInfo();

                //    foreach (var tranType in transactionTypeList)
                //    {
                //        var tt = new metadataTransactionType
                //        {
                //            Identifier = tranType.Identifier,
                //            IsActive = tranType.IsActive,
                //            Name = tranType.Name,
                //            SequenceNo = tranType.SequenceNo
                //        };
                //        context.metadataTransactionType.AddOrUpdate(a=>new { a.Name,a.Identifier},tt);
                //        context.SaveChanges();
                //        var tranTypeMapOnPipeTTId = pipelineTranTypeMapList.Where(a => a.TransactionTypeID == tranType.ID).ToList();
                //        foreach (var ttm in tranTypeMapOnPipeTTId)
                //        {
                //            var map = new Pipeline_TransactionType_Map
                //            {
                //                IsActive = ttm.IsActive,
                //                LastModifiedBy = ttm.LastModifiedBy,
                //                LastModifiedDate = ttm.LastModifiedDate,
                //                PathType = ttm.PathType,
                //                PipeDuns = "",//pipe.DUNSNo,
                //                PipelineID = ttm.PipelineID,
                //                TransactionTypeID = tt.ID,
                //                CreatedBy = "",
                //                CreatedDate = DateTime.Now
                //            };
                //            context.Pipeline_TransactionType_Map.AddOrUpdate(a=>new {a.IsActive,a.PathType,a.PipelineID,a.LastModifiedBy,a.LastModifiedDate },map);
                //            context.SaveChanges();
                //        }
                //    }
                //    foreach (var tsp in tspList)
                //    {
                //        var tspObj = new TransportationServiceProvider
                //        {
                //            CreatedBy = tsp.CreatedBy,
                //            CreatedDate = tsp.CreatedDate,
                //            DUNSNo = tsp.DUNSNo,
                //            IsActive = tsp.IsActive,
                //            ModifiedBy = tsp.ModifiedBy,
                //            ModifiedDate = tsp.ModifiedDate,
                //            Name = tsp.Name
                //        };
                //        context.TransportationServiceProvider.AddOrUpdate(a=>new { a.Name,a.CreatedBy,a.CreatedDate,a.DUNSNo,a.ModifiedBy,a.ModifiedDate},tspObj);
                //        context.SaveChanges();
                //        var tspPipeList = pipelineList.Where(a => a.TSPId == tsp.ID).ToList();
                //        foreach (var pipe in tspPipeList)
                //        {
                //            var pipeObj = new Pipeline
                //            {
                //                IsActive = pipe.IsActive,
                //                ModelTypeID = pipe.ModelTypeID,
                //                ModifiedBy = pipe.ModifiedBy,
                //                ModifiedDate = pipe.ModifiedDate,
                //                Name = pipe.Name,
                //                ToUseTSPDUNS = pipe.ToUseTSPDUNS,
                //                TSPId = tspObj.ID,
                //                CreatedBy = "",
                //                CreatedDate = DateTime.Now,
                //                DUNSNo = pipe.DUNSNo
                //            };
                //            context.Pipeline.AddOrUpdate(a=>new { a.Name,a.ToUseTSPDUNS,a.TSPId,a.DUNSNo }, pipeObj);
                //            context.SaveChanges();

                //            var pipeTpwList = tpwList.Where(a => a.PipelineID == pipe.ID).ToList();
                //            foreach (var tpw in pipeTpwList)
                //            {
                //                var tpwObj = new TradingPartnerWorksheet
                //                {
                //                    IsActive = tpw.IsActive,
                //                    IsTest = tpw.IsTest,
                //                    KeyLive = tpw.KeyLive,
                //                    KeyTest = tpw.KeyTest,
                //                    ModifiedBy = tpw.ModifiedBy,
                //                    ModifiedDate = tpw.ModifiedDate,
                //                    Name = tpw.Name,
                //                    PasswordLive = tpw.PasswordLive,
                //                    PasswordTest = tpw.PasswordTest,
                //                    PipeDuns = pipeObj.DUNSNo,
                //                    PipelineID = pipeObj.ID,
                //                    ReceiveDataSeperator = tpw.ReceiveDataSeperator,
                //                    ReceiveSegmentSeperator = tpw.ReceiveSegmentSeperator,
                //                    ReceiveSubSeperator = tpw.ReceiveSubSeperator,
                //                    SendDataSeperator = tpw.SendDataSeperator,
                //                    SendSegmentSeperator = tpw.SendSegmentSeperator,
                //                    SendSubSeperator = tpw.SendSubSeperator,
                //                    URLLive = tpw.URLLive,
                //                    URLTest = tpw.URLTest,
                //                    UsernameLive = tpw.UsernameLive,
                //                    UsernameTest = tpw.UsernameTest,
                //                    CreatedBy = "",
                //                    CreatedDate = DateTime.Now
                //                };
                //                context.TradingPartnerWorksheet.AddOrUpdate(a=>new {a.Name,a.PipeDuns,a.ModifiedBy,a.ModifiedDate,a.PasswordLive,a.PasswordTest,a.PipelineID }, tpwObj);
                //            }
                //            // var conPipeList = contractList.Where(a => a.PipelineID == pipe.ID).ToList();
                //            // foreach (var conPipe in conPipeList)
                //            // {
                //            //var con = new Contract
                //            //{
                //            //    IsActive = conPipe.IsActive,
                //            //    LocationFromID = conPipe.LocationFromID,
                //            //    LocationToID = conPipe.LocationToID,
                //            //    MDQ = conPipe.MDQ,
                //            //    ModifiedBy = conPipe.ModifiedBy,
                //            //    ModifiedDate = conPipe.ModifiedDate,
                //            //    PipeDuns = pipeObj.DUNSNo,
                //            //    PipelineID = pipeObj.ID,
                //            //    ReceiptZone = conPipe.ReceiptZone,
                //            //    RequestNo = conPipe.RequestNo,
                //            //    RequestTypeID = conPipe.RequestTypeID,
                //            //    ShipperID = 0,
                //            //    ValidUpto = conPipe.ValidUpto,
                //            //    CreatedBy = "",
                //            //    DeliveryZone = conPipe.DeliveryZone,
                //            //    FuelPercentage = conPipe.FuelPercentage,
                //            //    CreatedDate = DateTime.Now
                //            //};
                //            //context.Contract.Add(con);
                //            // }
                //            //var locPipeList = locationList.Where(a => a.PipelineID == pipe.ID).ToList();
                //            //foreach (var locPipe in locPipeList)
                //            //{
                //            //    var loc = new Location
                //            //    {
                //            //        Identifier = locPipe.Identifier,
                //            //        IsActive = locPipe.IsActive,
                //            //        ModifiedBy = locPipe.ModifiedBy,
                //            //        ModifiedDate = locPipe.ModifiedDate,
                //            //        Name = locPipe.Name,
                //            //        PipeDuns = pipeObj.DUNSNo,
                //            //        PipelineID = pipeObj.ID,
                //            //        PropCode = locPipe.PropCode,
                //            //        RDUsageID = locPipe.RDUsageID,
                //            //        CreatedBy = locPipe.CreatedBy,
                //            //        CreatedDate = locPipe.CreatedDate
                //            //    };
                //            //    context.Location.Add(loc);
                //            // }
                //            var ttmpList = context.Pipeline_TransactionType_Map.Where(a => a.PipelineID == pipe.ID).ToList();//pipelineTranTypeMapList.Where(a => a.PipelineID == pipe.ID).ToList();
                //            foreach (var map in ttmpList)
                //            {
                //                map.PipelineID = pipeObj.ID;
                //                map.PipeDuns = pipeObj.DUNSNo;
                //                context.Pipeline_TransactionType_Map.Attach(map);
                //                var entry = context.Entry(map);
                //                entry.Property(e => e.PipelineID).IsModified = true;
                //                entry.Property(e => e.PipeDuns).IsModified = true;
                //                context.SaveChanges();
                //            }
                //            var pipeEncKeyInfoPipeList = pipeEncKeyInfoList.Where(a => a.PipelineId == pipe.ID).ToList();
                //            foreach (var peki in pipeEncKeyInfoPipeList)
                //            {
                //                peki.PipelineId = pipeObj.ID;
                //                peki.PipeDuns = pipeObj.DUNSNo;
                //                context.metadataPipelineEncKeyInfo.AddOrUpdate(a=>a.PipelineId,peki);
                //            }

                //        }

                //    }
                //    foreach (var cp in counterPartyList)
                //    {
                //        context.CounterParty.AddOrUpdate(a=>new { a.Name,a.Identifier,a.PipeDuns,a.PropCode},cp);
                //    }
                //}
                //catch (Exception ex)
                //{

                //}

                //#endregion

               // context.SaveChanges();
           // }
        }

        private static List<Route> GetRoutes()
        {
            return new List<Route>() {
             new Route() { EDIRouteId="NN1",RouteDescription="North to North", DirectionId="North", DirectionDescription="North System"  },
             new Route() { EDIRouteId="SS1",RouteDescription="South to South", DirectionId="South", DirectionDescription="South System"  },
             new Route() { EDIRouteId="NS3",RouteDescription="North to South Havasu", DirectionId="NS-Hav SN-Lin", DirectionDescription="N/S via Havasu and S/N via Lincoln"  },
             new Route() { EDIRouteId="SN1",RouteDescription="South to North Havasu", DirectionId="NS-Lin SN-Hav", DirectionDescription="N/S via Lincoln and S/N via Havasu"  },
             new Route() { EDIRouteId="SN2",RouteDescription="South to North Lincoln", DirectionId="NS-Hav SN-Lin", DirectionDescription="N/S via Havasu and S/N via Lincoln"  },
             new Route() { EDIRouteId="NS2",RouteDescription="North to Maricopa", DirectionId="NS-Maricopa", DirectionDescription="N/S via Maricopa"  },
             new Route() { EDIRouteId="NS1",RouteDescription="North to South Lincoln", DirectionId="NS-Lin SN-Hav", DirectionDescription="N/S via Lincoln and S/N via Havasu"  },
             new Route() { EDIRouteId="NSV",RouteDescription="North to South Virtual", DirectionId="NS-Hav SN-Lin", DirectionDescription="N/S via Havasu and S/N via Lincoln"  },
             new Route() { EDIRouteId="NN3",RouteDescription="San Juan to Plains", DirectionId="North", DirectionDescription="North System"  },
             new Route() { EDIRouteId="XO1",RouteDescription="Permian to X-Over", DirectionId="NS-Hav SN-Lin", DirectionDescription="N/S via Havasu and S/N via Lincoln"  },
             new Route() { EDIRouteId="NN2",RouteDescription="Plains to Topock", DirectionId="North", DirectionDescription="North System"  },
             new Route() { EDIRouteId="SN3",RouteDescription="Permian to Plains N", DirectionId="NS-Hav SN-Lin", DirectionDescription="N/S via Havasu and S/N via Lincoln"  },
             new Route() { EDIRouteId="AN1",RouteDescription="Plains to Anadarko", DirectionId="North", DirectionDescription="North System"  },
             new Route() { EDIRouteId="AN2",RouteDescription="Anadarko to Plains", DirectionId="North", DirectionDescription="North System"  },
             new Route() { EDIRouteId="AN2-NSV",RouteDescription="", DirectionId="NS-Hav SN-Lin", DirectionDescription="N/S via Havasu and S/N via Lincoln"  },
             new Route() { EDIRouteId="AN2-NS3",RouteDescription="", DirectionId="NS-Hav SN-Lin", DirectionDescription="N/S via Havasu and S/N via Lincoln"  },
             new Route() { EDIRouteId="AN2-NN2",RouteDescription="", DirectionId="North", DirectionDescription="North System"  },
             new Route() { EDIRouteId="AN2-NS1",RouteDescription="", DirectionId="NS-Lin SN-Hav", DirectionDescription="N/S via Lincoln and S/N via Havasu"  },
             new Route() { EDIRouteId="NN3-AN1",RouteDescription="", DirectionId="North", DirectionDescription="North System"  },
             new Route() { EDIRouteId="SN3-AN1",RouteDescription="", DirectionId="NS-Hav SN-Lin", DirectionDescription="N/S via Havasu and S/N via Lincoln"  },
             new Route() { EDIRouteId="AN2-NS2",RouteDescription="", DirectionId="NS-Maricopa", DirectionDescription="N/S via Maricopa"  },
             new Route() { EDIRouteId="",RouteDescription="No Path Available", DirectionId="South", DirectionDescription="South System"  },
             new Route() { EDIRouteId="C/C1",RouteDescription="North to South via 1903", DirectionId="NS-1903 SN-Lin", DirectionDescription="N/S via 1903 and S/N via Lincoln"  },
             new Route() { EDIRouteId="C/C2",RouteDescription="Mojave to South via 1903", DirectionId="NS-1903 SN-Lin", DirectionDescription="N/S via 1903 and S/N via Lincoln"  },
             };
        }



        private static List<metadataBidUpIndicator> GetBidUpImdicator()
        {
            return new List<metadataBidUpIndicator>
            {
                new metadataBidUpIndicator
                {
                    Code = "",
                    IsActive = false,
                    Name = "-"
                },
                 new metadataBidUpIndicator
                {
                    Code = "MAX",
                    IsActive = true,
                    Name = "Maximum Tariff Rate"
                }
            };

        }

        private static List<DataSetConfigurationSetting> GetDatSetConfigSettings()
        {
            return new List<DataSetConfigurationSetting>
            {
                new DataSetConfigurationSetting
                {
                    DataSet = Enums.EnercrossDataSets.OACY,
                    EmailId = "",
                    Frequency = null,
                    IsTestMode = false,
                    SchedularStartTime = new TimeSpan(11,00,00),
                    SchedularCronJobTime = "0 0 11 1/1 * ? *"
                },
                new DataSetConfigurationSetting
                {
                    DataSet = Enums.EnercrossDataSets.UNSC,
                    EmailId = "",
                    Frequency = null,
                    IsTestMode = false,
                    SchedularStartTime = new TimeSpan(11,00,00),
                    SchedularCronJobTime = "0 0 11 1/1 * ? *"
                },
                new DataSetConfigurationSetting
                {
                    DataSet = Enums.EnercrossDataSets.SWNT,
                    EmailId = "",
                    Frequency = 1,
                    IsTestMode = false,
                    SchedularStartTime = new TimeSpan(11,00,00),
                    SchedularCronJobTime = "0 0 11 1/1 * ? *"
                }
            };
        }

        private static List<metadataCapacityTypeIndicator> GetCapacityTypeIndicator()
        {
            return new List<metadataCapacityTypeIndicator>
            {
                new metadataCapacityTypeIndicator
                {
                    Code = "135",
                    IsActive = true,
                    Name = "Authorized Overrun Incremental Capacity"
                },
                 new metadataCapacityTypeIndicator
                {
                    Code = "140",
                    IsActive = true,
                    Name = "Out of Cycle"
                },
                 new metadataCapacityTypeIndicator
                {
                    Code = "PP",
                    IsActive = true,
                    Name = "Primary to Primary"
                },
                 new metadataCapacityTypeIndicator
                {
                    Code = "PS",
                    IsActive = true,
                    Name = "Primary to Secondary"
                },
                 new metadataCapacityTypeIndicator
                {
                    Code = "SS",
                    IsActive = true,
                    Name = "Secondary to Secondary"
                },
                 new metadataCapacityTypeIndicator
                {
                    Code = "SP",
                    IsActive = true,
                    Name = "Secondary to Primary"
                },
                 new metadataCapacityTypeIndicator
                {
                    Code = "IT",
                    IsActive = true,
                    Name = "Interruptible"
                },
                 new metadataCapacityTypeIndicator
                {
                    Code = "TP",
                    IsActive = true,
                    Name = "Tertiary to Primary"
                },
                 new metadataCapacityTypeIndicator
                {
                    Code = "",
                    IsActive = false,
                    Name = "-"
                }
            };

        }

        public static List<metadataCycle> GetCycle()
        {
            return new List<metadataCycle>() {
                new metadataCycle() {
                    Code="TIM",
                    Name="Timely",
                    IsActive=true
                },
                 new metadataCycle() {
                    Code="EVE",
                    Name="Evening",
                    IsActive=true
                },
                  new metadataCycle() {
                    Code="ID1",
                    Name="Intra Day 1",
                    IsActive=true
                },
                   new metadataCycle() {
                    Code="ID2",
                    Name="Intra Day 2",
                    IsActive=true
                },
                    new metadataCycle() {
                    Code="ID3",
                    Name="Intra Day 3",
                    IsActive=true
                }
            };
        }

        private static List<metadataExportDeclaration> GetExportDeclaration()
        {
            return new List<metadataExportDeclaration>(){
                new metadataExportDeclaration() {
                    Code="",
                    Name="-",
                    IsActive=false
                },
                new metadataExportDeclaration() {
                    Code="GSTY",
                    Name="GST Export Declaration Yes",
                    IsActive=true
                },
                new metadataExportDeclaration() {
                    Code="GSTN",
                    Name="GST Export Declaration No",
                    IsActive=true
                }
            };
        }

        //private static List<metadataFileStatu> GetFileStatus()
        //{
        //    return new List<metadataFileStatu>() {
        //        new metadataFileStatu() {
        //            Name="XML",
        //        },
        //         new metadataFileStatu() {
        //            Name="EDI",
        //        },
        //          new metadataFileStatu() {
        //            Name="Encrypted",
        //        },
        //           new metadataFileStatu() {
        //            Name="Decrypted",
        //        },
        //            new metadataFileStatu() {
        //            Name="Success Gisb",
        //        },
        //             new metadataFileStatu() {
        //            Name="Success Ack",
        //        },
        //              new metadataFileStatu() {
        //            Name="Success NMQR",
        //        },
        //               new metadataFileStatu() {
        //            Name="Failure Gisb",
        //        },
        //                new metadataFileStatu() {
        //            Name="Failure Ack",
        //        },
        //                 new metadataFileStatu() {
        //            Name="Failure NMQR",
        //        },
        //                  new metadataFileStatu() {
        //            Name="Draft",
        //        }
        //    };
        //}

        private static List<metadataQuantityTypeIndicator> GetQuantityTypeIndicator()
        {
            return new List<metadataQuantityTypeIndicator>() {
                new metadataQuantityTypeIndicator() {
                    Name="Receipt",
                    Code="R",
                    IsActive=true
                },
                 new metadataQuantityTypeIndicator() {
                    Name="Delivery",
                    Code="D",
                    IsActive=true
                },
                  new metadataQuantityTypeIndicator() {
                    Name="Both",
                    Code="B",
                    IsActive=true
                }
            };
        }

        private static List<metadataRequestType> GetRequestType()
        {
            return new List<metadataRequestType>() {
                new metadataRequestType() {
                    Name="FTS",
                    IsActive=true
                },
                 new metadataRequestType() {
                    Name="ITS",
                    IsActive=true
                },
                  new metadataRequestType() {
                    Name="LROL",
                    IsActive=true
                },
                   new metadataRequestType() {
                    Name="LROP",
                    IsActive=true
                },
                    new metadataRequestType() {
                    Name="NSS",
                    IsActive=true
                }
            };
        }


        private static List<SendingStage> GetSendingStatus()
        {
            return new List<SendingStage>() {
                new SendingStage() { Name= "Initial" },
                new SendingStage() { Name= "XML" },
                new SendingStage() { Name= "EDI" },
                new SendingStage() { Name= "EncryptedEDI" },
                new SendingStage() { Name= "SendingFile" },
                new SendingStage() { Name="GISB"}
            };

        }

        private static List<ReceivingStage> GetReceivingStages()
        {
            return new List<ReceivingStage>() {
                new ReceivingStage() { Name= "Initial" },
                new ReceivingStage() { Name= "EncEDI_ToDB" },
                new ReceivingStage() { Name= "SendGisb" },
                new ReceivingStage() { Name= "EncEDIFile" },
                new ReceivingStage() { Name= "Decryption" },
                new ReceivingStage() { Name="XML"},
               new ReceivingStage() { Name="DBMapping"}
            };

        }
    }
}
