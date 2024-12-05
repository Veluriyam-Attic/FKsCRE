using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CalamityMod.Projectiles.BaseProjectiles;

namespace FKsCRE.Content.DeveloperItems.Weapon.TheGoldenFire
{
    public class TheGoldenFireHoldOut : BaseGunHoldoutProjectile
    {
        public override string Texture => "FKsCRE/Content/DeveloperItems/Weapon/TheGoldenFire/TheGoldenFire";

        public override int AssociatedItemID => ModContent.ItemType<TheGoldenFire>(); // 绑定武器

        public override float MaxOffsetLengthFromArm => 20f; // 设置与手臂的距离
        public override float OffsetXUpwards => -10f; // 武器向上的偏移
        public override float BaseOffsetY => -8f; // 基础的Y轴偏移
        public override float OffsetYDownwards => 10f; // 武器向下的偏移

        private int frameCounter = 0; // 帧计数器

        public override void HoldoutAI()
        {
            frameCounter++;

            // 每x帧射出三发子弹
            if (frameCounter % 3 == 0)
            {
                ShootTheGoldenFire();
            }
        }

        private void ShootTheGoldenFire()
        {
            Player player = Main.player[Projectile.owner];
            float baseDamage = TheGoldenFire.TheFinalDamage;
            float speed = Projectile.velocity.Length(); // 初始速度从武器传入

            // 随机生成三发子弹
            for (int i = 0; i < 2; i++)
            {
                // 随机生成偏移角度
                float randomAngle = MathHelper.ToRadians(Main.rand.NextFloat(-5f, 5f));
                Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedBy(randomAngle);

                // 随机化速度
                float randomSpeed = speed * Main.rand.NextFloat(6f, 12f);

                // 发射子弹
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    direction * randomSpeed,
                    ModContent.ProjectileType<TheGoldenFirePROJ>(),
                    //(int)baseDamage, // 伤害通过武器传递
                    Projectile.damage,
                    Projectile.knockBack,
                    player.whoAmI
                );

                //// 发射子弹
                //Projectile.NewProjectile(
                //    Projectile.GetSource_FromThis(),
                //    Projectile.Center,
                //    direction * randomSpeed,
                //    ProjectileID.Flames, // 使用原版85号弹幕（Flames）
                //    (int)baseDamage,     // 伤害通过武器传递
                //    Projectile.knockBack,
                //    player.whoAmI
                //);

            }

            // 播放开火音效
            //SoundStyle fire = new("CalamityMod/Sounds/Item/NorfleetFire");
            //SoundEngine.PlaySound(fire with { Volume = 0.9f, PitchVariance = 0.25f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.Item34, Projectile.position);

            // 武器后坐力
            OffsetLengthFromArm -= 1f;

            // 生成火焰粒子效果
            GenerateFireParticles();
        }

        private void GenerateFireParticles()
        {
            Vector2 mousePosition = Main.MouseWorld; // 获取鼠标位置

            for (int i = 0; i < 20; i++) // 每次射击生成20个粒子
            {
                // 计算粒子朝向的基础方向（从弹幕中心指向鼠标位置）
                Vector2 baseDirection = (mousePosition - Projectile.Center).SafeNormalize(Vector2.UnitX);

                // 随机在 ±10 度内偏移粒子的朝向
                float randomAngle = MathHelper.ToRadians(Main.rand.NextFloat(-15f, 15f));
                Vector2 adjustedDirection = baseDirection.RotatedBy(randomAngle);

                // 随机化速度，使粒子的运动更自然
                float speed = Main.rand.NextFloat(6f, 10f);
                Vector2 velocity = adjustedDirection * speed;

                // 随机选择粒子类型
                int dustType = Main.rand.Next(new int[] { DustID.Torch, DustID.Lava, DustID.Smoke });

                // 创建粒子，设置其位置、速度、大小和无重力属性
                Dust.NewDustPerfect(Projectile.Center, dustType, velocity, 100, default, 1.5f).noGravity = true;
            }
        }



    }
}
