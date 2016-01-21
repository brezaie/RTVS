﻿using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Design;
using Microsoft.R.Host.Client;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.R.Package.DataInspect.Commands;
using Microsoft.VisualStudio.R.Package.Feedback;
using Microsoft.VisualStudio.R.Package.Help;
using Microsoft.VisualStudio.R.Package.History;
using Microsoft.VisualStudio.R.Package.Options.R.Tools;
using Microsoft.VisualStudio.R.Package.Plots.Commands;
using Microsoft.VisualStudio.R.Package.Repl.Data;
using Microsoft.VisualStudio.R.Package.Repl.Debugger;
using Microsoft.VisualStudio.R.Package.Repl.Workspace;
using Microsoft.VisualStudio.R.Package.RPackages.Commands;

namespace Microsoft.VisualStudio.R.Packages.R {
    internal static class PackageCommands {
        public static IEnumerable<MenuCommand> GetCommands(ExportProvider exportProvider) {
            var rSessionProvider = exportProvider.GetExportedValue<IRSessionProvider>();
            var projectServiceAccessor = exportProvider.GetExportedValue<IProjectServiceAccessor>();

            return new List<MenuCommand> {
                new GoToOptionsCommand(),
                new GoToEditorOptionsCommand(),
                new ImportRSettingsCommand(),

                new SendSmileCommand(),
                new SendFrownCommand(),

                new LoadWorkspaceCommand(rSessionProvider, projectServiceAccessor),
                new SaveWorkspaceCommand(rSessionProvider, projectServiceAccessor),

                new AttachDebuggerCommand(rSessionProvider),
                new AttachToRInteractiveCommand(rSessionProvider),
                new StopDebuggingCommand(rSessionProvider),
                new ContinueDebuggingCommand(rSessionProvider),
                new StepOverCommand(rSessionProvider),
                new StepOutCommand(rSessionProvider),
                new StepIntoCommand(rSessionProvider),

                new InterruptRCommand(rSessionProvider),

                new ImportDataSetTextFileCommand(),
                new ImportDataSetUrlCommand(),

                new InstallPackagesCommand(),
                new CheckForPackageUpdatesCommand(),

                // Window commands
                new ShowPlotWindowsCommand(),
                new ShowRInteractiveWindowsCommand(),
                new ShowVariableWindowCommand(),
                new ShowHelpWindowCommand(),
                new ShowHelpOnCurrentCommand(),
                new ShowHistoryWindowCommand(),

                // Plot commands
                new ExportPlotAsImageCommand(),
                new ExportPlotAsPdfCommand(),
                new CopyPlotAsBitmapCommand(),
                new CopyPlotAsMetafileCommand(),
                new HistoryNextPlotCommand(),
                new HistoryPreviousPlotCommand()
            };
        }
    }
}
