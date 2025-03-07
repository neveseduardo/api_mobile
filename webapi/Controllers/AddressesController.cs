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

[Authorize(Policy = "AdminPolicy")]
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

    protected AddressViewModel GetViewModel(Address model)
    {
        var viewModel = new AddressViewModel
        {
            Id = model.Id,
            Logradouro = model.Logradouro,
            Cep = model.Cep,
            Bairro = model.Bairro,
            Cidade = model.Cidade,
            Estado = model.Estado,
            Pais = model.Pais,
            Numero = model.Numero,
            Complemento = model.Complemento,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
        };

        return viewModel;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();

        var viewModels = list.Select(a => GetViewModel(a)).ToList();

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

        var viewModel = GetViewModel(address);

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModel,
        }); ;
    }

    [HttpPost]
    public async Task<ActionResult> AddAsync([FromBody] CreateAddressDto dto)
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

            var result = await GetByIdAsync(address.Id);

            if (result.Result is ObjectResult objectResult)
            {
                objectResult.StatusCode = 201;
                return objectResult;
            }

            return StatusCode(201, result.Value);
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

            address.Logradouro = dto.Logradouro ?? address.Logradouro;
            address.Cep = dto.Cep ?? address.Cep;
            address.Bairro = dto.Bairro ?? address.Bairro;
            address.Cidade = dto.Cidade ?? address.Cidade;
            address.Estado = dto.Estado ?? address.Estado;
            address.Pais = dto.Pais ?? address.Pais;
            address.Numero = dto.Numero ?? address.Numero;
            address.Complemento = dto.Complemento ?? address.Complemento;

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