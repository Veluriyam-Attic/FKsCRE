//using CalamityMod.Particles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria.ID;
//using Terraria;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;

//namespace FKsCRE.Content.Arrows.CPreMoodLord.LifeAlloyArrow
//{
//    public class LifeAlloyArrowPROJSPLIT : ModProjectile
//    {
//        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.None; // 使用内置的空白贴图，这可以让他在没有同名文件贴图的情况下也能工作
//        private Color currentColor = Color.Black; // 添加颜色变量的定义
//        public override void SetDefaults()
//        {
//            Projectile.width = 12;
//            Projectile.height = 12;
//            Projectile.aiStyle = ProjAIStyleID.Arrow;
//            Projectile.friendly = true;
//            Projectile.tileCollide = false;
//            Projectile.penetrate = 2;
//            Projectile.timeLeft = 500;
//            Projectile.extraUpdates = 4;
//            Projectile.alpha = 255;
//            Projectile.ignoreWater = true;
//            AIType = ProjectileID.Bullet;
//        }

//        public override void AI()
//        {
//            if (currentColor == Color.Black) // 检查是否为默认颜色
//            {
//                currentColor = Main.rand.NextBool() ? Color.Red : Color.Blue; // 随机选择颜色
//                Projectile.velocity *= 0.8f; // 减缓速度
//            }

//            // 添加随机旋转效果
//            Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.05f, 0.05f));

//            // 生成粒子特效
//            GlowOrbParticle orb = new GlowOrbParticle(Projectile.Center, Projectile.velocity * 0.1f, false, 5, 0.4f, currentColor, true, true);
//            GeneralParticleHandler.SpawnParticle(orb);
//        }

//        public override bool PreDraw(ref Color lightColor)
//        {
//            return false; // 不绘制弹幕，使用粒子替代
//        }


//    }
//}
