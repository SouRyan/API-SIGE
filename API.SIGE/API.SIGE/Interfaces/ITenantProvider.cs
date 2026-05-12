namespace API.SIGE.Interfaces;

public interface ITenantProvider
{
    int GetTenantId();
    bool HasTenant();
}
