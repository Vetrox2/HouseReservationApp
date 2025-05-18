using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Contracts.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int UserCount { get; set; }
        public int HouseCount { get; set; }
        public EarningsReportViewModel EarningsReport { get; set; }
        public decimal SureEarnings { get; set; }
        public decimal PossibleEarnings { get; set; }
    }

}
