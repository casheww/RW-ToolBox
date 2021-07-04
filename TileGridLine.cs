using System;
using UnityEngine;

namespace HeadsShouldersKneesAndToes
{
    class TileGridLine : CosmeticSprite
    {
        public TileGridLine(Orientation o, int n)
        {
            this.o = o;
            this.n = n;
        }

        readonly Orientation o;
        readonly int n;

        public override void Update(bool eu)
        {
            if (room.abstractRoom?.name != TaggerMod.CurrentRoomName)
            {
                Destroy();
            }

            base.Update(eu);
        }

        public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[1];
            if (o == Orientation.Vertical)
            {
                sLeaser.sprites[0] = new FSprite("pixel", true)
                {
                    scaleX = 1f,
                    scaleY = rCam.room.PixelHeight
                };
            }
            else
            {
                sLeaser.sprites[0] = new FSprite("pixel", true)
                {
                    scaleX = rCam.room.PixelWidth,
                    scaleY = 1f
                };
            }
            AddToContainer(sLeaser, rCam, null);
        }

        public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            sLeaser.sprites[0].isVisible = TaggerMod.TileGridVisible;
            if (o == Orientation.Vertical)
            {
                sLeaser.sprites[0].SetPosition(rCam.room.MiddleOfTile(n, rCam.room.TileHeight / 2) - camPos - new Vector2(10, 0));
            }
            else
            {
                sLeaser.sprites[0].SetPosition(rCam.room.MiddleOfTile(rCam.room.TileWidth / 2, n) - camPos - new Vector2(0, 10));
            }
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

        public enum Orientation
        {
            Vertical, Horizontal
        }
    }
}
