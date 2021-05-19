// -----------------------------------------------------------------------
// <copyright file="CommandScript.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandLoader.API
{
    using System.Collections.Generic;

    /// <summary>
    /// A container for a parsed script.
    /// </summary>
    public class CommandScript
    {
        /// <summary>
        /// Gets or sets the executable name of the script.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the instructions to be executed when this script is ran.
        /// </summary>
        public List<Instruction> Instructions { get; set; }
    }
}