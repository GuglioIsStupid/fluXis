using System;
using System.Linq;
using System.Threading.Tasks;
using fluXis.Desktop.Integration;
using fluXis.Game;
using fluXis.Game.Configuration;
using fluXis.Game.Integration;
using fluXis.Game.IPC;
using fluXis.Game.Updater;
using fluXis.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Platform;

namespace fluXis.Desktop;

public partial class FluXisGameDesktop : FluXisGame
{
    private IPCImportChannel ipc;
    private VelopackUpdatePerformer updatePerformer;

    public override void SetHost(GameHost host)
    {
        base.SetHost(host);

        var window = host.Window;
        window.Title = "fluXis " + VersionString;
        // window.ConfineMouseMode.Value = ConfineMouseMode.Never;
        window.CursorState = CursorState.Hidden;
        window.DragDrop += f => Task.Run(() => HandleDragDrop(f));
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        ipc = new IPCImportChannel(Host, this);
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        new DiscordActivity().Initialize(this, APIClient);

        var args = Program.Args.ToList();

        if (args.Contains("--debug-join-multi"))
        {
            var idx = args.IndexOf("--debug-join-multi");
            var id = args[idx + 1];
            JoinMultiplayerRoom(id.ToIntInvariant(), "");
        }

        args.RemoveAll(a => a.StartsWith('-'));
        WaitForReady(() => HandleDragDrop(args.ToArray()));
    }

    protected override bool RestartOnClose() => updatePerformer?.RestartOnClose() ?? false;

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        ipc?.Dispose();
    }

    public override LightController CreateLightController() => Config.Get<bool>(FluXisSetting.OpenRGBIntegration) ? new OpenRGBController() : new LightController();
    public override IUpdatePerformer CreateUpdatePerformer() => OperatingSystem.IsWindows() ? updatePerformer ??= new VelopackUpdatePerformer(NotificationManager) : null;
}
