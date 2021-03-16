using System;
using UnityEngine;

namespace HeadsShouldersKneesAndToes
{
    class TileMarker : CosmeticSprite
    {
        public TileMarker(Room room, Room.Tile tile)
        {
            startRoomName = room.abstractRoom.name;
            this.tile = tile;
        }

        readonly Room.Tile tile;
        readonly string startRoomName;

        public override void Update(bool eu)
        {
            try
            {
                if (room.abstractRoom.name != startRoomName)
                {
                    Destroy();
                }
            }
            catch (NullReferenceException) { }

            base.Update(eu);
        }

        public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[1];
            sLeaser.sprites[0] = new FSprite("pixel", true)
            {
                scaleX = 5f,
                scaleY = 5f
            };
            AddToContainer(sLeaser, rCam, null);
        }

        public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            sLeaser.sprites[0].isVisible = TaggerMod.TileGridVisible;
            sLeaser.sprites[0].SetPosition(rCam.room.MiddleOfTile(tile.X, tile.Y) - camPos);
            base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
        }

        public override void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            base.AddToContainer(sLeaser, rCam, null);
            sLeaser.sprites[0].RemoveFromContainer();
            rCam.ReturnFContainer("Foreground").AddChild(sLeaser.sprites[0]);
        }

        public override void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            Color color = Color.white;
            color.a = 0.5f;
            sLeaser.sprites[0].color = color;
        }

    }
}
