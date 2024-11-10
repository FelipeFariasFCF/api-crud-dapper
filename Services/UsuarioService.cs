using AutoMapper;
using CrudDapper8.Dto;
using CrudDapper8.Models;
using Dapper;
using System.Data.SqlClient;

namespace CrudDapper8.Services
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UsuarioService(IConfiguration configuration, IMapper mapper) 
        { 
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ResponseModel<UsuarioListarDto>> BuscarUsuarioPorId(int usuarioId)
        {
            ResponseModel<UsuarioListarDto> response = new ResponseModel<UsuarioListarDto>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM Usuarios WHERE id = @Id", new {Id = usuarioId });
                if(usuarioBanco == null)
                {
                    response.Mensagem = "Nenhum usuário localizado!";
                    response.Status = false;
                    return response;
                }


                var usuarioMapeado = _mapper.Map<UsuarioListarDto>(usuarioBanco);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuário localizado com sucesso!";
            }
            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> BuscarUsuarios()
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuariosBanco = await connection.QueryAsync<Usuario>("SELECT * FROM Usuarios");
                if (usuariosBanco.Count() == 0)
                {
                    response.Mensagem = "Nenhum usuário localizado!";
                    response.Status = false;
                    return response;
                }

                var usuarioMapeado = _mapper.Map<List<UsuarioListarDto>>(usuariosBanco);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuários localizados com sucesso!";
            }
            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> CriarUsuario(UsuarioCriarDto usuarioCriarDto)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.ExecuteAsync("INSERT INTO Usuarios (NomeCompleto, Email, Cargo, Salario, CPF, Senha, Situacao) values (@NomeCompleto, @Email, @Cargo, @Salario, @CPF, @Senha, @Situacao)", usuarioCriarDto);
                if(usuarioBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao realizar o registro!";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);
                var usuarioMapeado = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados=usuarioMapeado;
                response.Mensagem = "Usuários listados com sucesso!";
            }
            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> EditarUsuario(UsuarioEditarDto usuarioEditarDto)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.ExecuteAsync("UPDATE Usuarios set NomeCompleto = @NomeCompleto, Email = @Email, Cargo = @Cargo, Salario = @Salario, Situacao = @Situacao, CPF = @CPF WHERE Id = @Id", usuarioEditarDto);
                if (usuarioBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao atualizar o registro!";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);
                var usuarioMapeado = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuários listados com sucesso!";
            }
            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> ExcluirUsuario(int usuarioId)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.ExecuteAsync("DELETE FROM Usuarios WHERE Id = @Id", new { Id = usuarioId });
                if (usuarioBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao excluir o registro!";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);
                var usuarioMapeado = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuários listados com sucesso!";
            }
            return response;
        }

        private static async Task<IEnumerable<Usuario>> ListarUsuarios(SqlConnection connection)
        {
            return await connection.QueryAsync<Usuario>("SELECT * FROM Usuarios");
        }     
    }
}
