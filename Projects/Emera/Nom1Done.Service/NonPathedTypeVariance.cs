using System.Collections.Generic;


namespace Nom1Done.Service
{
    public static class NonPathedTypeVariance
    {

        #region TransactionTypes Description and code

        public static string IPFTttDesc = "Imbalance Payback from Transportation Service Provider ";
        public static int IPFTtt = 03;
        public static string IPTTttDesc = "Imbalance Payback to Transportation Service Provider ";
        public static int IPTTtt = 04;
        public static string SIttDesc = "Storage Injection ";
        public static int SItt = 06;
        public static string SWttDesc = "Storage Withdrawal ";
        public static int SWtt = 07;
        public static string POOLttDesc = "Pooling ";
        public static int POOLtt = 08;
        public static string AIOttDesc = "Authorized Injection Overrun";
        public static int AIOtt = 12;
        public static string AWOttDesc = "Authorized Withdrawal Overrun";
        public static int AWOtt = 13;
        public static string SGCttDesc = "Suspense Gas Claim ";
        public static int SGCtt = 18;
        public static string SBOIttDesc = "SBO INJECTION";
        public static int SBOItt = 20;
        public static string SBOWttDesc = "SBO WITHDRAWAL";
        public static int SBOWtt = 21;
        public static string PMIPFTttDesc = "PRIOR MONTH IMBALANCE PAYBACK FROM TSP";
        public static int PMIPFTtt = 63;
        public static string PMIPTTttDesc = "PRIOR MONTH IMBALANCE PAYBACK TO TSP";
        public static int PMIPTTtt = 64;
        public static string LTWttDesc = "LNG TRUCK WITHDRAWAL";
        public static int LTWtt = 152;
        public static string LoanttDesc = "Loan ";
        public static int Loantt = 28;
        public static string LoanPayttDesc = "Loan Payback ";
        public static int LoanPaytt = 29;
        public static string PWttDesc = "Park Withdrawal ";
        public static int PWtt = 27;
        public static string ParkttDesc = "Park";
        public static int Parktt = 26;


        public static string AnrPoolttDesc = "Anr Pooling";
        public static int AnrPooltt = 01;

        public static string SACOttDesc = "Storage: Authorized Contract Overrun";
        public static int SACOtt = 02;

        public static string CBTransportTTDecs = "Current Business (Transport)";
        public static int CBTransportTT = 01;
        public static string PTRttDecs = "Plant Thermal Reduction";
        public static int PTRTT = 05;
        public static string MBttDesc = "Meter Bounce";
        public static int MBtt = 31;
        public static string APOttDesc = "Authorized Point Overrun (Auth Pt Ovr)";
        public static int APOtt = 48;
        public static string TACOttDesc = "Transport: Authorized Contract Overrun";
        public static int TACOtt = 02;
        public static string MBOvrttDesc = "Meter Bounce – Overrun(Mtr Bounce-Ovr)";
        public static int MBOvrtt = 102;
        public static string IPWCttDesc = "Imbalance Payback – within Contract(Imb PB-w/in K)";
        public static int IPWCtt = 111;
        public static string IPOttDesc = "Imbalance Payback – Overrun(Imb PB-Ovr)";
        public static int IPOtt = 112;

        public static string ACOttDesc = "Authorized Contract Overrun ";
        public static int ACOtt = 02;

        public static string MBDttDesc = "Meter Bounce Delivery";
        public static int MBDtt = 121;

        public static string MBRttDesc = "Meter Bounce Receipt";
        public static int MBRtt = 122;


        public static string SITttDesc = "Storage Inventory Transfer(Stor Inv Xfer)";
        public static int SITtt = 11;
        public static string SegttDesc = "Segmented(Seg)";
        public static int Segtt = 84;
        public static string SIFirmttDesc = "Storage Injection – Firm(Stor Inj-Firm)";
        public static int SIFirmTT = 113;
        public static string SWFirmttDesc = "Storage Withdrawal – Firm(Stor W/D-Firm)";
        public static int SWFirmtt = 115;
        public static string WheelingTTDesc = "Wheeling ";
        public static int WheelingTT = 132;
        public static string SRSwingttDesc = "Service Requester Swing (SR Swing)";
        public static int SRSwingtt = 147;
        public static string PACOttDesc = "Petal Authorized Contract Overrun";
        public static int PACOtt = 02;
        public static string PCBttDesc = "Petal Current Business";
        public static int PCBtt = 01;
        public static string PSIttDesc = "Petal Storage Injection";
        public static int PSItt = 06;
        public static string PSWttDesc = "Petal Storage Withdrawal";
        public static int PSWtt = 07;
      
        public static string PAIOttDesc = "Petal Authorized Injection Overrun";
        public static int PAIOtt = 12;
        public static string PAWOttDesc = "Petal Authorized Withdrawal Overrun";
        public static int PAWOtt = 13;
       
        public static string PParkttDesc = "Petal Park";
        public static int PParktt = 26;
        public static string PPWttDesc = "Petal Park Withdrawal";
        public static int PPWtt = 27;
       
        public static string PSegttDesc = "Petal Segmented";
        public static int PSegtt = 84;
        public static string PSRSwingttDesc = "Petal Service Requestor Swing";
        public static int PSRSwingtt = 147;

        #endregion

        // Type=-1  ==>Pathed
        // Type=-2  ==>PNT
        // Type= AnyPositiveNumber ==> NonPathedModel ( Different numbers for different TYPES in NonPathed)
        // Type=0 ==> TransactionType Or PipelineDuns Not Match (ERROR).
        public static int GetNomModelType(string PipelineDuns,int TransactionType, string TransactionTypeDesc)
        {
            int result = 0;

            // Transcontinental Gas Pipe Line Company, LLC -- 007933021
            if (PipelineDuns == "007933021") {              
                result = TGPNonPathedTypes(TransactionType,TransactionTypeDesc);
            }
            //ETC TIGER PIPELINE, LLC --- 829521983
            //FAYETTEVILLE EXPRESS PIPELINE LLC --- 829416002
            //Florida Gas Transmission Company, LLC --- 006924518
            //Transwestern Pipeline Company, LLC -- 007933047
            else if (PipelineDuns == "829521983" || PipelineDuns == "829416002" || PipelineDuns == "006924518" || PipelineDuns == "007933047")
            {
                result = TWPNonPathedTypes(TransactionType, TransactionTypeDesc);
            }
            //ANR Pipeline Company ---  006958581
            else if (PipelineDuns == "006958581")
            {
                result = ANRNonPathedTypes(TransactionType, TransactionTypeDesc);
            }
            //Vector Pipeline Limited Partnership---  259785194
            //Vector Pipeline L.P. ---  879136682
            //Dominion Energy Carolina Gas Transmission, LLC ---  094992187
            else if (PipelineDuns == "259785194" || PipelineDuns == "879136682" || PipelineDuns == "094992187" )
            {
                result = DECGTNonPathedTypes(TransactionType, TransactionTypeDesc);
            }
            //Northern Natural Gas Company ---  784158214
            else if (PipelineDuns == "784158214")
            {
                result = NNGCNonPathedTypes(TransactionType, TransactionTypeDesc);
            }
            // Texas Gas Transmission, LLC -- 115972101
            else if (PipelineDuns == "115972101")
            {
                result = TGTNonPathedTypes(TransactionType, TransactionTypeDesc);
            }
            // Iroquois Gas Transmission System, L.P.---  603955949
            else if (PipelineDuns == "603955949")
            {
                result = IGTNonPathedTypes(TransactionType, TransactionTypeDesc);
            }
            //Gulf South Pipeline Company, LP --- 078444247
            else if (PipelineDuns == "078444247")
            {
                result = GSPPntPathedType(TransactionType, TransactionTypeDesc);
            }
            return result;
        }

        #region Different Functions For Different Pipelines

        // Transcontinental Gas Pipe Line Company, LLC -- 007933021
        private static int TGPNonPathedTypes(int TT,string TTDesc)
        {
            int resulttype=0;           

            if (TT == IPFTtt || TT == PMIPFTtt)
            {
                resulttype = 6;
            }
            else if(TT==IPTTtt) {
                resulttype = 1;
            }
            else if (TT == SItt || TT==AIOtt || TT==SGCtt || TT==SBOWtt)
            {
                resulttype = 3;
            }
            else if (TT == SWtt || TT==AWOtt ||TT== SBOItt || TT==LTWtt)
            {
                resulttype = 5;
            }
            else if (TT == POOLtt)
            {
                resulttype = 7;
            }
            else if (TT == PMIPTTtt)
            {
                resulttype = 2;
            }
            else if (TT == CBTransportTT || TT==PTRTT)
            {
                resulttype = -1;  // Pathed Model
            }
            return resulttype;
        }

        //ETC TIGER PIPELINE, LLC --- 829521983
        //FAYETTEVILLE EXPRESS PIPELINE LLC --- 829416002
        //Florida Gas Transmission Company, LLC --- 006924518
        //Transwestern Pipeline Company, LLC -- 007933047
        private static int TWPNonPathedTypes(int TT, string TTDesc)
        {
            int resulttype = 0;
           
            if (TT == IPFTtt || TT == Loantt)
            {
                resulttype = 5;
            } else if (TT == IPTTtt || TT == PWtt)
            {
                resulttype = 3;
            } else if (TT==POOLtt)
            {
                resulttype = 4;
            }
            else if (TT == CBTransportTT  || TT == MBtt)
            {
                resulttype = -1;  //Pathed Model
            }

            return resulttype;
        }

        //ANR Pipeline Company ---  006958581
        private static int ANRNonPathedTypes(int TT, string TTDesc)
        {
            int resulttype = 0;
            
            if (TTDesc.ToLower().Trim() == CBTransportTTDecs.ToLower().Trim() || TT==SItt || TT==SWtt || TT==AIOtt || TT==AWOtt || TT==Parktt || TT==PWtt || TT==Loantt || TT==LoanPaytt || TT== APOtt)
            {
                resulttype = -1;  // Pathed
            }  
            else if (TTDesc.ToLower().Trim() == AnrPoolttDesc.ToLower().Trim() ) { resulttype = 5; }
            return resulttype;
        }

        //Vector Pipeline Limited Partnership---  259785194
        //Vector Pipeline L.P. ---  879136682
        //Dominion Energy Carolina Gas Transmission, LLC ---  094992187
        private static int DECGTNonPathedTypes(int TT, string TTDesc)
        {
            return 4;
        }

        //Northern Natural Gas Company ---  784158214
        private static int NNGCNonPathedTypes(int TT, string TTDesc)
        {
            int resultType = 0;
           
            if (TT == IPFTtt || TT==SWtt)
            {
                resultType = 5;
            } else if (TT==IPTTtt || TT==SItt)
            {
                resultType = 3;
            }
            else if (TT==POOLtt || TTDesc.ToLower().Trim() == SACOttDesc.ToLower().Trim())
            {
                resultType = 4;
            }
            else if (TT == CBTransportTT || TTDesc.ToLower().Trim() == TACOttDesc.ToLower().Trim() || TT==MBOvrtt || TT==IPWCtt || TT==IPOtt || TT==MBtt)
            {
                resultType = -1; // Pathed Model
            }
            return resultType;
        }


        // Texas Gas Transmission, LLC -- 115972101
        private static int TGTNonPathedTypes(int TT, string TTDesc)
        {
            int result = 0;
            if (TT == IPFTtt || TT == SWtt || TT == AWOtt  || TT == Parktt || TT == LoanPaytt) { result = 5; }
            else if (TT == IPTTtt) { result = 8; }
            else if (TT == SItt || TT == AIOtt || TT == PWtt || TT == Loantt) { result = 3; }
            else if (TT == POOLtt) { result = 4; }  
            else if (TT== CBTransportTT || TT==ACOtt || TT==MBDtt|| TT==MBRtt)
            {
                result = -1; // PathedModel
            }
            return result;
        }

        // Iroquois Gas Transmission System, L.P.---  603955949
        private static int IGTNonPathedTypes(int TT, string TTDesc)
        {
            int result = 0;
            if (TT == Parktt || TT == Loantt)
            {
                result = 11;
            }
            else if (TT == CBTransportTT)
            {
                result = -2; // PNT Model
            }
            return result;
        }


        //Gulf South Pipeline Company, LP --- 078444247
        private static int GSPPntPathedType(int TT, string TTDesc)
        {
            int result = 0;

            if (TT == PAIOtt || TT == PAWOtt || TTDesc.ToLower().Trim() == PCBttDesc.ToLower().Trim() || TTDesc.ToLower().Trim() == PACOttDesc.ToLower().Trim() || TTDesc.ToLower().Trim() == PSIttDesc.ToLower().Trim() || TTDesc.ToLower().Trim() == PSWttDesc.ToLower().Trim() || TTDesc.ToLower().Trim() == PParkttDesc.ToLower().Trim() || TTDesc.ToLower().Trim() == PPWttDesc.ToLower().Trim() || TTDesc.ToLower().Trim() == PSegttDesc.ToLower().Trim() || TTDesc.ToLower().Trim() == PSRSwingttDesc.ToLower().Trim())
            {
                result = -1;  // Pathed Model
            }
            else {
                result = -2;  // PNT Model
            }
            return result;
        }

        #endregion
    }

    public static class PathedNonpathedHybridProperties
    {
        #region Properties's titles

        public static string startDate = "StartDate";     
        public static string endDate = "EndDate";
        public static string createdDate = "CreatedDate";      
        public static string cycleId = "CycleID";       
        public static string contract = "Contract";
        public static string nomSubCycle = "NomSubCycle";      
        public static string recLocName = "RecLocation";
        public static string recLocProp = "RecLocProp";
        public static string recLocId = "RecLocID";
        public static string upName = "UpName";
        public static string upIdProp = "UpIDProp"; 
        public static string upId = "UpID";
        public static string upK = "UpKContract";
        public static string recRank = "RecRank";
        public static string recQty = "RecQty";
        public static string recpkgId = "PkgIDRec";
        public static string delLocName = "DelLoc";
        public static string delLocProp = "DelLocProp";
        public static string delLocId = "DelLocID";
        public static string downName = "DownName";
        public static string downIdProp = "DownIDProp";
        public static string downId = "DownID";
        public static string downK = "DownContract";
        public static string delRank = "DelRank";
        public static string delQty = "DelQuantity";
        public static string delpkgId = "PkgID";
        public static string qtyType = "QuantityType";
        
        public static string UpPkgID = "UpPkgID";
        public static string UpRank = "UpRank";
        public static string DownPkgID = "DownPkgID";
        public static string DownRank = "DownRank";
        public static string ActCode = "ActCode";
        public static string BidTransportRate = "BidTransportRate";
        public static string MaxRate = "MaxRate";
        public static string CapacityType = "CapacityType";
        public static string BidUp = "BidUp";
        public static string Export = "Export";
        public static string ProcessingRights = "ProcessingRights";
        public static string AssocContract = "AssocContract";
        public static string DealType = "DealType";
        public static string NomUserData1 = "NomUserData1";
        public static string NomUserData2 = "NomUserData2";
        public static string FuelPercentage = "FuelPercentage";

        #endregion

        public static List<string> GetPropertiesByTypes(int TypeId)
        {
            List<string> result = new List<string>();
            switch (TypeId) {
                case 1:
                    result = Type1GetProperties();
                    break;
                case 2:
                    result = Type2GetProperties();
                    break;
                case 3:
                    result = Type3GetProperties();
                    break;
                case 4:
                    result = Type4GetProperties();
                    break;
                case 5:
                    result = Type5GetProperties();
                    break;
                case 6:
                    result = Type6GetProperties();
                    break;
                case 7:
                    result = Type7GetProperties();
                    break;
                case 8:
                    result = Type8GetProperties();
                    break;
                case 9:
                    result = Type9GetProperties();
                    break;
                case 10:
                    result = Type10GetProperties();
                    break;
                case -1:
                    result = TypePurePathedModel();    // pathed model 
                    break;
            }
            return result;
        }


        #region PurePathedModel properties

        private static List<string> TypePurePathedModel()
        {
            return new List<string>() {
                             startDate,
                             endDate,
                             createdDate,
                             cycleId ,
                             contract,
                             nomSubCycle ,
                             recLocName ,
                             recLocProp ,
                             recLocId,
                             upName ,
                             upIdProp,
                             upId,
                             upK ,
                             recRank,
                             recQty ,                            
                             delLocName ,
                             delLocProp,
                             delLocId ,
                             downName,
                             downIdProp,
                             downId,
                             downK ,
                             delRank ,
                             delQty ,
                             delpkgId ,
                             qtyType ,
                             UpPkgID,
                             UpRank,
                             DownPkgID ,
                             DownRank,
                             ActCode ,
                             BidTransportRate,
                             MaxRate ,
                             CapacityType,
                             BidUp,
                             Export ,
                             ProcessingRights,
                             AssocContract ,
                             DealType ,
                             NomUserData1 ,
                             NomUserData2 ,
                             FuelPercentage 
                        };
           }

        #endregion

        #region Functions for Hybrid NonPathed Model Types
        private static List<string> Type1GetProperties()
        {
            return new List<string>() {
             startDate ,
             endDate ,
             createdDate ,
             cycleId ,
             contract ,
             nomSubCycle ,
             recLocName ,
             recLocProp,
             recLocId ,
             upName,
             upIdProp ,
             upId ,
             upK ,
             recRank ,
             recQty,            
             delLocName,
             delLocProp ,
             delLocId,
             downName ,
             downIdProp ,
             downId ,            
             delpkgId ,
             qtyType 
           };
        }
        private static List<string> Type2GetProperties()
        {
            return new List<string>() {
             startDate ,
             endDate ,
             createdDate ,
             cycleId ,
             contract ,
             nomSubCycle ,
             recLocName ,
             recLocProp,
             recLocId ,
             upName,
             upIdProp ,
             upId ,
             upK ,
             recRank ,
             recQty,
             recpkgId ,
             delLocName,
             delLocProp ,
             delLocId,
             downName ,
             downIdProp ,
             downId ,            
             delpkgId ,
             qtyType
           };
        }

        private static List<string> Type3GetProperties()
        {
            return new List<string>() {
             startDate ,
             endDate ,
             createdDate ,
             cycleId ,
             contract ,
             nomSubCycle ,
             recLocName ,
             recLocProp,
             recLocId ,
             upName,
             upIdProp ,
             upId ,
             upK ,
             recRank ,
             recQty,
             recpkgId ,            
             qtyType
           };
        }

        private static List<string> Type4GetProperties()
        {
            return new List<string>() {
             startDate ,
             endDate ,
             createdDate ,
             cycleId ,
             contract ,
             nomSubCycle ,
             recLocName ,
             recLocProp,
             recLocId ,
             upName,
             upIdProp ,
             upId ,
             upK ,
             recRank ,
             recQty,
             recpkgId ,
             delLocName,
             delLocProp ,
             delLocId,
             downName ,
             downIdProp ,
             downId ,
             downK ,
             delRank ,
             delQty ,
             delpkgId ,
             qtyType
           };
        }

        private static List<string> Type5GetProperties()
        {
            return new List<string>() {
             startDate ,
             endDate ,
             createdDate ,
             cycleId ,
             contract ,
             nomSubCycle ,            
             delLocName,
             delLocProp ,
             delLocId,
             downName ,
             downIdProp ,
             downId ,
             downK ,
             delRank ,
             delQty ,
             delpkgId ,
             qtyType
           };
        }

        private static List<string> Type6GetProperties()
        {
            return new List<string>() {
             startDate ,
             endDate ,
             createdDate ,
             cycleId ,
             contract ,
             nomSubCycle ,
             recLocName ,
             recLocProp,
             recLocId ,
             upName,
             upIdProp ,
             upId ,            
             delLocName,
             delLocProp ,
             delLocId,
             downName ,
             downIdProp ,
             downId ,
             downK ,
             delRank ,
             delQty ,
             delpkgId ,
             qtyType
           };
        }

        private static List<string> Type7GetProperties()
        {
            return new List<string>() {
             startDate ,
             endDate ,
             createdDate ,
             cycleId ,
             contract ,
             nomSubCycle ,            
             delLocName,
             delLocProp ,
             delLocId,
             downName ,
             downIdProp ,
             downId ,            
             delRank ,
             delQty ,
             delpkgId ,
             qtyType
           };
        }

        private static List<string> Type8GetProperties()
        {
            return new List<string>() {
             startDate ,
             endDate ,
             createdDate ,
             cycleId ,
             contract ,
             nomSubCycle ,
             recLocName ,
             recLocProp,
             recLocId ,
             upName,
             upIdProp ,
             upId ,
             upK ,
             recRank ,
             recQty,
             recpkgId ,            
             delpkgId ,
             qtyType
           };
        }


        private static List<string> Type9GetProperties()
        {
            return new List<string>() {           
             recLocName ,
             recLocProp,
             recLocId ,
             upName,
             upIdProp ,
             upId ,
             upK ,
             recRank ,
             recQty,
             recpkgId ,
             qtyType
           };
        }

        private static List<string> Type10GetProperties()
        {
            return new List<string>() {           
             delLocName,
             delLocProp ,
             delLocId,
             downName ,
             downIdProp ,
             downId ,
             downK ,
             delRank ,
             delQty ,
             delpkgId ,
             qtyType
           };
        }
       
        #endregion

    }   

}
