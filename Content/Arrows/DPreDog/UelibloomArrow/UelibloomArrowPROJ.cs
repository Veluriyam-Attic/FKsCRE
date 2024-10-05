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
using CalamityMod.Particles;
using CalamityMod;
using FKsCRE.CREConfigs;
using CalamityMod.Items.Accessories;

namespace FKsCRE.Content.Arrows.DPreDog.UelibloomArrow
{
    internal class UelibloomArrowPROJ : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.extraUpdates = 2;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 14; // 无敌帧冷却时间为14帧
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor, 1);
                return false;
            }
            return true;
        }

        public override void AI()
        {
            Projectile.velocity.Y *= 0.997f;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 飞行时的烟雾
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] > 6f)
                {
                    for (int d = 0; d < 5; d++)
                    {
                        Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1f)];
                        dust.velocity = Vector2.Zero;
                        dust.position -= Projectile.velocity / 5f * d;
                        dust.noGravity = true;
                        dust.scale = 0.65f;
                        dust.noLight = true;
                    }
                }
            }              
        }


        public override void OnKill(int timeLeft)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 召唤烟雾
                int Dusts = 15;
                float radians = MathHelper.TwoPi / Dusts;
                Vector2 spinningPoint = Vector2.Normalize(new Vector2(-1f, -1f));
                for (int i = 0; i < Dusts; i++)
                {
                    Vector2 dustVelocity = spinningPoint.RotatedBy(radians * i).RotatedBy(0.5f) * 6.5f;
                    Particle smoke = new HeavySmokeParticle(Projectile.Center, dustVelocity * Main.rand.NextFloat(1f, 2.6f), Color.DarkGreen, 18, Main.rand.NextFloat(0.9f, 1.6f), 0.35f, Main.rand.NextFloat(-1, 1), true);
                    GeneralParticleHandler.SpawnParticle(smoke);
                }
            }
              

            // 召唤藤蔓
            // if (Main.rand.NextFloat() <= 0.5f) // 如果想要让他有概率召唤藤蔓
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<UelibloomArrowVine>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            }

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {

                // 召唤生命之光
                Vector2 direction = -Projectile.velocity.SafeNormalize(Vector2.UnitX); // 反向的方向，即弹幕的反向

                Vector2 spawnPosition = Projectile.Center;
                Vector2 velocity = direction * 10f; // 速度为反方向，力度设置为10f

                // 只生成一个UelibloomArrowLight
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPosition, velocity, ModContent.ProjectileType<UelibloomArrowLight>(), 0, 0, Projectile.owner);

                // 生成深绿色粒子特效
                int Dusts = 8;
                float radians = MathHelper.TwoPi / Dusts;
                Vector2 spinningPoint = Vector2.Normalize(new Vector2(-1f, -1f));
                for (int d = 0; d < Dusts; d++)
                {
                    Vector2 dustVelocity = spinningPoint.RotatedBy(radians * d) * 3.5f;
                    Dust dust = Dust.NewDustPerfect(spawnPosition, DustID.GreenFairy, dustVelocity, 0); // 使用绿色相关的DustID
                    dust.color = Color.DarkGreen; // 强制设置为深绿色
                    dust.noGravity = true;

                    Dust dust2 = Dust.NewDustPerfect(spawnPosition, DustID.GreenFairy, dustVelocity * 0.6f, 0); // 同样使用绿色的DustID
                    dust2.color = Color.DarkGreen; // 强制设置为深绿色
                    dust2.noGravity = true;
                }
            }
            //// 生成深绿色粒子特效
            //Dusts = 8;
            //radians = MathHelper.TwoPi / Dusts;
            //spinningPoint = Vector2.Normalize(new Vector2(-1f, -1f));
            //for (int d = 0; d < Dusts; d++)
            //{
            //    Vector2 dustVelocity = spinningPoint.RotatedBy(radians * d) * 3.5f;
            //    Dust dust = Dust.NewDustPerfect(spawnPosition, Main.rand.NextBool() ? 262 : 87, dustVelocity, 0, Color.DarkGreen, 0.9f);
            //    dust.noGravity = true;

            //    Dust dust2 = Dust.NewDustPerfect(spawnPosition, Main.rand.NextBool() ? 262 : 87, dustVelocity * 0.6f, 0, Color.DarkGreen, 1.2f);
            //    dust2.noGravity = true;
            //}







            //if (Main.rand.NextFloat() <= 0.2f)
            //{
            //    for (int i = 0; i < 3; i++)
            //    {
            //        Vector2 spawnPos = new Vector2(Main.rand.NextFloat(-Main.screenWidth, Main.screenWidth), Main.rand.NextFloat(-Main.screenHeight, Main.screenHeight));
            //        Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, Projectile.velocity, ModContent.ProjectileType<UelibloomArrowLight>(), 0, 0, Projectile.owner);
            //    }
            //}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //if (Main.rand.NextFloat() <= 0.2f)
            //{
            //    for (int i = 0; i < 3; i++)
            //    {
            //        Vector2 spawnPos = new Vector2(Main.rand.NextFloat(-Main.screenWidth, Main.screenWidth), Main.rand.NextFloat(-Main.screenHeight, Main.screenHeight));
            //        Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, Projectile.velocity, ModContent.ProjectileType<UelibloomArrowLight>(), 0, 0, Projectile.owner);
            //    }
            //}

            target.AddBuff(ModContent.BuffType<UelibloomArrowEBuff>(), 300);  // 5 seconds
        }
    }
}