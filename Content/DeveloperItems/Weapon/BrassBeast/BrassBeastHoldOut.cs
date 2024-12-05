using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Audio;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Particles;

namespace FKsCRE.Content.DeveloperItems.Weapon.BrassBeast
{
    public class BrassBeastHoldOut : BaseGunHoldoutProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.BrassBeast";
        public override string Texture => "FKsCRE/Content/DeveloperItems/Weapon/BrassBeast/BrassBeast";

        public override int AssociatedItemID => ModContent.ItemType<BrassBeast>();
        public override float MaxOffsetLengthFromArm => 20f; // 武器持有距离
        public override float OffsetXUpwards => -10f;
        public override float BaseOffsetY => -15f;
        public override float OffsetYDownwards => 10f;

        private int frameCounter = 0; // 帧计数器

        public override void HoldoutAI()
        {
            Player player = Main.player[Projectile.owner];

            frameCounter++;
            if (frameCounter % 15 == 0) // 每15帧发射一次子弹
            {
                ShootProjectile(player);
                SpawnParticles(player);
                OffsetLengthFromArm -= 15f; // 模拟后坐力
            }

            // 逐渐恢复后坐力位置
            if (OffsetLengthFromArm < MaxOffsetLengthFromArm)
            {
                OffsetLengthFromArm = MathHelper.Lerp(OffsetLengthFromArm, MaxOffsetLengthFromArm, 0.1f);
            }
        }

        private void ShootProjectile(Player player)
        {
            Vector2 direction = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                direction * 20f, // 子弹速度
                ModContent.ProjectileType<BrassBeastPROJ>(),
                Projectile.damage,
                Projectile.knockBack,
                player.whoAmI
            );
            SoundEngine.PlaySound(SoundID.Item40, Projectile.Center); // 发射音效
        }

        private void SpawnParticles(Player player)
        {
            // 生成重型烟雾
            for (int i = 0; i < 5; i++)
            {
                Vector2 smokePosition = Projectile.Center + new Vector2(0, Main.rand.NextFloat(-5, 5));
                Particle heavySmoke = new HeavySmokeParticle(
                    smokePosition,
                    Main.rand.NextVector2Circular(2f, 2f),
                    Color.Goldenrod,
                    Main.rand.Next(40, 60),
                    Main.rand.NextFloat(1.2f, 1.8f),
                    0.8f,
                    Main.rand.NextFloat(-1f, 1f),
                    true
                );
                GeneralParticleHandler.SpawnParticle(heavySmoke);
            }

            // 生成尖刺型粒子
            for (int i = 0; i < 3; i++)
            {
                Vector2 offset = new Vector2(Main.rand.NextFloat(-20f, 20f), 0);
                PointParticle spark = new PointParticle(
                    Projectile.Center - Projectile.velocity + offset,
                    new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(20)),
                    false,
                    20,
                    1.3f,
                    Color.Orange
                );
                GeneralParticleHandler.SpawnParticle(spark);
            }

            // 生成轻型烟雾
            for (int i = 0; i < 10; i++)
            {
                Vector2 smokePos = Projectile.Center + new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-5f, 5f));
                Particle lightSmoke = new HeavySmokeParticle(
                    smokePos,
                    Vector2.Zero,
                    Color.Yellow * 0.5f,
                    20,
                    Main.rand.NextFloat(0.8f, 1.5f),
                    0.3f,
                    Main.rand.NextFloat(-1f, 1f),
                    true
                );
                GeneralParticleHandler.SpawnParticle(lightSmoke);
            }
        }
    }
}
