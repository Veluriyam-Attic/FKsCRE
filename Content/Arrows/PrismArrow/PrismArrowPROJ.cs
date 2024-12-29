//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using FKsCRE.Content.Ammunition.TinkleshardBullet;

//namespace FKsCRE.Content.Arrows.PrismArrow
//{
//    internal class PrismArrowPROJ : ModProjectile
//    {
//        public override void SetStaticDefaults()
//        {
//            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
//            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
//        }

//        public override void SetDefaults()
//        {
//            Projectile.width = 10;
//            Projectile.height = 20;
//            Projectile.friendly = true;
//            Projectile.DamageType = DamageClass.Ranged;
//            Projectile.arrow = true;  // 继承箭矢的 AI
//            Projectile.aiStyle = ProjAIStyleID.Arrow;
//            Projectile.penetrate = 1; // 只穿透一次敌人
//            Projectile.timeLeft = 1800; // 弹幕的持续时间

//            //// 如果弹幕在水中，增强其初始属性
//            //if (Main.player[Projectile.owner].wet)
//            //{
//            //    Projectile.velocity *= 1.5f; // 初始速度增加1.5倍
//            //    Projectile.extraUpdates += 2; // 弹幕的额外更新次数+2
//            //    Projectile.damage = (int)(Projectile.damage * 1.2f); // 增加20%的伤害
//            //}
//        }

//        public override void AI()
//        {
//            // 让 y 轴速度逐渐增加
//            Projectile.velocity.Y += 0.05f;
//            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
//            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.5f); // 添加光效

//            // 在水中时增强弹幕的速度和伤害
//            if (Projectile.wet)
//            {
//                Projectile.velocity = Projectile.velocity * 2.5f; // 速度加快至原来的2.5倍
//                Projectile.damage = (int)(ty.dam * 1.5f); // 将伤害提升1.5倍
//                Projectile.velocity.Y = 0; // 取消重力影响，保持y轴不再受重力影响
//            }
//        }


//        public override void OnKill(int timeLeft)
//        {
//            // 释放天蓝色的水系粒子特效
//            for (int i = 0; i < 10; i++)
//            {
//                // 随机生成粒子发射的速度和方向，沿着箭矢的朝向发射
//                Vector2 particleVelocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(0.5f, 1.5f);

//                // 创建天蓝色粒子，并调整缩放和亮度
//                Dust waterDust = Dust.NewDustPerfect(Projectile.Center, DustID.Water, particleVelocity, 0, Color.LightSkyBlue, 2f); // 设置颜色为天蓝色，缩放为2倍
//                waterDust.noGravity = true; // 无重力效果
//                waterDust.fadeIn = 1.5f; // 增加粒子的亮度和淡入效果
//            }
//        }

//        public override bool PreDraw(ref Color lightColor)
//        {
//            // 如果投射物不在水中，直接返回不绘制拖尾效果
//            if (!Projectile.wet)
//            {
//                return true; // 继续绘制默认的弹幕
//            }

//            // 获取 SpriteBatch 和投射物纹理
//            SpriteBatch spriteBatch = Main.spriteBatch;
//            Texture2D lightTexture = ModContent.Request<Texture2D>("FKsCRE/Content/Arrows/PrismArrow/PrismArrowPROJ").Value;

//            // 遍历投射物的旧位置数组，绘制光学拖尾效果
//            for (int i = 0; i < Projectile.oldPos.Length; i++)
//            {
//                // 计算颜色插值值，使颜色在旧位置之间平滑过渡
//                float colorInterpolation = (float)Math.Cos(Projectile.timeLeft / 32f + Main.GlobalTimeWrappedHourly / 20f + i / (float)Projectile.oldPos.Length * MathHelper.Pi) * 0.5f + 0.5f;

//                // 使用海蓝色渐变
//                Color color = Color.Lerp(Color.DeepSkyBlue, Color.Cyan, colorInterpolation) * 0.4f;
//                color.A = 0;

//                // 计算绘制位置，将位置调整到碰撞箱的中心
//                Vector2 drawPosition = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

//                // 计算外部和内部的颜色
//                Color outerColor = color;
//                Color innerColor = color * 0.5f;

//                // 计算强度，使拖尾逐渐变弱
//                float intensity = 0.9f + 0.15f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 60f * MathHelper.TwoPi);
//                intensity *= MathHelper.Lerp(0.15f, 1f, 1f - i / (float)Projectile.oldPos.Length);
//                if (Projectile.timeLeft <= 60)
//                {
//                    intensity *= Projectile.timeLeft / 60f; // 如果弹幕即将消失，则拖尾也逐渐消失
//                }

//                // 计算外部和内部的缩放比例，使拖尾具有渐变效果
//                Vector2 outerScale = new Vector2(2f) * intensity;
//                Vector2 innerScale = new Vector2(2f) * intensity * 0.7f;
//                outerColor *= intensity;
//                innerColor *= intensity;

//                // 绘制外部的拖尾效果，并应用旋转
//                Main.EntitySpriteDraw(lightTexture, drawPosition, null, outerColor, Projectile.rotation, lightTexture.Size() * 0.5f, outerScale * 0.6f, SpriteEffects.None, 0);

//                // 绘制内部的拖尾效果，并应用旋转
//                Main.EntitySpriteDraw(lightTexture, drawPosition, null, innerColor, Projectile.rotation, lightTexture.Size() * 0.5f, innerScale * 0.6f, SpriteEffects.None, 0);
//            }

//            // 绘制默认的弹幕，并应用旋转
//            Main.EntitySpriteDraw(lightTexture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, lightColor, Projectile.rotation, lightTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);

//            return false;
//        }






//    }
//}
