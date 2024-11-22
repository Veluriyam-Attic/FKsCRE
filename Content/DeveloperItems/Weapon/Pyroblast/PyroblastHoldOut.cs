using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Audio;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Particles;

namespace FKsCRE.Content.DeveloperItems.Weapon.Pyroblast
{
    public class PyroblastHoldOut : BaseGunHoldoutProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.Pyroblast";
        public override int AssociatedItemID => ModContent.ItemType<Pyroblast>(); // 绑定武器
        public override float MaxOffsetLengthFromArm => 15f; // 设置与手臂的距离
        public override float OffsetXUpwards => -12f; // 武器向上的偏移
        public override float BaseOffsetY => -10f; // 基础的Y轴偏移
        public override float OffsetYDownwards => 10f; // 武器向下的偏移

        public int upgradeLevel = 0; // 当前等级
        public int frameCounter = 0; // 帧计数器
        public int upgradeTimer = 0; // 用于升级的计时器

        public override void HoldoutAI()
        {
            Player player = Main.player[Projectile.owner];

            // 检查玩家是否未手持 Pyroblast
            if (player.HeldItem.type != ModContent.ItemType<Pyroblast>())
            {
                // 重置等级
                upgradeLevel = 1;
                return; // 退出逻辑，避免其他操作
            }

            // 检查玩家是否受到了伤害
            if (player.hurtCooldowns[0] > 0) // 检测玩家是否处于受伤状态
            {
                if (upgradeLevel > 1) // 降低等级至最低
                {
                    upgradeLevel = 1;
                }
            }

            // 每6帧生成一个子弹
            frameCounter++;
            if (frameCounter % 6 == 0)
            {
                ShootPyroblast(player);
            }

            // 升级逻辑
            upgradeTimer++;
            if (upgradeTimer >= 300) // 每5秒升级一次
            {
                UpgradeLevel();
                upgradeTimer = 0;
            }

            // 根据当前等级执行额外逻辑
            ExecuteUpgradeLogic(player);
        }


        private void ShootPyroblast(Player player)
        {
            // 生成 PyroblastPROJ 子弹，并设置初始速度与随机偏移
            Vector2 direction = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX).RotatedByRandom(MathHelper.ToRadians(2));
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                direction * 10f,
                ModContent.ProjectileType<PyroblastPROJ>(),
                Projectile.damage,
                Projectile.knockBack,
                player.whoAmI
            );

            // 后坐力效果：武器后退2像素
            OffsetLengthFromArm -= 2f;

            // 播放开火音效
            SoundEngine.PlaySound(SoundID.Item41, Projectile.Center);
        }


        private void UpgradeLevel()
        {
            upgradeLevel++;
            if (upgradeLevel > 9) upgradeLevel = 9; // 限制等级最大为9

            // 升级特效：玩家中心产生大量火焰与岩浆粒子
            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(10f, 10f);
                Dust.NewDustPerfect(Main.LocalPlayer.Center, DustID.Torch, velocity, 100, Color.OrangeRed, 2f).noGravity = true;
            }
            for (int i = 0; i < 30; i++) // 生成x个橙色粒子
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(10f, 10f).RotatedByRandom(MathHelper.ToRadians(20)) * Main.rand.NextFloat(2f, 5f);
                LineParticle subTrail = new LineParticle(Projectile.Center, velocity * 35, false, 30, 0.75f, Color.Orange);
                GeneralParticleHandler.SpawnParticle(subTrail);
            }
            SoundEngine.PlaySound(SoundID.Item25, Main.LocalPlayer.Center); // 播放升级音效
        }

        private void ExecuteUpgradeLogic(Player player)
        {
            for (int i = 1; i <= upgradeLevel; i++) // 遍历所有已解锁的等级
            {
                switch (i)
                {
                    case 1:
                        ShootLazharSolarBeam(player); // 解锁并保留 Lv1 功能
                        break;
                    case 2:
                        ApplyFireDebuff(); // 解锁并保留 Lv2 功能
                        break;
                    case 3:
                        EnableWeakHoming(); // 解锁并保留 Lv3 功能
                        break;
                    case 4:
                        ShootGhostFire(player); // 解锁并保留 Lv4 功能
                        break;
                    case 5:
                        RainExoFire(player); // 解锁并保留 Lv5 功能
                        break;
                    case 6:
                        EnableStrongHoming(); // 解锁并保留 Lv6 功能
                        break;
                    case 7:
                        EnhanceExplosion(); // 解锁并保留 Lv7 功能
                        break;
                    case 8:
                        ShootAirburstGrenade(player); // 解锁并保留 Lv8 功能
                        break;
                    case 9:
                        SummonGalileosPlanet(player); // 解锁并保留 Lv9 功能
                        break;
                }
            }
        }


        // 射出有一定随机角度的激光
        private void ShootLazharSolarBeam(Player player)
        {
            if (frameCounter % 10 == 0) // 每隔一段时间射击一次
            {
                // 计算朝向鼠标的基础方向
                Vector2 baseDirection = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);

                // 在基础方向上随机偏移 ±3 度
                Vector2 randomizedDirection = baseDirection.RotatedByRandom(MathHelper.ToRadians(3));

                // 发射激光
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    randomizedDirection * 10f, // 设置速度为方向的 10 倍
                    ModContent.ProjectileType<PyroblastSolarBeam>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    player.whoAmI
                );
            }
        }

        // 子弹命中敌人后会施加独特的火系效果
        private void ApplyFireDebuff()
        {
            // 在 PyroblastPROJ 中开启火系 debuff 的开关
            PyroblastPROJ.EnableFireDebuff = true;
        }

        private void EnableWeakHoming()
        {
            PyroblastPROJ.HomingMode = 1; // 启用弱追踪
        }

        private void ShootGhostFire(Player player)
        {
            if (frameCounter % 60 == 0) // 每隔一段时间在玩家正后方左右各15度射击
            {
                // 计算玩家到鼠标的基础方向
                Vector2 baseDirection = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);

                // 计算正后方方向
                Vector2 backwardDirection = -baseDirection;

                // 计算左右各 15 度方向
                Vector2 leftOffsetDirection = backwardDirection.RotatedBy(MathHelper.ToRadians(-15));
                Vector2 rightOffsetDirection = backwardDirection.RotatedBy(MathHelper.ToRadians(15));

                // 发射左侧 GhostFire
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    player.Center + leftOffsetDirection * 50f, // 起始位置：距离玩家 50 像素
                    leftOffsetDirection * 10f, // 速度：方向乘以 10
                    ModContent.ProjectileType<PyroblastGhostFire>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    player.whoAmI
                );

                // 发射右侧 GhostFire
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    player.Center + rightOffsetDirection * 50f, // 起始位置：距离玩家 50 像素
                    rightOffsetDirection * 10f, // 速度：方向乘以 10
                    ModContent.ProjectileType<PyroblastGhostFire>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    player.whoAmI
                );
            }
        }


        private void RainExoFire(Player player)
        {
            if (frameCounter % 25 == 0) // 每隔一段时间从天上降下激光
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 lightningSpawnPosition = Main.MouseWorld - Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(960f, 1020f);
                    Vector2 lightningVelocity = (Main.MouseWorld - lightningSpawnPosition).SafeNormalize(Vector2.UnitY) * 14f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), lightningSpawnPosition, lightningVelocity, ModContent.ProjectileType<PyroblastExoFire>(), Projectile.damage * 2, Projectile.knockBack, player.whoAmI);
                }
            }
        }

        private void EnableStrongHoming()
        {
            PyroblastPROJ.HomingMode = 2; // 启用强追踪
        }

        private void EnhanceExplosion()
        {
            PyroblastPROJ.EnableEnhancedExplosion = true; // 开启增强爆炸效果
        }

        private void ShootAirburstGrenade(Player player)
        {
            if (frameCounter % 300 == 0) // 每隔一段时间射出榴弹
            {
                // 计算朝向鼠标的方向
                Vector2 directionToMouse = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);

                // 发射榴弹，沿着鼠标方向
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    directionToMouse * 10f, // 设置速度为方向的 10 倍
                    ModContent.ProjectileType<PyroblastAirburstGrenade>(),
                    Projectile.damage * 3,
                    Projectile.knockBack,
                    player.whoAmI
                );
            }
        }


        private void SummonGalileosPlanet(Player player)
        {
            if (frameCounter % 150 == 0) // 每隔一段时间天降星球
            {
                // 定义生成位置：鼠标位置正上方 300~350 像素
                Vector2 spawnPosition = Main.MouseWorld - Vector2.UnitY * Main.rand.NextFloat(300f, 350f);

                // 定义初始速度：向下 6f
                Vector2 velocity = Vector2.UnitY * 6f;

                // 生成星球
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    spawnPosition,
                    velocity, // 设置初始速度
                    ModContent.ProjectileType<PyroblastPlanet>(),
                    Projectile.damage * 15, // 伤害倍率
                    Projectile.knockBack,
                    player.whoAmI
                );
            }
        }

    }
}
