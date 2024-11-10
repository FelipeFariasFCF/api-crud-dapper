using CrudDapper8.Dto;
using CrudDapper8.Models;

namespace CrudDapper8.Services
{
    public interface IUsuarioInterface
    {
        Task<ResponseModel<List<UsuarioListarDto>>> BuscarUsuarios();
        Task<ResponseModel<UsuarioListarDto>> BuscarUsuarioPorId(int usuarioId);
        Task<ResponseModel<List<UsuarioListarDto>>> CriarUsuario(UsuarioCriarDto usuarioCriarDto);
        Task<ResponseModel<List<UsuarioListarDto>>> EditarUsuario(UsuarioEditarDto usuarioEditarDto);
        Task<ResponseModel<List<UsuarioListarDto>>> ExcluirUsuario(int usuarioId);
    }
}
