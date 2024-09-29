using System;
using System.Collections.Generic;
using System.Text;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.IO;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework.Graphics;

namespace FKsCRE.Content.Arrows.PlasmaDriveCorePrototypeArrow
{
    public class PlasmaDriveCorePrototypeArrowPROJ : ModProjectile
    {
        // 定义初始速度、减速速度和减速时间
        public const float InitialSpeed = 44f;
        public const float SlowdownSpeed = 7f;
        public const int SlowdownTime = 50;
        public static readonly float SlowdownFactor = (float)Math.Pow(SlowdownSpeed / InitialSpeed, 1f / SlowdownTime);

        // 使用 ai[0] 来记录时间
        public ref float Time => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.timeLeft = SlowdownTime;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.Opacity = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;

            //// 确保初始速度被设置正确
            //if (Projectile.velocity == Vector2.Zero)
            //{
            //    Projectile.velocity = new Vector2(InitialSpeed, 0f).RotatedByRandom(MathHelper.TwoPi); // 随机方向发射
            //}
        }




        public override void AI()
        {
            // Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4; // 倾斜贴图需要转45度
            Projectile.rotation = Projectile.velocity.ToRotation(); // 我们的贴图不需要转

            // Very rapidly slow down and fade out, transforming into light.
            if (Time <= SlowdownTime)
            {
                Projectile.Opacity = (float)Math.Pow(1f - Time / SlowdownTime, 2D);
                Projectile.velocity *= SlowdownFactor;

                int lightDustCount = (int)MathHelper.Lerp(8f, 1f, Projectile.Opacity);
                for (int i = 0; i < lightDustCount; i++)
                {
                    Vector2 dustSpawnPosition = Projectile.Center + Main.rand.NextVector2Unit() * (1f - Projectile.Opacity) * 45f;
                    Dust light = Dust.NewDustPerfect(dustSpawnPosition, 267);
                    light.color = Color.Lerp(Color.Gold, Color.White, Main.rand.NextFloat(0.5f, 1f));
                    light.velocity = Main.rand.NextVector2Circular(10f, 10f);
                    light.scale = MathHelper.Lerp(1.3f, 0.8f, Projectile.Opacity) * Main.rand.NextFloat(0.8f, 1.2f);
                    light.noGravity = true;
                }
            }

            Time++;
        }

        private void CreateLightEffect()
        {
            // 发光粒子效果仿照SeraphimProjectile
            int lightDustCount = (int)MathHelper.Lerp(8f, 1f, Projectile.Opacity);
            for (int i = 0; i < lightDustCount; i++)
            {
                Vector2 dustSpawnPosition = Projectile.Center + Main.rand.NextVector2Unit() * (1f - Projectile.Opacity) * 45f;
                Dust light = Dust.NewDustPerfect(dustSpawnPosition, 267);
                light.color = Color.Lerp(Color.Gold, Color.White, Main.rand.NextFloat(0.5f, 1f));
                light.velocity = Main.rand.NextVector2Circular(10f, 10f);
                light.scale = MathHelper.Lerp(1.3f, 0.8f, Projectile.Opacity) * Main.rand.NextFloat(0.8f, 1.2f);
                light.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            // 在弹幕消失时，释放SHPExplosion
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SHPExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // 模仿SeraphimProjectile的PreDraw方法
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() * 0.5f;
            Vector2 baseDrawPosition = Projectile.Center - Main.screenPosition;

            // 逐渐消失的光效
            float endFade = Utils.GetLerpValue(0f, 12f, Projectile.timeLeft, true);
            Color mainColor = Color.White * Projectile.Opacity * endFade * 1.5f;
            mainColor.A = (byte)(255 - Projectile.alpha);

            Color afterimageLightColor = Color.White * endFade;
            afterimageLightColor.A = (byte)(255 - Projectile.alpha);

            // 绘制多个逐渐淡出的光影
            for (int i = 0; i < 18; i++)
            {
                Vector2 drawPosition = baseDrawPosition + (MathHelper.TwoPi * i / 18f).ToRotationVector2() * (1f - Projectile.Opacity) * 16f;
                Main.EntitySpriteDraw(texture, drawPosition, null, afterimageLightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }

            // 绘制特殊的残影效果
            for (int i = 0; i < 8; i++)
            {
                Vector2 drawPosition = baseDrawPosition - Projectile.velocity * i * 0.3f;
                Color afterimageColor = mainColor * (1f - i / 8f);
                Main.EntitySpriteDraw(texture, drawPosition, null, afterimageColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }

            return false; // 不使用默认绘制
        }
    }
}