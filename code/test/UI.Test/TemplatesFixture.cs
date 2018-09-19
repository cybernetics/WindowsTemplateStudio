﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Fakes;

namespace Microsoft.UI.Test
{
    public class TemplatesFixture : IContextProvider
    {
        private static bool syncExecuted = false;

        public string ProjectName => "Test";

        public string OutputPath { get; set; }

        public string DestinationPath => string.Empty;

        public string DestinationParentPath => string.Empty;

        public string TempGenerationPath => string.Empty;

        public List<string> Projects { get; } = new List<string>();

        public Dictionary<string, List<string>> ProjectReferences { get; } = new Dictionary<string, List<string>>();

        public List<string> ProjectItems => null;

        public List<string> FilesToOpen => null;

        public List<FailedMergePostActionInfo> FailedMergePostActions => null;

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject => null;

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics => null;

        public TemplatesRepository Repository { get; private set; }

        [SuppressMessage(
        "Usage",
        "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
        Justification = "Required por unit testing.")]
        public void InitializeFixture(string platform, string language)
        {
            var source = new LocalTemplatesSource();
            GenContext.Bootstrap(source, new FakeGenShell(platform, language), language);
            GenContext.Current = this;
            if (!syncExecuted)
            {
                GenContext.ToolBox.Repo.SynchronizeAsync(true).Wait();
                syncExecuted = true;
            }

            Repository = GenContext.ToolBox.Repo;
        }
    }
}
