using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NanTing.Content.Ammunition.棱翼弹
{
    internal static class ty
    {
        public static int dam = 10;
        public static Projectile 主弹幕 = null;
    }
    public class 棱翼弹 : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.damage = ty.dam;
            //消耗
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.crit = 5;
            Item.shoot = ModContent.ProjectileType<棱翼弹_弹幕>();
            base.SetDefaults();
        }
    }
    public class 棱翼弹_弹幕 : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.damage = ty.dam;
            Projectile.friendly = true;
            Projectile.timeLeft = 15;
        }
        int num = 0;
        Vector2 towardsMouse = Vector2.Zero;
        public override void AI()
        {
            Vector2 player = Main.player[Projectile.owner].Center;
            Vector2 Mouse = Main.MouseWorld;
            if (num == 0)
            {
                Projectile.velocity = Vector2.Normalize(Mouse - player) * 15f;
                towardsMouse = Main.MouseWorld - Projectile.position;
                num++;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi / 2;
            base.AI();
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
                float angleIncrement = MathHelper.ToRadians(20f); // 80度分散成4个，每个间隔20度  

                // 遍历并创建新子弹  
                for (int i = 0; i < 4; i++)
                {
                    int num3 = Main.rand.Next(5);
                    // 计算新子弹的旋转角度（基于原始方向加上随机偏移）  
                    float angle = (float)Math.Atan2(towardsMouse.Y, towardsMouse.X) + angleIncrement * i + (float)Main.rand.NextDouble() * MathHelper.ToRadians(30) - MathHelper.ToRadians(30); 
                    // 计算新子弹的速度向量  
                    Vector2 newVelocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 10f;
                    switch(num3)
                    {
                        case 0:
                            Projectile.NewProjectile(default, Projectile.position, newVelocity, ModContent.ProjectileType<棱翼弹_霰弹碎片1>(), (item.damage + ty.dam / 3) / 3, 0f, Projectile.owner);
                            break;
                        case 1:
                            Projectile.NewProjectile(default, Projectile.position, newVelocity, ModContent.ProjectileType<棱翼弹_霰弹碎片2>(), (item.damage + ty.dam / 3) / 3, 0f, Projectile.owner);
                            break;
                        case 2:
                            Projectile.NewProjectile(default, Projectile.position, newVelocity, ModContent.ProjectileType<棱翼弹_霰弹碎片3>(), (item.damage + ty.dam / 3) / 3, 0f, Projectile.owner);
                            break;
                        case 3:
                            Projectile.NewProjectile(default, Projectile.position, newVelocity, ModContent.ProjectileType<棱翼弹_霰弹碎片4>(), (item.damage + ty.dam / 3) / 3, 0f, Projectile.owner);
                            break;
                    }
                }
            }
                base.OnKill(timeLeft);
        }
    }
    public class 棱翼弹_霰弹碎片1 : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.damage = ty.dam / 3;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 20;
        }
        public override void AI()
        {
            //Vector2 v = new Vector2((float)Math.Cos(Main.rand.NextDouble()), ty.主弹幕.Center.Y);
            //Projectile.velocity = Vector2.Normalize(v - ty.主弹幕.Center);
            base.AI();
        }
    }
    public class 棱翼弹_霰弹碎片2 : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.damage = ty.dam / 3;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 20;
            base.SetDefaults();
        }
    }
    public class 棱翼弹_霰弹碎片3 : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.damage = ty.dam / 3;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 20;
            base.SetDefaults();
        }
    }
    public class 棱翼弹_霰弹碎片4 : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.damage = ty.dam / 3;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 20;
            base.SetDefaults();
        }
    }
}
