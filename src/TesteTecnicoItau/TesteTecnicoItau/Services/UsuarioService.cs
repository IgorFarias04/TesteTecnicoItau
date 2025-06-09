using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteTecnicoItau.Entities;

namespace TesteTecnicoItau.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task CadastrarUsuarioAsync(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Email))
                throw new ArgumentException("Nome e email são obrigatórios.");

            await _repository.AddAsync(usuario);
        }

        public async Task<IEnumerable<Usuario>> ListarUsuariosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Usuario?> BuscarUsuarioPorIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AtualizarUsuarioAsync(Usuario usuario)
        {
            await _repository.UpdateAsync(usuario);
        }

        public async Task RemoverUsuarioAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Usuario?> BuscarPorIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
