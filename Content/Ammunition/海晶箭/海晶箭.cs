using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//无合成表
namespace NanTing.Content.Ammunition.海晶箭
{
    class ty
    {
        public static int dam = 10;
    }
    public class 海晶箭 : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.damage = ty.dam;
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
            Projectile.damage = ty.dam;
            Projectile.timeLeft = 1000;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            //碰撞箱
            DrawOffsetX = -8;
            DrawOriginOffsetY = -40;
            base.SetDefaults();
        }
        Vector2 vector = default;
        int num = 0;
        public override void AI()
        {
            Vector2 playerCenton = Main.player[Projectile.owner].Center - Main.screenPosition;
            Vector2 MouseCenton = Main.MouseWorld - Main.screenPosition;
            NanTingGProje projectile =  Projectile.GetGlobalProjectile<NanTingGProje>();
            //速度确定 只进行一次
            if (projectile.GetItem().Name.Equals("Daedalus Stormbow"))
            {
                if (num == 0) vector = Projectile.velocity;
                if (!Projectile.wet) { Projectile.damage = ty.dam + projectile.GetItem().damage; Projectile.velocity = vector; }
                if (Projectile.wet) {Projectile.damage = (int)(ty.dam * 1.5f + projectile.GetItem().damage); Projectile.velocity = vector * 2.5f; }
            }
            else
            {
                if (num == 0)
                {
                    vector = Vector2.Normalize(MouseCenton - playerCenton) * 15f;
                }
                //在液体中
                if (Projectile.wet) Projectile.damage = (int)(ty.dam * 1.5f + projectile.GetItem().damage);
                if (Projectile.wet && Projectile.timeLeft % 2 == 0) vector.Y += 0.02f;
                if (Projectile.wet) Projectile.velocity = vector * 2.5f;
                if (!Projectile.wet) Projectile.damage = ty.dam + projectile.GetItem().damage;
                if (!Projectile.wet) Projectile.velocity = vector;
                if (!Projectile.wet) vector.Y += 0.25f;
            }
            //if (Projectile.wet)
            //{
            //    Projectile.damage = (int)(ty.dam * 1.5);
            //    if (Projectile.timeLeft % 2 == 0)
            //    {
            //        Projectile.velocity.Y += 0.02f;
            //        if (num == 1)
            //        {
            //            Projectile.velocity = vector * 2.5f;
            //            num++;
            //        }
            //    }
            //}
            //else
            //{
            //    Projectile.damage = ty.dam;
            //    Projectile.velocity = vector;
            //    //每5帧 下坠 0.2f
            //    if(Projectile.timeLeft % 2 == 0)
            //    {
            //        Projectile.velocity.Y += 0.25f;
            //        if(num >= 180)
            //        {
            //            Projectile.velocity.Y += 0.45f;
            //        }
            //    }
            //}
            //弹幕永远存在 除非被破坏
            if(Projectile.timeLeft <= 10)
            {
                Projectile.timeLeft = 1000;
            }
            //速度角 + 180°
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi / 2;
            num++;
            base.AI();
        }
        public int Getproj()
        {
            return (int)Projectile.ai[2];
        }
        public void setProj(int NUM)
        {
            Projectile.ai[2] = NUM;
        }

        //public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        //{
        //    //在液体(水)中
        //    if(Projectile.wet)
        //    {
        //        damageDone += 5;
        //    }
        //    base.OnHitNPC(target, hit, damageDone);
        //}
        //public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        //{
        //    if(Projectile.wet)
        //    {
        //        modifiers.FinalDamage += 5;
        //    }
        //    base.ModifyHitNPC(target, ref modifiers);
        //}
    }
}
