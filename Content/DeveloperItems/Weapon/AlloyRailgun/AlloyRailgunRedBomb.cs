using CalamityMod;
using FKsCRE.CREConfigs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Ranged;
using Terraria.Audio;
using Terraria.DataStructures;

namespace FKsCRE.Content.DeveloperItems.Weapon.AlloyRailgun
{
    internal class AlloyRailgunRedBomb : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj"; // 透明贴图

        public bool exploding
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value == true ? 1f : 0f;
        }

        public float sizeBonus { get; set; } = 1f;
        public Color mainColor { get; set; } = Color.Red; // 主色调为红色

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor, 1);
                return false;
            }
            return true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity *= 0.6f;
        }
        public override void SetDefaults()
        {
            // 设置弹幕的基础属性
            Projectile.width = 11; // 弹幕宽度
            Projectile.height = 24; // 弹幕高度
            Projectile.friendly = true; // 对敌人有效
            Projectile.DamageType = DamageClass.Ranged; // 远程伤害类型
            Projectile.penetrate = -1; // 穿透力为1，击中一个敌人就消失
            Projectile.timeLeft = 480; // 弹幕存在时间为x帧
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = -1; // 无敌帧冷却时间为14帧
            Projectile.ignoreWater = true; // 弹幕不受水影响
            Projectile.extraUpdates = 6;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) =>
            CalamityUtils.CircularHitboxCollision(Projectile.Center, exploding ? 120 * sizeBonus : 20 * sizeBonus, targetHitbox);

        public override void AI()
        {
            Projectile.velocity *= 0.988f; // 每帧速度减少
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2; // 调整旋转
            Lighting.AddLight(Projectile.Center, mainColor.ToVector3() * 0.7f); // 添加光照

            if (Projectile.timeLeft <= 65)
                exploding = true;

            if (exploding)
            {
                Projectile.velocity = Vector2.Zero;

                if (Projectile.timeLeft > 65)
                    Projectile.timeLeft = 65;

                if (Projectile.timeLeft == 65)
                {
                    // 爆炸光环
                    Particle blastRing = new CustomPulse(
                        Projectile.Center,
                        Vector2.Zero,
                        mainColor,
                        "CalamityMod/Particles/HighResHollowCircleHardEdge",
                        Vector2.One,
                        Main.rand.NextFloat(-10, 10),
                        0.12f * sizeBonus,
                        0f,
                        25
                    );
                    GeneralParticleHandler.SpawnParticle(blastRing);
                    SoundStyle fire = new("CalamityMod/Sounds/Item/ArcNovaDiffuserChargeImpact");
                    SoundEngine.PlaySound(fire with { Volume = 1.25f, Pitch = -0.2f, PitchVariance = 0.15f }, Projectile.Center);
                }
            }

            // 粒子效果
            if (Projectile.timeLeft % 2 == 0)
            {
                Vector2 dustVel = new Vector2(4, 4).RotatedByRandom(100) * Main.rand.NextFloat(0.1f, 0.8f) * (exploding ? sizeBonus : 1);
                Dust dust = Dust.NewDustPerfect(Projectile.Center + dustVel * (exploding ? 5 : 1), 267, dustVel, 0, default, Main.rand.NextFloat(0.9f, 1.2f) * (exploding ? 1.5f : 1));
                dust.noGravity = true;
                dust.color = Color.Lerp(mainColor, Color.White, Main.rand.NextFloat());
            }
        }

        public override void OnKill(int timeLeft)
        {
            // 寻找最近敌人
            NPC target = Projectile.Center.ClosestNPCAt(1200f);
            if (target != null)
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    direction * 15f, // 光束速度
                    ModContent.ProjectileType<AdamAcceleratorBeam>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );
            }

            // 爆炸粒子特效
            for (int i = 0; i < 30; i++)
            {
                Vector2 dustPos = Projectile.Center + Main.rand.NextVector2Circular(10f, 10f);
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Firework_Red, Vector2.Zero, 0, mainColor, Main.rand.NextFloat(1f, 1.5f));
                dust.noGravity = true;
            }
        }
    }
}