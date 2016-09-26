using Blobify.Uploader;
using Blobify.Shared.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using PR = Blobify.Shared.Tests.Properties.Resources;

namespace Blobify.Shared.Tests
{
    [TestClass]
    public class ArgsParserTests
    {
        [TestMethod]
        public void GoodDeployArgsTest() => ParseArgs(PR.GoodDeployArgs);

        [TestMethod]
        public void GoodConnArgsTest() => ParseArgs(PR.GoodConnArgs);

        [TestMethod]
        public void GoodDeleteConnArgsTest() => ParseArgs(@"/DELETECONN");

        [TestMethod]
        public void ArgValuesCanBeProceededBySpacesTest()
        {
            var options = ParseArgs(PR.GoodArgsWithSpaces);

            //Assert.IsTrue(options.SourcePath.IsTrimmed());
            //Assert.IsTrue(options.AppId.IsDefined());
            //Assert.IsTrue(options.BuildName.IsTrimmed());
            //Assert.IsTrue(options.HostNames != null &&
            //    options.HostNames.Count == 2 &&
            //    options.HostNames.All(hostName => hostName.IsTrimmed()));
            //Assert.IsTrue(options.AlertTos != null &&
            //    options.AlertTos.Count == 2 &&
            //    options.AlertTos.All(alertTo => alertTo.IsEmail()));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void BadAppIdDetectedTest()
        {
            var options = ParseArgs(PR.BadAppIdArgs);

            //Assert.IsTrue(options.AppId.IsDefined());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void MissingSourceArgDetected() => ParseArgs(PR.MissingSourceArg);

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void MissingAppIdArgDetected() => ParseArgs(PR.MissingAppIdArg);

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void MissingBuildArgDetected() => ParseArgs(PR.MissingBuildArg);

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void MissingHostsArgDetected() => ParseArgs(PR.MissingHostsArgs);

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void MissingAlertArgDetected() => ParseArgs(PR.MissingAlertArg);

        private Options ParseArgs(string cmd)
        {
            var parser = new ArgsParser<Options>();

            var options = parser.Parse(cmd.Split(' '));

            if ((options == null) || !options.IsValid)
                throw new Exception();

            return options;
        }
    }
}
