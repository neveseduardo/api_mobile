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
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/v1/enderecos")]
public class AddressesController : ControllerBase
{
    private readonly IRepository<Address> _repository;
    private readonly ILogger<AddressesController> _logger;

    public AddressesController(IRepository<Address> addressRepository, ILogger<AddressesController> logger)
    {
        _repository = addressRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();

        var viewModels = list.Select(a => new AddressViewModel
        {
            Id = a.Id,
            Logradouro = a.Logradouro,
            Cep = a.Cep,
            Bairro = a.Bairro,
            Cidade = a.Cidade,
            Estado = a.Estado,
            Pais = a.Pais,
            Numero = a.Numero,
            Complemento = a.Complemento
        }).ToList();

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModels,
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AddressViewModel>> GetByIdAsync(int id)
    {
        var address = await _repository.GetByIdAsync(id);

        if (address == null)
        {
            return StatusCode(404, new
            {
                success = true,
                message = "Item não encontrado",
                data = Array.Empty<object>(),
            });
        }

        var viewModel = new AddressViewModel
        {
            Id = address.Id,
            Logradouro = address.Logradouro,
            Cep = address.Cep,
            Bairro = address.Bairro,
            Cidade = address.Cidade,
            Estado = address.Estado,
            Pais = address.Pais,
            Numero = address.Numero,
            Complemento = address.Complemento
        };

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModel,
        }); ;
    }

    [HttpPost]
    public async Task<ActionResult<AddressViewModel>> AddAsync([FromBody] CreateAddressDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var address = new Address
            {
                Logradouro = dto.Logradouro,
                Cep = dto.Cep,
                Bairro = dto.Bairro,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                Pais = dto.Pais,
                Numero = dto.Numero,
                Complemento = dto.Complemento
            };

            await _repository.AddAsync(address);

            var viewModel = new AddressViewModel
            {
                Id = address.Id,
                Logradouro = address.Logradouro,
                Cep = address.Cep,
                Bairro = address.Bairro,
                Cidade = address.Cidade,
                Estado = address.Estado,
                Pais = address.Pais,
                Numero = address.Numero,
                Complemento = address.Complemento
            };

            return StatusCode(201, new
            {
                success = true,
                message = "Endereço criado com sucesso!",
                data = viewModel,
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao criar item");

            return StatusCode(400, new
            {
                success = true,
                message = "Falha ao criar item",
                trace = ex.Message,
                data = Array.Empty<object>(),
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateAddressDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var address = await _repository.GetByIdAsync(id);

            if (address == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            address.Logradouro = dto.Logradouro;
            address.Cep = dto.Cep;
            address.Bairro = dto.Bairro;
            address.Cidade = dto.Cidade;
            address.Estado = dto.Estado;
            address.Pais = dto.Pais;
            address.Numero = dto.Numero;
            address.Complemento = dto.Complemento;

            await _repository.UpdateAsync(address);

            return StatusCode(200, new
            {
                success = true,
                message = "Dados atualizados com sucesso",
                data = Array.Empty<object>(),
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao atualizar item");

            return StatusCode(400, new
            {
                success = true,
                message = "Falha ao atualizar item",
                trace = ex.Message,
                data = Array.Empty<object>(),
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var address = await _repository.GetByIdAsync(id);

            if (address == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            await _repository.DeleteAsync(address);

            return StatusCode(200, new
            {
                success = true,
                message = "Objeto deletado com sucesso",
                data = Array.Empty<object>(),
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao atualizar item");

            return StatusCode(400, new
            {
                success = true,
                message = "Falha ao atualizar item",
                trace = ex.Message,
                data = Array.Empty<object>(),
            });
        }
    }
}