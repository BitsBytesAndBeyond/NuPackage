using CommandLine;
using NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuPackage
{
    class Program
    {
        static void Main(string[] args)
        {
            Options o = new Options();

            var result = Parser.Default.ParseArguments(args, o);

            string packageId = "MyPackage";
            string packageVersion = "1.0.0";
            string packageAuthors = Environment.UserName;
            string packageDescription = "My package description";


        }
    }

    class Options
    {
        [Option(
  HelpText = "Sets the Id of the NuGet package.")]
        public string Id { get; set; }


        // Omitting long name, default --verbose
        [Option(
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

    }
}
