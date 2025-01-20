//using FKsCRE.Content.凝胶.WulfrimGels;
//using FKsCRE.Content.凝胶.寒元凝胶;
//using FKsCRE.Content.凝胶.霁云凝胶;
//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.DataStructures;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria.ModLoader.IO;

//namespace FKsCRE.Content.凝胶
//{
//    #region 基础继承
//    public abstract class 凝胶_DeBuff : ModBuff
//    {
//        public override void SetStaticDefaults()
//        {
//            Main.debuff[Type] = true;
//            //不能被护士消除
//            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
//            //不显示时间
//            Main.buffNoTimeDisplay[Type] = true;
//            Main.persistentBuff[Type] = false;//默认也是false 死亡清除
//            Main.vanityPet[Type] = false;//宠物?
//        }
//    }
//    public abstract class 凝胶 : ModItem
//    {
//        public override void SetDefaults()
//        {
//            //Item.damage = 10;
//            Item.consumable = true;
//            Item.ammo = AmmoID.Gel;
//            Item.maxStack = 9999;
//            base.SetDefaults();
//        }
//    }
//    #endregion

//    public class 玩家得益 : ModPlayer
//    {
//        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
//        {
//            if (target.HasBuff<寒元凝胶_DeBuff>()) Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<寒元凝胶_Buff>(), 180);
//            base.OnHitNPC(target, hit, damageDone);
//        }
//    }
//    public class 效果上身 : GlobalNPC
//    {
//        //NPC运行在服务端
//        public override bool InstancePerEntity => true;
//        #region 霁元凝胶
//        public bool 霁云凝胶_是否上身 = false;
//        public int 霁云凝胶_Time = 0;
//        //cent = 霁云凝胶
//        public Vector2 cnet = default;
//        #endregion

//        #region 寒元凝胶
//        bool 寒元凝胶 = false;
//        int 寒元凝胶_time = 0;
//        public int 寒元冲刺 = 0;
//        public int 寒元冲刺次数记录 = 0;
//        public Vector2 寒元位置记录 = default;
//        public Vector2 寒元NPC速度记录 = default;
//        public Double ange;
//        public int Get寒元凝胶Time(){ return 寒元凝胶_time; }
//        public void Set寒元凝胶Time(int e) { 寒元凝胶_time = e; }
//        public bool Get寒元凝胶() { return 寒元凝胶; }
//        public void Set寒元凝胶(bool l) { 寒元凝胶 = l; }
//        #endregion
//        public override void AI(NPC npc)
//        {
//            #region 霁云凝胶
//            if (霁云凝胶_是否上身 && !npc.boss)
//            {
//                霁云凝胶_Time++;
//                npc.GetGlobalNPC<效果上身>().cnet.X = npc.Center.X; 
//                if (霁云凝胶_Time == 120) 
//                { 
//                    霁云凝胶_Time = 0; 
//                    霁云凝胶_是否上身 = false;
//                    cnet = default;
//                }
//                #region 联机同步 2->霁云凝胶
//                if (Main.netMode == NetmodeID.MultiplayerClient)
//                {
//                    ModPacket packet = Mod.GetPacket();
//                    效果上身 xgss = npc.GetGlobalNPC<效果上身>();
//                    packet.Write(2);
//                    //NPC在Main.npc中的索引
//                    packet.Write(npc.whoAmI);
//                    packet.Write(xgss.霁云凝胶_是否上身);
//                    packet.Write(xgss.霁云凝胶_Time);
//                    packet.Write(xgss.cnet.X);
//                    packet.Write(xgss.cnet.Y);
//                    packet.Send();
//                }
//                #endregion
//            }
//            #endregion
//            #region 寒元凝胶
//            for (int i = 0; i < 1; i++)
//            {
//                if (npc.GetGlobalNPC<效果上身>().Get寒元凝胶())
//                {
//                    npc.GetGlobalNPC<效果上身>().Set寒元凝胶Time(npc.GetGlobalNPC<效果上身>().Get寒元凝胶Time() + 1);
//                    if (npc.GetGlobalNPC<效果上身>().Get寒元凝胶Time() >= 120)
//                    {
//                        npc.GetGlobalNPC<效果上身>().Set寒元凝胶(false);
//                        npc.GetGlobalNPC<效果上身>().Set寒元凝胶Time(0);
//                        npc.GetGlobalNPC<效果上身>().寒元冲刺 = 0;
//                        npc.GetGlobalNPC<效果上身>().寒元冲刺次数记录 = 0;
//                        npc.GetGlobalNPC<效果上身>().寒元NPC速度记录 = default;
//                        npc.GetGlobalNPC<效果上身>().寒元位置记录 = default;
//                        npc.netUpdate = true;
//                        if(Main.netMode == NetmodeID.MultiplayerClient)
//                        {
//                            ModPacket packet = Mod.GetPacket();
//                            //30001
//                            packet.Write(30001);
//                            packet.Write(npc.whoAmI);
//                            packet.Send();
//                        }
//                        break;

//                    }
//                    if (npc.GetGlobalNPC<效果上身>().寒元冲刺 == 0)
//                    {
//                        if (npc.GetGlobalNPC<效果上身>().寒元冲刺次数记录 == 0)
//                        {
//                            npc.GetGlobalNPC<效果上身>().寒元位置记录 = npc.Center;
//                            npc.GetGlobalNPC<效果上身>().寒元NPC速度记录 = npc.velocity;
//                            npc.GetGlobalNPC<效果上身>().寒元冲刺次数记录++;

//                            //生成一个随机角度
//                            ange = Main.rand.NextDouble() * 2 * Math.PI;
//                            #region 寒元联机同步 部分-001
//                            if (Main.netMode == NetmodeID.MultiplayerClient)
//                            {
//                                ModPacket packet = Mod.GetPacket();
//                                //30002
//                                packet.Write(30002);
//                                packet.Write(npc.whoAmI);
//                                //寒元位置记录
//                                packet.Write(npc.GetGlobalNPC<效果上身>().寒元位置记录.X);
//                                packet.Write(npc.GetGlobalNPC<效果上身>().寒元位置记录.Y);
//                                //寒元NPC速度记录
//                                packet.Write(npc.GetGlobalNPC<效果上身>().寒元NPC速度记录.X);
//                                packet.Write(npc.GetGlobalNPC<效果上身>().寒元NPC速度记录.Y);
//                                //寒元冲刺次数记录
//                                packet.Write(npc.GetGlobalNPC<效果上身>().寒元冲刺次数记录);
//                                packet.Send();
//                            }
//                            #endregion
//                            float x = (float)Math.Cos(ange);
//                            float y = (float)Math.Sin(ange);
//                            npc.velocity = Vector2.Normalize(new Vector2(x, y)) * 20f;
//                        }
//                        if (Vector2.Distance(npc.GetGlobalNPC<效果上身>().寒元位置记录, npc.Center) > 100)
//                        {
//                            寒元冲刺++;
//                            npc.velocity = Vector2.Zero;
//                            if (Main.netMode == NetmodeID.MultiplayerClient)
//                            {
//                                ModPacket packet = Mod.GetPacket();
//                                packet.Write(30003);
//                                packet.Write(npc.whoAmI);
//                                packet.Send();
//                            }
//                            //npc.velocity = npc.GetGlobalNPC<效果上身>().寒元NPC速度记录;
//                            //npc.velocity = npc.GetGlobalNPC<效果上身>().寒元NPC速度记录;
//                        }
//                    }
//                }
//            }
//            #endregion
//            base.AI(npc);
//        }
//        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
//        {
//            if(npc.HasBuff<霁云凝胶_DeBuff>())
//            {
//                modifiers.FinalDamage *= 0.95f;
//            }
//            base.ModifyHitPlayer(npc, target, ref modifiers);
//        }

//        public override void HitEffect(NPC npc, NPC.HitInfo hit)
//        {
//            base.HitEffect(npc, hit);
//        }
//    }

//    public class 被附魔弹幕 : GlobalProjectile
//    {
//        public override bool InstancePerEntity => true;
//        public bool 霁云凝胶_是否被附魔 = false;
//        public bool 寒元凝胶_是否被附魔 = false;
//        public bool WulfrimGel = false;//钨钢凝胶
//        //击中NPC时执行
//        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
//        {
//            #region 霁云凝胶
//            bool 霁云凝胶_是否被附魔 = projectile.GetGlobalProjectile<被附魔弹幕>().霁云凝胶_是否被附魔;
//            //霁云凝胶  如果弹幕被附魔 并且敌人没有凝胶buff
//            if (霁云凝胶_是否被附魔 && target.HasBuff<霁云凝胶_DeBuff>() == false && target.GetGlobalNPC<效果上身>().霁云凝胶_Time ==0 && !target.boss)
//            {
//                target.GetGlobalNPC<效果上身>().霁云凝胶_是否上身 = true;
//                target.GetGlobalNPC<效果上身>().cnet = target.Center;
//                if(Main.netMode == NetmodeID.MultiplayerClient)
//                {
//                    ModPacket packet = Mod.GetPacket();
//                    packet.Write(30000);
//                    packet.Write(target.GetGlobalNPC<效果上身>().cnet.X);
//                    packet.Write(target.GetGlobalNPC<效果上身>().cnet.Y);
//                    packet.Write(target.whoAmI);
//                    packet.Send();
//                }
//                //给它上buff
//                target.AddBuff(ModContent.BuffType<霁云凝胶_DeBuff>(), 30);
//            }
//            if(霁云凝胶_是否被附魔 && target.HasBuff<霁云凝胶_DeBuff>() == false)
//            {
//                target.AddBuff(ModContent.BuffType<霁云凝胶_DeBuff>(),300);
//                if(Main.netMode == NetmodeID.MultiplayerClient)
//                {
//                    ModPacket packet = Mod.GetPacket();
//                    packet.Write(30004);
//                    packet.Write(target.whoAmI);
//                    packet.Send();
//                }
//            }
//            #endregion 霁云凝胶
//            #region 寒元凝胶
//            bool 寒元凝胶 = projectile.GetGlobalProjectile<被附魔弹幕>().寒元凝胶_是否被附魔;
//            if(寒元凝胶 && target.active && !target.friendly && !target.HasBuff<寒元凝胶_DeBuff>() && target.boss)
//            {
//                target.AddBuff(ModContent.BuffType<寒元凝胶_DeBuff>(),600);
//            }
//            if(寒元凝胶 && target.active && !target.friendly && !target.boss && !target.GetGlobalNPC<效果上身>().Get寒元凝胶())
//            {
//                target.GetGlobalNPC<效果上身>().Set寒元凝胶(true);
//                if(Main.netMode == NetmodeID.MultiplayerClient)
//                {
//                    ModPacket packet = Mod.GetPacket();
//                    packet.Write(3);
//                    packet.Write(true);
//                    packet.Write(target.whoAmI);
//                    packet.Send();
//                }
//            }
//            #endregion
//            #region 钨钢凝胶
//            if(projectile.GetGlobalProjectile<被附魔弹幕>().WulfrimGel)
//            {
//                target.AddBuff(ModContent.BuffType<WulfrimGelBuff>(),300);
//            }
//            #endregion
//            base.OnHitNPC(projectile, target, hit, damageDone);
//        }
//        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
//        {
//            int num = binaryReader.ReadInt32();
//            switch(num)
//            {
//                case 0:
//                    projectile.GetGlobalProjectile<被附魔弹幕>().霁云凝胶_是否被附魔 = binaryReader.ReadBoolean();
//                    break;
//                case 1:
//                    projectile.GetGlobalProjectile<被附魔弹幕>().寒元凝胶_是否被附魔 = binaryReader.ReadBoolean();
//                    break;
//                case 2:
//                    projectile.GetGlobalProjectile<被附魔弹幕>().WulfrimGel = binaryReader.ReadBoolean();
//                    break;
//            }
//            base.ReceiveExtraAI(projectile, bitReader, binaryReader);
//        }
//        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
//        {
//            #region 霁云凝胶
//            if (projectile.GetGlobalProjectile<被附魔弹幕>().霁云凝胶_是否被附魔)
//            {
//                binaryWriter.Write(0);
//                binaryWriter.Write(projectile.GetGlobalProjectile<被附魔弹幕>().霁云凝胶_是否被附魔);
//            }
//            #endregion

//            #region 寒元凝胶
//            else if(projectile.GetGlobalProjectile<被附魔弹幕>().寒元凝胶_是否被附魔)
//            {
//                binaryWriter.Write(1);
//                binaryWriter.Write(projectile.GetGlobalProjectile<被附魔弹幕>().寒元凝胶_是否被附魔);
//            }
//            #endregion

//            #region 钨钢凝胶
//            else if(projectile.GetGlobalProjectile<被附魔弹幕>().WulfrimGel)
//            {
//                binaryWriter.Write(2);
//                binaryWriter.Write(projectile.GetGlobalProjectile<被附魔弹幕>().WulfrimGel);
//            }
//            #endregion
//            else
//            {
//                binaryWriter.Write(999);
//            }
//            base.SendExtraAI(projectile, bitWriter, binaryWriter);
//        }

//        public override void OnSpawn(Projectile projectile, IEntitySource source)
//        {
//            //source.
//            if (source is EntitySource_ItemUse_WithAmmo)
//            {
//                #region 霁云凝胶
//                if ((source as EntitySource_ItemUse_WithAmmo).AmmoItemIdUsed == ModContent.ItemType<霁云凝胶.霁云凝胶>())
//                {
//                    //Main.NewText(Main.netMode);
//                    projectile.GetGlobalProjectile<被附魔弹幕>().霁云凝胶_是否被附魔 = true;
//                    projectile.netUpdate = true;
//                }
//                #endregion

//                //#region 寒元凝胶
//                //if((source as EntitySource_ItemUse_WithAmmo).AmmoItemIdUsed == ModContent.ItemType<寒元凝胶.寒元凝胶>())
//                //{
//                //    projectile.GetGlobalProjectile<被附魔弹幕>().寒元凝胶_是否被附魔 = true;
//                //    projectile.netUpdate = true;
//                //}
//                //#endregion

//                //#region 钨钢凝胶 WulfrimGel
//                //if((source as EntitySource_ItemUse_WithAmmo).AmmoItemIdUsed == ModContent.ItemType<WulfrimGels.WulfrimGel>())
//                //{
//                //    projectile.GetGlobalProjectile<被附魔弹幕>().WulfrimGel = true;
//                //    projectile.netUpdate = true;
//                //}
//                //#endregion
//            }
//            base.OnSpawn(projectile, source);
//        }
//    }
//}
