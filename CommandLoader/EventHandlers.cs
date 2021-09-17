// -----------------------------------------------------------------------
// <copyright file="EventHandlers.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CommandLoader.EventHandlers
{
    using Exiled.Events.EventArgs;
    using MEC;

    /// <summary>
    /// Runs methods subscribed to events in <see cref="Exiled.Events.Handlers.Server"/>.
    /// </summary>
    public class EventHandlers
    {
        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundEnded(RoundEndedEventArgs)"/>
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (CoroutineHandle coroutineHandle in Coroutines)
                Timing.KillCoroutines(coroutineHandle);

            Coroutines.Clear();
        }
    }
}