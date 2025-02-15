using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/v1/clientes")]
public class ApiCustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<ApiCustomersController> _logger;

    public ApiCustomersController(ICustomerRepository customerRepository, ILogger<ApiCustomersController> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetCustomers()
    {
        _logger.LogInformation("Obter todos os clientes.");

        var customers = await _customerRepository.GetAllCustomersAsync();
        var viewModels = customers.Select(c => new CustomerViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Cpf = c.Cpf,
        }).ToList();

        return Ok(viewModels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerViewModel>> GetCustomer(int id)
    {
        _logger.LogInformation($"Obter cliente com ID: {id}");

        var customer = await _customerRepository.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            _logger.LogWarning($"Cliente com ID {id} não encontrado.");
            return NotFound();
        }

        var viewModel = new CustomerViewModel
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Cpf = customer.Cpf,
        };

        return Ok(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerViewModel>> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        _logger.LogInformation("Criar um novo cliente.");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Erros de validação encontrados ao criar cliente.");
            return BadRequest(ModelState);
        }

        var customer = new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = PasswordHelper.HashPassword(dto.Password),
        };

        await _customerRepository.AddCustomerAsync(customer);

        var viewModel = new CustomerViewModel
        {
            Name = customer.Name,
            Email = customer.Email,
            Cpf = customer.Cpf,
        };

        _logger.LogInformation($"Cliente criado com ID: {customer.Id}");
        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, viewModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto dto)
    {
        _logger.LogInformation($"Atualizar cliente com ID: {id}");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Erros de validação encontrados ao atualizar cliente.");
            return BadRequest(ModelState);
        }

        var customer = await _customerRepository.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            _logger.LogWarning($"Cliente com ID {id} não encontrado.");
            return NotFound();
        }

        customer.Name = dto.Name;
        customer.Email = dto.Email;
        customer.Cpf = dto.Cpf;

        await _customerRepository.UpdateCustomerAsync(customer);

        _logger.LogInformation($"Cliente com ID {id} atualizado com sucesso.");
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        _logger.LogInformation($"Excluir cliente com ID: {id}");

        var customer = await _customerRepository.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            _logger.LogWarning($"Cliente com ID {id} não encontrado.");
            return NotFound();
        }

        await _customerRepository.DeleteCustomerAsync(id);

        _logger.LogInformation($"Cliente com ID {id} excluído com sucesso.");
        return NoContent();
    }
}