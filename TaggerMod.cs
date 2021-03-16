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

                if (rCam.room != CurrentTiledRoom)
                {
                    int n = 0;
                    foreach (Room.Tile tile in rCam.room.Tiles)
                    {
                        TileMarker tileMarker = new TileMarker(rCam.room, tile);
                        rCam.room.AddObject(tileMarker);
                        n++;
                    }
                    Debug.Log($"grid : {rCam.room.Width}*{rCam.room.Height} = {n}");
                }
            }
        }

        public static Room CurrentTiledRoom { get; private set; }

        public static bool ChunkTagsVisible { get; private set; }
        public static bool SpriteTagsVisible { get; private set; }
        public static bool TileGridVisible { get; private set; }
    }
}
