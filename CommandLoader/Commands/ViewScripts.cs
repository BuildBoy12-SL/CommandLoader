// -----------------------------------------------------------------------
// <copyright file="ViewScripts.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandLoader.Commands
{
    using System;
    using System.Text;
    using CommandLoader.API;
    using CommandSystem;
    using NorthwoodLib.Pools;

    /// <summary>
    /// A command to view all loaded <see cref="API.CommandScript"/>s in <see cref="Loader.Commands"/>.
    /// </summary>
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ViewScripts : ICommand
    {
        /// <inheritdoc />
        public string Command => "scripts";

        /// <inheritdoc />
        public string[] Aliases { get; } = Array.Empty<string>();

        /// <inheritdoc />
        public string Description => "Shows all loaded command scripts.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Loaded Scripts:");
            foreach (CommandScript command in Loader.Commands)
            {
                stringBuilder.AppendLine(command.Name);
                stringBuilder.AppendLine(string.Join(", ", command.Instructions));
            }

            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder).TrimEnd();
            return true;
        }
    }
}