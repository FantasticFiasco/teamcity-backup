using System;
using Plossum.CommandLine;

namespace TeamCityBackup
{
    [CommandLineManager(EnabledOptionStyles = OptionStyles.Windows)]
    public class Options
    {
        private string server;
        private string username;
        private string password;
        private string fileName;
        private string backupDirectory;
        private int maxBackupCount;

        public Options()
        {
            MaxBackupCount = 10;
            AddTimestamp = true;
            IncludeConfigs = true;
            IncludeDatabase = true;
            IncludeBuildLogs = true;
            IncludePersonalChanges = true;
        }

        [CommandLineOption(
            Name = "?",
            Aliases = "h,help",
            Description = "Displays this help text.")]
        public bool Help { get; set; }

        [CommandLineOption(
            Name = "server",
            Aliases = "s",
            Description = "The address of the TeamCity server.",
            MinOccurs = 1)]
        public string Server
        {
            get { return server; }
            set
            {
                AssertIsNotNullOrWhiteSpace(value, "The address of the server must not be empty.");
                server = value;
            }
        }

        [CommandLineOption(
            Name = "username",
            Aliases = "u",
            Description = "The username of the TeamCity administrator.",
            MinOccurs = 1)]
        public string Username
        {
            get { return username; }
            set
            {
                AssertIsNotNullOrWhiteSpace(value, "The username must not be empty.");
                username = value;
            }
        }

        [CommandLineOption(
            Name = "password",
            Aliases = "p",
            Description = "The password of the TeamCity administrator.",
            MinOccurs = 1)]
        public string Password
        {
            get { return password; }
            set
            {
                AssertIsNotNullOrWhiteSpace(value, "The password must not be empty.");
                password = value;
            }
        }

        [CommandLineOption(
            Name = "filename",
            Aliases = "f",
            Description = "The prefix of the file name to save backup to.",
            MinOccurs = 1)]
        public string FileName
        {
            get { return fileName; }
            set
            {
                AssertIsNotNullOrWhiteSpace(value, "The file name must not be empty.");
                fileName = value;
            }
        }

        [CommandLineOption(
            Name = "backupdir",
            Description = "The directory where backups are stored.")]
        public string BackupDirectory
        {
            get { return backupDirectory; }
            set
            {
                AssertIsNotNullOrWhiteSpace(value, "The backup directory must not be empty.");
                backupDirectory = value;
            }
        }

        [CommandLineOption(
            Name = "maxbackupcount",
            Description = "The maximum number of backups stored at the backup directory. The default value is 10.")]
        public int MaxBackupCount
        {
            get { return maxBackupCount; }
            set
            {
                AssertNumber(value, count => count >= 1, "Maximum number of backups must be at least one.");
                maxBackupCount = value;
            }
        }

        [CommandLineOption(
            Name = "addtimestamp",
            Description = "Whether file name should be suffixed with a timestamp. Default value is true.",
            BoolFunction = BoolFunction.Value)]
        public bool AddTimestamp { get; set; }

        [CommandLineOption(
            Name = "includeconfigs",
            Description = "Whether to include configuration. Default value is true.",
            BoolFunction = BoolFunction.Value)]
        public bool IncludeConfigs { get; set; }

        [CommandLineOption(
            Name = "includedatabase",
            Description = "Whether to include database. Default value is true.",
            BoolFunction = BoolFunction.Value)]
        public bool IncludeDatabase { get; set; }

        [CommandLineOption(
            Name = "includebuildlogs",
            Description = "Whether to include build logs. Default value is true.",
            BoolFunction = BoolFunction.Value)]
        public bool IncludeBuildLogs { get; set; }

        [CommandLineOption(
            Name = "includepersonalchanges",
            Description = "Whether to include personal changes. Default value is true.",
            BoolFunction = BoolFunction.Value)]
        public bool IncludePersonalChanges { get; set; }

        private static void AssertIsNotNullOrWhiteSpace(string value, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOptionValueException(errorMessage);
        }

        private static void AssertNumber(int value, Predicate<int> validity, string errorMessage)
        {
            if (!validity(value))
                throw new InvalidOptionValueException(errorMessage);
        }
    }
}