using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace NanTing
{
    public class TimeVer
    {
        public Vector2 v2 = default;
        public TimeVer(Vector2 vector2)
        {
            v2 = vector2;
        }
    }

	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class NanTing : Mod
	{
        //重写这个方法来处理收发包
        //public override void HandlePacket(BinaryReader reader, int whoAmI)
        //{
        //    //接收 目前该消息来自 HurricaneArrow
        //    float st = reader.ReadSingle();
        //    float st_ = reader.ReadSingle();
        //    Console.WriteLine(st + st_);
        //    base.HandlePacket(reader, whoAmI);
        //}
    }

	public class ModTime : ModSystem
	{
		public static int Time = 0;
        public override void UpdateUI(GameTime gameTime)
        {
            if (!Main.gamePaused)
            {
                Time++;
            }
            if (Time >= 86400)
            {
                Time = 0;
            }
            base.UpdateUI(gameTime);
        }
    }

    public class NanTingGProje : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private int num = 0;
        private Player player = default;
        private Item item = default;
        public override bool PreAI(Projectile projectile)
        {
            if (num == 0)
            {
                player = Main.player[projectile.owner];
                //手上的物品
                item = player.inventory[player.selectedItem];
            }
            return base.PreAI(projectile);
        }
        public Item GetItem()
        {
            return item;
        }
        public Player GetPlayer()
        {
            return player;
        }

    }
}
