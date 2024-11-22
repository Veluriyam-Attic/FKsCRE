using CalamityMod;
using FKsCRE.Content.Ammunition.DPreDog.UelibloomBullet;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Particles;

namespace FKsCRE.Content.Ammunition.BPrePlantera.StarblightSootBullet
{
    internal class StarblightSootBulletPROJ : ModProjectile, ILocalizedModType
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

            // 飞行粒子特效
            if (Main.rand.NextBool(5))
            {
                Vector2 position = Projectile.Center + Main.rand.NextVector2Circular(5f, 5f);
                GeneralParticleHandler.SpawnParticle(new DirectionalPulseRing(position, Vector2.Zero, Color.DarkTurquoise, new Vector2(1f, 1f), 0f, 0.1f, 0f, 20));
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            // 击中敌人释放更多粒子特效
            for (int i = 0; i < 10; i++) // 更快更密集的粒子
            {
                Vector2 position = target.Center + Main.rand.NextVector2Circular(target.width / 2, target.height / 2);
                GeneralParticleHandler.SpawnParticle(new DirectionalPulseRing(position, Vector2.Zero, Color.Coral, new Vector2(1.5f, 1.5f), 0f, 0.2f, 0f, 30));
            }

            // 给 GlobalNPC 添加标记
            if (!target.boss && target.TryGetGlobalNPC<StarblightSootBulletGlobalNPC>(out var modNPC))
            {
                modNPC.MarkedByBullet = true;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }

        public override void OnKill(int timeLeft)
        {

        }
    }
}
