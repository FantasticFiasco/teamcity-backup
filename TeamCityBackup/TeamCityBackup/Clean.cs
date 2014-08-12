using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

namespace TeamCityBackup
{
    public class Clean
    {
        private readonly Options options;

        public Clean(Options options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            this.options = options;
        }

        public bool Run()
        {
            if (!Directory.Exists(options.BackupDirectory))
            {
                Console.Error.WriteLine("The backup directory '{0}' does not exist.", options.BackupDirectory);
                return false;
            }

            Console.WriteLine("Cleaning backup directory '{0}'", options.BackupDirectory);

            try
            {
                var directoryInfo = new DirectoryInfo(options.BackupDirectory);

                // Removing one more backup than specified in max backup count to accommodate for
                // the backup that will be created after clean is run
                IEnumerable<FileInfo> filesToRemove = directoryInfo.GetFiles()
                    .OrderByDescending(file => file.CreationTimeUtc)
                    .Skip(options.MaxBackupCount - 1);

                foreach (FileInfo file in filesToRemove)
                {
                    Console.WriteLine("Deleting old backup file '{0}'", file.Name);
                    file.Delete();
                }

                return true;
            }
            catch (SecurityException e)
            {
                WriteErrorMessage(e);
                return false;
            }
            catch (IOException e)
            {
                WriteErrorMessage(e);
                return false;
            }
            catch (ArgumentException e)
            {
                WriteErrorMessage(e);
                return false;
            }
        }

        private static void WriteErrorMessage(Exception e)
        {
            Console.Error.WriteLine("Error occurred when cleaning backup directory." + Environment.NewLine + e);
        }
    }
}