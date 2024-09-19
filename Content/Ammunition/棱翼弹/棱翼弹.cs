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
        public override void AI()
        {
            Vector2 player = Main.player[Projectile.owner].Center;
            Vector2 Mouse = Main.MouseWorld;
            if (num == 0)
            {
                Projectile.velocity = Vector2.Normalize(Mouse - player) * 10f;
                num++;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi / 2;
            base.AI();
        }
        public override void OnKill(int timeLeft)
        {
            Player pl = Main.player[Projectile.owner];
            int NUM2 = pl.handon;
            //inventory是背包
            Item item = pl.inventory[pl.selectedItem];
            Vector2 v2 = Main.rand.NextVector2Square(1, 10);
            v2.X = v2.X + Projectile.Center.X;
            v2.Y = v2.Y + Projectile.Center.Y;
            Vector2 No = Vector2.Normalize(v2 - Projectile.Center);

            ty.主弹幕 = Projectile;
            Projectile proje = Projectile.NewProjectileDirect(default, Projectile.Center,  default/*No * 5f*/, ModContent.ProjectileType<棱翼弹_霰弹碎片1>(),(item.damage + ty.dam/3) / 3, 1, Projectile.owner);
            proje.velocity.RotatedBy(Projectile.velocity.ToRotation());  
            base.OnKill(timeLeft);
        }
    }
    public class 棱翼弹_霰弹碎片1 : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.damage = ty.dam / 3;
            Projectile.friendly = true;
            //Projectile.aiStyle = 1;
            Projectile.timeLeft = 20;
        }
        public override void AI()
        {
            Vector2 v = new Vector2((float)Math.Cos(Main.rand.NextDouble()), ty.主弹幕.Center.Y);
            Projectile.velocity = Vector2.Normalize(v - ty.主弹幕.Center);
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
