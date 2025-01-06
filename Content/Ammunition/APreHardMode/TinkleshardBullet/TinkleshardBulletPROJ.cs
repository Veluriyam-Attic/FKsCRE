using CalamityMod.NPCs;
using CalamityMod.Particles;
using CalamityMod;
using FKsCRE.Content.Ammunition.BPrePlantera.CryonicBullet;
using FKsCRE.CREConfigs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace FKsCRE.Content.Ammunition.APreHardMode.TinkleshardBullet
{
    internal class TinkleshardBulletPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.APreHardMode";
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
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.MaxUpdates = 1;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
            Projectile.scale = 0.5f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Lighting.AddLight(Projectile.Center, Color.Lerp(Color.Blue, Color.AliceBlue, 0.5f).ToVector3() * 0.49f);

            // Projectile becomes visible after a few frames
            if (Projectile.timeLeft == 298)
                Projectile.alpha = 0;

            // 飞行过程中生成冰蓝色特效
            if (Main.rand.NextBool(1)) // 每帧有 1/3 概率生成粒子
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.WaterCandle, // 粒子类型
                    -Projectile.velocity.RotatedByRandom(0.3f) * Main.rand.NextFloat(0.2f, 0.5f), // 添加随机偏移的速度
                    150,
                    Color.AliceBlue, // 颜色
                    Main.rand.NextFloat(0.9f, 1.6f) // 随机大小
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
          
        }


        public override void OnKill(int timeLeft)
        {
            // 播放音效
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

            // 发射 4 发弹幕
            for (int i = 0; i < 4; i++)
            {
                float randomAngle = Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2); // 随机前方 180 度内
                Vector2 velocity = Projectile.velocity.RotatedBy(randomAngle) * 5f; // 速度
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<TinkleshardBulletSPIT>(),
                    (int)(Projectile.damage * 0.25f), // 伤害倍率为 0.25
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
        }

    }
}
