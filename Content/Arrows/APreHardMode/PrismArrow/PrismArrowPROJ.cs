using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using FKsCRE.CREConfigs;

namespace FKsCRE.Content.Arrows.APreHardMode.PrismArrow
{
    internal class PrismArrowPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.APreHardMode";
        public override string Texture => "FKsCRE/Content/Arrows/APreHardMode/PrismArrow/PrismArrow";
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
                // 画残影效果
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
                return false;
            }
            return true;
        }
        public override void SetDefaults()
        {
            // 设置弹幕的基础属性
            Projectile.width = 11; // 弹幕宽度
            Projectile.height = 24; // 弹幕高度
            Projectile.friendly = true; // 对敌人有效
            Projectile.DamageType = DamageClass.Ranged; // 远程伤害类型
            Projectile.penetrate = 1; // 穿透力为1，击中一个敌人就消失
            Projectile.timeLeft = 300; // 弹幕存在时间为x帧
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 14; // 无敌帧冷却时间为14帧
            Projectile.ignoreWater = true; // 弹幕不受水影响
            Projectile.arrow = true;
            Projectile.extraUpdates = 0;
            Projectile.aiStyle = ProjAIStyleID.Arrow; // 受重力影响
        }

        public override void AI()
        {
            // 实时检测是否在液体中，并动态调整 aiStyle
            if (Projectile.wet) // 在水中时
            {
                Projectile.aiStyle = ProjAIStyleID.Arrow; // 启用重力影响
            }
            else // 不在水中
            {
                Projectile.aiStyle = -1; // 禁用重力影响
            }

            // 调整弹幕的旋转，使其在飞行时保持水平
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;

            // Lighting - 添加天蓝色光源，光照强度为 0.49
            Lighting.AddLight(Projectile.Center, Color.LightSkyBlue.ToVector3() * 0.49f);

            // 液体中的独特行为
            if (Projectile.wet)
            {
                // 提高伤害至原始伤害的 1.5 倍
                Projectile.damage = (int)(Projectile.originalDamage * 1.5f);

                // 提升速度至原速度的 2.5 倍
                Projectile.velocity *= 2.5f;

                // 模拟液体中的抛物线行为
                Projectile.velocity.Y += 0.02f; // 逐渐增加垂直速度
            }
            else
            {
                // 恢复原始伤害
                Projectile.damage = Projectile.originalDamage;

                // 垂直方向速度逐渐增加，模拟重力影响
                Projectile.velocity.Y += 0.25f;
            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public override void OnKill(int timeLeft)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 释放天蓝色的水系粒子特效
                for (int i = 0; i < 10; i++)
                {
                    // 随机生成粒子发射的速度和方向，沿着箭矢的朝向发射
                    Vector2 particleVelocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(0.5f, 1.5f);

                    // 创建天蓝色粒子，并调整缩放和亮度
                    Dust waterDust = Dust.NewDustPerfect(Projectile.Center, DustID.Water, particleVelocity, 0, Color.LightSkyBlue, 2f); // 设置颜色为天蓝色，缩放为2倍
                    waterDust.noGravity = true; // 无重力效果
                    waterDust.fadeIn = 1.5f; // 增加粒子的亮度和淡入效果
                }
            }
        }







    }
}