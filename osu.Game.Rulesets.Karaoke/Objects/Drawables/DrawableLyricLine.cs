﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Objects.Drawables;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    public class DrawableLyricLine : DrawableKaraokeHitObject
    {
        private readonly KarakeSpriteText karoakText;
        private readonly OsuSpriteText translateText;

        public new LyricLine HitObject => (LyricLine)base.HitObject;

        public DrawableLyricLine(LyricLine hitObject)
            : base(hitObject)
        {
            Scale = new Vector2(2f);
            AutoSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                karoakText = new KarakeSpriteText(),
                translateText = new OsuSpriteText
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.TopLeft,
                    Text = hitObject.TranslateText
                }
            };

            hitObject.TextBindable.BindValueChanged(text =>
            {
                karoakText.Text = text.NewValue;
            }, true);

            hitObject.TimeTagsBindable.BindValueChanged(timeTags =>
            {
                karoakText.TimeTags = timeTags.NewValue;
            }, true);

            hitObject.RubyTagsBindable.BindValueChanged(rubyTags =>
            {
                karoakText.Rubies = rubyTags.NewValue?.Select(x => new PositionText(x.Ruby, x.StartIndex, x.EndIndex)).ToArray();
            }, true);

            hitObject.RomajiTagsBindable.BindValueChanged(romajiTags =>
            {
                karoakText.Romajies = romajiTags.NewValue?.Select(x => new PositionText(x.Romaji, x.StartIndex, x.EndIndex)).ToArray();
            }, true);

            hitObject.FontIndexBindable.BindValueChanged(index =>
            {
                ApplySkin(CurrentSkin, false);
            }, true);

            hitObject.LayoutIndexBindable.BindValueChanged(index =>
            {
                ApplySkin(CurrentSkin, false);
            }, true);

            hitObject.TranslateTextBindable.BindValueChanged(text =>
            {
                translateText.Text = text.NewValue ?? "";
            }, true);

            LifetimeEnd = hitObject.EndTime + 1000;
        }

        protected override void ClearInternal(bool disposeChildren = true)
        {
        }

        protected override void ApplySkin(ISkinSource skin, bool allowFallback)
        {
            base.ApplySkin(skin, allowFallback);

            if (CurrentSkin == null)
                return;

            var karaokeFont = skin.GetConfig<KaraokeSkinLookup, KaraokeFont>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricStyle, HitObject.FontIndex))?.Value;
            if (karaokeFont != null)
                ApplyFont(karaokeFont);

            var karaokeLayout = skin.GetConfig<KaraokeSkinLookup, KaraokeLayout>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricLayout, HitObject.LayoutIndex))?.Value;
            if (karaokeLayout != null)
                ApplyLayout(karaokeLayout);
        }

        protected virtual void ApplyFont(KaraokeFont font)
        {
            // From text sample
            karoakText.FrontTextTexture = new SolidTexture { SolidColor = Color4.Blue }; // font.FrontTextBrushInfo.TextBrush.ConvertToTextureSample();
            karoakText.FrontBorderTexture = font.FrontTextBrushInfo.BorderBrush.ConvertToTextureSample();
            karoakText.FrontTextShadowTexture = font.FrontTextBrushInfo.ShadowBrush.ConvertToTextureSample();

            // Back text sample
            karoakText.BackTextTexture = font.BackTextBrushInfo.TextBrush.ConvertToTextureSample();
            karoakText.BackBorderTexture = font.BackTextBrushInfo.BorderBrush.ConvertToTextureSample();
            karoakText.BackTextShadowTexture = font.BackTextBrushInfo.ShadowBrush.ConvertToTextureSample();

            // Apply text font info
            var lyricFont = font.LyricTextFontInfo.LyricTextFontInfo;
            karoakText.Font = new FontUsage(size: lyricFont.CharSize); // TODO : FontName and Bold
            karoakText.Border = lyricFont.EdgeSize > 0;
            karoakText.BorderRadius = lyricFont.EdgeSize;

            var rubyFont = font.RubyTextFontInfo.LyricTextFontInfo;
            karoakText.RubyFont = new FontUsage(size: rubyFont.CharSize); // TODO : FontName and Bold

            var romajiFont = font.RomajiTextFontInfo.LyricTextFontInfo;
            karoakText.RomajiFont = new FontUsage(size: romajiFont.CharSize); // TODO : FontName and Bold

            // Apply shadow
            karoakText.Shadow = font.UseShadow;
            karoakText.ShadowOffset = font.ShadowOffset;
        }

        protected virtual void ApplyLayout(KaraokeLayout layout)
        {
            // Layout relative to parent
            Anchor = layout.Alignment;
            Origin = layout.Alignment;
            Margin = new MarginPadding
            {
                Left = layout.Alignment.HasFlag(Anchor.x0) ? layout.HorizontalMargin : 0,
                Right = layout.Alignment.HasFlag(Anchor.x2) ? layout.HorizontalMargin : 0,
                Top = layout.Alignment.HasFlag(Anchor.y0) ? layout.VerticalMargin : 0,
                Bottom = layout.Alignment.HasFlag(Anchor.y2) ? layout.VerticalMargin : 0
            };
            Padding = new MarginPadding(30);

            // Layout to text
            karoakText.Continuous = layout.Continuous;
            karoakText.KaraokeTextSmartHorizon = layout.SmartHorizon;
            karoakText.Spacing = new Vector2(layout.LyricsInterval, karoakText.Spacing.Y);

            // Ruby
            karoakText.RubySpacing = new Vector2(layout.RubyInterval, karoakText.RubySpacing.Y);
            karoakText.RubyAlignment = layout.RubyAlignment;
            karoakText.RubyMargin = layout.RubyMargin;

            // Romaji
            karoakText.RomajiSpacing = new Vector2(layout.RomajiInterval, karoakText.RomajiSpacing.Y);
            karoakText.RomajiAlignment = layout.RomajiAlignment;
            karoakText.RomajiMargin = layout.RomajiMargin;
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (userTriggered || Time.Current < HitObject.EndTime)
                return;

            ApplyResult(r => { r.Type = HitResult.Meh; });
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            using (BeginDelayedSequence(HitObject.Duration, true))
            {
                const float fade_out_time = 500;
                this.FadeOut(fade_out_time);
            }
        }

        public override double LifetimeStart
        {
            get => base.LifetimeStart;
            set
            {
                base.LifetimeStart = value;
                karoakText.LifetimeStart = value;
                translateText.LifetimeStart = value;
            }
        }

        public override double LifetimeEnd
        {
            get => base.LifetimeEnd;
            set
            {
                base.LifetimeEnd = value;
                karoakText.LifetimeEnd = value;
                translateText.LifetimeEnd = value;
            }
        }
    }
}
