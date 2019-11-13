using System;

namespace AETest.Domain
{
    /// <summary>
    /// Represents domain customer entity
    /// </summary>
    public class Customer : Entity
    {
        public int Id { get; set; }

        /// <summary>
        /// First name of the customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Surname of the customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Date of Birth of the customer
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }
}
