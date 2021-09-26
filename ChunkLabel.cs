using System;
using UnityEngine;

namespace HeadsShouldersKneesAndToes
{
    class ChunkLabel : CosmeticSprite
    {
        public ChunkLabel(BodyChunk chunk, int index)
        {
            this.chunk = chunk;

            // set label text properties
            label = new FLabel("DisplayFont", index.ToString())
            {
                color = Color.white,
                isVisible = TaggerMod.ChunkTagsVisible,
                anchorX = 0.5f,
                scale = 0.7f
            };
        }

        public override void Update(bool eu)
        {
            if (chunk.owner?.room?.abstractRoom?.name != TaggerMod.CurrentRoomName || chunk.owner.slatedForDeletetion)
            {
                Destroy();
            }

            base.Update(eu);
        }

        public override void Destroy()
        {
            label.isVisible = false;
            label.text = "";
            base.Destroy();
        }

        public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            // set label background properties (sans color)
            sLeaser.sprites = new FSprite[1];
            sLeaser.sprites[0] = new FSprite("pixel", true);
            AddToContainer(sLeaser, rCam, null);
        }

        public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            Vector2 pos = Vector2.Lerp(chunk.lastPos, chunk.pos, timeStacker) - camPos;
            sLeaser.sprites[0].SetPosition(pos);
            sLeaser.sprites[0].scaleX = sLeaser.sprites[0].scaleY = chunk.rad * 2;
            label.SetPosition(pos);
            sLeaser.sprites[0].isVisible = TaggerMod.ChunkTagsVisible;
            label.isVisible = TaggerMod.ChunkTagsVisible;

            base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
        }

        public override void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            base.AddToContainer(sLeaser, rCam, null);
            label.RemoveFromContainer();
            sLeaser.sprites[0].RemoveFromContainer();
            rCam.ReturnFContainer("Foreground").AddChild(sLeaser.sprites[0]);
            rCam.ReturnFContainer("Foreground").AddChild(label);
        }

        public override void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            // set colour of label background
            Color color = palette.skyColor * 0.9f;
            color.a = 0.7f;
            sLeaser.sprites[0].color = color;
        }

        readonly BodyChunk chunk;
        readonly FLabel label;
    }
}
