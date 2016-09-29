using Blobify.Shared.Helpers;
using Blobify.Uploader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PR = Blobify.Shared.Tests.Properties.Resources;

namespace Blobify.Shared.Tests
{
    [TestClass]
    public class ArgsParserTests
    {
        [TestMethod]
        public void ArgsParserTests_GoodBlobifyWithAllOptions() =>
            ParseArgs(PR.GoodBlobifyWithAllOptions);

        [TestMethod]
        public void ArgsParserTests_GoodConnNoLogLevel() =>
            ParseArgs(PR.GoodConnNoLogLevel);

        [TestMethod]
        public void ArgsParserTests_GoodConnWithLogLevel() =>
            ParseArgs(PR.GoodConnWithLogLevel);

        [TestMethod]
        public void ArgsParserTests_GoodBlobifyWithMinOptions() =>
            ParseArgs(PR.GoodBlobifyWithMinOptions);

        [TestMethod]
        public void ArgsParserTests_GoodNoConnNoLogLevel() =>
            ParseArgs(PR.GoodNoConnNoLogLevel);

        [TestMethod]
        public void ArgsParserTests_GoodNoConnWithLogLevel() =>
            ParseArgs(PR.GoodNoConnWithLogLevel);

        [TestMethod]
        public void ArgsParserTests_GoodParams() =>
            ParseArgs(PR.GoodParams);

        [TestMethod]
        public void ArgsParserTests_GoodParamsWithLogLevel() =>
            ParseArgs(PR.GoodParamsWithLogLevel);

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgsParserTests_BadSource() => ParseArgs(PR.BadSource);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgsParserTests_BadRegex() => ParseArgs(PR.BadRegex);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgsParserTests_BadContainerName() =>
            ParseArgs(PR.BadContainerName);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgsParserTests_BadPath() => ParseArgs(PR.BadPath);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgsParserTests_BadLogLevel() => ParseArgs(PR.BadLogLevel);

        [TestMethod]
        public void ArgsParserTests_UnregognizedOptionIgnored() =>
            ParseArgs(PR.UnregognizedOptionIgnored);

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgsParserTests_BadParams() => ParseArgs(PR.BadParams);

        private Options ParseArgs(string cmd) =>
            ArgsParser<Options>.Parse(cmd.Split(' '));
    }
}
