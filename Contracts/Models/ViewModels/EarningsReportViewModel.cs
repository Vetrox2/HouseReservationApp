using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Contracts.Models.ViewModels
{
    public class EarningsReportViewModel
    {
        public IEnumerable<MonthlyEarningsViewModel> Months { get; set; }
    }
}
