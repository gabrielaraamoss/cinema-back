using Microsoft.AspNetCore.Mvc;
using Reserva_Butacas.Dtos;
using Reserva_Butacas.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reserva_Butacas.Models;

namespace Reserva_Butacas.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerDto)
        {
            try
            {
                var customer = new DatabaseEntities.CustomerEntity
                {
                    DocumentNumber = customerDto.DocumentNumber,
                    Name = customerDto.Name,
                    Lastname = customerDto.Lastname,
                    Age = customerDto.Age,
                    PhoneNumber = customerDto.PhoneNumber,
                    Email = customerDto.Email
                };

                await _customerRepository.AddAsync(customer);
                return Ok("Customer created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating customer: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving customer: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDTO customerDto)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                customer.DocumentNumber = customerDto.DocumentNumber;
                customer.Name = customerDto.Name;
                customer.Lastname = customerDto.Lastname;
                customer.Age = customerDto.Age;
                customer.PhoneNumber = customerDto.PhoneNumber;
                customer.Email = customerDto.Email;

                await _customerRepository.UpdateAsync(customer);

                return Ok("Customer updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating customer: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                await _customerRepository.DeleteAsync(customer);

                return Ok("Customer deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting customer: {ex.Message}");
            }
        }
    }
}
