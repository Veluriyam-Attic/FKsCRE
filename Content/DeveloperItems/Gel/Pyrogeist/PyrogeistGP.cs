using CalamityMod.Projectiles.Ranged;
using FKsCRE.Content.Gel.APreHardMode.HurricaneGel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using FKsCRE.Content.Gel.EAfterDog.AuricGel;

namespace FKsCRE.Content.DeveloperItems.Gel.Pyrogeist
{
    internal class PyrogeistGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsPyrogeistInfused = false;


        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<Pyrogeist>())
            {
                IsPyrogeistInfused = true;
                projectile.netUpdate = true;

                // 20% 概率生成 PyrogeistPROJ
                if (Main.rand.NextFloat() < 0.2f)
                {
                    Player player = Main.player[projectile.owner];
                    Vector2 spawnPosition = player.Center;
                    Vector2 direction = Main.MouseWorld - player.Center;
                    direction.Normalize();
                    direction *= 12f;

                    int pyrogeistProj = Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        spawnPosition,
                        direction,
                        ModContent.ProjectileType<PyrogeistPROJ>(),
                        projectile.damage, // 使用当前弹幕的伤害值
                        0f,
                        projectile.owner
                    );

                    // 设置 PyrogeistPROJ 的属性
                    Projectile proj = Main.projectile[pyrogeistProj];
                    proj.friendly = true;
                    proj.hostile = false;
                    proj.penetrate = -1;
                    proj.localNPCHitCooldown = 60;
                    proj.usesLocalNPCImmunity = true;
                    proj.DamageType = DamageClass.Ranged; // 设置为远程伤害类型
                }
            }

            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsPyrogeistInfused && target.active && !target.friendly)
            {
              
            }
        }
    }
}