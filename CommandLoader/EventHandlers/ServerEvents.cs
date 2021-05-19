// -----------------------------------------------------------------------
// <copyright file="ServerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandLoader.EventHandlers
{
    using System;
    using System.Collections.Generic;
    using CommandLoader.API;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;

    /// <summary>
    /// Runs methods subscribed to events in <see cref="Exiled.Events.Handlers.Server"/>.
    /// </summary>
    public class ServerEvents
    {
        private static readonly List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs)"/>
        public void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            Log.Debug($"{ev.Sender.Nickname} sent a command: {ev.Name} {string.Join(" ", ev.Arguments)}", Plugin.Instance.Config.ShowDebug);
            CommandScript commandScript = GetCommand(ev.Name);
            if (commandScript == null)
                return;

            ev.IsAllowed = false;
            Log.Debug($"Command by the name of {ev.Name} was found in the loaded scripts, running.", Plugin.Instance.Config.ShowDebug);
            Coroutines.Add(Timing.RunCoroutine(RunInstructionSet(commandScript.Instructions, ev.CommandSender)));
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundEnded(RoundEndedEventArgs)"/>
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (CoroutineHandle coroutineHandle in Coroutines)
                Timing.KillCoroutines(coroutineHandle);

            Coroutines.Clear();
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

        private static CommandScript GetCommand(string name)
        {
            foreach (CommandScript commandScript in Loader.Commands)
            {
                if (string.Equals(commandScript.Name, name, StringComparison.OrdinalIgnoreCase))
                    return commandScript;
            }

            return null;
        }
    }
}