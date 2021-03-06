﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class TestSceneFlashlightMod : TestSceneKaraokePlayer
    {
        protected override Player CreatePlayer(Ruleset ruleset)
        {
            SelectedMods.Value = new[] { new KaraokeModFlashlight() };

            return base.CreatePlayer(ruleset);
        }
    }
}
