// -----------------------------------------------------------------------
// <copyright file="ProcessQueryPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandLoader.Patches
{
    using HarmonyLib;
    using RemoteAdmin;

    /// <summary>
    /// Patches <see cref="CommandProcessor.ProcessQuery"/> to implement <see cref="CommandLoader.CommandProcessor.TryRunCommand"/>.
    /// </summary>
    [HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
    internal static class ProcessQueryPatch
    {
        private static bool Prefix(string q, CommandSender sender)
        {
            return !Plugin.Instance.CommandProcessor.TryRunCommand(sender, q);
        }
    }
}