namespace Nom1Done.Enums
{
    public enum EnercrossDataSets
    {
        OACY = 1,
        UNSC = 2,
        SWNT = 3
    }

    public enum WatchlistAlertType
    {
        Text = 1,
        Email = 2,
        Both = 3
    }

    public enum WatchlistAlertFrequency
    {
        Daily = 1,
        WhenAvailable = 2,        
    }    

    public enum SqtsPartials
    {
        NomQty=1,
        SqtsQty=2,
        SqtsOrphan =3,
        NomQtyReceipt=4,
        SqtsQtyReceipt=5,
        SqtsOrphanReceipt=6,
        NomQtyDelivery = 7,
        SqtsQtyDelivery = 8,
        SqtsOrphanDelivery = 9,
        NomQtyContractPath = 10,
        SqtsQtyContractPath = 11,
        SqtsOrphanContractPath = 12,
        OperSqts=13
    }
}
