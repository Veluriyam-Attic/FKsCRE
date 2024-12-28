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

namespace FKsCRE.Content.Gel.CPreMoodLord.LifeAlloyGel
{
    internal class LifeAlloyGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsLifeAlloyGelInfused = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<LifeAlloyGel>())
            {
                IsLifeAlloyGelInfused = true;
                projectile.netUpdate = true;
            }
            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsLifeAlloyGelInfused && target.active && !target.friendly)
            {
                // 减少伤害到 75%
                projectile.damage = (int)(projectile.damage * 0.75f);

                // 随机生成 0 到 4 个 HyperiusSplit 弹幕
                int splitCount = Main.rand.Next(0, 5);
                for (int b = 0; b < splitCount; b++)
                {
                    Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        projectile.Center,
                        projectile.velocity.RotatedByRandom(0.5f) * 0.2f,
                        ModContent.ProjectileType<HyperiusSplit>(),
                        (int)(projectile.damage * 0.11f), // 0.11 倍伤害
                        0f,
                        projectile.owner,
                        ai0: 0f,
                        ai1: 0f,
                        ai2: Main.rand.Next(0, 5) // 随机生成 0 到 4 的数值，决定颜色
                    );
                }
            }
        }
    }
}
