using CommandLine;
using nanoFirmwareFlasher.Common;
using NUnit.Framework;
using System.Collections.Generic;

namespace nanoFirmwareFlasher.Tests
{
    [TestFixture]
    public class ParserTest
    {
        [Test]
        [TestCaseSource(nameof(GenerateCommandLineArgs))]
        public void CommandLineTest(IEnumerable<string> args, bool expectedParsing)
        {
            Parser.Default.ParseArguments<ListOptions, BackupOptions, WriteOptions, EraseOptions>(args)
                .WithParsed<ListOptions>(options => { if (!expectedParsing) Assert.Fail("Fully parsed but expecting not", args); })
                .WithParsed<BackupOptions>(options => { if (!expectedParsing) Assert.Fail("Fully parsed but expecting not", args); })
                .WithParsed<WriteOptions>(options => { if (!expectedParsing) Assert.Fail("Fully parsed but expecting not", args); })
                .WithParsed<EraseOptions>(options => { if (!expectedParsing) Assert.Fail("Fully parsed but expecting not", args); })
                .WithNotParsed(errs => { if (expectedParsing) Assert.Fail("Not parsed but expecting it", args); });
        }

        private static IEnumerable<object[]> GenerateCommandLineArgs()
        {
            // Action : 'list'
            yield return new object[] { "list --platform stm32dfu".Split(' '), true };
            yield return new object[] { "list --platform stm32jtag".Split(' '), true };
            yield return new object[] { "list --platform esp32".Split(' '), true };
            yield return new object[] { "list --platform ti".Split(' '), true };
            yield return new object[] { "list --platform foo".Split(' '), false };
            yield return new object[] { "list foo".Split(' '), false };
            yield return new object[] { "list --erase --target deviceid".Split(' '), false };

            // Action : 'write'
            yield return new object[] { "write --platform stm32dfu --image filepath".Split(' '), true };
            yield return new object[] { "write --platform stm32dfu --image filepath1;filepath2".Split(' '), true };
            yield return new object[] { "write --platform stm32dfu --image filepath1;filepath2;".Split(' '), true };
            yield return new object[] { "write --platform stm32dfu --community filepath1".Split(' '), true };
            yield return new object[] { "write --platform stm32dfu".Split(' '), false };
            yield return new object[] { "write --platform foo".Split(' '), false };
            yield return new object[] { "write foo".Split(' '), false };
            yield return new object[] { "write --erase --target deviceid".Split(' '), false };

            // Action : 'backup'
            yield return new object[] { "backup --platform stm32dfu --savepath filepath".Split(' '), true };
            yield return new object[] { "backup --platform stm32dfu".Split(' '), false };
            yield return new object[] { "backup --platform foo".Split(' '), false };
            yield return new object[] { "backup foo".Split(' '), false };
            yield return new object[] { "backup --erase --target deviceid".Split(' '), false };

            // Action : 'erase'
            yield return new object[] { "erase --platform stm32dfu".Split(' '), true };
            yield return new object[] { "erase --platform foo".Split(' '), false };
            yield return new object[] { "erase foo".Split(' '), false };
            yield return new object[] { "erase --erase --target deviceid".Split(' '), false };
        }
    }
}
