using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using CalamityMod;
using FKsCRE.CREConfigs;

namespace FKsCRE.Content.Arrows.EAfterDog.AuricArrow
{
    internal class AuricArrowBALL : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.EAfterDog";
        private const int NoDamageTime = 2;  // 0.15秒不造成伤害（60帧/秒）

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 4;  // 穿透次数
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;  // 存活时间
            Projectile.alpha = 80;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 5; // 无敌帧冷却时间为5帧
        }

        public override void AI()
        {
            // 在飞行过程中逐渐变透明和加速
            Projectile.alpha += 5;
            Projectile.velocity *= 1.01f;

            // 如果触碰到屏幕边缘，则删除该弹幕
            if (!ProjectileWithinScreen())
            {
                Projectile.Kill();
            }

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 产生金色粒子效果
                int dustType = Main.rand.NextBool(3) ? 244 : 246;
                float scale = 0.8f + Main.rand.NextFloat(0.6f);
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity = Projectile.velocity / 3f;
                Main.dust[idx].scale = scale;
            }

            // 每帧向右偏转 2 度（弧度）
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(2));

            Time++;
        }
        public ref float Time => ref Projectile.ai[1];

        public override bool? CanDamage() => Time >= 20f; // 初始的时候不会造成伤害，直到x为止

        private bool ProjectileWithinScreen()
        {
            Vector2 screenPosition = Main.screenPosition;
            return Projectile.position.X > screenPosition.X && Projectile.position.X < screenPosition.X + Main.screenWidth
                   && Projectile.position.Y > screenPosition.Y && Projectile.position.Y < screenPosition.Y + Main.screenHeight;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                Main.spriteBatch.SetBlendState(BlendState.Additive);

                Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                Vector2 drawPosition = Projectile.Center - Main.screenPosition;
                Vector2 origin = texture.Size() * 0.5f;
                Color color = new Color(255, 215, 0); // 金黄色
                Main.EntitySpriteDraw(texture, drawPosition, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
                Main.spriteBatch.SetBlendState(BlendState.AlphaBlend);
                return false;
            }
            return true;
        }
    }
}