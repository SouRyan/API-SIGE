using API.SIGE.Data;
using API.SIGE.Interfaces;
using API.SIGE.Models;
using Microsoft.EntityFrameworkCore;
using SIGE.API.Models;

namespace GerenciamentoProducao.Repositories
{
    public class FamiliaCaixilhoRepository : IFamiliaCaixilhoRepository
    {
        private readonly AppDbData _context;
        public FamiliaCaixilhoRepository(AppDbData context)
        {
            _context = context;
        }
        public async Task AddAsync(FamiliaCaixilho familiaCaixilho)
        {
            await _context.FamiliaCaixilhos.AddAsync(familiaCaixilho);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var FamiliaCaixilho = await _context.FamiliaCaixilhos.FindAsync(id);
            if (FamiliaCaixilho != null)
            {
                _context.FamiliaCaixilhos.Remove(FamiliaCaixilho);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<FamiliaCaixilho>> GetAllAsync()
        {
            return await _context.FamiliaCaixilhos.ToListAsync();
        }

        public async Task<FamiliaCaixilho> GetByIdAsync(int id)
        {
            return await _context.FamiliaCaixilhos.FindAsync(id);

        }

        public async Task UpdateAsync(FamiliaCaixilho familiaCaixilho)
        {
            // Buscar a entidade rastreada do contexto
            var familiaTracked = await _context.FamiliaCaixilhos.FindAsync(familiaCaixilho.IdFamiliaCaixilho);
            if (familiaTracked == null)
            {
                throw new InvalidOperationException($"FamiliaCaixilho com ID {familiaCaixilho.IdFamiliaCaixilho} não encontrado.");
            }

            // Atualizar as propriedades da entidade rastreada
            familiaTracked.DescricaoFamilia = familiaCaixilho.DescricaoFamilia;
            // Não atualizar PesoTotal aqui, pois é calculado automaticamente
            // familiaTracked.PesoTotal = familiaCaixilho.PesoTotal;

            await _context.SaveChangesAsync();
        }

        // Método para calcular o peso total automaticamente
        public async Task<float> CalcularPesoTotalAsync(int familiaId)
        {
            var caixilhos = await _context.Caixilhos
                .Where(c => c.IdFamiliaCaixilho == familiaId)
                .ToListAsync();

            return caixilhos.Sum(c => c.PesoUnitario * c.Quantidade);
        }

        // Método para atualizar o peso total de uma família
        public async Task AtualizarPesoTotalAsync(int familiaId)
        {
            var familia = await _context.FamiliaCaixilhos.FindAsync(familiaId);
            if (familia != null)
            {
                familia.PesoTotal = (int)await CalcularPesoTotalAsync(familiaId);
                await _context.SaveChangesAsync();
            }
        }
    }
}
