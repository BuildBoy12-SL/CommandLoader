﻿// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandLoader
{
    using System;
    using Exiled.API.Features;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        /// <summary>
        /// Gets an instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(3, 0, 0);

        /// <summary>
        /// Gets an instance of the <see cref="CommandLoader.CommandProcessor"/> class.
        /// </summary>
        public CommandProcessor CommandProcessor { get; private set; }

        /// <summary>
        /// Gets an instance of the <see cref="CommandLoader.EventHandlers"/> class.
        /// </summary>
        public EventHandlers EventHandlers { get; private set; }

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Instance = this;
            Loader.LoadCommands();

            CommandProcessor = new CommandProcessor(this);
            EventHandlers = new EventHandlers(this);
            ServerHandlers.RoundEnded += EventHandlers.OnRoundEnded;

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            ServerHandlers.RoundEnded -= EventHandlers.OnRoundEnded;
            EventHandlers = null;
            CommandProcessor = null;

            Instance = null;
            base.OnDisabled();
        }
    }
}