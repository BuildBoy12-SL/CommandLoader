// -----------------------------------------------------------------------
// <copyright file="Loader.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandLoader
{
    using System.Collections.Generic;
    using System.IO;
    using CommandLoader.API;

    /// <summary>
    /// Handles the loading and storing of all command scripts to <see cref="CommandScript"/>.
    /// </summary>
    public static class Loader
    {
        static Loader()
        {
            Folder = Plugin.Instance.Config.Folder;
            if (Directory.Exists(Folder))
                return;

            Directory.CreateDirectory(Folder);
            File.WriteAllText(Path.Combine(Folder, "ExampleScript"), Exiled.Loader.Loader.Serializer.Serialize(new List<Instruction>
            {
                new Instruction
                {
                    Command = "doortp",
                    Arguments = new[] { "*SenderId", "SURFACE_GATE" },
                    Delay = 2f,
                },
                new Instruction
                {
                    Command = "pbc",
                    Arguments = new[] { "*SenderId", "5", "Welcome, *SenderName, to the funhouse." },
                },
            }));
        }

        /// <summary>
        /// Gets all generated <see cref="CommandScript"/>s.
        /// </summary>
        public static List<CommandScript> Commands { get; } = new List<CommandScript>();

        /// <summary>
        /// Gets the path for the directory containing commands.
        /// </summary>
        public static string Folder { get; }

        /// <summary>
        /// Handles the loading of all command scripts.
        /// </summary>
        public static void LoadCommands()
        {
            foreach (string file in Directory.GetFiles(Folder))
            {
                Commands.Add(new CommandScript
                {
                    Name = Path.GetFileName(file),
                    Instructions = Exiled.Loader.Loader.Deserializer.Deserialize<List<Instruction>>(File.ReadAllText(file)),
                });
            }
        }
    }
}