using Nom1Done.Nom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{
    public class DashboardModel
    {
        public List<RejectedNomModel> RejectedNomList { get; set; }
        public List<BONotice> BONoticeCriteriaList { get; set; }
        public List<BONotice> BONonNoticeCriteriaList { get; set; }
    }
}
