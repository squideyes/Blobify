using Blobify.Uploader;
using Blobify.Shared.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR = Blobify.Shared.Tests.Properties.Resources;

namespace Blobify.Shared.Tests
{
    [TestClass]
    public class ArgsParserTests
    {
        [TestMethod]
        public void ArgsParserTests_GoodAllBlobifyArgs() =>
            ParseArgs(PR.GoodAllBlobifyArgs);

        [TestMethod]
        public void ArgsParserTests_GoodConnNoLogLevelArgs() =>
            ParseArgs(PR.GoodConnNoLogLevelArgs);

        [TestMethod]
        public void ArgsParserTests_GoodConnWithLogLevelArgs() =>
            ParseArgs(PR.GoodConnWithLogLevelArgs);

        [TestMethod]
        public void ArgsParserTests_GoodMinimalBlobifyArgs() =>
            ParseArgs(PR.GoodMinimalBlobifyArgs);

        [TestMethod]
        public void ArgsParserTests_GoodNoConnNoLogLevelArgs() =>
            ParseArgs(PR.GoodNoConnNoLogLevelArgs);

        [TestMethod]
        public void ArgsParserTests_GoodNoConnWithLogLevelArgs() =>
            ParseArgs(PR.GoodNoConnWithLogLevelArgs);

        [TestMethod]
        public void ArgsParserTests_GoodParamsArgs() =>
            ParseArgs(PR.GoodParamsArgs);

        private Options ParseArgs(string cmd) =>
            ArgsParser<Options>.Parse(cmd.Split(' '));
    }
}
