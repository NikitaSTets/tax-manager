using TaxManager.Contants;

namespace TaxManager;

public interface IAuthenticationService
{
    public bool CheckIfActionAllowed(Roles expectedRole, Roles currentRole);
}
