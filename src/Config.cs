using Menu.Remix.MixedUI;

namespace PhysicalObjectTools;

public sealed class ConfigOI : OptionInterface {
    public ConfigOI(Plugin plugin) {
        this.plugin = plugin;
        ChunkKey = config.Bind<KeyCode>("chunk_key", KeyCode.Minus);
        SpriteKey = config.Bind<KeyCode>("sprite_key", KeyCode.Equals);
        TileGridKey = config.Bind<KeyCode>("tile_grid_key", KeyCode.Delete);
    }

    public override void Initialize()
    {
        base.Initialize();

        Tabs = new OpTab[1] {new OpTab(this)};
        OpTab t = Tabs[0];

        t.AddItems(
            new OpLabel(new Vector2(150f, 550f), new Vector2(300f, 30f), plugin.Info.Metadata.Name, bigText: true),
            new OpLabel(new Vector2(150f, 520f), new Vector2(300f, 30f), plugin.Info.Metadata.Version.ToString())
        );

        t.AddItems(
            new OpLabel(75f, 475f, "Chunk key"),
            new OpKeyBinder(ChunkKey, new Vector2(180f, 475f), new Vector2(50f, 30f)),
            new OpLabel(75f, 440f, "Sprite key"),
            new OpKeyBinder(SpriteKey, new Vector2(180f, 440f), new Vector2(50f, 30f)),
            new OpLabel(75f, 405f, "Tile grid key"),
            new OpKeyBinder(TileGridKey, new Vector2(180f, 405f), new Vector2(50f, 30f))
        );
    }

    private readonly Plugin plugin;
    public static Configurable<KeyCode> ChunkKey {get; private set;}
    public static Configurable<KeyCode> SpriteKey {get; private set;}
    public static Configurable<KeyCode> TileGridKey {get; private set;}

}
