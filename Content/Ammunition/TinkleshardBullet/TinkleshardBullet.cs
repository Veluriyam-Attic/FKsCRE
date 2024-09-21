using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition.TinkleshardBullet
{
    internal static class ty
    {
        public static int dam = 10;
        public static Projectile MainProje = null;
        public static int PieceDeath = 40;
    }
    public class TinkleshardBullet : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.damage = ty.dam;
            //消耗
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.crit = 5;
            Item.shoot = ModContent.ProjectileType<TinkleshardBullet_Proje>();
            base.SetDefaults();
        }
    }
    public class TinkleshardBullet_Proje : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.damage = ty.dam;
            Projectile.friendly = true;
            Projectile.timeLeft = 45;
            Projectile.aiStyle = ProjAIStyleID.Bubble;
            //Projectile.ai[0] = 0;
        }
        Vector2 towardsMouse = Vector2.Zero;
        Vector2 cs = default;
        int num = 0;
        public override void AI()
        {
            Vector2 player = Main.player[Projectile.owner].Center;
            Vector2 Mouse = Main.MouseWorld;

            if (num == 0)
            {
                cs = Vector2.Normalize(Mouse - player) * 15f;
                //    //鼠标在世界的坐标减去弹幕在世界的坐标 形成新的 v2
                towardsMouse = Main.MouseWorld - Projectile.position;
                num++;
                Projectile.netUpdate = true;
            }
            if (Projectile.wet)
            {
                Projectile.velocity = cs * 3f;
                Projectile.damage = (int)(ty.dam + ty.dam / 2);
            }
            else
            {
                Projectile.velocity = cs;
                Projectile.damage = ty.dam;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi / 2;
            
            base.AI();
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            float twx = reader.ReadSingle();
            float twy = reader.ReadSingle();
            int num = reader.ReadInt32();
            this.num = num;
            towardsMouse = new(twx, twy);
            cs = new Vector2(x, y)*15f;
            base.ReceiveExtraAI(reader);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(cs.X);
            writer.Write(cs.Y);
            writer.Write(towardsMouse.X);
            writer.Write(towardsMouse.Y);
            writer.Write(num);
            base.SendExtraAI(writer);
        }

        public override void OnKill(int timeLeft)
        {
            Player pl = Main.player[Projectile.owner];
            //inventory是背包
            Item item = pl.inventory[pl.selectedItem];

            // 计算从子弹到鼠标的向量  
            float distanceToMouse = towardsMouse.Length();
            if (distanceToMouse > 0)
            {
                towardsMouse.Normalize(); // 标准化向量  
                // 分散的角度增量（以弧度为单位）  
                float angleIncrement = MathHelper.ToRadians(30f); // 80度分散成4个，每个间隔20度  

                // 遍历并创建新子弹  
                for (int i = 0; i < 4; i++)
                {
                    int num3 = Main.rand.Next(5);
                    // 计算新子弹的旋转角度（基于原始方向加上随机偏移）  
                    //Atan2 计算角度（鼠标与弹幕之间的）
                    float angle = (float)Math.Atan2(towardsMouse.Y, towardsMouse.X) + angleIncrement * i + (float)Main.rand.NextDouble() * MathHelper.ToRadians(30) - MathHelper.ToRadians(60); 
                    // 计算新子弹的速度向量  
                    Vector2 newVelocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 12f;
                    Projectile proje = null;
                    switch(num3)
                    {
                        case 0:
                            proje = Projectile.NewProjectileDirect(default, Projectile.position, newVelocity, ModContent.ProjectileType<TinkleshardBullet_Piece1>(), (item.damage + ty.dam / 3) / 3, 0f, Projectile.owner);
                            break;
                        case 1:
                            proje =  Projectile.NewProjectileDirect(default, Projectile.position, newVelocity, ModContent.ProjectileType<TinkleshardBullet_Piece2>(), (item.damage + ty.dam / 3) / 3, 0f, Projectile.owner);
                            break;
                        case 2:
                            proje = Projectile.NewProjectileDirect(default, Projectile.position, newVelocity, ModContent.ProjectileType<TinkleshardBullet_Piece3>(), (item.damage + ty.dam / 3) / 3, 0f, Projectile.owner);
                            break;
                        case 3:
                            proje = Projectile.NewProjectileDirect(default, Projectile.position, newVelocity, ModContent.ProjectileType<TinkleshardBullet_Piece4>(), (item.damage + ty.dam / 3) / 3, 0f, Projectile.owner);
                            break;
                    }
                }
            }
                base.OnKill(timeLeft);
        }
    }
    public class TinkleshardBullet_Piece1 : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.damage = ty.dam / 3;
            Projectile.friendly = true;
            //Projectile.aiStyle = 1;
            Projectile.timeLeft = ty.PieceDeath;
        }
        public override void AI()
        {
            //Vector2 v = new Vector2((float)Math.Cos(Main.rand.NextDouble()), ty.MainProje.Center.Y);
            //Projectile.velocity = Vector2.Normalize(v - ty.MainProje.Center);
            Projectile.rotation += 2f;
            base.AI();
        }
    }
    public class TinkleshardBullet_Piece2 : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.damage = ty.dam / 3;
            Projectile.friendly = true;
            //Projectile.aiStyle = 1;
            Projectile.timeLeft = ty.PieceDeath;
            base.SetDefaults();
        }
        public override void AI()
        {
            Projectile.rotation *= 2f;
            base.AI();
        }
    }
    public class TinkleshardBullet_Piece3 : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.damage = ty.dam / 3;
            Projectile.friendly = true;
            //Projectile.aiStyle = 1;
            Projectile.timeLeft = ty.PieceDeath;
            base.SetDefaults();
        }
        public override void AI()
        {
            Projectile.rotation *= 2f;
            base.AI();
        }
    }
    public class TinkleshardBullet_Piece4 : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.damage = ty.dam / 3;
            Projectile.friendly = true;
            //Projectile.aiStyle = 1;
            Projectile.timeLeft = ty.PieceDeath;
            base.SetDefaults();
        }
        public override void AI()
        {
            Projectile.rotation *= 2f;
            base.AI();
        }
    }
}
