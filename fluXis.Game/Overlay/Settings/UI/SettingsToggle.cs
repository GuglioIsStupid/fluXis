using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace fluXis.Game.Overlay.Settings.UI;

public partial class SettingsToggle : SettingsItem
{
    public Bindable<bool> Bindable { get; init; } = new();

    private SpriteIcon icon;

    private Sample toggleOn;
    private Sample toggleOff;

    [BackgroundDependencyLoader]
    private void load(ISampleStore samples)
    {
        toggleOn = samples.Get(@"UI/toggle-on");
        toggleOff = samples.Get(@"UI/toggle-off");

        AddRange(new Drawable[]
        {
            icon = new SpriteIcon
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Size = new(25),
                Icon = FontAwesome.Solid.Check
            }
        });

        Bindable.BindValueChanged(e => icon.FadeTo(e.NewValue ? 1 : 0.4f), true);
    }

    protected override bool OnClick(ClickEvent e)
    {
        Bindable.Value = !Bindable.Value;

        if (Bindable.Value)
            toggleOn?.Play();
        else
            toggleOff?.Play();

        return true;
    }
}
