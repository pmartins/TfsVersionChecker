using System;
using System.Configuration;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;

namespace TfsVersionChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            // "http://s-ctrlverp-s01.besp.dsp.gbes:8080/"
            // string tfsServerUrl = "http://s-sftmfsp-s01:8080/";
            string tfsServerUrl = ConfigurationManager.AppSettings["TFS_SERVER_URL"] as string;
            TeamFoundationServer server = new TeamFoundationServer(tfsServerUrl);
            IRegistration registrationService = (IRegistration)server.GetService(typeof(IRegistration));

            RegistrationEntry[] frameworkEntries = registrationService.GetRegistrationEntries("Framework");
            if (frameworkEntries.Length > 0)
            {
                //We are talking to TFS 2010
                Console.WriteLine("We are talking to TFS 2010");
            }
            else
            {
                RegistrationEntry[] vstfsEntries = registrationService.GetRegistrationEntries("vstfs");
                if (vstfsEntries.Length != 1)
                {
                    //We must be talking to an unknown version of TFS
                    Console.WriteLine("We must be talking to an unknown version of TFS");
                }
                else
                {
                    Boolean groupSecurity2Found = false;
                    foreach (ServiceInterface serviceInterface in vstfsEntries[0].ServiceInterfaces)
                    {
                        if (serviceInterface.Name.Equals("GroupSecurity2", StringComparison.OrdinalIgnoreCase))
                        {
                            groupSecurity2Found = true;
                        }
                    }

                    if (groupSecurity2Found)
                    {
                        //We are talking to TFS 2008
                        Console.WriteLine("We are talking to TFS 2008");
                    }
                    else
                    {
                        //We are talking to TFS 2005
                        Console.WriteLine("We are talking to TFS 2005");
                    }
                }
            }
                        
            Console.ReadKey();
        }
    }
}
