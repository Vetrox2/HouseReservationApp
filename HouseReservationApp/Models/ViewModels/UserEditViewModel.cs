using HouseReservationApp.Models.DB.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HouseReservationApp.Models.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        [StringLength(100)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Bank Account")]
        public string BankAccount { get; set; }
    }
}
