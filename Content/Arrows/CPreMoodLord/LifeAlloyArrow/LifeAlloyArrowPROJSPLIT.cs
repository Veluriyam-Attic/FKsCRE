﻿using CalamityMod.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Ammo;
using CalamityMod;

namespace FKsCRE.Content.Arrows.CPreMoodLord.LifeAlloyArrow
{
    public class LifeAlloyArrowPROJSPLIT : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.CPreMoodLord";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        private Color currentColor = Color.Black;
        private int rotDirection = 1;
        private float rotIntensity;
        private bool rotPhase2 = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 500;
            Projectile.extraUpdates = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10 * Projectile.extraUpdates;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            // 增加局部计时器
            Projectile.localAI[0]++;

            // 初始化弹幕属性
            if (currentColor == Color.Black)
            {
                Projectile.scale = 0.025f;
                Projectile.alpha = 255;
                rotDirection = Main.rand.NextBool() ? 1 : -1; // 随机决定顺时针或逆时针
                rotIntensity = Main.rand.NextFloat(1f, 1f); // 保持固定角度步进（90度）
                Projectile.timeLeft = Main.rand.Next(250, 300 + 1);

                switch (Projectile.ai[2])
                {
                    case 4: // Yellow shot
                        currentColor = Color.Yellow * 0.65f;
                        break;
                    case 3: // Magenta shot
                        currentColor = Color.Magenta * 0.65f;
                        break;
                    case 2: // Red shot
                        currentColor = Color.Red * 0.65f;
                        break;
                    case 1: // Blue shot
                        currentColor = Color.Cyan * 0.65f;
                        break;
                    default: // Green shot
                        currentColor = Color.Lime * 0.65f;
                        break;
                }
            }

            // 每 20~90 帧拐 90 度
            if (Projectile.localAI[0] % Main.rand.Next(20, 91) == 0)
            {
                // 旋转 90 度，方向取决于 rotDirection
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2 * rotDirection);
            }

            // 速度减缓
            Projectile.velocity *= Main.rand.NextFloat(0.995f, 0.999f);
        }


        public override void OnKill(int timeLeft)
        {
            for (int b = 0; b < 2; b++)
            {
                
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 66, new Vector2(2, 2).RotatedByRandom(100) * Main.rand.NextFloat(0.2f, 1.5f));
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(0.5f, 1.1f);
                dust.color = currentColor;
                dust.fadeIn = 0;
                
                GlowOrbParticle orb = new GlowOrbParticle(Projectile.Center, new Vector2(2, 2).RotatedByRandom(100) * Main.rand.NextFloat(0.2f, 1.5f), false, 5, Main.rand.NextFloat(0.35f, 0.45f), currentColor, true, true);
                GeneralParticleHandler.SpawnParticle(orb);
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, 12, targetHitbox);
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle").Value;

            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], Color.Lerp(currentColor, Color.White, 0.15f), 1, texture);
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], Color.White with { A = 0 }, 1, texture2);
            //Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, texture.Size() * 0.5f, Projectile.scale * 1.5f, 0, 0f);
            return false;
        }
        public override bool? CanDamage() => Projectile.localAI[0] < 20 ? false : null;

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => modifiers.SourceDamage.Flat += HyperiusBullet.SplitBulletBonusDamage;
    }
}
