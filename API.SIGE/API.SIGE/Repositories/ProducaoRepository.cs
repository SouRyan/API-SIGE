//using API.SIGE.Data;
//using API.SIGE.Interfaces;
//using API.SIGE.Models;
//using Microsoft.EntityFrameworkCore;
//using SIGE.API.Models;

//namespace GerenciamentoProducao.Repositories
//{
//    public class ProducaoRepository : IProducaoRepository
//    {
//        private readonly AppDbData _context;
//        public ProducaoRepository(AppDbData context)
//        {
//            _context = context;
//        }
//        public async Task AddAsync(Producao producao)
//        {
//            await _context.Producoes.AddAsync(producao);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var producao = await _context.Producoes.FindAsync(id);
//            if (producao != null)
//            {
//                _context.Producoes.Remove(producao);
//                await _context.SaveChangesAsync();
//            }
//        }

//        public async Task<List<Producao>> GetAllAsync()
//        {
//            return await _context.Producoes.Include(p => p.Usuario).Include(p => p.FamiliaCaixilho).ToListAsync();

//        }

//        public async Task<Producao> GetByIdAsync(int id)
//        {
//            return await _context.Producoes.FindAsync(id);
//        }

//        public async Task UpdateAsync(Producao producao)
//        {
//            _context.Producoes.Update(producao);
//            await _context.SaveChangesAsync();
//        }
//    }
//}
