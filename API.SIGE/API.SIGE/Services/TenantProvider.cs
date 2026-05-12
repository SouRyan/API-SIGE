using System.Security.Claims;
using API.SIGE.Interfaces;

namespace API.SIGE.Services;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetTenantId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("empresaId");
        return claim != null && int.TryParse(claim.Value, out var id) ? id : 0;
    }

    public bool HasTenant()
    {
        return GetTenantId() > 0;
    }
}
