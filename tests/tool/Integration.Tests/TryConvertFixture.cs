﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;

namespace Microsoft.DotNet.UpgradeAssistant.Fixtures
{
    /// <summary>
    /// Makes sure that try-convert is available on disk for test usage.
    /// </summary>
    public class TryConvertFixture
    {
        private const string TryConvertPath = @"%USERPROFILE%\.dotnet\tools\try-convert.exe";

        public TryConvertFixture()
        {
            var tryConvertPath = Environment.ExpandEnvironmentVariables(TryConvertPath);
            if (!File.Exists(tryConvertPath))
            {
                // Attempt to install try-convert
                var p = Process.Start("dotnet", "tool install -g try-convert");
                p.WaitForExit();
            }
        }
    }
}
