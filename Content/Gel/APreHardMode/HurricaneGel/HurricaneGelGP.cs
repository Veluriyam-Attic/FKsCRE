using FKsCRE.Content.Gel.CPreMoodLord.AstralGel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Ranged;

namespace FKsCRE.Content.Gel.APreHardMode.HurricaneGel
{
    public class HurricaneGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsHurricaneGelInfused = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<HurricaneGel>())
            {
                IsHurricaneGelInfused = true;
                projectile.netUpdate = true;
            }
            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsHurricaneGelInfused && target.active && !target.friendly)
            {
                // 随机生成 1~2 个额外弹幕
                int extraProjectiles = Main.rand.Next(1, 3);
                for (int i = 0; i < extraProjectiles; i++)
                {
                    float randomAngle = MathHelper.ToRadians(Main.rand.Next(1, 3)); // 随机角度
                    Vector2 velocity = projectile.velocity.RotatedBy(randomAngle);
                    Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        projectile.Center,
                        velocity,
                        ModContent.ProjectileType<Aquashard>(),
                        (int)(projectile.damage * 0.25f), // 伤害为原来的 25%
                        projectile.knockBack,
                        projectile.owner
                    );
                }
            }
        }
    }
}