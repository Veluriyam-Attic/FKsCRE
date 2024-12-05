using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using CalamityMod.Particles;

namespace FKsCRE.Content.DeveloperItems.Weapon.BrassBeast
{
    public class BrassBeastPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.BrassBeast";
        // 使用透明贴图
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            // 基本参数设定
            Projectile.width = Projectile.height = 30; // 碰撞箱大小
            Projectile.friendly = true; // 友方投射物
            Projectile.ignoreWater = true; // 无视水
            Projectile.tileCollide = false; // 不与地形碰撞
            Projectile.DamageType = DamageClass.Ranged; // 远程伤害
            Projectile.penetrate = -1; // 无限穿透
            Projectile.MaxUpdates = 30; // 最大更新帧数
            Projectile.timeLeft = 240; // 存活时间
            Projectile.usesIDStaticNPCImmunity = true; // 开启静态NPC击中冷却
            Projectile.idStaticNPCHitCooldown = 6; // 每6帧可击中一次相同NPC
        }

        public override void AI()
        {
            // 添加光照
            Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3() * 0.2f);

            // 飞行期间释放粒子特效
            ReleaseParticles();
        }

        private void ReleaseParticles()
        {
            // 使用尖刺型粒子特效
            for (int i = 0; i < 3; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(5f, 5f);
                PointParticle spikeParticle = new PointParticle(
                    Projectile.Center + offset,
                    Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(1f, 1f), // 粒子速度
                    false,
                    20, // 粒子存活时间
                    1.2f, // 粒子大小倍率
                    Color.Goldenrod // 颜色为黄铜色
                );
                GeneralParticleHandler.SpawnParticle(spikeParticle);
            }

            // 使用轻型烟雾粒子特效
            for (int i = 0; i < 5; i++)
            {
                Particle lightSmoke = new HeavySmokeParticle(
                    Projectile.Center + Main.rand.NextVector2Circular(10f, 10f),
                    Vector2.Zero, // 烟雾速度
                    Color.Orange * 0.5f, // 半透明橙色
                    30, // 存活时间
                    Main.rand.NextFloat(0.8f, 1.5f), // 粒子缩放
                    0.3f, // 透明度
                    Main.rand.NextFloat(-1f, 1f), // 随机旋转
                    true
                );
                GeneralParticleHandler.SpawnParticle(lightSmoke);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 施加火焰Debuff
            target.AddBuff(BuffID.OnFire, 300); // 5秒燃烧
            target.AddBuff(BuffID.CursedInferno, 300); // 5秒诅咒地狱火

            // 击中后释放大量粒子特效
            SpawnHitParticles();
        }

        private void SpawnHitParticles()
        {
            // 使用重型烟雾粒子
            for (int i = 0; i < 10; i++)
            {
                Vector2 smokePosition = Projectile.Center + Main.rand.NextVector2Circular(20f, 20f);
                Particle heavySmoke = new HeavySmokeParticle(
                    smokePosition,
                    Main.rand.NextVector2Circular(3f, 3f),
                    Color.DarkGray, // 烟雾颜色为深灰色
                    Main.rand.Next(40, 60), // 存活时间
                    Main.rand.NextFloat(1.5f, 2f), // 缩放大小
                    1f, // 透明度
                    MathHelper.ToRadians(Main.rand.NextFloat(0f, 2f)), // 随机旋转
                    true
                );
                GeneralParticleHandler.SpawnParticle(heavySmoke);
            }

            // 使用尖刺型粒子
            for (int i = 0; i < 6; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(15f, 15f);
                PointParticle spikeParticle = new PointParticle(
                    Projectile.Center + offset,
                    Main.rand.NextVector2Circular(2f, 2f), // 粒子速度
                    false,
                    25, // 粒子存活时间
                    1.5f, // 粒子大小倍率
                    Color.OrangeRed // 颜色为橙红色
                );
                GeneralParticleHandler.SpawnParticle(spikeParticle);
            }
        }
    }
}

