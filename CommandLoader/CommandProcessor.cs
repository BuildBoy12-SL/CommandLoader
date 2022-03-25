// -----------------------------------------------------------------------
// <copyright file="CommandProcessor.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandLoader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandLoader.API;
    using Exiled.API.Features;
    using MEC;

    /// <summary>
    /// Handles the locating and executing of <see cref="CommandScript"/>s.
    /// </summary>
    public class CommandProcessor
    {
        private readonly List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessor"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public CommandProcessor(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Attempts to find a run a command script.
        /// </summary>
        /// <param name="sender">The sender of the command.</param>
        /// <param name="query">The command.</param>
        /// <returns>A value indicating whether a custom command was found and executed.</returns>
        public bool TryRunCommand(CommandSender sender, string query)
        {
            Log.Debug($"{sender.Nickname} sent a command: {query}", plugin.Config.ShowDebug);
            CommandScript commandScript = GetCommand(query);
            if (commandScript == null)
                return false;

            Log.Debug($"Command by the name of '{query}' was found in the loaded scripts, running.", plugin.Config.ShowDebug);
            coroutines.Add(Timing.RunCoroutine(RunInstructionSet(commandScript.Instructions, sender)));
            return true;
        }

        /// <summary>
        /// Kills and clears all command coroutines.
        /// </summary>
        public void Clear()
        {
            foreach (CoroutineHandle coroutineHandle in coroutines)
                Timing.KillCoroutines(coroutineHandle);

            coroutines.Clear();
        }

        private static CommandScript GetCommand(string name)
        {
            return Loader.Commands.FirstOrDefault(commandScript => string.Equals(commandScript.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        private static IEnumerator<float> RunInstructionSet(List<Instruction> instructions, CommandSender sender)
        {
            foreach (Instruction instruction in instructions)
            {
                instruction.Run(sender);

                if (instruction.Delay > 0f)
                    yield return Timing.WaitForSeconds(instruction.Delay);
            }
        }
    }
}