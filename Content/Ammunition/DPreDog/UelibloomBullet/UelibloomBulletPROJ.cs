﻿using CalamityMod;
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

namespace FKsCRE.Content.Ammunition.DPreDog.UelibloomBullet
{
    public class UelibloomBulletPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.DPreDog";
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
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.MaxUpdates = 6;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        public override void AI()
        {
            // 由于我们是水平贴图，因此什么也不需要转动
            Projectile.rotation = Projectile.velocity.ToRotation();

            // 添加光效
            Lighting.AddLight(Projectile.Center, Color.Lerp(Color.Blue, Color.AliceBlue, 0.5f).ToVector3() * 0.49f);

            // 子弹在出现之后很短一段时间会变得可见
            if (Projectile.timeLeft == 296)
                Projectile.alpha = 0;

            // 添加飞行期间的橙色粒子特效
            if (Main.rand.NextBool(3)) // 随机1/3概率生成
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    Main.rand.NextBool() ? 2 : 3, // 随机选择两种粒子类型
                    -Projectile.velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(0.01f, 0.3f) // 轻微随机速度
                );
                dust.noGravity = true; // 粒子无重力
                dust.scale = Main.rand.NextFloat(0.5f, 0.9f); // 随机大小
                if (dust.type == 2)
                    dust.scale = Main.rand.NextFloat(0.35f, 0.55f); // 特殊类型的粒子缩小
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
            base.OnHitNPC(target, hit, damageDone);

            // 生成 3 个随机方向的 UelibloomBulletLEAF 弹幕
            for (int i = 0; i < 3; i++)
            {
                // 随机生成一个角度（0 到 360 度）
                float angle = MathHelper.ToRadians(Main.rand.Next(0, 360));

                // 计算初始速度，方向为当前速度旋转随机角度后，速度为原速度的 0.7 倍
                Vector2 velocity = Projectile.velocity.RotatedBy(angle) * 0.7f;

                // 创建 UelibloomBulletLEAF 弹幕
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),  // 弹幕来源
                    Projectile.Center,               // 生成位置
                    velocity,                        // 初始速度
                    ModContent.ProjectileType<UelibloomBulletLEAF>(), // UelibloomBulletLEAF 弹幕类型
                    (int)(Projectile.damage * 1.0f), // 伤害倍率为 1.0
                    Projectile.knockBack,            // 使用当前弹幕的击退力
                    Projectile.owner                 // 拥有者
                );
            }
        }

        public override void OnKill(int timeLeft)
        {
            // 创建叶子形状的粒子特效
            for (int i = -1; i <= 1; i += 2) // 两条曲线，正方向和反方向
            {
                for (int j = 0; j < 8; j++) // 每条曲线 x 个粒子
                {
                    float progress = j / 20f; // 进度 0 到 1
                    float angle = progress * MathHelper.PiOver2 * i; // 曲线弯曲程度
                    Vector2 direction = Projectile.velocity.RotatedBy(angle).SafeNormalize(Vector2.UnitY) * (2f + progress * 5f); // 曲线方向和速度

                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center, // 粒子生成位置
                        DustID.Plantera_Green, // 157号粒子
                        direction, // 速度
                        100, // 透明度
                        Color.ForestGreen, // 深绿色
                        Main.rand.NextFloat(1.2f, 1.8f) // 粒子大小
                    );
                    dust.noGravity = true; // 粒子不受重力影响
                    dust.fadeIn = 0.1f; // 快速淡入效果
                }
            }
        }
    }
}