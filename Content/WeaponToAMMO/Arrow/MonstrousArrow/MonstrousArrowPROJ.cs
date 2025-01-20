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
using Terraria.DataStructures;
using FKsCRE.CREConfigs;
using CalamityMod.Balancing;
using CalamityMod.Projectiles;

namespace FKsCRE.Content.WeaponToAMMO.Arrow.MonstrousArrow
{
    internal class MonstrousArrowPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "WeaponToAMMO.Arrow.MonstrousArrow";
        //public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1; // 根据模式设置穿透次数
            Projectile.timeLeft = 200;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        public override void AI()
        {
            // 保持弹幕旋转
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;

            // Lighting - 添加深橙色光源，光照强度为 0.55
            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 0.55f);

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 添加粒子特效，随机 1/3 概率
                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center,
                        Main.rand.NextBool() ? 5 : 12, // 粒子特效 ID 5 和 12 交替
                        -Projectile.velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(0.01f, 0.3f)
                    );
                    dust.noGravity = true; // 粒子无重力
                    dust.scale = Main.rand.NextFloat(0.5f, 0.9f); // 随机大小
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 吸血逻辑
            int heal = (int)Math.Round(hit.Damage * Main.rand.NextFloat(0.03f, 0.05f)); // 治疗量为伤害的 3%-5%
            if (heal > 100)
                heal = 100;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0 || target.lifeMax <= 5)
                return;

            CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ProjectileID.VampireHeal, 3000f);


        }

        public override void OnKill(int timeLeft)
        {
            // 生成红色粒子效果，以三角形速度扩散
            for (int i = 0; i < 20; i++) // 生成 20 个粒子
            {
                float angle = MathHelper.TwoPi / 20 * i; // 计算粒子角度
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(2f, 4f); // 三角形扩散速度
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Blood, // 红色粒子特效
                    velocity
                );
                dust.noGravity = true; // 粒子无重力
                dust.scale = Main.rand.NextFloat(1f, 1.5f); // 粒子缩放大小
            }
        }


    }
}