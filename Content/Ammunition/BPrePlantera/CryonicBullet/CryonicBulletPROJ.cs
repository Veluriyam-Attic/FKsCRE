using CalamityMod.Items.Ammo;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityMod.Items.Ammo;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.NPCs;

namespace FKsCRE.Content.Ammunition.BPrePlantera.CryonicBullet
{
    public class CryonicBulletPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.BPrePlantera";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesFromEdge(Projectile, 0, Color.White);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.MaxUpdates = 5;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Lighting.AddLight(Projectile.Center, Color.Lerp(Color.Blue, Color.AliceBlue, 0.5f).ToVector3() * 0.49f);

            // Projectile becomes visible after a few frames
            if (Projectile.timeLeft == 298)
                Projectile.alpha = 0;

            // 飞行过程中生成冰蓝色特效
            if (Main.rand.NextBool(3)) // 每帧有 1/3 概率生成粒子
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.IceTorch, // 冰蓝色粒子类型
                    -Projectile.velocity.RotatedByRandom(0.3f) * Main.rand.NextFloat(0.2f, 0.5f), // 添加随机偏移的速度
                    150,
                    Color.LightBlue, // 颜色为冰蓝色
                    Main.rand.NextFloat(0.3f, 0.6f) // 随机大小
                );
                dust.noGravity = true; // 粒子无重力
                dust.fadeIn = 0.5f; // 逐渐淡入
            }

        }

        public override void OnSpawn(IEntitySource source)
        {

        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CalamityGlobalNPC modNPC = target.Calamity();
            if (!modNPC.veriumDoomMarked)
            {
                modNPC.veriumDoomMarked = true;
                modNPC.veriumDoomTimer = CalamityGlobalNPC.veriumDoomTime;
            }
            modNPC.veriumDoomStacks++;

            // 命中敌人时生成淡粉红色线性特效
            for (int i = 0; i < 2; i++) // 往多个方向生成粒子
            {
                Vector2 trailVelocity = Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.5f, 1.5f);
                Particle trail = new SparkParticle(
                    Projectile.Center, // 起始位置为命中点
                    trailVelocity,     // 粒子速度随机偏移
                    false,
                    60,                // 生命周期
                    1.0f,              // 粒子缩放
                    Color.LightPink    // 淡粉红色
                );
                GeneralParticleHandler.SpawnParticle(trail);
            }

        }


        public override void OnKill(int timeLeft)
        {

            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            GenericSparkle impactParticle = new GenericSparkle(Projectile.Center, Vector2.Zero, Color.Goldenrod, Color.White, Main.rand.NextFloat(0.7f, 1.2f), 12);
            GeneralParticleHandler.SpawnParticle(impactParticle);


            // 随机生成 3 到 10 个碎片弹幕
            int fragmentCount = Main.rand.Next(3, 11);

            for (int i = 0; i < fragmentCount; i++)
            {
                // 随机方向
                float angle = MathHelper.ToRadians(Main.rand.Next(0, 360)); // 0 到 360 度随机角度
                Vector2 velocity = Projectile.velocity.RotatedBy(angle) * 0.25f; // 初始速度为自身速度的 x 倍

                // 创建碎片弹幕
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(), // 弹幕来源
                    Projectile.Center,               // 生成位置
                    velocity,                        // 初始速度
                    ModContent.ProjectileType<CryonicBulletFragment>(), // 碎片弹幕类型
                    (int)(Projectile.damage * 1.5f), // 伤害为 1.5 倍
                    Projectile.knockBack,           // 使用当前弹幕的击退力
                    Projectile.owner                // 拥有者
                );
            }
        }

    }
}
