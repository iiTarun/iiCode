using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Nom1Done.Data.SeedData;
using Nom1Done.DTO;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Nom1Done.Data
{
    public class NomSeed : DropCreateDatabaseIfModelChanges<NomEntities>
    {
        protected override void Seed(NomEntities context)
        {
            GetBidUpImdicator().ForEach(c => context.BidUpIndicators.Add(c));
            GetCapacityTypeIndicator().ForEach(c => context.metadataCapacityTypeIndicator.Add(c));
            GetCycle().ForEach(c => context.metadataCycle.Add(c));
            GetExportDeclaration().ForEach(c => context.metadataExportDeclaration.Add(c));
           // GetFileStatus().ForEach(c => context.metadataFileStatu.Add(c));
            GetQuantityTypeIndicator().ForEach(c => context.metadataQuantityTypeIndicator.Add(c));
            GetRequestType().ForEach(c => context.metadataRequestType.Add(c));
            var contractList=ContractSeed.GetContract(); //.ForEach(c => context.Contract.Add(c));
            EmailTemplateSeed.GetEmailSeed().ForEach(c => context.EmailTemplates.Add(c));
            FileStatusSeed.GetFileStatus().ForEach(c => context.metadataFileStatu.Add(c));
            MetadataDataSetSeed.GetDataSet().ForEach(c => context.metadataDataset.Add(c));
            PipelineEncKeyInfoSeed.GetPipelineEncKeyInfo().ForEach(c => context.metadataPipelineEncKeyInfo.Add(c));
            SettingSeed.GetSettingData().ForEach(c => context.Setting.Add(c));
            metadataErrorCodeSeed.GetErrorCode().ForEach(c => context.metadataErrorCode.Add(c));

            GetSendingStatus().ForEach(c => context.SendingStages.Add(c));
            GetReceivingStages().ForEach(c => context.ReceivingStages.Add(c));
            var locationList = LocationsSeed.GetLocations();

            #region Identity Seed
            // add roles
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };
                manager.Create(role);
            }
            //add users
            if (!(context.ShipperCompany.Any(u => u.DUNS == "837565548")))
            {
                var shipperCompany = new ShipperCompany
                {
                    Name = "Shell",
                    DUNS = "837565548",
                    IsActive = true,
                    CreatedBy = "",
                    CreatedDate = DateTime.Now,
                    ModifiedBy = "",
                    ModifiedDate = DateTime.Now,
                    SubscriptionID = 0,
                    ShipperAddress = ""
                };
                context.ShipperCompany.Add(shipperCompany);
                if (!(context.Users.Any(u => u.UserName == "tiffanyP.on@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser
                    {
                        UserName = "tiffanyP.on@sena.com",
                        PhoneNumber = "0797697898",
                        EmailConfirmed = true,
                        Email = "tiffanyP.on@sena.com",
                        PhoneNumberConfirmed = true,
                    };
                    userManager.Create(userToInsert, "Monday02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper
                    {
                        UserId = userToInsert.Id,
                        FirstName = "Tiffany P",
                        LastName = "On",
                        ShipperCompanyID = shipperCompany.ID,
                        IsActive = true,
                        CreatedBy = "",
                        CreatedDate = DateTime.Now,
                        ModifiedBy = "",
                        ModifiedDate = DateTime.Now
                    };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "rebeccaJ.newson@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "rebeccaJ.newson@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "rebeccaJ.newson@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "Tuesday02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Rebecca J", LastName = "Newson", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "terryA.mcmillin@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "terryA.mcmillin@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "terryA.mcmillin@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "Wednesday02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Terry A", LastName = "Mcmillin", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "amanda.boettcher@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "amanda.boettcher@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "amanda.boettcher@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "Thursday02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Amanda", LastName = "Boettcher", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "jasonJ.babin@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "jasonJ.babin@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "jasonJ.babin@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "Friday02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Jason J", LastName = "Babin", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "deepthi.bollu@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "deepthi.bollu@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "deepthi.bollu@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "Saturday02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Deepthi", LastName = "Bollu", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "carlyM.billington@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "carlyM.billington@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "carlyM.billington@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "Sunday02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Carly M", LastName = "Billington", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "jaymeC.dagnolo@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "jaymeC.dagnolo@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "jaymeC.dagnolo@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "Jan02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Jayme C", LastName = "D'Agnolo", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "brandon.johnson@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "brandon.johnson@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "brandon.johnson@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "Feb02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Brandon", LastName = "Johnson", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "tamiL.covington@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "tamiL.covington@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "tamiL.covington@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "March02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Tami L", LastName = "Covington", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "danielle.foster@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "danielle.foster@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "danielle.foster@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "April02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Danielle", LastName = "Foster", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "christen.schaffer@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "christen.schaffer@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "christen.schaffer@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "May02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Christen", LastName = "Schaffer", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "john.powell@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "john.powell@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "john.powell@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "June02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "John", LastName = "Powell", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "leslie.may@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "leslie.may@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "leslie.may@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "July02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Leslie", LastName = "May", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
                if (!(context.Users.Any(u => u.UserName == "justin.babb@sena.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "justin.babb@sena.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "justin.babb@sena.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "August02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Justin", LastName = "Babb", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
            }
            if (!(context.ShipperCompany.Any(u => u.DUNS == "078711334")))
            {
                var shipperCompany = new ShipperCompany
                {
                    Name = "Enercross",
                    DUNS = "078711334",
                    IsActive = true,
                    CreatedBy = "",
                    CreatedDate = DateTime.Now,
                    ModifiedBy = "",
                    ModifiedDate = DateTime.Now,
                    SubscriptionID = 0,
                    ShipperAddress = ""
                };
                context.ShipperCompany.Add(shipperCompany);
                if (!(context.Users.Any(u => u.UserName == "support@invertedi.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "support@invertedi.com", PhoneNumber = "0797697898", EmailConfirmed = true, Email = "support@invertedi.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "Monday02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Gagan", LastName = "Deep", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }

                if (!(context.Users.Any(u => u.UserName == "Jay@enercross.com")))
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "Jay@enercross.com", PhoneNumber = "0000000000", EmailConfirmed = true, Email = "Jay@enercross.com", PhoneNumberConfirmed = true, };
                    userManager.Create(userToInsert, "Monday02__");
                    userManager.AddToRole(userToInsert.Id, "Admin");

                    var shipper = new Shipper { UserId = userToInsert.Id, FirstName = "Gagan", LastName = "Deep", ShipperCompanyID = shipperCompany.ID, IsActive = true, CreatedBy = "", CreatedDate = DateTime.Now, ModifiedBy = "", ModifiedDate = DateTime.Now };
                    context.Shipper.Add(shipper);
                }
            }
            #endregion

            #region Add Pipeline
            try
            {
                var pipelineList = PipelineSeed.GetPipelines();
                var transactionTypeList = TransactionTypesSeed.GetTransactionTypes();
                var tspList = TransportationServiceProviderSeed.GetTransportationServiceProvider();
                var pipelineTranTypeMapList = PipelineTransactionTypeMapSeed.GetPipelineTransactionTypeMap();
                var tpwList = TradingPartnerWorksheetSeed.GetTradingPartnerWorksheet();
                var counterPartyList = CounterPartySeed.GetCounterParty();
                var pipeEncKeyInfoList = PipelineEncKeyInfoSeed.GetPipelineEncKeyInfo();

                foreach (var tranType in transactionTypeList)
                {
                    var tt = new metadataTransactionType
                    {
                        Identifier = tranType.Identifier,
                        IsActive = tranType.IsActive,
                        Name = tranType.Name,
                        SequenceNo = tranType.SequenceNo
                    };
                    context.metadataTransactionType.Add(tt);
                    context.Commit();
                    var tranTypeMapOnPipeTTId = pipelineTranTypeMapList.Where(a => a.TransactionTypeID == tranType.ID).ToList();
                    foreach (var ttm in tranTypeMapOnPipeTTId)
                    {
                        var map = new Pipeline_TransactionType_Map
                        {
                            IsActive = ttm.IsActive,
                            LastModifiedBy = ttm.LastModifiedBy,
                            LastModifiedDate = ttm.LastModifiedDate,
                            PathType = ttm.PathType,
                            PipeDuns = "",//pipe.DUNSNo,
                            PipelineID = ttm.PipelineID,
                            TransactionTypeID = tt.ID,
                            CreatedBy = "",
                            CreatedDate = DateTime.Now
                        };
                        context.Pipeline_TransactionType_Map.Add(map);
                        context.Commit();
                    }
                }
                foreach (var tsp in tspList)
                {
                    var tspObj = new TransportationServiceProvider
                    {
                        CreatedBy = tsp.CreatedBy,
                        CreatedDate = tsp.CreatedDate,
                        DUNSNo = tsp.DUNSNo,
                        IsActive = tsp.IsActive,
                        ModifiedBy = tsp.ModifiedBy,
                        ModifiedDate = tsp.ModifiedDate,
                        Name = tsp.Name
                    };
                    context.TransportationServiceProvider.Add(tspObj);
                    context.Commit();
                    var tspPipeList = pipelineList.Where(a => a.TSPId == tsp.ID).ToList();
                    foreach (var pipe in tspPipeList)
                    {
                        var pipeObj = new Pipeline
                        {
                            IsActive = pipe.IsActive,
                            ModelTypeID = pipe.ModelTypeID,
                            ModifiedBy = pipe.ModifiedBy,
                            ModifiedDate = pipe.ModifiedDate,
                            Name = pipe.Name,
                            ToUseTSPDUNS = pipe.ToUseTSPDUNS,
                            TSPId = tspObj.ID,
                            CreatedBy = "",
                            CreatedDate = DateTime.Now,
                            DUNSNo = pipe.DUNSNo
                        };
                        context.Pipeline.Add(pipeObj);
                        context.Commit();

                        var pipeTpwList = tpwList.Where(a => a.PipelineID == pipe.ID).ToList();
                        foreach (var tpw in pipeTpwList)
                        {
                            var tpwObj = new TradingPartnerWorksheet
                            {
                                IsActive = tpw.IsActive,
                                IsTest = tpw.IsTest,
                                KeyLive = tpw.KeyLive,
                                KeyTest = tpw.KeyTest,
                                ModifiedBy = tpw.ModifiedBy,
                                ModifiedDate = tpw.ModifiedDate,
                                Name = tpw.Name,
                                PasswordLive = tpw.PasswordLive,
                                PasswordTest = tpw.PasswordTest,
                                PipeDuns = pipeObj.DUNSNo,
                                PipelineID = pipeObj.ID,
                                ReceiveDataSeperator = tpw.ReceiveDataSeperator,
                                ReceiveSegmentSeperator = tpw.ReceiveSegmentSeperator,
                                ReceiveSubSeperator = tpw.ReceiveSubSeperator,
                                SendDataSeperator = tpw.SendDataSeperator,
                                SendSegmentSeperator = tpw.SendSegmentSeperator,
                                SendSubSeperator = tpw.SendSubSeperator,
                                URLLive = tpw.URLLive,
                                URLTest = tpw.URLTest,
                                UsernameLive = tpw.UsernameLive,
                                UsernameTest = tpw.UsernameTest,
                                CreatedBy = "",
                                CreatedDate = DateTime.Now
                            };
                            context.TradingPartnerWorksheet.Add(tpwObj);
                        }
                        var conPipeList = contractList.Where(a => a.PipelineID == pipe.ID).ToList();
                        foreach (var conPipe in conPipeList)
                        {
                            var con = new Contract
                            {
                                IsActive = conPipe.IsActive,
                                LocationFromID = conPipe.LocationFromID,
                                LocationToID = conPipe.LocationToID,
                                MDQ = conPipe.MDQ,
                                ModifiedBy = conPipe.ModifiedBy,
                                ModifiedDate = conPipe.ModifiedDate,
                                PipeDuns = pipeObj.DUNSNo,
                                PipelineID = pipeObj.ID,
                                ReceiptZone = conPipe.ReceiptZone,
                                RequestNo = conPipe.RequestNo,
                                RequestTypeID = conPipe.RequestTypeID,
                                ShipperID = 0,
                                ValidUpto = conPipe.ValidUpto,
                                CreatedBy = "",
                                DeliveryZone = conPipe.DeliveryZone,
                                FuelPercentage = conPipe.FuelPercentage,
                                CreatedDate = DateTime.Now
                            };
                            context.Contract.Add(con);
                        }
                        var locPipeList = locationList.Where(a => a.PipeDuns.Trim() == pipe.DUNSNo.Trim()).ToList();
                        foreach (var locPipe in locPipeList)
                        {
                            var loc = new Location
                            {
                                Identifier = locPipe.Identifier,
                                IsActive = locPipe.IsActive,
                                ModifiedBy = locPipe.ModifiedBy,
                                ModifiedDate = locPipe.ModifiedDate,
                                Name = locPipe.Name,
                                PipeDuns = pipeObj.DUNSNo,
                                PipelineID = pipeObj.ID,
                                PropCode = locPipe.PropCode,
                                RDUsageID = locPipe.RDUsageID,
                                CreatedBy = locPipe.CreatedBy,
                                CreatedDate = locPipe.CreatedDate
                            };
                            context.Location.Add(loc);
                        }
                        var ttmpList = context.Pipeline_TransactionType_Map.Where(a => a.PipelineID == pipe.ID).ToList();//pipelineTranTypeMapList.Where(a => a.PipelineID == pipe.ID).ToList();
                        foreach (var map in ttmpList)
                        {
                            map.PipelineID = pipeObj.ID;
                            map.PipeDuns = pipeObj.DUNSNo;
                            context.Pipeline_TransactionType_Map.Attach(map);
                            var entry = context.Entry(map);
                            entry.Property(e => e.PipelineID).IsModified = true;
                            entry.Property(e => e.PipeDuns).IsModified = true;
                            context.Commit();
                        }
                        var pipeEncKeyInfoPipeList = pipeEncKeyInfoList.Where(a => a.PipelineId == pipe.ID).ToList();
                        foreach (var peki in pipeEncKeyInfoPipeList)
                        {
                            peki.PipelineId = pipeObj.ID;
                            peki.PipeDuns = pipeObj.DUNSNo;
                            context.metadataPipelineEncKeyInfo.Add(peki);
                        }

                    }

                }
                foreach (var cp in counterPartyList)
                {
                    context.CounterParty.Add(cp);
                }
            }
            catch (Exception ex)
            {

            }

            #endregion


            context.Commit();
        }

        //look up table for BidUpIndicator
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
