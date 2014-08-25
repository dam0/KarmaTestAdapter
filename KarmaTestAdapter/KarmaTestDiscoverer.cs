﻿using KarmaTestAdapter.Commands;
using KarmaTestAdapter.KarmaTestResults;
using KarmaTestAdapter.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using TwoPS.Processes;
using IO = System.IO;

namespace KarmaTestAdapter
{
    [FileExtension(Globals.JavaScriptExtension)]
    [FileExtension(Globals.JsonExtension)]
    [DefaultExecutorUri(Globals.ExecutorUriString)]
    public class KarmaTestDiscoverer : ITestDiscoverer
    {
        private KarmaDiscoverCommand _karmaDiscoverCommand;

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            var karmaLogger = KarmaLogger.Create(messageLogger: logger);
            karmaLogger.Info("DiscoverTests start");
            foreach (var testcase in GetTests(sources, karmaLogger))
            {
                discoverySink.SendTestCase(testcase);
            }
            karmaLogger.Info("DiscoverTests end");
        }

        public IEnumerable<TestCase> GetTests(IEnumerable<string> sources, IKarmaLogger logger)
        {
            try
            {
                return sources.SelectMany(s => GetTests(s, logger)).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Enumerable.Empty<TestCase>();
            }
        }

        public IEnumerable<TestCase> GetTests(string source, IKarmaLogger logger)
        {
            logger.Info("Source: {0}", source);
            var karma = Discover(source, logger);
            return karma == null ? Enumerable.Empty<TestCase>() : karma.GetTestCases(source);
        }

        public Karma Discover(string source, IKarmaLogger logger)
        {
            if (_karmaDiscoverCommand != null)
            {
                throw new Exception("Test discovery already running");
            }
            _karmaDiscoverCommand = new KarmaDiscoverCommand(source, logger);
            try
            {
                return _karmaDiscoverCommand.Run();
            }
            finally
            {
                _karmaDiscoverCommand = null;
            }
        }
    }
}
