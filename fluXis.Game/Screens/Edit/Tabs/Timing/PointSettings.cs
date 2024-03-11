using fluXis.Game.Map.Structures;
using fluXis.Game.Screens.Edit.Tabs.Timing.Settings.UI;
using fluXis.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace fluXis.Game.Screens.Edit.Tabs.Timing;

public partial class PointSettings<T> : PointSettings
    where T : ITimedObject
{
    public T Point { get; }

    public BasicPointSettingsField TimeField { get; }

    [Resolved]
    protected EditorValues Values { get; private set; }

    public PointSettings(T point)
    {
        Point = point;

        Add(TimeField = new BasicPointSettingsField
        {
            Label = "Time",
            Text = Point.Time.ToStringInvariant()
        });
    }
}

public partial class PointSettings : FillFlowContainer
{
    protected PointSettings()
    {
        Direction = FillDirection.Vertical;
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        Spacing = new Vector2(0, 10);
    }
}
