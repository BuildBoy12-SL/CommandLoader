// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandLoader
{
    using System.IO;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;

    /// <inheritdoc cref="IConfig"/>
    public class Config : IConfig
    {
        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether debug messages should be displayed.
        /// </summary>
        public bool ShowDebug { get; set; } = false;

        /// <summary>
        /// Gets or sets the directory containing command scripts.
        /// </summary>
        public string Folder { get; set; } = Path.Combine(Paths.Configs, "CommandLoader");
    }
}