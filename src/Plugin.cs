global using UnityEngine;

using BepInEx;
using System.Security.Permissions;

#pragma warning disable CS0618 // Do not remove the following line.
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace PhysicalObjectTools;
using DebuggingSprites;

[BepInPlugin("casheww.physical_object_tools", "Physical Object Tools", "0.3.0")]
public sealed class Plugin : BaseUnityPlugin
{
    public void OnEnable()
    {
        On.RainWorld.OnModsInit += (orig, rw) => {
            orig(rw);
            ChunkTagsVisible = false;
            SpriteTagsVisible = false;
            TileGridVisible = false;

            try {
                MachineConnector.SetRegisteredOI(Info.Metadata.GUID, new ConfigOI(this));
            }
            catch (System.Exception e) {
                Logger.LogError(e);
            }
        };

        On.Player.Update += (orig, self, eu) => {
            orig(self, eu);
            if (self.room?.abstractRoom != null)
                CurrentRoomName = self.room.abstractRoom.name;
        };

        On.RoomCamera.SpriteLeaser.ctor += SpriteLeaser_ctor;
    }

    public void Update() {
        if (ConfigOI.ChunkKey == null)
            return;

        if (Input.GetKeyDown(ConfigOI.ChunkKey.Value))
            ChunkTagsVisible = !ChunkTagsVisible;

        if (Input.GetKeyDown(ConfigOI.SpriteKey.Value))
            SpriteTagsVisible = !SpriteTagsVisible;

        if (Input.GetKeyDown(ConfigOI.TileGridKey.Value))
            TileGridVisible = !TileGridVisible;
    }

    private void SpriteLeaser_ctor(On.RoomCamera.SpriteLeaser.orig_ctor orig, RoomCamera.SpriteLeaser self, 
            IDrawable obj, RoomCamera rCam) {
        orig(self, obj, rCam);

        bool roomChanged = CurrentRoomName != rCam.room.abstractRoom.name;
        CurrentRoomName = rCam.room.abstractRoom.name;

        if (obj is ChunkLabel || obj is SpriteLabel || obj is TileGridLine)
            return;

        PhysicalObject physobj = obj as PhysicalObject ?? (obj is GraphicsModule g ? g.owner : null);
        
        if (physobj != null) {
            AddChunkLabels(self, physobj);
            AddSpriteLabels(self, physobj);
        }

        if (roomChanged) {
            AddTileGrid(rCam.room);
        }
    }

    public void AddChunkLabels(RoomCamera.SpriteLeaser sLeaser, PhysicalObject physobj) {
        for (int i = 0; i < physobj.bodyChunks.Length; i++) {
            ChunkLabel l = new ChunkLabel(physobj.bodyChunks[i], i);
            physobj.room.AddObject(l);
        }
    }

    public void AddSpriteLabels(RoomCamera.SpriteLeaser sLeaser, PhysicalObject physobj) {
        for (int i = 0; i < sLeaser.sprites.Length; i++) {
            SpriteLabel l = new SpriteLabel(sLeaser.sprites[i], physobj, i);
            physobj.room.AddObject(l);
        }
    }

    public void AddTileGrid(Room room) {
        for (int x = 0; x < room.TileWidth; x++) {
            TileGridLine l = new TileGridLine(TileGridLine.Orientation.Vertical, x);
            room.AddObject(l);
        }
        for (int y = 0; y < room.TileHeight; y++) {
            TileGridLine l = new TileGridLine(TileGridLine.Orientation.Horizontal, y);
            room.AddObject(l);
        }
    }
    

    public static bool ChunkTagsVisible { get; private set; }
    public static bool SpriteTagsVisible { get; private set; }
    public static bool TileGridVisible { get; private set; }
    public static string CurrentRoomName { get; private set;}
}
