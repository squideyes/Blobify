﻿using Blobify.Shared;
using Blobify.Shared.Helpers;
using Nito.AsyncEx;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blobify.Uploader
{
    class Program
    {
        private const string CREATING =
            "Creating the \"{0}\" {1}, if it doesn't already exist.";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Options options;

            var exitCode = GetOptions(args, out options);

            if (exitCode != ExitCode.Success)
            {
                logger.Warn(exitCode.GetDescription());

                Console.WriteLine();

                ArgsParser<Options>.ShowHelp();

                return;
            }

            logger.Info($"Parsed {nameof(Options)}: {options}");
        }

        private static ExitCode GetOptions(string[] args, out Options options)
        {
            options = null;

            Options parsed = null;

            if (args.Length == 0)
                return ExitCode.NoArgs;

            if (!Safe.Run(() => parsed = ArgsParser<Options>.Parse(string.Join(" ", args))))
                return ExitCode.BadArgs;

            if (parsed.ArgsFile != null)
            {
                if (!File.Exists(parsed.ArgsFile))
                    return ExitCode.NoArgsFile;

                if (!Safe.Run(() => parsed = LoadArgsFile(parsed.ArgsFile)))
                    return ExitCode.BadArgsFile;
            }

            if (!Safe.Run(parsed, o =>
                Validator.ValidateObject(o, new ValidationContext(o), true)))
            {
                return ExitCode.BadArgs;
            }

            if (!Safe.Run(parsed, o => o.ValidateDependencies()))
                return ExitCode.BadDependencies;

            options = parsed;

            return ExitCode.Success;

            //try
            //{






            //    }

            //    try
            //    {
            //        logger.Debug("Kicking off the Blobify process");

            //        AsyncContext.Run(() => DoWork(options));

            //        Environment.ExitCode = (int)ExitCode.Success;
            //    }
            //    catch (Exception error)
            //    {
            //        logger.Error("ProcessingError: " + error);

            //        Environment.ExitCode = (int)ExitCode.ProcessingError;
            //    }
            //}
            //catch (Exception error)
            //{
            //    logger.Error("InitError: " + error);

            //    ShowHelpAndSetExitCode(error, ExitCode.InitError);
            //}

            //Console.CancelKeyPress += (s, e) =>
            //    Environment.ExitCode = (int)ExitCode.Cancelled;
        }

        private static Options LoadArgsFile(string argsFile)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(argsFile))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (!line.StartsWith("//"))
                        lines.Add(line);
                }
            }

            return ArgsParser<Options>.Parse(string.Join(" ", lines));
        }

        private static async Task DoWork(Options options)
        {
            if (options.NoConnString == true)
                DeleteConnString();
            else if (!string.IsNullOrWhiteSpace(options.ConnString))
                SaveConnString(options);
            else
                await ZipAndUpload(options);
        }

        private static List<FileInfo> GetFileInfos(Options options)
        {
            var fileNames = Directory.GetFiles(
                options.Source, "*.*", options.Recurse == true ?
                SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            var fileInfos = new List<FileInfo>();

            if (options.Regex == null)
            {
                fileInfos.AddRange(fileNames.Select(f => new FileInfo(f)));
            }
            else
            {
                var regex = new Regex(options.Regex, RegexOptions.IgnoreCase |
                    RegexOptions.IgnorePatternWhitespace);

                foreach (var fileName in fileNames)
                {
                    var localPath = fileName.Substring(options.Source.Length);

                    if (regex.IsMatch(localPath))
                        fileInfos.Add(new FileInfo(fileName));
                }
            }

            return fileInfos;
        }

        private static async Task ZipAndUpload(Options options)
        {
            var fileInfos = GetFileInfos(options);


            //CloudTable logTable = null;
            //Logger logger = null;

            //try
            //{
            //    var startedOn = DateTime.UtcNow;

            //    ////////////////////////////////////////////////////////////////

            //    var connString = new ConfigManager()
            //        .AtFolder(Properties.Settings.Default.SettingsFolder)
            //        //.WithCurrentUserScope()  // discusss issues with team
            //        .Load()
            //        .Get<string>(CONNSTRING);

            //    if (connString == null)
            //        throw new Exception("A deployment may not be kicked off before a connection string is first saved!");

            //    var account = CloudStorageAccount.Parse(connString);

            //    var blobClient = account.CreateCloudBlobClient();

            //    var tableClient = account.CreateCloudTableClient();

            //    var cts = new CancellationTokenSource();

            //    ////////////////////////////////////////////////////////////////

            //    logger = new Logger(typeof(Program), account, cts,
            //        Properties.Settings.Default.MinSeverity);

            //    //logger.LogToConsole(Severity.Info,
            //    //    $"Deploying \"{options.SourcePath}\\*.*\"");

            //    await logger.Init();

            //    ////////////////////////////////////////////////////////////////

            //    //var zipFileContainer = await CreateContainer(logger,
            //    //    blobClient, options.AppId.ToString().ToLower(), logTable);

            //    ////////////////////////////////////////////////////////////////

            //    var deployContolTable = tableClient.GetTableReference(
            //        WellKnown.ControlTableName);

            //    await logger.Log(Severity.Debug,
            //        CREATING, WellKnown.ControlTableName, "table");

            //    var wasCreated = await deployContolTable.CreateIfNotExistsAsync();

            //    await logger.Log(Severity.Debug,
            //        "The \"{0}\" {1} {2}.", WellKnown.ControlTableName,
            //        "table", wasCreated ? "was created" : "already exists");

            //    ////////////////////////////////////////////////////////////////

            //    var zipFileName = Path.Combine(Path.GetTempPath(),
            //        Guid.NewGuid().ToString("N") + ".zip");

            //    //await logger.Log(Severity.Debug,
            //    //    "Zipping \"{0}\\*.*\" into \"{1}\".",
            //    //    options.SourcePath, zipFileName);

            //    //ZipFile.CreateFromDirectory(options.SourcePath, zipFileName);

            //    await logger.Log(Severity.Debug,
            //        $"The \"{zipFileName}\" archive was created!");

            //    ////////////////////////////////////////////////////////////////

            //    //var blob = zipFileContainer.GetBlockBlobReference(
            //    //    options.BuildName + ".zip");

            //    //await logger.Log(Severity.Debug,
            //    //    $"Uploading the \"{zipFileName}\" archive to \"{blob.Name}\".");

            //    //await blob.UploadFromFileAsync(zipFileName);

            //    //await logger.Log(Severity.Info,
            //    //    "The \"{0}\" blob was uploaded to the \"{1}\" container.",
            //    //    blob.Name, zipFileContainer.Name);

            //    ////////////////////////////////////////////////////////////////

            //    File.Delete(zipFileName);

            //    await logger.Log(Severity.Debug,
            //        $"The temporary \"{zipFileName}\" archive was deleted.");

            //    ////////////////////////////////////////////////////////////////

            //    //await logger.Log(Severity.Debug,
            //    //    "Upserting \"{0}\" table entries for \"{1}\".",
            //    //    options.AppId, string.Join(",", options.HostNames));

            //    //for (int i = 0; i < options.HostNames.Count; i++)
            //    //{
            //    //    var entity = new ControlEntity()
            //    //    {
            //    //        Timestamp = DateTime.UtcNow,
            //    //        PartitionKey = options.HostNames[i],
            //    //        RowKey = options.AppId.ToString(),
            //    //        BlobName = options.BuildName + ".zip",
            //    //        AlertTos = string.Join(";", options.AlertTos),
            //    //        Status = (int)(i == options.HostNames.Count - 1 ?
            //    //            DeployStatus.DeployNow : DeployStatus.CanDeploy)
            //    //    };

            //    //    await deployContolTable.ExecuteAsync(
            //    //        TableOperation.InsertOrReplace(entity));

            //    //    await logger.Log(Severity.Info,
            //    //        "Upserted an \"{0}\" entry into the \"{1}\" table for \"{2}\".",
            //    //        entity.RowKey, WellKnown.ControlTableName, entity.PartitionKey);
            //    //}

            //    ////////////////////////////////////////////////////////////////

            //    await logger.Log(Severity.Debug,
            //        $"Elapsed: {DateTime.UtcNow - startedOn}");
            //}
            //catch (Exception error)
            //{
            //    try
            //    {
            //        await logger.Log(error);
            //    }
            //    catch (Exception loggingError)
            //    {
            //        logger.LogToConsole(Severity.Failure,
            //            "The \"{0}\" error couldn't be logged.  See the \"{1}\" folder for details.",
            //            error.Message.ToSingleLine(),
            //            Properties.Settings.Default.FailureLogsPath);

            //        var info = new FailureInfo()
            //        {
            //            Options = options,
            //            OriginalError = error,
            //            LoggingError = loggingError
            //        };

            //        var fileName = Path.Combine(
            //            Properties.Settings.Default.FailureLogsPath,
            //            string.Format("{0}_Failure_{1:yyyyMMdd_HHmmssff}.json",
            //            typeof(Program).Namespace, DateTime.UtcNow));

            //        if (!Directory.Exists(Properties.Settings.Default.FailureLogsPath))
            //            Directory.CreateDirectory(Properties.Settings.Default.FailureLogsPath);

            //        using (var writer = new StreamWriter(fileName))
            //            writer.Write(JsonConvert.SerializeObject(info, Formatting.Indented));
            //    }

            //    throw;
            //}
        }

        //private static async Task<CloudBlobContainer> CreateContainer(
        //    Logger logger, CloudBlobClient blobClient,
        //    string containerName, CloudTable logTable)
        //{
        //    var container = blobClient.GetContainerReference(
        //        containerName.ToLower());

        //    await logger.Log(Severity.Debug,
        //        CREATING, container.Name, "container");

        //    var wasCreated = await container.CreateIfNotExistsAsync();

        //    await logger.Log(Severity.Debug,
        //        "The \"{0}\" {1} {2}.", container.Name,
        //        "container", wasCreated ? "was created" : "already exists");

        //    return container;
        //}

        private static void SaveConnString(Options options)
        {
            //new ConfigManager()
            //    .WithLocalMachineScope()
            //    .Set(WellKnown.ConnStringName, options.ConnString)
            //    .AtFolder(Properties.Settings.Default.SettingsFolder)
            //    .Save();

            Console.WriteLine("The Azure Storage connection string was saved!");
        }

        private static void DeleteConnString()
        {
            //if (Directory.Exists(
            //    Properties.Settings.Default.SettingsFolder))
            //{
            //    Directory.Delete(
            //        Properties.Settings.Default.SettingsFolder, true);
            //}

            Console.WriteLine("The Azure Storage connection string was deleted!");
        }
    }
}
