// -----------------------------------------------------------------------
// <copyright file="Instruction.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandLoader.API
{
#pragma warning disable SA1118
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
    using NorthwoodLib.Pools;
    using RemoteAdmin;

    /// <summary>
    /// A container for a parsed command line.
    /// </summary>
    public class Instruction
    {
        /// <summary>
        /// Gets or sets the base command to execute.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the arguments of the <see cref="Command"/>.
        /// </summary>
        public string[] Arguments { get; set; }

        /// <summary>
        /// Gets or sets the delay, in seconds, to wait after the command has executed.
        /// </summary>
        public float Delay { get; set; }

        /// <summary>
        /// Runs the <see cref="Instruction"/> as the <see cref="CommandSender"/> provided.
        /// </summary>
        /// <param name="sender">The sender of the original command.</param>
        public void Run(CommandSender sender)
        {
            List<string> parsedArguments = ListPool<string>.Shared.Rent();
            foreach (string argument in Arguments)
                parsedArguments.Add(ParseWildcard(argument, sender));

            CommandProcessor.ProcessQuery($"{Command} {string.Join(" ", parsedArguments)}", sender);
            ListPool<string>.Shared.Return(parsedArguments);
        }

        /// <inheritdoc />
        public override string ToString() => $"{Command} {string.Join(" ", Arguments)}";

        private static string ParseWildcard(string wildcard, CommandSender sender)
        {
            GetSenderInfo(sender, out string name, out int id);
            return wildcard.ReplaceAfterToken('*', new[]
            {
                new Tuple<string, object>("SenderName", name),
                new Tuple<string, object>("SenderId", id),
                new Tuple<string, object>("AllPlayerIds", string.Join(".", Player.List.Select(player => player.Id))),
            });
        }

        private static void GetSenderInfo(CommandSender sender, out string name, out int id)
        {
            if (sender is PlayerCommandSender playerCommandSender)
            {
                name = playerCommandSender.Nickname;
                id = playerCommandSender.PlayerId;
                return;
            }

            name = ReferenceHub.HostHub.nicknameSync.Network_myNickSync;
            id = ReferenceHub.HostHub.playerId;
        }
    }
}