using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenAuthenticationWEBAPI.Models
{
    public class ClientMaster
    {
        public ClientMaster(int clientKeyId, string clientId, string clientSecret, string clientName, DateTime createdOn)
        {
            this.ClientKeyId = clientKeyId;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.ClientName = clientName;
            this.CreatedOn = createdOn;
        }
        public int ClientKeyId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientName { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class ClientMasterRepository : IDisposable
    {
        // SECURITY_DBEntities it is your context class
        //SECURITY_DBEntities context = new SECURITY_DBEntities();

        public enum eCLIENTS
        {
            STANDARD = 1,
            WP = 2,
            ODC = 3
        }

        private List<ClientMaster> _clientMasterList = new List<ClientMaster>()
        {
            new ClientMaster((int) eCLIENTS.STANDARD, "C1A03B10-7D59-407A-A93E-B71AB17AD8C2", "177E3295-0656-4317-BC91-DD271A19ACFF","Standard",new DateTime(2001,1,1)),
            new ClientMaster((int) eCLIENTS.WP , "D1A03B10-7D59-407A-A93E-B71AB17AD8C2", "277E3295-0656-4317-BC91-DD271A19ACFF","WP",new DateTime(2002,1,1)),
            new ClientMaster((int) eCLIENTS.ODC, "E1A03B10-7D59-407A-A93E-B71AB17AD8C2", "377E3295-0656-4317-BC91-DD271A19ACFF","ODC",new DateTime(2003,1,1)),
        };

        //This method is used to check and validate the Client credentials
        public ClientMaster ValidateClient(string ClientID, string ClientSecret)
        {
            return this._clientMasterList.FirstOrDefault(user =>
                        user.ClientId == ClientID
                        && user.ClientSecret == ClientSecret);
        }
        public void Dispose()
        {
            //context.Dispose();
        }

        public ClientMaster Get(string clientID)
        {
            foreach (ClientMaster client in this._clientMasterList)
            {
                if (client.ClientId.Equals(clientID))
                    return client;
            }

            return null;
        }
    }
}