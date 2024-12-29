using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Particles;
using Terraria.Audio;
using FKsCRE.CREConfigs;

namespace FKsCRE.Content.Ammunition.DPreDog.PolterplasmBullet
{
    internal class PolterplasmBulletPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.DPreDog";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                CalamityUtils.DrawAfterimagesFromEdge(Projectile, 0, Color.White);
                return false;
            }
            return true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.MaxUpdates = 6;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        private int particleTimer = 0; // 计时器
        private float additionalRotation = 0f; // 额外的旋转角度

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            // 添加光效
            Lighting.AddLight(Projectile.Center, Color.Lerp(Color.Pink, Color.White, 0.5f).ToVector3() * 0.49f);

            // 子弹在出现之后很短一段时间会变得可见
            if (Projectile.timeLeft == 296)
                Projectile.alpha = 0;

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 每15帧生成浅粉色椭圆形专有粒子特效
                if (Projectile.ai[0] % 30 == 0)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        Particle pulse = new DirectionalPulseRing(Projectile.Center, Projectile.velocity * 0.75f, Color.Pink, new Vector2(1f, 2.5f), Projectile.rotation, 0.2f, 0.03f, 20);
                        GeneralParticleHandler.SpawnParticle(pulse);
                    }
                }
            }

            Projectile.ai[0]++;
        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                int dustCount = Main.rand.Next(5, 8); // 随机生成 5~7 个粒子
                for (int i = 0; i < dustCount; i++)
                {
                    Vector2 velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(0.5f, 1.5f); // 随机方向
                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.PinkTorch, // 使用粉色主题的 Dust
                        velocity,
                        0,
                        Color.Pink,
                        Main.rand.NextFloat(1f, 1.5f) // 随机缩放
                    );
                    dust.noGravity = true; // 无重力效果
                }
            }



            // 如果击中时是暴击，触发特殊效果
            if (hit.Crit)
            {
                HandleCriticalHit(target);
            }
        }

        private void HandleCriticalHit(NPC target)
        {
            int flowerCount = Main.rand.Next(3, 6); // 随机生成 3~5 个花弹幕
            for (int i = 0; i < flowerCount; i++)
            {
                // 生成随机方向
                Vector2 direction = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi).SafeNormalize(Vector2.Zero);

                // 计算初始速度为当前弹幕速度的 x 倍
                Vector2 initialVelocity = direction * Projectile.velocity.Length() * 1.0f;

                // 创建新弹幕
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    initialVelocity, // 使用初始速度
                    ModContent.ProjectileType<PolterplasmBulletFlower>(),
                    Projectile.damage, // 伤害倍率为 1.0
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
            // 播放音效
            SoundStyle sound = new("CalamityMod/Sounds/Item/PhantomSpirit");
            SoundEngine.PlaySound(sound with { Volume = 0.65f, PitchVariance = 0.3f, Pitch = -0.5f }, target.Center);
        }

        public override void OnKill(int timeLeft)
        {

        }
    }
}
