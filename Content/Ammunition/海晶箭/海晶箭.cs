using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//无合成表
namespace NanTing.Content.Ammunition.海晶箭
{
    public class 海晶箭 : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.damage = 10;
            Item.ammo = AmmoID.Arrow;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<海晶箭_Proje>();
        }
    }
    public class 海晶箭_Proje : ModProjectile
    {
        public override void SetDefaults()
        {
            //Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.damage = 10;
            Projectile.timeLeft = 1000;
            base.SetDefaults();
        }
        int num = 0;
        public override void AI()
        {
            Vector2 playerCenton = Main.player[Projectile.owner].Center - Main.screenPosition;
            Vector2 MouseCenton = Main.MouseWorld - Main.screenPosition;
            if(num == 0)
            {
                Projectile.velocity = Vector2.Normalize(MouseCenton - playerCenton) * 15f;
                num++;
            }
            //在液体中
            if (Projectile.wet)
            {
                if (Projectile.timeLeft % 2 == 0)
                {
                    Projectile.velocity.Y += 0.02f;
                    if (num == 1)
                    {
                        Projectile.velocity *= 1.3f;
                        num++;
                    }
                }
            }
            else
            {
                //每5帧 下坠 0.2f
                if(Projectile.timeLeft % 2 == 0)
                {
                    Projectile.velocity.Y += 0.25f;
                    if(num >= 180)
                    {
                        Projectile.velocity.Y += 0.25f;
                    }
                }
            }
            //弹幕永远存在 除非被破坏
            if(Projectile.timeLeft <= 10)
            {
                Projectile.timeLeft = 1000;
            }
            //速度角 + 180°
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi / 2;
            
            base.AI();
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //在液体(水)中
            if(Projectile.wet)
            {
                damageDone = (int)(damageDone * 1.5f);
            }
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if(Projectile.wet)
            {
                modifiers.FinalDamage *= 1.5f;
            }
            base.ModifyHitNPC(target, ref modifiers);
        }
    }
}
