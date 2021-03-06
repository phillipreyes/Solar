﻿using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Solar.Model;
using System.Net.Http;
using System.Diagnostics;

namespace Solar.Controller
{
    // Object created to Log the User in 
    // Returning Authorization token upon success
    class Login
    {
        //testss
        HttpClient client;
        private User User;
        //private List<PlantDTO> PlantItems;        
        private TokenDTO tokenobj;
        // Log in takes user credentials
        public Login(User user)
        {
            User = user;
            client = new HttpClient();
            tokenobj = new TokenDTO();
        }
        public async Task<Tuple<TokenDTO, bool>> TryLogin()
        {
            // Returns an authorization token for required by the other Web API 
            // Returns boolean for whether it was successful
            
            bool isAuth = false;
            
            Debug.WriteLine("trying login!");
            
            // The URL for this login service
            var uri = "http://fsdevweb.azurewebsites.net/api/account/authenticate";
            Debug.WriteLine("------try---------");
            try
            {                                                
                var prms = new List<KeyValuePair<string, string>>
                    {
                        
                        new KeyValuePair<string, string>("username", User.email),
                        new KeyValuePair<string, string>("password", User.password),
                        new KeyValuePair<string, string>("grant_type", "password")
                    };                              


                Debug.WriteLine(prms.ToString());
                // HTTP response message given from the Web Service
                // Includes the aforementioned authorization token
                var response =  await client.PostAsync(uri, new FormUrlEncodedContent(prms));

                Debug.WriteLine("-------------before status code------------");
                Debug.WriteLine(response);
                Debug.WriteLine("");
                                          
                // if the web service returns successfully
                if (response.IsSuccessStatusCode)
                {
                    isAuth = true;
                    Debug.WriteLine("================after status code=============");
                    // The authorization token is deserialized from JSON into ourC# TokenDTO object we use elsewhere                    
                   tokenobj = JsonConvert.DeserializeObject<TokenDTO>(response.Content.ReadAsStringAsync().Result);
                    Debug.WriteLine("access_token : " + tokenobj.access_token + "\ntoken_ type : " + tokenobj.token_type + "\nexpires_in : " + tokenobj.expires_in +
                    "\nuserName : " + tokenobj.userName + "\n.issued : " + tokenobj.issued + "\n.expires : " + tokenobj.expires);
                    

                }

           }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            Debug.WriteLine("returning ");
            return new Tuple<TokenDTO, bool>(tokenobj, isAuth);
        }
    }
    
}
