using FKsCRE.Content.凝胶.霁云凝胶;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FKsCRE.Content.凝胶
{
    #region 基础继承
    public abstract class 凝胶_DeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            //不能被护士消除
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            //不显示时间
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = false;//默认也是false 死亡清除
            Main.vanityPet[Type] = false;//宠物?
        }
    }
    public abstract class 凝胶 : ModItem
    {
        public override void SetDefaults()
        {
            //Item.damage = 10;
            Item.consumable = true;
            Item.ammo = AmmoID.Gel;
            Item.maxStack = 9999;
            base.SetDefaults();
        }
    }
    #endregion
    public class 效果上身 : GlobalNPC
    {
        //NPC运行在服务端
        public override bool InstancePerEntity => true;
        public bool 霁云凝胶_是否上身 = false;
        public int 霁云凝胶_Time = 0;
        //cent = 霁云凝胶
        public Vector2 cnet = default;
        public override void AI(NPC npc)
        {
            #region 霁云凝胶
            if (霁云凝胶_是否上身)
            {
                霁云凝胶_Time++;
                npc.GetGlobalNPC<效果上身>().cnet.X = npc.Center.X; 
                if (霁云凝胶_Time == 120) 
                { 
                    霁云凝胶_Time = 0; 
                    霁云凝胶_是否上身 = false;
                    cnet = default;
                }
                #region 联机同步 2->霁云凝胶
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = Mod.GetPacket();
                    效果上身 xgss = npc.GetGlobalNPC<效果上身>();
                    packet.Write(2);
                    //NPC在Main.npc中的索引
                    packet.Write(npc.whoAmI);
                    packet.Write(xgss.霁云凝胶_是否上身);
                    packet.Write(xgss.霁云凝胶_Time);
                    packet.Write(xgss.cnet.X);
                    packet.Write(xgss.cnet.Y);
                    packet.Send();
                }
                #endregion

            }
            #endregion
            base.AI(npc);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            base.ReceiveExtraAI(npc, bitReader, binaryReader);
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            base.SendExtraAI(npc, bitWriter, binaryWriter);
        }
    }

    public class 被附魔弹幕 : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool 霁云凝胶_是否被附魔 = false;
        //击中NPC时执行
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            #region 霁云凝胶
            bool 霁云凝胶_是否被附魔 = projectile.GetGlobalProjectile<被附魔弹幕>().霁云凝胶_是否被附魔;
            //霁云凝胶  如果弹幕被附魔 并且敌人没有凝胶buff
            if (霁云凝胶_是否被附魔 && target.HasBuff<霁云凝胶_DeBuff>() == false && target.GetGlobalNPC<效果上身>().霁云凝胶_Time ==0)
            {
                target.GetGlobalNPC<效果上身>().霁云凝胶_是否上身 = true;
                target.GetGlobalNPC<效果上身>().cnet = target.Center;
                if(Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write(30000);
                    packet.Write(target.GetGlobalNPC<效果上身>().cnet.X);
                    packet.Write(target.GetGlobalNPC<效果上身>().cnet.Y);
                    packet.Write(target.whoAmI);
                    packet.Send();
                }
                //给它上buff
                target.AddBuff(ModContent.BuffType<霁云凝胶_DeBuff>(), 30);
            }
            #endregion 霁云凝胶
            base.OnHitNPC(projectile, target, hit, damageDone);
        }
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            int num = binaryReader.ReadInt32();
            switch(num)
            {
                case 0:
                    projectile.GetGlobalProjectile<被附魔弹幕>().霁云凝胶_是否被附魔 = binaryReader.ReadBoolean();
                    break;
            }
            base.ReceiveExtraAI(projectile, bitReader, binaryReader);
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            #region 霁云凝胶
            if (projectile.GetGlobalProjectile<被附魔弹幕>().霁云凝胶_是否被附魔)
            {
                binaryWriter.Write(0);
                binaryWriter.Write(projectile.GetGlobalProjectile<被附魔弹幕>().霁云凝胶_是否被附魔);
            }
            #endregion
            else
            {
                binaryWriter.Write(999);
            }
            base.SendExtraAI(projectile, bitWriter, binaryWriter);
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            //source.
            if (source is EntitySource_ItemUse_WithAmmo)
            {
                if ((source as EntitySource_ItemUse_WithAmmo).AmmoItemIdUsed == ModContent.ItemType<霁云凝胶.霁云凝胶>())
                {
                    Main.NewText(Main.netMode);
                    projectile.GetGlobalProjectile<被附魔弹幕>().霁云凝胶_是否被附魔 = true;
                    projectile.netUpdate = true;
                }
            }
            base.OnSpawn(projectile, source);
        }
    }
}
