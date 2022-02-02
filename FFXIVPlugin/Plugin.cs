using System.IO;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace FFXIVPlugin;

public sealed class Plugin : IDalamudPlugin
{
    private const string CommandName = "/goatsarecool";

    public Plugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
        [RequiredVersion("1.0")] CommandManager commandManager,
        [RequiredVersion("1.0")] ObjectTable objectTable)
    {
        PluginInterface = pluginInterface;
        CommandManager = commandManager;
        ObjectTable = objectTable;

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);

        // you might normally want to embed resources and load them from the manifest stream
        var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
        var goatImage = PluginInterface.UiBuilder.LoadImage(imagePath);

        PluginUi = new PluginUi(Configuration, goatImage, ObjectTable);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "A useful message to display in /xlhelp"
        });

        PluginInterface.UiBuilder.Draw += DrawUi;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUi;
    }

    private DalamudPluginInterface PluginInterface { get; }
    private CommandManager CommandManager { get; }
    private ObjectTable ObjectTable { get; }
    private Configuration Configuration { get; }
    private PluginUi PluginUi { get; }
    public string Name => "Sample Plugin";

    public void Dispose()
    {
        PluginUi.Dispose();
        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just display our main ui
        PluginUi.Visible = true;
    }

    private void DrawUi()
    {
        PluginUi.Draw();
    }

    private void DrawConfigUi()
    {
        PluginUi.SettingsVisible = true;
    }
}