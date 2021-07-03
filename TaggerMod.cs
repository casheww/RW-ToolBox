using System;
using BepInEx;
using UnityEngine;

namespace HeadsShouldersKneesAndToes
{
    [BepInPlugin("casheww.chunk_tagger", "Tagger", "1.1")]
    class TaggerMod : BaseUnityPlugin
    {
        public void OnEnable()
        {
            On.RoomCamera.SpriteLeaser.ctor += SpriteLeaser_ctor;
            ChunkTagsVisible = false;
            SpriteTagsVisible = false;
            TileGridVisible = false;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                ChunkTagsVisible = !ChunkTagsVisible;
            }
            if (Input.GetKeyDown(KeyCode.Minus))
            {
                SpriteTagsVisible = !SpriteTagsVisible;
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                TileGridVisible = !TileGridVisible;
            }
        }

        private void SpriteLeaser_ctor(On.RoomCamera.SpriteLeaser.orig_ctor orig, RoomCamera.SpriteLeaser self, IDrawable obj, RoomCamera rCam)
        {
            orig(self, obj, rCam);

            if (!(obj is ChunkLabel || obj is SpriteLabel || obj is TileGridLine))
            {
                var physobjs = obj as PhysicalObject ?? (obj is GraphicsModule g ? g.owner : null);

                if (physobjs != null)
                {
                    for (int i = 0; i < physobjs.bodyChunks.Length; i++)
                    {
                        ChunkLabel chunkLabel = new ChunkLabel(physobjs.bodyChunks[i], i);
                        rCam.room.AddObject(chunkLabel);
                    }
                    for (int i = 0; i < self.sprites.Length; i++)
                    {
                        SpriteLabel spriteLabel = new SpriteLabel(self.sprites[i], physobjs, i);
                        rCam.room.AddObject(spriteLabel);
                    }
                }

                if (rCam.room.abstractRoom.name != CurrentRoomName)
                {
                    for (int x = 0; x < rCam.room.TileWidth; x++)
                    {
                        TileGridLine tileMarker = new TileGridLine(TileGridLine.Orientation.Vertical, x);
                        rCam.room.AddObject(tileMarker);
                    }
                    for (int y = 0; y < rCam.room.TileHeight; y++)
                    {
                        TileGridLine tileMarker = new TileGridLine(TileGridLine.Orientation.Horizontal, y);
                        rCam.room.AddObject(tileMarker);
                    }
                }
            }

            CurrentRoomName = rCam.room.abstractRoom.name;
        }

        public static string CurrentRoomName { get; private set; }

        public static bool ChunkTagsVisible { get; private set; }
        public static bool SpriteTagsVisible { get; private set; }
        public static bool TileGridVisible { get; private set; }
    }
}
