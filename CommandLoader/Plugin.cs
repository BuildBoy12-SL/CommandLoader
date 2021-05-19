// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandLoader
{
    using System;
    using CommandLoader.EventHandlers;
    using Exiled.API.Features;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private static readonly Plugin InstanceValue = new Plugin();

        private Plugin()
        {
        }

        /// <summary>
        /// Gets an instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; } = InstanceValue;

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(2, 10, 0);

        /// <summary>
        /// Gets an instance of <see cref="CommandLoader.EventHandlers.ServerEvents"/>.
        /// </summary>
        public ServerEvents ServerEvents { get; private set; }

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Loader.LoadCommands();

            ServerEvents = new ServerEvents();
            ServerHandlers.SendingRemoteAdminCommand += ServerEvents.OnSendingRemoteAdminCommand;
            ServerHandlers.RoundEnded += ServerEvents.OnRoundEnded;

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            ServerHandlers.SendingRemoteAdminCommand -= ServerEvents.OnSendingRemoteAdminCommand;
            ServerHandlers.RoundEnded -= ServerEvents.OnRoundEnded;
            ServerEvents = null;

            base.OnDisabled();
        }
    }
}