using fluXis.Game.Configuration;
using fluXis.Game.Map;
using fluXis.Game.Screens.Gameplay.Ruleset.TimingLines;
using fluXis.Game.Skinning;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace fluXis.Game.Screens.Gameplay.Ruleset;

public partial class Playfield : Container
{
    [Resolved]
    private SkinManager skinManager { get; set; }

    public FillFlowContainer<Receptor> Receptors;
    public GameplayScreen Screen;
    public HitObjectManager Manager;
    public Stage Stage;

    private TimingLineManager timingLineManager;
    private Drawable hitLine;

    private Container laneCovers;
    private Drawable topCover;
    private Drawable bottomCover;

    private Bindable<float> topCoverHeight;
    private Bindable<float> bottomCoverHeight;
    private Bindable<ScrollDirection> scrollDirection;

    public bool IsUpScroll => scrollDirection.Value == ScrollDirection.Up;

    public MapInfo Map { get; }

    public Playfield(GameplayScreen screen)
    {
        Screen = screen;
        Map = screen.Map;
    }

    [BackgroundDependencyLoader]
    private void load(FluXisConfig config)
    {
        RelativeSizeAxes = Axes.Both;
        Anchor = Anchor.Centre;
        Origin = Anchor.Centre;

        topCoverHeight = config.GetBindable<float>(FluXisSetting.LaneCoverTop);
        bottomCoverHeight = config.GetBindable<float>(FluXisSetting.LaneCoverBottom);
        scrollDirection = config.GetBindable<ScrollDirection>(FluXisSetting.ScrollDirection);

        Stage = new Stage(this);
        Receptors = new FillFlowContainer<Receptor>
        {
            AutoSizeAxes = Axes.X,
            RelativeSizeAxes = Axes.Y,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Direction = FillDirection.Horizontal
        };

        hitLine = skinManager.GetHitLine();
        hitLine.Y = -skinManager.CurrentSkin.GetKeymode(Map.KeyCount).HitPosition;

        Manager = new HitObjectManager(this);
        Manager.LoadMap(Map);

        timingLineManager = new TimingLineManager(Manager);

        for (int i = 0; i < Map.KeyCount; i++)
        {
            Receptor receptor = new Receptor(i) { Playfield = this };
            Receptors.Add(receptor);
        }

        laneCovers = new Container
        {
            RelativeSizeAxes = Axes.Y,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Children = new[]
            {
                topCover = skinManager.GetLaneCover(false),
                bottomCover = skinManager.GetLaneCover(true)
            }
        };

        InternalChildren = new[]
        {
            Stage,
            timingLineManager,
            Manager,
            Receptors,
            hitLine,
            laneCovers
        };
    }

    protected override void LoadComplete()
    {
        timingLineManager.CreateLines(Map);
        base.LoadComplete();
    }

    protected override void Update()
    {
        hitLine.Width = Stage.Width;
        laneCovers.Width = Stage.Width;

        topCover.Y = (topCoverHeight.Value - 1f) / 2f;
        bottomCover.Y = (1f - bottomCoverHeight.Value) / 2f;
    }
}
