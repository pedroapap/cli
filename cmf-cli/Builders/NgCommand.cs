using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using Cmf.CLI.Core.Objects;
using Cmf.CLI.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cmf.CLI.Builders;

public class NgCommand : ProcessCommand, IBuildCommand
{
    public string Command { get; set; }
    
    public string[] Args { get; set; }

    public string[] Projects { get; set; }


    public override ProcessBuildStep[] GetSteps()
    {
        var args = new List<string>
        {
            this.Command
        };

        if (Projects != null)
        {
            // we're building some of the projects
            return Projects.Select(projectName => new ProcessBuildStep()
            {
                Command = "ng" + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".cmd" : ""),
                Args = args.Append(projectName).Concat(this.Args ?? Array.Empty<string>()).ToArray(),
                WorkingDirectory = this.WorkingDirectory,
                EnvironmentVariables = new() { { "NODE_OPTIONS", "--max-old-space-size=8192" } }
            }).ToArray();
        }
        else
        {
            // we're building the entire app
            return new[]
            {
                new ProcessBuildStep()
                {
                    Command = "ng" + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".cmd" : ""),
                    Args = args.Concat(this.Args ?? Array.Empty<string>()).ToArray(),
                    WorkingDirectory = this.WorkingDirectory,
                    EnvironmentVariables = new() { { "NODE_OPTIONS", "--max-old-space-size=8192" } }
                }
            };
        }
    }

    public string DisplayName { get; set; }
    public bool Test { get; set; }
}