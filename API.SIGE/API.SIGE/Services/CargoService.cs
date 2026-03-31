using API.SIGE.DTOs;
using API.SIGE.Interfaces.Repositories;
using API.SIGE.Interfaces.Services;
using API.SIGE.Model;

namespace API.SIGE.Services;

public class CargoService : ICargoService
{
    private readonly ICargoRepository _cargoRepository;

    public CargoService(ICargoRepository cargoRepository)
    {
        _cargoRepository = cargoRepository;
    }

    public async Task<List<CargoResponseDto>> GetAllAsync()
    {
        var list = await _cargoRepository.GetAllAsync();
        return list.Select(Map).ToList();
    }

    public async Task<CargoResponseDto?> GetByIdAsync(int id)
    {
        var c = await _cargoRepository.GetByIdAsync(id);
        return c == null ? null : Map(c);
    }

    public async Task<CargoResponseDto> CreateAsync(CargoCreateDto dto)
    {
        var entity = new Cargo
        {
            TipoCargo = dto.TipoCargo,
            DescricaoCargo = dto.DescricaoCargo
        };
        await _cargoRepository.AddAsync(entity);
        return Map(entity);
    }

    public async Task UpdateAsync(int id, CargoCreateDto dto)
    {
        var entity = await _cargoRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Cargo não encontrado.");
        entity.TipoCargo = dto.TipoCargo;
        entity.DescricaoCargo = dto.DescricaoCargo;
        await _cargoRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id) => await _cargoRepository.DeleteAsync(id);

    private static CargoResponseDto Map(Cargo c) => new()
    {
        IdCargo = c.IdCargo,
        TipoCargo = c.TipoCargo,
        DescricaoCargo = c.DescricaoCargo
    };
}
