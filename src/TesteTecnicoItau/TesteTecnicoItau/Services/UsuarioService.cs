using Microsoft.Extensions.Logging;
using TesteTecnicoItau.Entities;

namespace TesteTecnicoItau.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IUsuarioRepository repository, ILogger<UsuarioService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task CadastrarUsuarioAsync(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Email))
            {
                _logger.LogWarning("Tentativa de cadastro com nome ou email vazio.");
                throw new ArgumentException("Nome e email são obrigatórios.");
            }

            try
            {
                await _repository.AddAsync(usuario);
                _logger.LogInformation("Usuário cadastrado com sucesso. Id={Id}, Nome={Nome}", usuario.Id, usuario.Nome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar usuário.");
                throw;
            }
        }

        public async Task<IEnumerable<Usuario>> ListarUsuariosAsync()
        {
            try
            {
                var usuarios = await _repository.GetAllAsync();
                _logger.LogInformation("Listagem de usuários realizada. Total: {Total}", usuarios.Count());
                return usuarios;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar usuários.");
                throw;
            }
        }

        public async Task<Usuario?> BuscarUsuarioPorIdAsync(int id)
        {
            try
            {
                var usuario = await _repository.GetByIdAsync(id);
                _logger.LogInformation("Busca de usuário por ID. Id={Id}, Encontrado={Encontrado}", id, usuario != null);
                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuário. Id={Id}", id);
                throw;
            }
        }

        public async Task AtualizarUsuarioAsync(Usuario usuario)
        {
            try
            {
                await _repository.UpdateAsync(usuario);
                _logger.LogInformation("Usuário atualizado. Id={Id}, Nome={Nome}", usuario.Id, usuario.Nome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário. Id={Id}", usuario.Id);
                throw;
            }
        }

        public async Task RemoverUsuarioAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                _logger.LogInformation("Usuário removido. Id={Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover usuário. Id={Id}", id);
                throw;
            }
        }

        public async Task<Usuario?> BuscarPorIdAsync(int id)
        {
            return await BuscarUsuarioPorIdAsync(id);
        }
    }
}