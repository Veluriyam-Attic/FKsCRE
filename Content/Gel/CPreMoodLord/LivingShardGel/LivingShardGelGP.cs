using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace FKsCRE.Content.Gel.CPreMoodLord.LivingShardGel
{
    internal class LivingShardGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsLivingShardGelInfused = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<LivingShardGel>())
            {
                IsLivingShardGelInfused = true;
                projectile.netUpdate = true;
            }
            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsLivingShardGelInfused && target.active && !target.friendly)
            {
                // 0.5% 概率释放 LivingShardGelHealPROJ
                if (Main.rand.NextFloat() <= 0.005f)
                {
                    Vector2 randomDirection = Main.rand.NextVector2CircularEdge(1f, 1f).SafeNormalize(Vector2.Zero) * 10f;
                    Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        projectile.Center,
                        randomDirection,
                        ModContent.ProjectileType<LivingShardGelHealPROJ>(),
                        (int)(projectile.damage * 2.5f), // 250% 伤害
                        projectile.knockBack,
                        projectile.owner
                    );
                }
            }
        }
    }
}