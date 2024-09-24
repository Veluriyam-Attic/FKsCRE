using FKsCRE.Content.Arrows.WulfrimArrow;
using FKsCRE.Content.凝胶;
using FKsCRE.Content.凝胶.霁云凝胶;
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
        #region 收发
        //重写这个方法来处理收发包
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            #region 序号说明
            // 30000 以后做其他 以前用于弹幕
            /*
             * 目前已使用 1 2 30000 30001
             * 30001 -> 寒元凝胶
            */
            // 2 -> 霁云凝胶
            // 1 -> 钨钢箭 WulfrimArrow
            #endregion
            Vector2 WulfrimArrowHold_cent = default;
            NPC WulfrimArrowHold_npc = null;
            int WulfrimArrowHold_player = default;
            //接收 目前该消息来自 HurricaneArrow
            //目前已经使用 1 2 3 30001 30002 30003 30004
            //霁云凝胶 2 30000 30004
            int a = reader.ReadInt32();
            switch(a)
            {
                case 1:
                    #region 钨钢箭
                    //WulfrimArrowHold_npc = Main.npc[reader.ReadInt32()];
                    WulfrimArrowHold_cent.X = reader.ReadSingle();
                    WulfrimArrowHold_cent.Y = reader.ReadSingle();
                    NPC npc = Main.npc[reader.ReadInt32()];
                    Hold hold  = npc.GetGlobalNPC<Hold>();
                    hold.setcent(WulfrimArrowHold_cent);
                    break;
                    #endregion
                case 2:
                    # region 霁云凝胶
                    NPC 霁云凝胶_npc = Main.npc[reader.ReadInt32()];
                    效果上身 xgss = 霁云凝胶_npc.GetGlobalNPC<效果上身>();
                    xgss.霁云凝胶_是否上身 = reader.ReadBoolean();
                    xgss.霁云凝胶_Time = reader.ReadInt32();
                    xgss.cnet.X = reader.ReadSingle();
                    xgss.cnet.Y = reader.ReadSingle();
                    break;
                case 30000:
                    Vector2 ve = default;
                    ve.X = reader.ReadInt32();
                    ve.Y = reader.ReadInt32();
                    NPC 霁云凝胶_npc_坐标同步 = Main.npc[reader.ReadInt32()];
                    霁云凝胶_npc_坐标同步.GetGlobalNPC<效果上身>().cnet = ve;
                    break;
                case 30004:
                    NPC npc云凝胶 = Main.npc[reader.ReadInt32()];
                    npc云凝胶.AddBuff(ModContent.BuffType<霁云凝胶_DeBuff>(),300);
                    break;
                #endregion
                case 3:
                    #region 寒元凝胶
                    bool r = reader.ReadBoolean();
                    NPC c = Main.npc[reader.ReadInt32()];
                    c.GetGlobalNPC<效果上身>().Set寒元凝胶(r);
                    break;
                case 30001:
                    NPC npc_ = Main.npc[reader.ReadInt32()];
                    效果上身 x = npc_.GetGlobalNPC<效果上身>();
                    x.Set寒元凝胶(false);
                    x.Set寒元凝胶Time(0);
                    x.寒元冲刺 = 0;
                    x.寒元冲刺次数记录 = 0;
                    x.寒元NPC速度记录 = default;
                    x.寒元位置记录 = default;
                    break;
                case 30002: //凝胶 - 寒元联机同步 部分-001
                    NPC 寒元凝胶 = Main.npc[reader.ReadInt32()];
                    效果上身 寒元NPC = 寒元凝胶.GetGlobalNPC<效果上身>();
                    寒元NPC.寒元位置记录.X = reader.ReadSingle();
                    寒元NPC.寒元位置记录.X = reader.ReadSingle();
                    寒元NPC.寒元NPC速度记录.X = reader.ReadSingle();
                    寒元NPC.寒元NPC速度记录.Y = reader.ReadSingle();
                    寒元NPC.寒元冲刺次数记录 = reader.ReadInt32();
                    break;
                case 30003:
                    Main.npc[reader.ReadInt32()].velocity = Vector2.Zero;
                    break;
                    #endregion
            }

            #region 废弃代码 置于末尾 2024/9/22 联机同步学习
            /*
            if (WulfrimArrowHold_npc != null)
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
            */
            #endregion
            base.HandlePacket(reader, whoAmI);
        }
            
        #endregion
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
