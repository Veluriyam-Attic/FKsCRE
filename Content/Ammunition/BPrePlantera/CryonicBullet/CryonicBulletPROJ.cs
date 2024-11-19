using CalamityMod.Items.Ammo;
using CalamityMod.Particles;
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
using CalamityMod;
using CalamityMod.Items.Ammo;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace FKsCRE.Content.Ammunition.BPrePlantera.CryonicBullet
{
    public class CryonicBulletPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.BPrePlantera";
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
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.MaxUpdates = 5;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Lighting.AddLight(Projectile.Center, Color.Lerp(Color.Blue, Color.AliceBlue, 0.5f).ToVector3() * 0.49f);

            // Projectile becomes visible after a few frames
            if (Projectile.timeLeft == 298)
                Projectile.alpha = 0;

            // Once projectile is visible, spawn trailing sparkles
            if (Projectile.timeLeft <= 298 && Main.rand.NextBool(5))
            {
                int idx = Dust.NewDust(Projectile.Center, 1, 1, DustID.GoldFlame, 0f, 0f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].noLight = true;
                Main.dust[idx].position = Projectile.Center;
                Main.dust[idx].velocity = Vector2.Zero;
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

        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            GenericSparkle impactParticle = new GenericSparkle(Projectile.Center, Vector2.Zero, Color.Goldenrod, Color.White, Main.rand.NextFloat(0.7f, 1.2f), 12);
            GeneralParticleHandler.SpawnParticle(impactParticle);
        }
    }
}
