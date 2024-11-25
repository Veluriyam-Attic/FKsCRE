using CalamityMod;
using FKsCRE.CREConfigs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace FKsCRE.Content.DeveloperItems.Bullet.ChineseChess.Zu
{
    internal class ZuPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.ChineseChess.Zu";
        public override string Texture => "FKsCRE/Content/DeveloperItems/Bullet/ChineseChess/Zu/Zu";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 画残影效果
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
                return false;
            }
            return true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90;
            Projectile.MaxUpdates = 4;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        public override void AI()
        {
            // 减速飞行：每帧将速度乘以 0.975
            Projectile.velocity *= 0.975f;

            // 子弹在短时间后变得可见
            if (Projectile.timeLeft == 89)
                Projectile.alpha = 0;

            // 旋转跟随飞行方向
            Projectile.rotation = Projectile.velocity.ToRotation();

            // 添加螺旋状粒子特效（黑色烟雾）
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 15f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 dustPos = Projectile.Center - Projectile.velocity * (i * 0.25f);
                    int dustType = DustID.Smoke; // 使用烟雾粒子
                    int dustIndex = Dust.NewDust(dustPos, 1, 1, dustType, 0f, 0f, 0, Color.Black, 1.5f); // 黑色粒子
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 0.2f;
                }
                Projectile.localAI[0] = 0f; // 重置计数器
            }
        }


        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 随机选择一个方向（左或右）
            bool shootLeft = Main.rand.NextBool();

            // 设置起始方向
            float baseAngle = Projectile.velocity.ToRotation();
            float directionOffset = shootLeft ? -MathHelper.PiOver2 : MathHelper.PiOver2; // 左或右 90 度
            float shootAngle = baseAngle + directionOffset;

            // 发射子弹
            for (int i = 0; i < 10; i++)
            {
                float spread = MathHelper.ToRadians(10); // 左右各 10 度的范围
                float randomOffset = Main.rand.NextFloat(-spread, spread);
                Vector2 velocity = new Vector2(10f, 0f).RotatedBy(shootAngle + randomOffset);

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    target.Center,
                    velocity,
                    ProjectileID.ThrowingKnife, // 投刀 ID
                    Projectile.damage / 2, // 子弹伤害减半
                    Projectile.knockBack,
                    Projectile.owner
                );
            }

            // 播放音效
            SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
        }

        public override void OnKill(int timeLeft)
        {

        }
    }
}
