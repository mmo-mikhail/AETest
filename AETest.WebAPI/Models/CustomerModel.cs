using System;
using System.ComponentModel.DataAnnotations;

namespace AETest.WebAPI.Models.Request
{
    /// <summary>
    /// Reprensts customer DTO model
    /// </summary>
    public class CustomerModel
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        //[DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
