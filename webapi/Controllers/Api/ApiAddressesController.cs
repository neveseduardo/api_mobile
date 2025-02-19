using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Repositories.Api;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/v1/enderecos")]
public class ApiAddressesController : ControllerBase
{
    private readonly IApiAddressRepository _addressRepository;
    private readonly ILogger<ApiAddressesController> _logger;

    public ApiAddressesController(IApiAddressRepository addressRepository, ILogger<ApiAddressesController> logger)
    {
        _addressRepository = addressRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressViewModel>>> GetAddresses()
    {
        _logger.LogInformation("Obter todos os endereços.");

        var addresses = await _addressRepository.GetAllAddressesAsync();
        var viewModels = addresses.Select(a => new AddressViewModel
        {
            Id = a.Id,
            Logradouro = a.Logradouro,
            Cep = a.Cep,
            Cidade = a.Cidade,
            Estado = a.Estado,
            Pais = a.Pais,
            Numero = a.Numero,
            Complemento = a.Complemento
        }).ToList();

        return Ok(viewModels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AddressViewModel>> GetAddress(int id)
    {
        _logger.LogInformation($"Obter endereço com ID: {id}");

        var address = await _addressRepository.GetAddressByIdAsync(id);
        if (address == null)
        {
            _logger.LogWarning($"Endereço com ID {id} não encontrado.");
            return NotFound();
        }

        var viewModel = new AddressViewModel
        {
            Id = address.Id,
            Logradouro = address.Logradouro,
            Cep = address.Cep,
            Cidade = address.Cidade,
            Estado = address.Estado,
            Pais = address.Pais,
            Numero = address.Numero,
            Complemento = address.Complemento
        };

        return Ok(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult<AddressViewModel>> CreateAddress([FromBody] CreateAddressDto dto)
    {
        _logger.LogInformation("Criar um novo endereço.");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Erros de validação encontrados ao criar endereço.");
            return BadRequest(ModelState);
        }

        var address = new Address
        {
            Logradouro = dto.Logradouro,
            Cep = dto.Cep,
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            Pais = dto.Pais,
            Numero = dto.Numero,
            Complemento = dto.Complemento
        };

        await _addressRepository.AddAddressAsync(address);

        var viewModel = new AddressViewModel
        {
            Id = address.Id,
            Logradouro = address.Logradouro,
            Cep = address.Cep,
            Cidade = address.Cidade,
            Estado = address.Estado,
            Pais = address.Pais,
            Numero = address.Numero,
            Complemento = address.Complemento
        };

        _logger.LogInformation($"Endereço criado com ID: {address.Id}");
        return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, viewModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAddress(int id, [FromBody] UpdateAddressDto dto)
    {
        _logger.LogInformation($"Atualizar endereço com ID: {id}");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Erros de validação encontrados ao atualizar endereço.");
            return BadRequest(ModelState);
        }

        var address = await _addressRepository.GetAddressByIdAsync(id);
        if (address == null)
        {
            _logger.LogWarning($"Endereço com ID {id} não encontrado.");
            return NotFound();
        }

        address.Logradouro = dto.Logradouro;
        address.Cep = dto.Cep;
        address.Cidade = dto.Cidade;
        address.Estado = dto.Estado;
        address.Pais = dto.Pais;
        address.Numero = dto.Numero;
        address.Complemento = dto.Complemento;

        await _addressRepository.UpdateAddressAsync(address);

        _logger.LogInformation($"Endereço com ID {id} atualizado com sucesso.");
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        _logger.LogInformation($"Excluir endereço com ID: {id}");

        var address = await _addressRepository.GetAddressByIdAsync(id);
        if (address == null)
        {
            _logger.LogWarning($"Endereço com ID {id} não encontrado.");
            return NotFound();
        }

        await _addressRepository.DeleteAddressAsync(id);

        _logger.LogInformation($"Endereço com ID {id} excluído com sucesso.");
        return NoContent();
    }
}