using API.SIGE.Model;

namespace API.SIGE.Interfaces.Repositories
{
    public interface IEmpresaRepository
    {

        Task<List<Empresa>> GetAllAsync();
        Task<Empresa?> GetById(int id);
        Task<Empresa?> GetInativados(int id);
        Task AddAsync(Empresa empresa);
        Task UpdateAsync(Empresa empresa);
        Task DeleteAsync(int id);

    }
}
