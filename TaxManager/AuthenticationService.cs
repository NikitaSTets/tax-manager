using TaxManager.Contants;

namespace TaxManager;

public class AuthenticationService: IAuthenticationService
{
    public bool CheckIfActionAllowed(Roles expectedRole, Roles currentRole)
    {
        return expectedRole == currentRole;
    }
}
