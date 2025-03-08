using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebApi.Helpers;

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

    protected MedicalCenterViewModel GetViewModel(MedicalCenter medicalCenter)
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalCenterViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        var viewModels = list.Select(u => GetViewModel(u)).ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<MedicalCenterViewModel>>> GetByIdAsync([FromRoute] int id)
    {
        var model = await _repository.GetByIdAsync(id);

        if (model == null)
        {
            return StatusCode(404, ApiHelper.NotFound());
        }

        var viewModel = GetViewModel(model);

        return StatusCode(200, ApiHelper.Ok(viewModel));
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateMedicalCenterDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
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

            var model = new MedicalCenter
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
            };

            await _addressRepository.AddAsync(address);

            if (address.Id != 0)
            {
                model.AddressId = address.Id;
            }

            await _repository.AddAsync(model);

            var result = await GetByIdAsync(model.Id);

            if (result.Result is ObjectResult objectResult)
            {
                objectResult.StatusCode = 201;
                return objectResult;
            }

            return StatusCode(201, result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateMedicalCenterDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
            }

            var model = await _repository.GetByIdAsync(Id);

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            model.Name = dto.Name ?? model.Name;
            model.Email = dto.Email ?? model.Email;
            model.PhoneNumber = dto.PhoneNumber ?? model.PhoneNumber;
            model.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(model);

            if (model.Address != null)
            {
                var address = await _addressRepository.GetByIdAsync(model.Address.Id);

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

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var model = await _repository.GetByIdAsync(id);

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            await _repository.DeleteAsync(model);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}