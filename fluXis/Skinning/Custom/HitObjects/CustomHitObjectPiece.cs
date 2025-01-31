using fluXis.Skinning.Bases;
using fluXis.Skinning.Bases.HitObjects;
using fluXis.Skinning.Json;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;

namespace fluXis.Skinning.Custom.HitObjects;

public partial class CustomHitObjectPiece : ColorableSkinDrawable, ICanHaveSnapColor
{
    private int mode { get; }
    private bool isEnd { get; }
    private Drawable sprite { get; }

    public CustomHitObjectPiece(SkinJson skinJson, Texture texture, int mode, bool end)
        : base(skinJson)
    {
        this.mode = mode;
        isEnd = end;

        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        Anchor = Anchor.BottomCentre;
        Origin = Anchor.BottomCentre;
        InternalChild = sprite = new SkinnableSprite
        {
            RelativeSizeAxes = Axes.X,
            Texture = texture,
            Width = 1
        };
    }

    protected override void SetColor(Colour4 color)
    {
        var keymode = SkinJson.GetKeymode(mode);

        if (!keymode.TintNotes)
            return;

        if (isEnd && !keymode.TintLongNotes)
            return;

        sprite.Colour = color;
    }

    public void ApplySnapColor(int start, int end)
    {
        UseCustomColor = true;
        SetColor(SkinJson.SnapColors.GetColor(start));
    }
}
