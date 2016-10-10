using Blobify.Shared.Helpers;
using Blobify.Uploader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using PR = Blobify.Shared.Tests.Properties.Resources;

namespace Blobify.Shared.Tests
{
    [TestClass]
    public class ArgsParserTests
    {
        [TestMethod]
        public void ArgsParserTests_GoodBlobifyWithAllOptions() =>
            ArgsParser<Options>.Parse(PR.GoodBlobifyWithAllOptions);

        [TestMethod]
        public void ArgsParserTests_GoodConnNoLogLevel() =>
            ArgsParser<Options>.Parse(PR.GoodConnNoLogLevel);

        [TestMethod]
        public void ArgsParserTests_GoodConnWithLogLevel() =>
            ArgsParser<Options>.Parse(PR.GoodConnWithLogLevel);

        [TestMethod]
        public void ArgsParserTests_GoodBlobifyWithMinOptions() =>
            ArgsParser<Options>.Parse(PR.GoodBlobifyWithMinOptions);

        [TestMethod]
        public void ArgsParserTests_GoodNoConnNoLogLevel() =>
            ArgsParser<Options>.Parse(PR.GoodNoConnNoLogLevel);

        [TestMethod]
        public void ArgsParserTests_GoodNoConnWithLogLevel() =>
            ArgsParser<Options>.Parse(PR.GoodNoConnWithLogLevel);

        [TestMethod]
        public void ArgsParserTests_GoodParams() =>
            ArgsParser<Options>.Parse(PR.GoodParams);

        [TestMethod]
        [ExpectedException(typeof(ValidationResult))]
        public void ArgsParserTests_BadSource() =>
            ArgsParser<Options>.Parse(PR.BadSource);

        [TestMethod]
        public void ArgsParserTests_GoodParamsWithLogLevel() =>
            ArgsParser<Options>.Parse(PR.GoodParamsWithLogLevel);

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ArgsParserTests_BadRegex() => 
            ArgsParser<Options>.Parse(PR.BadRegex);

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ArgsParserTests_BadContainerName() =>
            ArgsParser<Options>.Parse(PR.BadContainerName);

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ArgsParserTests_BadPath() => 
            ArgsParser<Options>.Parse(PR.BadPath);

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ArgsParserTests_BadLogLevel() => 
            ArgsParser<Options>.Parse(PR.BadLogLevel);

        [TestMethod]
        public void ArgsParserTests_UnregognizedOptionIgnored() =>
            ArgsParser<Options>.Parse(PR.UnregognizedOptionIgnored);

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ArgsParserTests_BadParams() =>
            ArgsParser<Options>.Parse(PR.BadParams);
    }
}
