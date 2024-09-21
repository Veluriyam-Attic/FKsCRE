using FKsCRE.Content.Ammunition.WulfrimArrow;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Testing;

namespace FKsCRE
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
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            Vector2 WulfrimArrowHold_cent = default;
            NPC WulfrimArrowHold_npc = null;
            int WulfrimArrowHold_player = default;
            //接收 目前该消息来自 HurricaneArrow
            int a = reader.ReadInt32();
            switch(a)
            {
                case 1:
                    WulfrimArrowHold_npc = Main.npc[reader.ReadInt32()];
                    break;
            }
            if(WulfrimArrowHold_npc != null)
            {
                Hold WulfrimArrowHold = WulfrimArrowHold_npc.GetGlobalNPC<Hold>();
                ModPacket packet = GetPacket();
                //标识符
                packet.Write("WulfrimArrowHold_NPC");
                //发送x
                packet.Write(WulfrimArrowHold.getcent().X);
                //发送y
                packet.Write(WulfrimArrowHold.getcent().Y);
                //发送Time
                packet.Write(WulfrimArrowHold.gettime());
                packet.Send(default, default);
            }

            if(WulfrimArrowHold_npc != default && WulfrimArrowHold_npc != null)
            {
                ModPacket packet = GetPacket();
                packet.Write(WulfrimArrowHold_cent.X);
                packet.Write(WulfrimArrowHold_cent.Y);
                packet.Send(default,-1);
                if (Main.netMode == NetmodeID.Server)
                {
                    
                }
                else
                {
                    Hold hold = WulfrimArrowHold_npc.GetGlobalNPC<Hold>();
                    hold.setcent(WulfrimArrowHold_cent);
                }
            }
            base.HandlePacket(reader, whoAmI);
        }
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
                if (Main.player[projectile.owner] != null)
                {
                    player = Main.player[projectile.owner];
                    //手上的物品
                    item = player.inventory[player.selectedItem];
                }
            }
            return base.PreAI(projectile);
        }
        public Item GetItem()
        {
            if (item == null && item == default)
            {
                return ModContent.GetModItem(1).Item;
            }
            return item;
        }
        public Player GetPlayer()
        {
            if (player == null && player == default)
            {
                return Main.player[0];
            }
            return player;
        }

    }
}
