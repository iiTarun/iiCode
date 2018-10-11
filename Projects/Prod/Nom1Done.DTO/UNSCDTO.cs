using System;

namespace Nom1Done.DTO
{
    public class UNSCDTO
    {
        public DateTime? PostingDateTime { get; set; }
        public string Loc { get; set; }
        public DateTime? EffectiveGasDayTime { get; set; }
        public string TransactionServiceProvider { get; set; }
        public string LocQTIDesc { get; set; }
    }
}
