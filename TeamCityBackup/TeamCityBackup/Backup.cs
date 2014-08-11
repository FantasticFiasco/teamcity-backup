using System;
using System.Net;
using System.Text;

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

        public bool Run()
        {
            using (var client = new WebClient())
            {
                client.Credentials = Credentials;

                try
                {
                    string response = client.UploadString(Url, string.Empty);
                    Console.WriteLine("Created backup with filename '{0}'", response);
                    return true;
                }
                catch (WebException e)
                {
                    Console.Error.WriteLine(
                        "Unable to initiate backup using REST URL {0}.{1}",
                        Url,
                        Environment.NewLine + e);

                    return false;
                }
            }
        }

        private string Url
        {
            get
            {
                return new StringBuilder()
                    .AppendFormat("http://{0}/httpAuth/app/rest/server/backup", options.Server)
                    .AppendFormat("?addTimestamp={0}", options.AddTimestamp)
                    .AppendFormat("&includeConfigs={0}", options.IncludeConfigs)
                    .AppendFormat("&includeDatabase={0}", options.IncludeDatabase)
                    .AppendFormat("&includeBuildLogs={0}", options.IncludeBuildLogs)
                    .AppendFormat("&includePersonalChanges={0}", options.IncludePersonalChanges)
                    .AppendFormat("&fileName={0}", options.FileName)
                    .ToString();
            }
        }

        private NetworkCredential Credentials
        {
            get { return new NetworkCredential(options.Username, options.Password); }
        }
    }
}