using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Contracts.Models.ViewModels
{
    public class MonthlyEarningsViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName
            => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);
        public decimal SureEarnings { get; set; }
        public decimal PossibleEarnings { get; set; }
    }
}
