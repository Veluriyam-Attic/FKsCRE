using CalamityMod.Projectiles.Rogue;
using CalamityMod.Projectiles;
using CalamityMod;
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
using FKsCRE.CREConfigs;

namespace FKsCRE.Content.WeaponToAMMO.Arrow.TNTArrow
{
    internal class TNTArrowPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "WeaponToAMMO.Arrow.TNTArrow";
        public override string Texture => "FKsCRE/Content/WeaponToAMMO/Arrow/TNTArrow/TNTArrow";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
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
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.Calamity().pointBlankShotDuration = CalamityGlobalProjectile.DefaultPointBlankDuration;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }





        }

        public override void OnKill(int timeLeft)
        {
            // 添加新功能：在上方生成Grenade弹幕
            Vector2 centerPosition = Projectile.Center - new Vector2(0, 60 * 16); // 计算圆心
            int grenadeCount = Main.rand.Next(1, 4); // 随机生成1~3个
            for (int i = 0; i < grenadeCount; i++)
            {
                // 在圆形区域随机生成点
                Vector2 spawnPosition = centerPosition + Main.rand.NextVector2Circular(10 * 16, 10 * 16);
                Vector2 velocity = (Projectile.Center - spawnPosition).SafeNormalize(Vector2.Zero) * 10f; // 向下坠落

                // 生成弹幕
                int grenadeProj = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    spawnPosition,
                    velocity,
                    ProjectileID.Grenade, // 33号原版Grenade弹幕
                    (int)(Projectile.damage * 0.3), // 伤害倍率
                    0f,
                    Projectile.owner
                );

                // 设置弹幕属性
                if (grenadeProj.WithinBounds(Main.maxProjectiles))
                {
                    Projectile proj = Main.projectile[grenadeProj];
                    proj.friendly = true;
                    proj.hostile = false;
                    proj.tileCollide = false;
                    proj.penetrate = 1;
                    proj.localNPCHitCooldown = 1;
                    proj.usesLocalNPCImmunity = true;
                    proj.timeLeft = 90;
                }
            }

            Projectile.ExpandHitboxBy(32);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int j = 0; j < 5; j++)
            {
                int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 2f);
                Main.dust[fire].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[fire].scale = 0.5f;
                    Main.dust[fire].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int k = 0; k < 10; k++)
            {
                int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 3f);
                Main.dust[fire].noGravity = true;
                Main.dust[fire].velocity *= 5f;
                fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 2f);
                Main.dust[fire].velocity *= 2f;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Vector2 goreSource = Projectile.Center;
                int goreAmt = 3;
                Vector2 source = new Vector2(goreSource.X - 24f, goreSource.Y - 24f);
                for (int goreIndex = 0; goreIndex < goreAmt; goreIndex++)
                {
                    float velocityMult = 0.33f;
                    if (goreIndex < goreAmt / 3)
                    {
                        velocityMult = 0.66f;
                    }
                    if (goreIndex >= 2 * goreAmt / 3)
                    {
                        velocityMult = 1f;
                    }
                    //Mod mod = ModContent.GetInstance<CalamityMod>();
                    int type = Main.rand.Next(61, 64);
                    int smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    Gore gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y -= 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y -= 1f;
                }
            }

            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f, 0.1f);
                    int flames = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<TotalityFire>(), (int)(Projectile.damage * 0.3), 0f, Projectile.owner);
                    if (flames.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[flames].DamageType = DamageClass.Ranged;
                        Main.projectile[flames].penetrate = 3;
                        Main.projectile[flames].usesLocalNPCImmunity = false;
                        Main.projectile[flames].usesIDStaticNPCImmunity = true;
                        Main.projectile[flames].idStaticNPCHitCooldown = 10;
                    }
                }
            }
        }
    }
}
