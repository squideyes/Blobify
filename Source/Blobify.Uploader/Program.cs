using Blobify.Shared;
using Blobify.Shared.Constants;
using Blobify.Shared.Helpers;
using Blobify.Shared.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Nito.AsyncEx;
using NLog;
using SafeConfig;
using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Blobify.Uploader
{
    class Program
    {
        private class FailureInfo
        {
            public Options Options { get; set; }
            public Exception OriginalError { get; set; }
            public Exception LoggingError { get; set; }
        }

        private const string CREATING =
            "Creating the \"{0}\" {1}, if it doesn't already exist.";

        private const string CONNSTRING = "ConnString";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Options options;

            try
            {
                options = ArgsParser<Options>.Parse(args);
            }
            catch (Exception error)
            {
                // log the error here!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                ArgsParser<Options>.ShowHelp(error);

                Environment.ExitCode = (int)ExitCode.InitError;

                return;
            }

            Console.CancelKeyPress += (s, e) =>
                Environment.ExitCode = (int)ExitCode.Cancelled;

            try
            {
                Environment.ExitCode =
                    (int)AsyncContext.Run(() => DoWork(args));
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);

                Environment.ExitCode = (int)ExitCode.ProcessingError;
            }

            Console.WriteLine();
            Console.Write("Press any key to terminate the program...");

            Console.ReadKey(true);
        }

        private static async Task<ExitCode> DoWork(string[] args)
        {
            return ExitCode.Success;

            //var options = parser.Parse(args);

            //if (options == null)
            //    parser.ShowHelp();
            //else if (options.NoConnString == true)
            //    DeleteConnString();
            //else if (!string.IsNullOrWhiteSpace(options.ConnString))
            //    SaveConnString(options);
            //else
            //    await ZipUploadAndDeploy(options);
        }

        //private static async Task ZipUploadAndDeploy(Options options)
        //{
        //    CloudTable logTable = null;
        //    Logger logger = null;

        //    try
        //    {
        //        var startedOn = DateTime.UtcNow;

        //        ////////////////////////////////////////////////////////////////

        //        var connString = new ConfigManager()
        //            .AtFolder(Properties.Settings.Default.SettingsFolder)
        //            //.WithCurrentUserScope()  // discusss issues with team
        //            .Load()
        //            .Get<string>(CONNSTRING);

        //        if (connString == null)
        //            throw new Exception("A deployment may not be kicked off before a connection string is first saved!");

        //        var account = CloudStorageAccount.Parse(connString);

        //        var blobClient = account.CreateCloudBlobClient();

        //        var tableClient = account.CreateCloudTableClient();

        //        var cts = new CancellationTokenSource();

        //        ////////////////////////////////////////////////////////////////

        //        logger = new Logger(typeof(Program), account, cts,
        //            Properties.Settings.Default.MinSeverity);

        //        //logger.LogToConsole(Severity.Info,
        //        //    $"Deploying \"{options.SourcePath}\\*.*\"");

        //        await logger.Init();

        //        ////////////////////////////////////////////////////////////////

        //        //var zipFileContainer = await CreateContainer(logger,
        //        //    blobClient, options.AppId.ToString().ToLower(), logTable);

        //        ////////////////////////////////////////////////////////////////

        //        var deployContolTable = tableClient.GetTableReference(
        //            WellKnown.ControlTableName);

        //        await logger.Log(Severity.Debug,
        //            CREATING, WellKnown.ControlTableName, "table");

        //        var wasCreated = await deployContolTable.CreateIfNotExistsAsync();

        //        await logger.Log(Severity.Debug,
        //            "The \"{0}\" {1} {2}.", WellKnown.ControlTableName,
        //            "table", wasCreated ? "was created" : "already exists");

        //        ////////////////////////////////////////////////////////////////

        //        var zipFileName = Path.Combine(Path.GetTempPath(),
        //            Guid.NewGuid().ToString("N") + ".zip");

        //        //await logger.Log(Severity.Debug,
        //        //    "Zipping \"{0}\\*.*\" into \"{1}\".",
        //        //    options.SourcePath, zipFileName);

        //        //ZipFile.CreateFromDirectory(options.SourcePath, zipFileName);

        //        await logger.Log(Severity.Debug,
        //            $"The \"{zipFileName}\" archive was created!");

        //        ////////////////////////////////////////////////////////////////

        //        //var blob = zipFileContainer.GetBlockBlobReference(
        //        //    options.BuildName + ".zip");

        //        //await logger.Log(Severity.Debug,
        //        //    $"Uploading the \"{zipFileName}\" archive to \"{blob.Name}\".");

        //        //await blob.UploadFromFileAsync(zipFileName);

        //        //await logger.Log(Severity.Info,
        //        //    "The \"{0}\" blob was uploaded to the \"{1}\" container.",
        //        //    blob.Name, zipFileContainer.Name);

        //        ////////////////////////////////////////////////////////////////

        //        File.Delete(zipFileName);

        //        await logger.Log(Severity.Debug,
        //            $"The temporary \"{zipFileName}\" archive was deleted.");

        //        ////////////////////////////////////////////////////////////////

        //        //await logger.Log(Severity.Debug,
        //        //    "Upserting \"{0}\" table entries for \"{1}\".",
        //        //    options.AppId, string.Join(",", options.HostNames));

        //        //for (int i = 0; i < options.HostNames.Count; i++)
        //        //{
        //        //    var entity = new ControlEntity()
        //        //    {
        //        //        Timestamp = DateTime.UtcNow,
        //        //        PartitionKey = options.HostNames[i],
        //        //        RowKey = options.AppId.ToString(),
        //        //        BlobName = options.BuildName + ".zip",
        //        //        AlertTos = string.Join(";", options.AlertTos),
        //        //        Status = (int)(i == options.HostNames.Count - 1 ?
        //        //            DeployStatus.DeployNow : DeployStatus.CanDeploy)
        //        //    };

        //        //    await deployContolTable.ExecuteAsync(
        //        //        TableOperation.InsertOrReplace(entity));

        //        //    await logger.Log(Severity.Info,
        //        //        "Upserted an \"{0}\" entry into the \"{1}\" table for \"{2}\".",
        //        //        entity.RowKey, WellKnown.ControlTableName, entity.PartitionKey);
        //        //}

        //        ////////////////////////////////////////////////////////////////

        //        await logger.Log(Severity.Debug,
        //            $"Elapsed: {DateTime.UtcNow - startedOn}");
        //    }
        //    catch (Exception error)
        //    {
        //        try
        //        {
        //            await logger.Log(error);
        //        }
        //        catch (Exception loggingError)
        //        {
        //            logger.LogToConsole(Severity.Failure,
        //                "The \"{0}\" error couldn't be logged.  See the \"{1}\" folder for details.",
        //                error.Message.ToSingleLine(),
        //                Properties.Settings.Default.FailureLogsPath);

        //            var info = new FailureInfo()
        //            {
        //                Options = options,
        //                OriginalError = error,
        //                LoggingError = loggingError
        //            };

        //            var fileName = Path.Combine(
        //                Properties.Settings.Default.FailureLogsPath,
        //                string.Format("{0}_Failure_{1:yyyyMMdd_HHmmssff}.json",
        //                typeof(Program).Namespace, DateTime.UtcNow));

        //            if (!Directory.Exists(Properties.Settings.Default.FailureLogsPath))
        //                Directory.CreateDirectory(Properties.Settings.Default.FailureLogsPath);

        //            using (var writer = new StreamWriter(fileName))
        //                writer.Write(JsonConvert.SerializeObject(info, Formatting.Indented));
        //        }

        //        throw;
        //    }
        //}

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

        //private static void SaveConnString(Options options)
        //{
        //    new ConfigManager()
        //        .WithLocalMachineScope()
        //        .Set(WellKnown.ConnStringName, options.ConnString)
        //        .AtFolder(Properties.Settings.Default.SettingsFolder)
        //        .Save();

        //    Console.WriteLine("The Azure Storage connection string was saved!");
        //}

        //private static void DeleteConnString()
        //{
        //    if (Directory.Exists(
        //        Properties.Settings.Default.SettingsFolder))
        //    {
        //        Directory.Delete(
        //            Properties.Settings.Default.SettingsFolder, true);
        //    }

        //    Console.WriteLine("The Azure Storage connection string was deleted!");
        //}
    }
}
