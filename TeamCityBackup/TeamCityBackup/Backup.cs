using System;
using System.Net;
using System.Threading.Tasks;

namespace TeamCityBackup
{
    public class Backup
    {
        private readonly Options options;

        public Backup(Options options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            this.options = options;
        }

        public async Task RunAsync()
        {
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(options.Username, options.Password);

                try
                {
                    string response = await client.UploadStringTaskAsync(Address, Parameters);
                    Console.WriteLine(response);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private string Address
        {
            get
            {
                return string.Format(
                    "http://{0}/httpAuth/app/rest/server/backup",
                    options.Server);
            }
        }

        private string Parameters
        {
            get
            {
                return string.Format(
                    "addTimestamp={0}&includeConfigs={1}&includeDatabase={2}&includeBuildLogs={3}&includePersonalChanges={4}&fileName={5}",
                    options.AddTimestamp ? "true" : "false",
                    options.IncludeConfigs ? "true" : "false",
                    options.IncludeDatabase ? "true" : "false",
                    options.IncludeBuildLogs ? "true" : "false",
                    options.IncludePersonalChanges ? "true" : "false",
                    options.FileName);
            }
        }
    }
}