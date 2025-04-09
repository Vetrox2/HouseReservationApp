using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HouseReservation.Contracts.Models.ViewModels
{
    public class UserEditViewModel
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        [DisplayName("Bank Account")]
        public string BankAccount { get; set; }
    }
}
