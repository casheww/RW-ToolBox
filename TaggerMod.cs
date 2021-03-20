using System;
using BepInEx;
using UnityEngine;

namespace HeadsShouldersKneesAndToes
{
    [BepInPlugin("casheww.chunk_tagger", "Tagger", "1.0")]
    class TaggerMod : BaseUnityPlugin
    {

        void OnEnable()
        {
            On.GraphicsModule.InitiateSprites += GraphicsModule_InitiateSprites;
            ChunkTagsVisible = false;
            SpriteTagsVisible = false;
            TileGridVisible = false;
        }

        void Update()
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

        void GraphicsModule_InitiateSprites(On.GraphicsModule.orig_InitiateSprites orig,
                GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            orig(self, sLeaser, rCam);

            if (self.owner is Creature)
            {
                for (int i = 0; i < self.owner.bodyChunks.Length; i++)
                {
                    ChunkLabel chunkLabel = new ChunkLabel(self.owner.bodyChunks[i], i, rCam.room);
                    rCam.room.AddObject(chunkLabel);
                }

                for (int i = 0; i < sLeaser.sprites.Length; i++)
                {
                    SpriteLabel spriteLabel = new SpriteLabel(sLeaser.sprites[i],
                            self.owner as Creature, i, rCam.room);
                    rCam.room.AddObject(spriteLabel);
                }

                if (rCam.room.abstractRoom.name != CurrentTiledRoomName)
                {
                    for (int x = 0; x < rCam.room.Width; x++)
                    {
                        TileGridLine tileMarker = new TileGridLine(rCam.room, TileGridLine.Orientation.Vertical, x);
                        rCam.room.AddObject(tileMarker);
                    }
                    for (int y = 0; y < rCam.room.Height; y++)
                    {
                        TileGridLine tileMarker = new TileGridLine(rCam.room, TileGridLine.Orientation.Horizontal, y);
                        rCam.room.AddObject(tileMarker);
                    }
                    CurrentTiledRoomName = rCam.room.abstractRoom.name;
                }
            }
        }

        public static string CurrentTiledRoomName { get; private set; }

        public static bool ChunkTagsVisible { get; private set; }
        public static bool SpriteTagsVisible { get; private set; }
        public static bool TileGridVisible { get; private set; }
    }
}
