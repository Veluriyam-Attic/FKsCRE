using CalamityMod.Projectiles.BaseProjectiles;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Ranged;
using CalamityMod;
using System;
using Terraria.DataStructures;

namespace FKsCRE.Content.DeveloperItems.Weapon.AlloyRailgun
{
    internal class AlloyRailgunHold : BaseGunHoldoutProjectile
    {
        public override int AssociatedItemID => ModContent.ItemType<AlloyRailgun>();
        public override Vector2 GunTipPosition => Projectile.Center + Vector2.UnitX.RotatedBy(Projectile.rotation) * (Projectile.width * 0.5f + 8f);
        private const int MaxChargeTime = 180; // 最大蓄力时间（3秒）
        private int chargeCounter = 0; // 记录蓄力时间
        private bool fullyCharged = false; // 标记是否蓄力完成

        public override float MaxOffsetLengthFromArm => 35f; // 设置与手臂的距离
        public override float OffsetXUpwards => -12f; // 武器向上的偏移
        public override float BaseOffsetY => -10f; // 基础的Y轴偏移
        public override float OffsetYDownwards => 10f; // 武器向下的偏移

        private int scopeProjectileID = -1; // 用于记录 AlloyRailgunScope 的弹幕 ID
        //public override void OnSpawn(IEntitySource source)
        //{
        //    // 生成 AlloyRailgunScope 弹幕
        //    scopeProjectileID = Projectile.NewProjectile(
        //        Projectile.GetSource_FromThis(),
        //        GunTipPosition,
        //        Vector2.Zero,
        //        ModContent.ProjectileType<AlloyRailgunScope>(),
        //        0,
        //        0,
        //        Projectile.owner
        //    );
        //}
        public override void HoldoutAI()
        {
            Player player = Main.player[Projectile.owner];

            if (chargeCounter < MaxChargeTime)
            {
                Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitX); // 使用正前方方向
                Vector2 sparkVelocity = direction.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * 6f; // 随机旋转方向
                CritSpark spark = new CritSpark(
                    GunTipPosition, // 粒子生成位置
                    sparkVelocity + Main.player[Projectile.owner].velocity, // 粒子初始速度
                    Color.White, // 粒子起始颜色
                    Color.LightBlue, // 粒子结束颜色
                    1f, // 粒子缩放
                    16 // 粒子寿命
                );
                GeneralParticleHandler.SpawnParticle(spark);
            }
            else
            {
                // 超过三秒后生成枪口冒烟效果
                Vector2 smokePosition = GunTipPosition + Main.rand.NextVector2Circular(5f, 5f);
                Particle smoke = new HeavySmokeParticle(
                    smokePosition,
                    Vector2.UnitY * -1 * Main.rand.NextFloat(3f, 7f),
                    Color.Gray,
                    Main.rand.Next(30, 60),
                    Main.rand.NextFloat(0.25f, 0.5f),
                    1.0f,
                    MathHelper.ToRadians(Main.rand.NextFloat(-5f, 5f)),
                    true
                );
                GeneralParticleHandler.SpawnParticle(smoke);
            }

            // 检测玩家是否松开左键或不能继续使用手持武器
            if (player.CantUseHoldout() || !player.channel)
            {
                ReleaseProjectile(player); // 松开时释放弹幕
                
                //// 删除 AlloyRailgunScope 弹幕
                //if (scopeProjectileID != -1)
                //{
                //    Projectile scopeProjectile = Main.projectile[scopeProjectileID];
                //    if (scopeProjectile.active && scopeProjectile.type == ModContent.ProjectileType<AlloyRailgunScope>())
                //    {
                //        scopeProjectile.Kill();
                //    }
                //}

                Projectile.Kill(); // 手持弹幕销毁
                return;
            }

            // 蓄力逻辑
            if (chargeCounter < MaxChargeTime)
            {
                chargeCounter++;
                if (chargeCounter >= MaxChargeTime)
                {
                    fullyCharged = true;

                    // 在蓄力完成时生成黑白正方形特效
                    for (int i = 0; i < 4; i++)
                    {
                        float angle = MathHelper.PiOver4 + i * MathHelper.PiOver2;
                        float nextAngle = MathHelper.PiOver4 + (i + 1) * MathHelper.PiOver2;
                        Vector2 start = angle.ToRotationVector2() * 8f;
                        Vector2 end = nextAngle.ToRotationVector2() * 8f;

                        for (int j = 0; j < 20; j++)
                        {
                            Dust squareDust = Dust.NewDustPerfect(GunTipPosition, DustID.WhiteTorch);
                            squareDust.scale = 2.5f;
                            squareDust.velocity = Vector2.Lerp(start, end, j / 20f) * 0.8f;
                            squareDust.color = Color.Lerp(Color.Black, Color.White, j / 20f);
                            squareDust.noGravity = true;
                        }
                    }
                }
            }
        }

        private void ReleaseProjectile(Player player)
        {
            if (chargeCounter >= 60 && chargeCounter <= MaxChargeTime) // 检查蓄力时间是否超过1秒
            {
                // 计算蓄力伤害倍率
                float chargePercent = MathHelper.Clamp(chargeCounter / (float)MaxChargeTime, 0f, 1f);
                int extraDamage = (int)(Projectile.damage * chargePercent * 1.2f);

                //// 发射极光
                //Projectile.NewProjectile(
                //    new EntitySource_ItemUse_WithAmmo(Main.player[Projectile.owner], Main.player[Projectile.owner].HeldItem, -1), // 来源信息
                //    GunTipPosition,                                                        // 生成位置
                //    (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX) * 20f,                 // 使用正前方方向，设置固定速度
                //    ModContent.ProjectileType<AlloyRailgunShot>(),                         // 弹幕类型
                //    Projectile.damage + extraDamage,                                       // 伤害
                //    Projectile.knockBack * chargePercent,                                  // 击退量
                //    Projectile.owner,                                                      // 拥有者
                //    ai1: chargePercent                                                     // 传递 ChargePercent
                //);


                // 发射激光
                //Projectile.NewProjectile(
                //    new EntitySource_ItemUse_WithAmmo(player, player.HeldItem, -1),
                //    GunTipPosition,
                //    Projectile.velocity.SafeNormalize(Vector2.UnitX) * 5f,
                //    ModContent.ProjectileType<AlloyRailgunBEAM>(),
                //    Projectile.damage + extraDamage,
                //    Projectile.knockBack * chargePercent,
                //    Projectile.owner,
                //    ai1: ammoType // 将弹药类型传递给激光
                //);

                // 获取玩家当前的弹药
                int ammoType = 0; // 默认值为 0（无效弹药）
                if (player.inventory[player.selectedItem].useAmmo == AmmoID.Bullet) // 检测武器是否使用子弹弹药
                {
                    foreach (Item item in player.inventory)
                    {
                        if (item.ammo == AmmoID.Bullet && item.stack > 0) // 找到有效子弹
                        {
                            ammoType = item.shoot; // 获取弹药的发射类型
                            break;
                        }
                    }
                }

                for (int i = 0; i < 4; i++) // 随机生成 4 个角度
                {
                    // 在 -5 到 5 度范围内随机选择角度
                    float randomAngle = Main.rand.NextFloat(-MathHelper.ToRadians(5), MathHelper.ToRadians(5));

                    // 根据随机角度计算方向
                    Vector2 offsetDirection = (Main.MouseWorld - GunTipPosition).SafeNormalize(Vector2.UnitX).RotatedBy(randomAngle);

                    // 发射激光
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        GunTipPosition,
                        offsetDirection * 3f, // 设置激光速度
                        ModContent.ProjectileType<AlloyRailgunBEAM>(), // 激光类型
                        Projectile.damage, // 伤害
                        Projectile.knockBack, // 击退
                        Projectile.owner,
                        ai1: ammoType // 将弹药类型传递给激光
                    );
                }


                // 屏幕震动效果
                float shakePower = 5f; // 设置震动强度
                float distanceFactor = Utils.GetLerpValue(1000f, 0f, Projectile.Distance(Main.LocalPlayer.Center), true); // 距离衰减
                Main.LocalPlayer.Calamity().GeneralScreenShakePower = Math.Max(Main.LocalPlayer.Calamity().GeneralScreenShakePower, shakePower * distanceFactor);

                // 播放音效
                SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.5f * chargePercent }, GunTipPosition);


                float[] angles = { -MathHelper.ToRadians(10), -MathHelper.ToRadians(5), MathHelper.ToRadians(5), MathHelper.ToRadians(10) };
                foreach (float angle in angles)
                {
                    Vector2 offsetDirection = (Main.MouseWorld - GunTipPosition).SafeNormalize(Vector2.UnitX).RotatedBy(angle);
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        GunTipPosition,
                        offsetDirection * 3f,
                        ModContent.ProjectileType<AlloyRailgunRedBomb>(),
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner
                    );
                }                
            }
        }
    }
}
