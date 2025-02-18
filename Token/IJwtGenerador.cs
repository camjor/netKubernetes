using NetCamjor.Models;

namespace NetCamjor.Token;

public interface IJwtGenerador{
    string CrearToken (Usuario usuario);
}