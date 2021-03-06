﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class ColumnBackground : Box
    {
        public const float COLUMN_HEIGHT = 20;

        public ColumnBackground(int index)
        {
            RelativeSizeAxes = Axes.X;
            Height = COLUMN_HEIGHT;
            Alpha = 0.15f;
        }

        private bool isSpecial;

        public bool IsSpecial
        {
            get => isSpecial;
            set
            {
                if (isSpecial == value)
                    return;

                isSpecial = value;
            }
        }
    }
}
