using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Repositories;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers;

[Authorize(Policy = "AdminPolicy")]
[ApiController]
[Route("api/v1/unidades")]
public class MedicalCenterController : ControllerBase
{
    private readonly IRepository<MedicalCenter> _repository;
    private readonly IRepository<Address> _addressRepository;
    private readonly ILogger<MedicalCenterController> _logger;

    public MedicalCenterController(
        IRepository<MedicalCenter> repository,
        IRepository<Address> addressRepository,
        ILogger<MedicalCenterController> logger
    )
    {
        _addressRepository = addressRepository;
        _repository = repository;
        _logger = logger;
    }

    protected MedicalCenterViewModel? GetViewModel(MedicalCenter? medicalCenter)
    {
        if (medicalCenter != null)
        {
            AddressViewModel? address = null;

            if (medicalCenter.Address != null)
            {
                address = new AddressViewModel
                {
                    Id = medicalCenter.Address.Id,
                    Logradouro = medicalCenter.Address.Logradouro,
                    Cep = medicalCenter.Address.Cep,
                    Bairro = medicalCenter.Address.Bairro,
                    Cidade = medicalCenter.Address.Cidade,
                    Estado = medicalCenter.Address.Estado,
                    Pais = medicalCenter.Address.Pais,
                    Numero = medicalCenter.Address.Numero,
                    Complemento = medicalCenter.Address.Complemento
                };
            }


            var viewModel = new MedicalCenterViewModel
            {
                Id = medicalCenter.Id,
                Name = medicalCenter.Name,
                PhoneNumber = medicalCenter.PhoneNumber,
                Email = medicalCenter.Email,
                address = address,
            };

            return viewModel;
        }
        return null;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalCenterViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        var viewModelList = list.Select(u => GetViewModel(u)).ToList();

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModelList,
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<MedicalCenterViewModel>>> GetByIdAsync([FromRoute] int id)
    {
        var MedicalCenter = await _repository.GetByIdAsync(id);

        if (MedicalCenter == null)
        {
            return StatusCode(404, new
            {
                success = true,
                message = "Item não encontrado",
                data = Array.Empty<object>(),
            });
        }

        var viewModel = GetViewModel(MedicalCenter);

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModel,
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateMedicalCenterDto dto)
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

            var medicalCenter = new MedicalCenter
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
            };

            await _addressRepository.AddAsync(address);

            if (address.Id != 0)
            {
                medicalCenter.AddressId = address.Id;
            }

            await _repository.AddAsync(medicalCenter);

            var result = await GetByIdAsync(medicalCenter.Id);

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

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateMedicalCenterDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicalCenter = await _repository.GetByIdAsync(Id);

            if (medicalCenter == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            medicalCenter.Name = dto.Name ?? medicalCenter.Name;
            medicalCenter.Email = dto.Email ?? medicalCenter.Email;
            medicalCenter.PhoneNumber = dto.PhoneNumber ?? medicalCenter.PhoneNumber;

            await _repository.UpdateAsync(medicalCenter);

            if (medicalCenter.Address != null)
            {
                var address = await _addressRepository.GetByIdAsync(medicalCenter.Address.Id);

                if (address != null)
                {
                    address.Logradouro = dto.Logradouro ?? address.Logradouro;
                    address.Cep = dto.Cep ?? address.Cep;
                    address.Bairro = dto.Bairro ?? address.Bairro;
                    address.Cidade = dto.Cidade ?? address.Cidade;
                    address.Estado = dto.Estado ?? address.Estado;
                    address.Pais = dto.Pais ?? address.Pais;
                    address.Numero = dto.Numero ?? address.Numero;
                    address.Complemento = dto.Complemento ?? address.Complemento;

                    await _addressRepository.UpdateAsync(address);
                }
            }

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
                success = false,
                message = "Falha ao atualizar item",
                trace = ex.Message,
                data = Array.Empty<object>(),
            });
        }

    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            var MedicalCenter = await _repository.GetByIdAsync(id);

            if (MedicalCenter == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            await _repository.DeleteAsync(MedicalCenter);

            return StatusCode(200, new
            {
                success = true,
                message = "Dados deletados com sucesso",
                data = Array.Empty<object>(),
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao deletar item");

            return StatusCode(400, new
            {
                success = false,
                message = "Falha ao deletar item",
                trace = ex.Message,
                data = Array.Empty<object>(),
            });
        }
    }
}