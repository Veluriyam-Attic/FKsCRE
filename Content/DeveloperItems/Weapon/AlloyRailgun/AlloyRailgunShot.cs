﻿using CalamityMod.Particles;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.DeveloperItems.Weapon.AlloyRailgun
{
    public class AlloyRailgunShot : BaseLaserbeamProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Magic/YharimsCrystalBeam";
        public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayStart", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayMid", AssetRequestMode.ImmediateLoad).Value;
        public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayEnd", AssetRequestMode.ImmediateLoad).Value;
        //public override float MaxScale => 1.5f * ChargePercent;
        public override float MaxScale => 1.95f;
        public override float Lifetime => 15f;
        public override float MaxLaserLength => 2200f;
        public ref float ChargePercent => ref Projectile.ai[1];
        public override Color LaserOverlayColor => Color.White;
        public override Color LightCastColor => LaserOverlayColor;

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.scale = MaxScale;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();


            // 每10帧生成浅粉色椭圆形专有粒子特效
            if (Projectile.ai[0] % 10 == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Particle pulse = new DirectionalPulseRing(Projectile.Center, Projectile.velocity * 0.75f, Color.BlueViolet, new Vector2(1f, 2.5f), Projectile.rotation - MathHelper.PiOver4, 0.2f, 0.03f, 20);
                    GeneralParticleHandler.SpawnParticle(pulse);
                }
            }

            base.AI();
        }
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i <= 55; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.BlueTorch, Projectile.velocity);
                dust.scale = Main.rand.NextFloat(1.6f, 2.5f);
                dust.velocity = Projectile.velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.3f, 1.6f);
                dust.noGravity = true;
            }
        }
        public override void ExtraBehavior()
        {
            if (Projectile.timeLeft == 15)
            {

                Vector2 beamVector = Projectile.velocity;
                float beamLength = DetermineLaserLength_CollideWithTiles(12);

                //Rapid dust
                int dustCount = Main.rand.Next(10, 30);
                for (int i = 0; i < dustCount; i++)
                {
                    float dustProgressAlongBeam = beamLength * Main.rand.NextFloat(0f, 0.8f);
                    Vector2 dustPosition = Projectile.Center + dustProgressAlongBeam * beamVector + beamVector.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-6f, 6f) * Projectile.scale;

                    Dust dust = Dust.NewDustPerfect(dustPosition, 187, beamVector * Main.rand.NextFloat(5f, 26f), 0, Color.White, 2.2f);
                    dust.noGravity = true;
                }

                //Put a titanium shell into the impact tile (if it collided with one)
                if (beamLength < MaxLaserLength)
                {
                    Vector2 endPoint = beamLength * beamVector + Projectile.Center + beamVector * 8.5f;
                    Point anchorPos = new Point((int)endPoint.X / 16, (int)endPoint.Y / 16);

                    Color burnColor = Main.rand.NextBool(4) ? Color.PaleGreen : Main.rand.NextBool(4) ? Color.PaleTurquoise : Color.OrangeRed;
                    Particle shell = new TitaniumRailgunShell(endPoint, anchorPos, Projectile.rotation + MathHelper.PiOver2, burnColor);
                    GeneralParticleHandler.SpawnParticle(shell);
                }
            }
        }

        public override void DetermineScale() => Projectile.scale = Projectile.timeLeft / Lifetime * MaxScale;

        public override float DetermineLaserLength() => DetermineLaserLength_CollideWithTiles(5);

        public override bool ShouldUpdatePosition() => false;

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * LaserLength, Projectile.width + 16, DelegateMethods.CutTiles);
        }
    }
}
