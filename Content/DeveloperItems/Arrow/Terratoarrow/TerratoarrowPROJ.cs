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

namespace FKsCRE.Content.DeveloperItems.Arrow.Terratoarrow
{
    internal class TerratoarrowPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.Terratoarrow";
        public override string Texture => "FKsCRE/Content/DeveloperItems/Arrow/Terratoarrow/Terratoarrow";

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
            Projectile.timeLeft = 20; // 弹幕存在时间为x帧
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 14; // 无敌帧冷却时间为14帧
            Projectile.ignoreWater = true; // 弹幕不受水影响
            Projectile.arrow = true;
            Projectile.extraUpdates = 4;
        }

        public override void AI()
        {
            // 调整弹幕的旋转，使其在飞行时保持水平
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.49f);
            if (Projectile.ai[0] == 0)
            {
                //Projectile.damage = (int)(Projectile.damage * 0.3f);
                Projectile.velocity *= 0.5f;
                LineParticle spark = new LineParticle(Projectile.Center + Projectile.velocity * 4, Projectile.velocity * 4.95f, false, 9, 2.4f, Color.LimeGreen);
                GeneralParticleHandler.SpawnParticle(spark);
            }
            Projectile.ai[0]++;


            // 检查剩余时间
            if (Projectile.timeLeft == 2)
            {
                // 基础方向为弹幕当前的朝向
                Vector2 baseDirection = Projectile.velocity.SafeNormalize(Vector2.UnitX);

                // 计算三个弹幕的方向：中心、左偏1度、右偏1度
                Vector2[] directions = new Vector2[3]
                {
            baseDirection.RotatedBy(MathHelper.ToRadians(-1)), // 左偏1度
            baseDirection,                                   // 中心
            baseDirection.RotatedBy(MathHelper.ToRadians(1))  // 右偏1度
                };

                // 释放三个弹幕
                foreach (Vector2 direction in directions)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,               // 起始位置
                        direction * Projectile.velocity.Length(), // 方向保持与原投射物速度一致
                        ModContent.ProjectileType<TerratoarrowSPIT>(),
                        (int)((Projectile.damage) * 0.33),               // 伤害倍率为x
                        Projectile.knockBack,           // 保持相同的击退效果
                        Projectile.owner                // 投射物归属
                    );
                }

                // 遍历三个方向并生成粒子
                foreach (Vector2 direction in directions)
                {
                    PointParticle spark1 = new PointParticle(
                        Projectile.Center - Projectile.velocity + direction, // 起点位置
                        direction * 0.5f,                                    // 粒子速度
                        false,                                               // 是否高亮
                        5,                                                   // 生命周期
                        1.1f,                                                // 粒子大小
                        Color.LimeGreen                                      // 粒子颜色
                    );
                    GeneralParticleHandler.SpawnParticle(spark1);
                }

                // 手动删除弹幕
                Projectile.Kill();
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // 检查弹幕的剩余时间
            if (Projectile.timeLeft >= 10)
            {
                // 增加 2.5 倍伤害
                modifiers.FinalDamage *= 2.5f;
            }
        }

        public override void OnKill(int timeLeft)
        {
           

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }







    }
}