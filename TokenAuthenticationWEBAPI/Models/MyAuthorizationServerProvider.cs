using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;


namespace TokenAuthenticationWEBAPI.Models
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            // The TryGetBasicCredentials method checks the Authorization header and
            // Return the ClientId and clientSecret
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.SetError("invalid_client", "Client credentials could not be retrieved through the Authorization header.");
                context.Rejected();
                return;
            }
            //Check the existence of by calling the ValidateClient method
            ClientMaster client = (new ClientMasterRepository()).ValidateClient(clientId, clientSecret);
            if (client != null)
            {
                // Client has been verified.
                context.OwinContext.Set<ClientMaster>("oauth:client", client);
                context.Validated(clientId);
            }
            else
            {
                // Client could not be validated.
                context.SetError("invalid_client", "Client credentials are invalid.");
                context.Rejected();
            }
            context.Validated();
        }


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserMasterRepository _repo = new UserMasterRepository())
            {
                // identifichiamo il client
                ClientMasterRepository repo = new ClientMasterRepository();
                ClientMaster client = repo.Get(context.ClientId);

                UserMaster user = null;
                switch ((ClientMasterRepository.eCLIENTS)client.ClientKeyId)
                {
                    // classic username/password
                    case ClientMasterRepository.eCLIENTS.STANDARD:
                        user = _repo.ValidateUser(context.UserName, context.Password);
                        break;

                    // token (username)
                    case ClientMasterRepository.eCLIENTS.ODC:
                    case ClientMasterRepository.eCLIENTS.WP:
                        user = _repo.ValidateUser(context.UserName);
                        break;

                    default:
                        user = _repo.ValidateUser(context.UserName, context.Password);
                        break;
                }
               
                if (user == null)
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    return;
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, user.UserRoles));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                identity.AddClaim(new Claim("Email", user.UserEmailID));
                identity.AddClaim(new Claim("ClientName", client.ClientName));
                context.Validated(identity);
            }
        }
    }
}