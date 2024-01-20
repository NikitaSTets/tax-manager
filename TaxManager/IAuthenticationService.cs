using TaxManager.Contants;

namespace TaxManager;

public interface IAuthenticationService
{
    public bool CheckIfHavePermission(Roles expectedRole, Roles currentRole);
}
