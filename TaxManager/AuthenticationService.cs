using TaxManager.Contants;

namespace TaxManager;

public class AuthenticationService: IAuthenticationService
{
    public bool CheckIfHavePermission(Roles expectedRole, Roles currentRole)
    {
        return expectedRole == currentRole;
    }
}
