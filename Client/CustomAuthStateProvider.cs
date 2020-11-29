
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorBattles.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;

        public CustomAuthStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal();
            
            // when the Login component calls upon us, we'll wait for the isAuthenticated to be set
            // TODO - Here, we don't actually care if it is set to true or false (?!)
            if (await _localStorageService.GetItemAsync<bool>("isAuthenticated"))
            {
                // then we're going to establish the identity and a user with that identity
                var identity = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, "Steve")
                    }, "test authentication type"
                );
                user = new ClaimsPrincipal(identity);
            }
            // Whoever the user (authenticated or not) build a state
            var state = new AuthenticationState(user);
                
            // next we want to fire an event indicating the authentication changed and return the state.
            NotifyAuthenticationStateChanged(Task.FromResult<AuthenticationState>(state));
            return state;
        }
    }
}