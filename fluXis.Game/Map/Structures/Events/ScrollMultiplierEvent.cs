﻿using fluXis.Game.Map.Structures.Bases;
using fluXis.Game.Screens.Gameplay.Ruleset.HitObjects;
using Newtonsoft.Json;
using osu.Framework.Graphics;

namespace fluXis.Game.Map.Structures.Events;

public class ScrollMultiplierEvent : IMapEvent, IHasDuration, IHasEasing, IApplicableToHitManager
{
    [JsonProperty("time")]
    public double Time { get; set; }

    [JsonProperty("duration")]
    public double Duration { get; set; }

    [JsonProperty("multiplier")]
    public float Multiplier { get; set; } = 1;

    [JsonProperty("ease")]
    public Easing Easing { get; set; }

    public void Apply(HitObjectManager manager)
    {
        using (manager.BeginAbsoluteSequence(Time))
            manager.TransformTo(nameof(manager.DirectScrollMultiplier), Multiplier, Duration, Easing);
    }
}
