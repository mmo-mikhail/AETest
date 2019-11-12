using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AETest.WebAPI.Models.Request
{
    public class CustomerModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
