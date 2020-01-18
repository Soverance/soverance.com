//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Drive.v3;
//using Google.Apis.Drive.v3.Data;
//using Google.Apis.Json;
//using Google.Apis.Requests;
//using Google.Apis.Services;
//using Google.Apis.Util.Store;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using System.Data;
//using System.Data.OleDb;

//// In order to use this class, you must install the following prerequisites:

//// - Google API .NET Library - https://developers.google.com/api-client-library/dotnet/get_started
//// - Google OAuth2 Library - https://developers.google.com/identity/protocols/OAuth2
//// - Google DCM/DFA Reporting and Trafficking API for .NET Library version 3.0 - https://developers.google.com/doubleclick-advertisers/getting_started

//// Google API Service Account Credential References:
//// - You must have created a service account within your Google API project that has "Domain-Wide Delegation" enabled for it
//// - You can then use this API service account to impersonate other accounts within the Google-managed domain
//// - see here for more info:  https://developers.google.com/api-client-library/php/auth/service-accounts#delegatingauthority
//// - This service account must be granted API access to various services in our Google-managed domain by using the Google Admin portal.
//// - Within the Google Admin console, go to "Security -> Advanced -> Manage API Client Access"
//// - The client ID for this app is 111845787581380415754
//// - The client ID must be granted the following API scopes:
//// - https://www.googleapis.com/auth/dfareporting 

//namespace soverance.com.Helpers
//{
//    class TeamDrives
//    {
//        // This is the return string used by the primary HttpGet method from the AsyncController
//        static string getOutput = "";
        
//        // configure a storage array for the OAuth 2.0 scopes we will request
//        private static readonly IEnumerable<string> OAuthScopes = new[] {
//            DriveService.Scope.Drive
//        };

//        // Get the Google API service account credentials from the JSON file (GoogleApiKeys.json)
//        private static ServiceAccountCredential GetServiceAccountCredentials(String pathToJsonFile, String emailToImpersonate)
//        {
//            // Load and deserialize credential parameters from the specified JSON file.
//            JsonCredentialParameters parameters;
//            using (Stream json = new FileStream(pathToJsonFile, FileMode.Open, FileAccess.Read))
//            {
//                parameters = NewtonsoftJsonSerializer.Instance.Deserialize<JsonCredentialParameters>(json);
//            }

//            // Create a credential initializer with the correct scopes.
//            ServiceAccountCredential.Initializer initializer =
//                new ServiceAccountCredential.Initializer(parameters.ClientEmail)
//                {
//                    Scopes = OAuthScopes
//                };

//            // Configure impersonation (if applicable).
//            // This does nothing if the string is empty
//            if (!String.IsNullOrEmpty(emailToImpersonate))
//            {
//                initializer.User = emailToImpersonate;
//            }

//            // Create a service account credential object using the deserialized private key.
//            ServiceAccountCredential credential =
//                new ServiceAccountCredential(initializer.FromPrivateKey(parameters.PrivateKey));

//            return credential;
//        }

//        // Connect to Google Drive Api with a service account - DEPRECATED
//        public static void ConnectToServiceAccount(DriveService service, string emailToImpersonate)
//        {
//            // This file is obtained from the Google API portal, in the "Credentials -> Service Account Keys" section
//            // You must download that file from the portal, and copy it's contents into the GoogleApiKeys.json file located in this project
//            // Note that the GoogleApiKeys.json file has been configured to copy to the application's build output directory (THIS METHOD IS INSECURE!)
//            // This could be secured by removing the build copy, and distributing the .json file separately from the application via Windows AD Group Policy 
//            // (distributing the keys separately would limit application usage to specified domain users)
//            // You would then replace this string with a full, direct local path to the .json file

//            string pathToJsonFile = Directory.GetCurrentDirectory() + "\\GoogleServiceKeys.json";

//            // The "emailToImpersonate" variable allows the user of this application to specify a DCM account to impersonate
//            // This is only applicable to service accounts which have enabled domain-wide delegation
//            // Setting this field will not allow you to impersonate a user from a domain you don't own (e.g., gmail.com).

//            // Build service account credential.
//            ServiceAccountCredential credential =
//                GetServiceAccountCredentials(pathToJsonFile, emailToImpersonate);

//            // validate the credentials
//            if (credential != null)
//            {
//                getOutput = "Authentication for " + credential.Id.ToString() + " was successful." + Environment.NewLine;
//                Console.WriteLine(getOutput);
//            }
//            else
//            {
//                getOutput = "Failed to obtain service account credentials." + Environment.NewLine;
//                Console.WriteLine(getOutput);
//            }

//            // Create a new DriveService object.
//            service = new DriveService(new BaseClientService.Initializer()
//                {
//                    HttpClientInitializer = credential,
//                    ApplicationName = "Team Drives API Interface"
//                }
//            );

//            // this is just a simple debug check to make sure the reporting service was successfully initialized
//            if (service != null)
//            {
//                getOutput = "Google Api service was successfully initialized." + Environment.NewLine;
//                Console.WriteLine(getOutput);
//            }
//        }

//        // Connect to Google Drive Api with a user account - NECESSARY FOR TEAM DRIVES, because Team Drives cannot be owned by a service account.
//        public static UserCredential ConnectToOAuthUser()
//        {
//            UserCredential credential;

//            using (var stream =
//                new FileStream("GoogleOAuthKeys.json", FileMode.Open, FileAccess.Read))
//            {
//                // The file token.json stores the user's access and refresh tokens, and is created
//                // automatically when the authorization flow completes for the first time.
//                string credPath = "token.json";
//                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
//                    GoogleClientSecrets.Load(stream).Secrets,
//                    OAuthScopes,
//                    "user",
//                    CancellationToken.None,
//                    new FileDataStore(credPath, true)).Result;
//                Console.WriteLine("Credential file saved to: " + credPath);
//            }

//            // validate the credentials
//            if (credential != null)
//            {
//                getOutput = "Authentication for " + credential.UserId.ToString() + " was successful." + Environment.NewLine;
//                Console.WriteLine(getOutput);
//            }
//            else
//            {
//                getOutput = "Failed to obtain service account credentials." + Environment.NewLine;
//                Console.WriteLine(getOutput);
//            }            
            
//            return credential;
//        }

//        // Lists all Team Drives in the current user context (admin@drumagency.com)
//        public static IList<TeamDrive> ListTeamDrives(DriveService service)
//        {
//            try
//            {
//                List<TeamDrive> teamDrives = new List<TeamDrive>();  // initialize a list of team drives that we'll eventually return to the caller of this function
//                string pageToken = null;  // page token must start null to make the initial request 
//                string fields = "teamDrives(kind, id, name),nextPageToken";
//                int pageSize = 100;
                                
//                do
//                {
//                    var teamDriveList = service.Teamdrives.List();  // initial list request
//                    teamDriveList.Fields = fields;
//                    teamDriveList.PageSize = pageSize;
//                    teamDriveList.PageToken = pageToken;  // page token is updated with a value after the initial list request, returning the first page of results by default

//                    var result = teamDriveList.Execute();
//                    var teamDrivesPage = result.TeamDrives;  // collect all drives on this page

//                    if (teamDrivesPage != null && teamDrivesPage.Count > 0)
//                    {
//                        foreach (var drive in teamDrivesPage)
//                        {
//                            Console.WriteLine("{0} ({1})", drive.Name, drive.Id);  // print this drive to the console
//                            teamDrives.Add(drive);  // add the drive to our return list
//                        }

//                        pageToken = result.NextPageToken;  // go to the next page of results
//                    }

//                } while (!String.IsNullOrEmpty(pageToken));  // process this loop while pageToken is anything but null

                
//                return teamDrives;  // return the full drive list
//            }
//            // any exceptions thrown during this process will be printed to the console, and the app restarted
//            catch (Exception ex)
//            {
//                string error = "Error returned from the service: " + Environment.NewLine + ex.Message;
//                getOutput = error;
//                Console.WriteLine(getOutput);
//                Console.Read();
//                return null;
//            }            
//        }

//        // Creates a new team drive with the specified name
//        public static void CreateNewTeamDrive(DriveService service, DrumFolder drive)
//        {
//            try
//            {
//                var teamDriveMetadata = new TeamDrive()
//                {
//                    Name = drive.FolderName
//                };

//                var requestId = System.Guid.NewGuid().ToString();
//                var request = service.Teamdrives.Create(teamDriveMetadata, requestId);
//                request.Fields = "id";
//                var teamDrive = request.Execute();
//                Console.WriteLine("Created Team Drive ID: " + teamDrive.Id + Environment.NewLine);

//                ModifyTeamDrivePermissions(service, teamDrive, drive.SecurityGroup);
                
//            }
//            // any exceptions thrown during this process will be printed to the console, and the app restarted
//            catch (Exception ex)
//            {
//                string error = "Error returned from the service: " + Environment.NewLine + ex.Message;
//                getOutput = error;
//                Console.WriteLine(getOutput);
//                Console.Read();
//            }            
//        }

//        // Modifies the new Team Drive with the appropriate permissions
//        public static void ModifyTeamDrivePermissions(DriveService service, TeamDrive drive, string group)
//        {
//            // PERMISSION STRUCTURE FOR TEAM DRIVES
//            // organizer = "Manager"
//            // fileOrganizer = "Content Manager"
//            // writer = "Contributor"
//            // commenter = "Commenter"
//            // reader = "Reader"

//            try
//            {
//                var batch = new BatchRequest(service);
//                BatchRequest.OnResponse<Permission> callback = delegate (
//                    Permission permission, 
//                    RequestError error,
//                    int index,
//                    System.Net.Http.HttpResponseMessage message)
//                {
//                    if (error != null)
//                    {
//                        // Handle batch error
//                        Console.WriteLine(error.Message);
//                    }
//                    else
//                    {
//                        Console.WriteLine("Permission ID: " + permission.Id);
//                    }
//                };

//                // we want to always add the support@drumagency.com group to every folder with organizer permissions
//                Permission supportPermission = new Permission()
//                {
//                    Type = "group",
//                    Role = "organizer",
//                    EmailAddress = "support@soverance.com"
//                };

//                var requestSupportPermission = service.Permissions.Create(supportPermission, drive.Id);
//                requestSupportPermission.Fields = "id";
//                requestSupportPermission.SupportsTeamDrives = true;
//                requestSupportPermission.SendNotificationEmail = false;
//                //requestSupportPermission.UseDomainAdminAccess = true;  // for whatever reason this line makes the program error out... maybe because admin@drumagency.com is already a full domain admin?
//                batch.Queue(requestSupportPermission, callback);

//                // if this function was called with a custom group, add it to the permissions list with fileOrganizer permissions
//                if (group != null)
//                {
//                    // create the permission structure for the client security groups
//                    Permission groupPermission = new Permission()
//                    {
//                        Type = "group",
//                        Role = "fileOrganizer",
//                        EmailAddress = group
//                    };

//                    var requestGroupPermission = service.Permissions.Create(groupPermission, drive.Id);
//                    requestGroupPermission.Fields = "id";
//                    requestGroupPermission.SupportsTeamDrives = true;
//                    requestGroupPermission.SendNotificationEmail = false;
//                    batch.Queue(requestGroupPermission, callback);
//                }
                
//                // execute the permissions update batch
//                var task = batch.ExecuteAsync();                
//            }
//            // any exceptions thrown during this process will be printed to the console, and the app restarted
//            catch (Exception ex)
//            {
//                string error = "Error returned from the service: " + Environment.NewLine + ex.Message;
//                getOutput = error;
//                Console.WriteLine(getOutput);
//                Console.Read();
//            }            
//        }

//        // imports the excel file containing data for all of the Team Drives we intend to create
//        // for this function to work correctly, the excel sheet must have the following parameters:
//        // TO DO : we should parameterize these later...
//        // filename = "Fileshare Folders Pruned.xlsx"
//        // sheet name = "Folders"
//        // column 1 = "Date", values formatted as MM/DD/YYYY
//        // column 2 = "StreamName", values as string.  The value here becomes the name of the new Team Drive in Google
//        // column 3 = "Group", values as string (email address).  The value here must correspond to the email address associated with the Google Group responsible for this Team Drive
//        public static DataTable ImportExcelFile()
//        {
//            var fileName = string.Format("{0}\\Fileshare Folders Pruned.xlsx", Directory.GetCurrentDirectory());
//            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", fileName);

//            var adapter = new OleDbDataAdapter("SELECT * FROM [Folders$]", connectionString);
//            var ds = new DataSet();

//            adapter.Fill(ds, "FolderList");

//            DataTable data = ds.Tables["FolderList"];
//            return data;
//        }

//        // Drum Folder Object
//        public struct DrumFolder
//        {
//            public string FolderName { get; set; }
//            public string SecurityGroup { get; set; }

//            public DrumFolder(string name, string group)
//            {
//                FolderName = "folderName";
//                SecurityGroup = "securityGroup";
//            }            
//        }

//        // MAIN FUNCTION
//        static void Main(string[] args)
//        {
//            getOutput = "Initializing Google Team Drive API Interface." + Environment.NewLine;
//            Console.WriteLine(getOutput);
            
//            //ConnectToServiceAccount(service, "admin@drumagency.com");
//            UserCredential credential = ConnectToOAuthUser();

//            // Create Drive API service.
//            DriveService service = new DriveService(new BaseClientService.Initializer()
//            {
//                HttpClientInitializer = credential,
//                ApplicationName = "Team Drives API Interface",
//            });

//            // this is just a simple debug check to make sure the reporting service was successfully initialized
//            if (service != null)
//            {
//                getOutput = "Google Api service was successfully initialized." + Environment.NewLine;
//                Console.WriteLine(getOutput);
//            }

//            // If you want to create new team drives from a given spreadsheet, uncomment this function
//            //CreateTeamDrivesFromSpreadsheet(service);

//            IList<TeamDrive> AllDrives = ListTeamDrives(service);

//            if (AllDrives != null && AllDrives.Count > 0)
//            {
//                foreach (var drive in AllDrives)
//                {
//                    ModifyTeamDrivePermissions(service, drive, "allclients-g@drumagency.com");
//                }
//            }

//            Console.Read();
//        }

//        public static void CreateTeamDrivesFromSpreadsheet(DriveService service)
//        {
//            // we import the excel file as an enumerable variable so that we may run LINQ queries against it
//            var data = ImportExcelFile().AsEnumerable();

//            var query = data.Where(x => x.Field<string>("StreamName") != string.Empty).Select(x =>
//                new DrumFolder
//                {
//                    FolderName = x.Field<string>("StreamName"),
//                    SecurityGroup = x.Field<string>("Group"),
//                });

//            foreach (DrumFolder Folder in query)
//            {
//                CreateNewTeamDrive(service, Folder);
//            }
//            Console.ReadLine();
//        }
//    }
//}