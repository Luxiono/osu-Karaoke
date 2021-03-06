﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Tests.Visual;
using static osu.Game.Rulesets.Karaoke.Mods.KaraokeModPractice;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class TestCaseKaraokeOverlay : OsuTestScene
    {
        public KaraokePanelOverlay KaraokePanelOverlay { get; set; }

        [BackgroundDependencyLoader]
        private void load(RulesetStore rulesets)
        {
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Add(KaraokePanelOverlay = new KaraokePanelOverlay(null,null)
            {
                RelativeSizeAxes = Axes.Both,
            });

            AddStep("Toggle", KaraokePanelOverlay.ToggleVisibility);
        }
    }
}
