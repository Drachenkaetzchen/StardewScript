using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSScriptLibrary;
using StardewModdingAPI;
using StardewScript.Framework;

namespace StardewScript
{
    // ReSharper disable once UnusedType.Global
    public class ModEntry : Mod
    {
        private IModHelper _helper;
        private readonly Dictionary<string, IScript> _loadedScripts = new Dictionary<string, IScript>();

        private readonly string _scriptsDirectory = Path.Combine(Constants.ExecutionPath, "Scripts");

        public override void Entry(IModHelper helper)
        {
            _helper = helper;
            CSScript.EvaluatorConfig.Engine = EvaluatorEngine.Roslyn;

            var doc = new StringBuilder();
            doc.AppendLine("Stardew Valley Script Commands:");
            doc.AppendLine();
            doc.AppendLine("  load [filename]     Loads or reloads a script by name");
            doc.AppendLine("  unload [filename]   Unloads a script by name");
            doc.AppendLine("  list                Lists loaded scripts");
            doc.AppendLine("  clear               Unloads all loaded scripts");
            doc.AppendLine();
            doc.AppendLine($"All scripts must be placed in {_scriptsDirectory}. Script filenames are relative to that");
            doc.AppendLine("directory. For example, if you place a script named 'TestScript.cs' in the scripts");
            doc.AppendLine("directory, you can load it by typing 'load TestScript.cs'. Note that script names are");
            doc.AppendLine("case-sensitive on many operating systems. Script filenames must have .cs as extension.");

            helper.ConsoleCommands.Add("script", doc.ToString(), HandleCommand);

            if (!Directory.Exists(_scriptsDirectory))
            {
                Directory.CreateDirectory(_scriptsDirectory);
            }
        }

        /// <summary>Handle a console command.</summary>
        /// <param name="commandName">The command name specified by the user.</param>
        /// <param name="args">The command arguments.</param>
        private void HandleCommand(string commandName, string[] args)
        {
            if (args.Length < 1)
            {
                PrintUsageError("Error: No sub-command given.");
                return;
            }

            switch (args[0].ToLowerInvariant())
            {
                case "load":
                    HandleLoadScript(args);
                    break;
                case "unload":
                    HandleUnloadScript(args);
                    break;
                case "list":
                    HandleListScripts(args);
                    break;
                case "clear":
                    HandleClearScripts(args);
                    break;
                default:
                    PrintUsageError($"Unknown sub command {args[0]}");
                    break;
            }
        }

        #region Sub Command Handlers

        private void HandleLoadScript(string[] args)
        {
            if (args.Length != 2)
            {
                PrintUsageError("No filename given.");
                return;
            }

            var finalScriptName = GetScriptFilename(args[1]);

            if (finalScriptName == null)
            {
                return;
            }

            if (!File.Exists(finalScriptName))
            {
                PrintUsageError($"The script {finalScriptName} could not be loaded because it does not exist.");
            }

            LoadScript(finalScriptName);
        }

        private void HandleUnloadScript(string[] args)
        {
            if (args.Length != 2)
            {
                PrintUsageError("No filename given.");
                return;
            }

            var finalScriptName = GetScriptFilename(args[1]);

            if (finalScriptName == null)
            {
                return;
            }

            UnloadScript(finalScriptName);
        }

        private void HandleListScripts(string[] args)
        {
            if (args.Length != 1)
            {
                PrintUsageError("This sub-command takes no parameters.");
                return;
            }

            var sb = new StringBuilder("Loaded Scripts:");

            foreach (var script in _loadedScripts.Keys)
            {
                sb.AppendLine($" - {Path.GetFileName(script)}");
            }

            Monitor.Log(sb.ToString(), LogLevel.Info);
        }

        private void HandleClearScripts(string[] args)
        {
            if (args.Length != 1)
            {
                PrintUsageError("This sub-command takes no parameters.");
                return;
            }

            foreach (var script in _loadedScripts.Keys.ToList())
            {
                UnloadScript(script);
            }
        }

        #endregion

        private void PrintUsageError(string error)
        {
            var sb = new StringBuilder();
            sb.AppendLine(error);
            sb.AppendLine("Type 'help script' for the usage.");
            
            Monitor.Log(sb.ToString(), LogLevel.Error);
        }

        private void LoadScript(string scriptPath)
        {
            Monitor.Log($"Loading and compiling script {scriptPath}, please wait...", LogLevel.Info);

            if (_loadedScripts.ContainsKey(scriptPath))
            {
                UnloadScript(scriptPath);
            }

            try
            {
                var script = CSScript.Evaluator.LoadFile<IScript>(scriptPath);

                _loadedScripts.Add(scriptPath, script);
                script.SetEmergencyShutdownCallback(scriptPath, EmergencyShutdown);
                script.Initialize(_helper, Monitor);
                

                Monitor.Log($"Script {scriptPath} loaded successfully.", LogLevel.Info);
            }
            catch (Exception e)
            {
                var sb = new StringBuilder();
                
                sb.AppendLine($"Script {scriptPath} has caused an error.");
                sb.AppendLine();
                sb.AppendLine(e.ToString());
                Monitor.Log(sb.ToString(), LogLevel.Error);

                if (_loadedScripts.ContainsKey(scriptPath))
                {
                    UnloadScript(scriptPath);
                }
            }
        }

        private string GetScriptFilename(string filename)
        {
            // Convenience: Add .cs to the filename if it hasn't an extension
            if (!Path.HasExtension(filename))
            {
                filename += ".cs";
            }

            var extension = Path.GetExtension(filename);

            if (extension != ".cs")
            {
                PrintUsageError($"The given extension {extension} is not valid - only .cs files are supported.");
                return null;
            }

            return Path.Combine(_scriptsDirectory, filename);
        }

        private void UnloadScript(string scriptPath)
        {
            if (!_loadedScripts.ContainsKey(scriptPath))
            {
                Monitor.Log($"The script {Path.GetFileName(scriptPath)} is not loaded.", LogLevel.Error);
                return;
            }

            try
            {
                _loadedScripts[scriptPath].DeInitialize();
                _loadedScripts[scriptPath].Dispose();
            }
            catch (Exception unloadException)
            {
                Monitor.Log($"Attempting to unload {scriptPath} has caused an error: {unloadException}",
                    LogLevel.Error);
            }

            _loadedScripts.Remove(scriptPath);
        }

        private void EmergencyShutdown(string scriptFilename, Exception e)
        {
            UnloadScript(scriptFilename);
            
            var sb = new StringBuilder();

            sb.AppendLine(
                $"The script {Path.GetFileName(scriptFilename)} has encountered an exception in a event handler and will now be unloaded.");

            sb.AppendLine(e.ToString());
            Monitor.Log(sb.ToString(), LogLevel.Error);
        }
    }
}