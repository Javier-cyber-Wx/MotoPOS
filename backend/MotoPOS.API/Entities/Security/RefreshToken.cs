using MotoPOS.API.Entities.Base;

namespace MotoPOS.API.Entities.Security;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    
    public DateTime ExpiraEn { get; set; }
    
    public DateTime CreadoEn { get; set; }
    
    public DateTime? RevocadoEn { get; set; }
    
    public string? MotivoRevocacion { get; set; }
    
    public bool EstaRevocado => RevocadoEn.HasValue;
    
    public bool EstaExpirado => DateTime.UtcNow >= ExpiraEn;
    
    public bool EstaActivo => !EstaRevocado && !EstaExpirado;
    
    public int UsuarioId { get; set; }
    
    public Usuario Usuario { get; set; } = null!;
    
    public string? JwtTokenId { get; set; }
}