using CommandLine;
using NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine.Text;

namespace NuPackage
{
    class Program
    {

        static void Main(string[] args)
        {
            Options nuPackageOptions = new Options();

            var result = Parser.Default.ParseArguments(args, nuPackageOptions);

            if (!result)
            {
                
            }

            string packageId = nuPackageOptions.PackageId;
            string packageVersion = nuPackageOptions.PackageVersion;
            string packageAuthors = nuPackageOptions.PackageAuthors;
            string packageDescription = nuPackageOptions.PackageDescription;


            ManifestMetadata metadata = new ManifestMetadata()
            {
                Authors = packageAuthors,
                Version = packageVersion,
                Id = packageId,
                Description = packageDescription,
            };

            string manifestFileTarget = nuPackageOptions.ManifestFileTarget;
            string manifestFileSource = nuPackageOptions.ManifestFileSource;
            string manifestFileExclude = nuPackageOptions.ManifestFileExclude;

            string basePath = nuPackageOptions.BasePath;

            PackageBuilder builder = new PackageBuilder();

            builder.PopulateFiles(basePath,
                new[]
                {
                    new ManifestFile()
                    {
                        Source = manifestFileSource,
                        Target = manifestFileTarget,
                        Exclude = manifestFileExclude
                    }
                });

            builder.Populate(metadata);

            var nugetPackageFullName = builder.GetFullName().Replace(" ", "-");

            string nugetPackageDirectory = Environment.ExpandEnvironmentVariables(nuPackageOptions.BasePath);

            string nugetPackageFileName = Path.Combine(nugetPackageDirectory, $"{nugetPackageFullName}.nupkg");
           

            using (FileStream stream = File.Open(nugetPackageFileName, FileMode.OpenOrCreate))
            {
                builder.Save(stream);
            }

        }
    }

    class Options
    {
        const string DefaultPackageId = "MyPackage";
        const string DefaultPackageVersion = "1.0.0";
        const string DefaultPackageDescription = "My package description";

        const string DefaultManifestFileTarget = "lib/net45";
        const string DefaultManifestFileSource = "**";
        const string DefaultManifestFileExclude = "";

        [Option]
        public string ManifestFileTarget { get; set; } = DefaultManifestFileTarget;

        [Option]
        public string ManifestFileSource { get; set; } = DefaultManifestFileSource;

        [Option]
        public string ManifestFileExclude { get; set; } = DefaultManifestFileExclude;

        [Option(HelpText = "Sets the id of the NuGet package.")]
        public string PackageId { get; set; } = DefaultPackageId;

        [Option(HelpText = "Sets the version of the NuGet package.")]
        public string PackageVersion { get; set; } = DefaultPackageVersion;

        [Option("Sets the description of the NuGet package.")]
        public string PackageDescription { get; set; } = DefaultPackageDescription;

        [Option("Sets the author(s) of the NuGet package.")]
        public string PackageAuthors { get; set; } = Environment.UserName;

        // Omitting long name, default --verbose
        [Option(
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option]
        public string BasePath { get; set; } = Environment.CurrentDirectory;
    }
}
