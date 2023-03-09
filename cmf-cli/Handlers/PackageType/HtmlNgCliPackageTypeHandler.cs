
using System.Collections.Generic;
using Cmf.CLI.Builders;
using Cmf.CLI.Commands.restore;
using Cmf.CLI.Core.Enums;
using Cmf.CLI.Core.Objects;

namespace Cmf.CLI.Handlers
{
    /// <summary>
    /// Handler for packages managed with @angular/cli
    /// </summary>
    /// <seealso cref="PresentationPackageTypeHandler" />
    public class HtmlNgCliPackageTypeHandler : PresentationPackageTypeHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlGulpPackageTypeHandler" /> class.
        /// </summary>
        /// <param name="cmfPackage"></param>
        public HtmlNgCliPackageTypeHandler(CmfPackage cmfPackage) : base(cmfPackage)
        {
            cmfPackage.SetDefaultValues
            (
                targetDirectory:
                    "UI/Html",
                targetLayer:
                    "ui",
                steps:
                    new List<Step>
                    {
                        new Step(StepType.DeployFiles)
                        {
                            ContentPath = "**"
                        }
                    }
            );

            BuildSteps = new IBuildCommand[]
            {
                new ExecuteCommand<RestoreCommand>()
                {
                    Command = new RestoreCommand(),
                    DisplayName = "cmf restore",
                    Execute = command =>
                    {
                        command.Execute(cmfPackage.GetFileInfo().Directory, null);
                    }
                },
                ///TO-DO: Projects logic
                //new NgCommand()
                //{
                //    DisplayName = "ng build",
                //    Command = "build",
                //    WorkingDirectory = cmfPackage.GetFileInfo().Directory
                //},
                new NgCommand()
                {
                    DisplayName = "ng build",
                    Command = "build",
                    WorkingDirectory = cmfPackage.GetFileInfo().Directory
                }
            };

            cmfPackage.DFPackageType = PackageType.Presentation;
        }
    }
}