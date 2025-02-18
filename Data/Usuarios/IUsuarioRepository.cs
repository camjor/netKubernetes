using NetCamjor.Dtos.UsuarioDtos;

namespace NetCamjor.Data.Usuarios;

public interface IUsuarioRepository {
    Task<UsuarioResponseDto> GetUsuario();
    Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto request);
    Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRequestDto request);
}