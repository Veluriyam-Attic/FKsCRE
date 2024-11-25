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
using CalamityMod.Particles;

namespace FKsCRE.Content.DeveloperItems.Bullet.ChineseChess.Xiang
{
    internal class XiangPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.ChineseChess.Xiang";
        public override string Texture => "FKsCRE/Content/DeveloperItems/Bullet/ChineseChess/Xiang/Xiang";

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
            Projectile.penetrate = 5;
            Projectile.timeLeft = 450;
            Projectile.MaxUpdates = 4;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        public override void AI()
        {
            // 子弹旋转逻辑
            Projectile.rotation = Projectile.velocity.ToRotation();
            // 子弹在短时间后变得可见
            if (Projectile.timeLeft == 445)
                Projectile.alpha = 0;

            // 在首次执行时，设置弹幕的速度为四个斜方向之一
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1; // 标记已初始化

                // 定义四个斜方向
                Vector2[] directions = new Vector2[]
                {
            new Vector2(1, 1),   // 右下方（东南）
            new Vector2(-1, -1), // 左上方（西北）
            new Vector2(-1, 1),  // 左下方（西南）
            new Vector2(1, -1)   // 右上方（东北）
                };

                // 随机选择一个方向
                Vector2 selectedDirection = directions[Main.rand.Next(4)].SafeNormalize(Vector2.Zero);

                // 设置弹幕的速度（可以根据需要调整速度值）
                float speed = 10f; // 设定固定速度，例如10f
                Projectile.velocity = selectedDirection * speed;
            }

            // 检查弹幕是否与玩家的屏幕边缘发生碰撞
            Rectangle screenRect = new Rectangle(
                (int)Main.screenPosition.X,
                (int)Main.screenPosition.Y,
                Main.screenWidth,
                Main.screenHeight
            );

            // 如果弹幕超出屏幕边缘，则销毁自己
            if (!screenRect.Contains(Projectile.Center.ToPoint()))
            {
                Projectile.Kill();
                return;
            }

            // 在飞行过程中生成灰色烟雾特效
            if (Main.rand.NextBool(2)) // 50% 概率
            {
                Vector2 position = Projectile.Center;
                Vector2 dustVelocity = Projectile.velocity.RotatedByRandom(0.5f) * Main.rand.NextFloat(0.1f, 0.5f);

                Particle smoke = new HeavySmokeParticle(
                    position,
                    dustVelocity * Main.rand.NextFloat(1f, 2.6f),
                    Color.Gray, // 修改为灰色
                    18,
                    Main.rand.NextFloat(0.9f, 1.6f),
                    0.35f,
                    Main.rand.NextFloat(-1, 1),
                    true
                );
                GeneralParticleHandler.SpawnParticle(smoke);
            }
        }


        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public override void OnKill(int timeLeft)
        {
            // 在死亡时释放一堆灰色的原版粒子特效
            int numParticles = 20; // 粒子数量，可根据需要调整
            for (int i = 0; i < numParticles; i++)
            {
                // 创建灰色烟雾粒子
                Dust dust = Dust.NewDustPerfect(
                    Projectile.position,
                    DustID.Smoke,
                    new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)),
                    100,
                    Color.Gray,
                    1.5f
                );
                dust.noGravity = true;
            }
        }

    }
}
