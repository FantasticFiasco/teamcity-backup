using System;
using Plossum.CommandLine;

namespace TeamCityBackup
{
    internal class Program
    {
        private const int TextWidth = 78;
        
        private static CommandLineParser parser;

        private static int Main()
        {
            var options = new Options();
            parser = new CommandLineParser(options);
            parser.Parse();

            // Print header
            Console.WriteLine(parser.UsageInfo.GetHeaderAsString(TextWidth));

            // Help
            if (options.Help)
            {
                PrintUsage();
                return 0;
            }
            
            // Parse errors
            if (parser.HasErrors)
            {
                PrintError();
                return -1;
            }

            // Clean backup directory
            if (!Clean(options))
            {
                return -1;
            }

            // Run backup
            return Backup(options) ? 0 : -1;
        }

        private static void PrintUsage()
        {
            Console.WriteLine(parser.UsageInfo.GetOptionsAsString(TextWidth));
        }

        private static void PrintError()
        {
            Console.WriteLine(parser.UsageInfo.GetErrorsAsString(TextWidth));
        }

        private static bool Clean(Options options)
        {
            var clean = new Clean(options);
            return clean.Run();
        }

        private static bool Backup(Options options)
        {
            var backup = new Backup(options);
            return backup.Run();
        }
    }
}