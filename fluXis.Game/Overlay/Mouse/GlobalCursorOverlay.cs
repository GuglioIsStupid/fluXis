using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace fluXis.Game.Overlay.Mouse;

public partial class GlobalCursorOverlay : Container
{
    public Vector2 RelativePosition => new(Cursor.Position.X / DrawWidth, Cursor.Position.Y / DrawHeight);

    protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };
    public MenuCursor Cursor { get; }

    private BindableBool showCursor { get; } = new(true);

    public bool ShowCursor
    {
        set => showCursor.Value = value;
    }

    public GlobalCursorOverlay()
    {
        RelativeSizeAxes = Axes.Both;
        AlwaysPresent = true;

        AddRangeInternal(new Drawable[]
        {
            Content,
            Cursor = new MenuCursor()
        });
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        showCursor.BindValueChanged(visible =>
        {
            if (visible.NewValue)
                Cursor.Show();
            else
                Cursor.Hide();
        }, true);
    }
}
