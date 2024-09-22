//using CalamityMod.Tiles.Furniture;
using Microsoft.Xna.Framework;
using System;
using System.Drawing.Text;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
//无合成表
namespace FKsCRE.Content.Ammunition.WulfrimArrow
{
    static class ty
    {
        public static int dam = 10;
    }
    #region 僵硬
    public class Hold : GlobalNPC
    {
        int time = 0;
        Vector2 cent;
        public void setcent(Vector2 e) { cent = e; }
        public int gettime() { return time; }
        public void settime() { time++; }
        public Vector2 getcent() { return cent; }
        public void timeToZero() { time = 0; }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.ModProjectile != null)
            {
                if(projectile.ModProjectile.Name.Equals("WulfrimArrow_proje"))
                {
                    cent = npc.Center;
                    npc.netUpdate = true;
                    ModPacket packet = Mod.GetPacket();
                    packet.Write(1);
                    packet.Write(cent.X);
                    packet.Write(cent.Y);
                    //npc在Main.npc中的索引
                    packet.Write(npc.whoAmI);
                    //packet.Write(Main.myPlayer);
                    packet.Send();
                    //MessageID.SyncNPC
                }
            }
            //if ((ModContent.GetModProjectile(ModContent.ProjectileType<WulfrimArrow_proje>()) == projectile.ModProjectile))
            //{
            //}
            base.OnHitByProjectile(npc, projectile, hit, damageDone);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            #region 第一次
            //int a = binaryReader.ReadInt32();
            //cent = new (binaryReader.ReadSingle(),binaryReader.ReadSingle());
            //time = binaryReader.ReadInt32();
            #endregion
            #region 第二次
            /*
            Hold hd = npc.GetGlobalNPC<Hold>();
            hd.setcent(new(binaryReader.ReadSingle(), binaryReader.ReadSingle()));
            hd.time = binaryReader.ReadInt32();
            Console.WriteLine(hd.cent);
            */
            #endregion
            #region 第三次
            /*
            if(binaryReader.ReadString().Equals("WulfrimArrowHold_NPC"))
            {
                Console.WriteLine("收到了!");
                cent = new Vector2(binaryReader.ReadSingle(),binaryReader.ReadSingle());
                time = binaryReader.ReadInt32();
            }
            */
            #endregion
            #region
            cent.X = binaryReader.ReadSingle();
            cent.Y = binaryReader.ReadSingle();
            time = binaryReader.ReadInt32();
            #endregion
            base.ReceiveExtraAI(npc, bitReader, binaryReader);
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            //ModPacket packet = Mod.GetPacket();
            //packet.Write(1);
            //packet.Send();
            //Console.WriteLine(Main.netMode);
            //Hold e = npc.GetGlobalNPC<Hold>();
            //binaryWriter.Write(MessageID.SyncNPC);
            binaryWriter.Write(cent.X);
            binaryWriter.Write(cent.Y);
            binaryWriter.Write(time);
            //binaryWriter.Write(e.time);
            base.SendExtraAI(npc, bitWriter, binaryWriter);
        }
        public override bool InstancePerEntity => true;
    }
    #endregion

    #region 钨钢箭
    public class WulfrimArrow : 子弹
    {
        public override void SetDefaults()
        {
            Item.ammo = AmmoID.Arrow;
            Item.consumable = true;
            Item.damage = ty.dam;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.shoot = ModContent.ProjectileType<WulfrimArrow_proje>();
        }
    }
    #endregion

    #region 钨钢箭弹幕
    public class WulfrimArrow_proje : ModProjectile
    {
        Vector2 Mouse_initial = default;
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Mouse_initial = new(reader.ReadSingle(),reader.ReadSingle());
            num = reader.ReadInt32();
            base.ReceiveExtraAI(reader);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Mouse_initial.X);
            writer.Write(Mouse_initial.Y);
            writer.Write(num);
            base.SendExtraAI(writer);
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.damage = ty.dam;
            Projectile.timeLeft = 999;
            //碰撞箱修复
            DrawOffsetX = -12;
            DrawOriginOffsetY = -45;
            //Projectile.ai[0] = 0;
        }
        int num = 0;
        public override void AI()
        {
            if (num == 0)
            {
                Mouse_initial = Main.MouseWorld;
                Projectile.velocity = Vector2.Normalize(Mouse_initial - Main.player[Projectile.owner].Center) * 12;
            }
            //下坠
            //Projectile.velocity.Y += (num >= 180) ? Projectile.velocity.Y += 3f : Projectile.velocity.Y += 0.05f;
            if (num >= 180) Projectile.velocity.Y += 0.25f;
            if (num < 180) Projectile.velocity.Y += 0.15f;
            //角度
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi / 2;

            num++;
            Projectile.netUpdate = true;
            base.AI();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //5秒
            target.AddBuff(ModContent.BuffType<WulfrimShock>(), 300);
            //30秒
            //int[] ints = target.buffType;
            //如果有
            if (target.HasBuff<WulfrimHold>())
            {
            }
            else
            {
                Hold hold = target.GetGlobalNPC<Hold>();
                hold.setcent(target.Center);
                Main.NewText("OnHitNPC : "  + hold.getcent());
                target.AddBuff(ModContent.BuffType<WulfrimHold>(), 600);
                target.netUpdate = true;
            }
            //Hold e = target.GetGlobalNPC<Hold>();
            //e.setcent(target.Center);
            ////请求同步
            //ModPacket packet = Mod.GetPacket();
            //packet.Write(1);
            //packet.Write(target.whoAmI);
            //packet.Send();
            base.OnHitNPC(target, hit, damageDone);
        }
    }
    #endregion

    #region 钨钢定身
    public class WulfrimHold : ModBuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            Hold modnpc = npc.GetGlobalNPC<Hold>();
            if (modnpc.gettime() <= 60)
            {
                npc.Center = modnpc.getcent();
            }
            if (modnpc.gettime() >= 600) modnpc.timeToZero();
            modnpc.settime();
            base.Update(npc, ref buffIndex);
        }
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            //不能被护士消除
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            //不显示时间
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = false;//默认也是false 死亡清除
            Main.vanityPet[Type] = false;//宠物?
            base.SetStaticDefaults();
        }

    }
    #endregion

    #region 钨钢箭电减
    public class WulfrimShock : ModBuff
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
            base.SetStaticDefaults();
        }
        int num = 0;
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (num == 0)
            {
                npc.defense -= 3;
            }
            if (num % 60 == 0)
            {
                //扣血 2点
                Rectangle rectangle = new Rectangle((int)npc.Center.X, (int)npc.Center.Y, 10, 10);
                npc.life -= 2;
                CombatText.NewText(rectangle /*npc.getRect()*/, Color.Aqua, "2");

            }
            Dust du = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.UltraBrightTorch, 2F, 2F);
            Dust du2 = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.UltraBrightTorch, -2F, 2F);
            du.noGravity = true;
            du2.noGravity = true;
            num++;
            base.Update(npc, ref buffIndex);
        }
    }
    #endregion
}