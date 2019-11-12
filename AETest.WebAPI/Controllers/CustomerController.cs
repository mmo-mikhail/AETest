﻿using System.Threading.Tasks;
using AETest.DataAccess;
using AETest.Domain;
using AETest.WebAPI.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace AETest.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IEntityRepository<Customer> _customerRepo;

        public CustomerController(IEntityRepository<Customer> customerRepo)
        {
            _customerRepo = customerRepo;
        }

        [HttpGet("{name:string}")]
        public async Task<ActionResult<CustomerModel>> Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            var domainCustomer = await _customerRepo.FindEntity(c => c.FirstName == name || c.LastName == name);
            if (domainCustomer == null)
            {
                return NotFound();
            }
            return new CustomerModel
            {
                Id = domainCustomer.Id,
                FirstName = domainCustomer.FirstName,
                LastName = domainCustomer.LastName,
                DateOfBirth = domainCustomer.DateOfBirth
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerModel customerModel)
        {
            if (customerModel.Id < 0)
            {
                return BadRequest();
            }

            if (customerModel.Id == 0)
            {
                var newDomainCustomer = new Customer()
                {
                    FirstName = customerModel.FirstName,
                    LastName = customerModel.LastName,
                    DateOfBirth = customerModel.DateOfBirth
                };
                await _customerRepo.Add(newDomainCustomer);
            }
            else
            {
                var existingCustomer = await _customerRepo
                    .FindEntity(c => c.Id == customerModel.Id);
                if (existingCustomer == null)
                {
                    return NotFound();
                }
                existingCustomer.FirstName = customerModel.FirstName;
                existingCustomer.LastName = customerModel.LastName;
                await _customerRepo.Update(existingCustomer);
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int customerId)
        {
            if (customerId < 0)
            {
                return BadRequest();
            }

            var customer = await _customerRepo.FindEntity(c => c.Id == customerId);
            if (customer == null)
            {
                return NotFound();
            }
            await _customerRepo.Delete(customer);
            return Ok();
        }
    }
}
