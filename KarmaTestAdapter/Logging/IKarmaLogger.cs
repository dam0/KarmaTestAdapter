﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestWindow.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaTestAdapter.Logging
{
    public interface IKarmaLogger : IMessageLogger, ILogger
    {
        void Info(string message);
        void Info(string message, params object[] args);
        void Warn(string message);
        void Warn(string message, params object[] args);
        void Error(string message);
        void Error(string message, params object[] args);
        void Error(Exception ex, string message = null);
        void Error(Exception ex, string message = null, params object[] args);
        void AddLogger(IKarmaLogger logger);
        void AddLogger(ILogger logger);
        void AddLogger(IMessageLogger logger);
        void AddLogger(string filename);
    }
}
