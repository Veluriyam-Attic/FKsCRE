
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace FKsCRE.Content.Gel.EAfterDog.MiracleMatterGel
{
    public class MiracleMatterGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsMiracleMatterGelInfused = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<MiracleMatterGel>())
            {
                IsMiracleMatterGelInfused = true;
                //projectile.damage = (int)(projectile.damage * 0.05f); // 减少 95% 伤害
                projectile.netUpdate = true;
            }
            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsMiracleMatterGelInfused && target.active && !target.friendly)
            {
                // 检查场上是否已存在 MiracleMatterGelLighting 弹幕
                bool lightningExists = Main.projectile.Any(p => p.active && p.type == ModContent.ProjectileType<MiracleMatterGelLighting>());

                if (!lightningExists)
                {
                    // 召唤一条闪电
                    Vector2 lightningSpawnPosition = projectile.Center - Vector2.UnitY * Main.rand.NextFloat(960f, 1020f);
                    Vector2 lightningShootVelocity = (projectile.Center - lightningSpawnPosition).SafeNormalize(Vector2.UnitY) * 14f;

                    int lightning = Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        lightningSpawnPosition,
                        lightningShootVelocity,
                        ModContent.ProjectileType<MiracleMatterGelLighting>(),
                        projectile.damage * 15, // 1500% 伤害
                        0f,
                        projectile.owner
                    );

                    if (Main.projectile.IndexInRange(lightning))
                    {
                        Main.projectile[lightning].ai[0] = lightningShootVelocity.ToRotation();
                        Main.projectile[lightning].ai[1] = Main.rand.Next(100);
                    }

                    // 播放音效
                    SoundEngine.PlaySound(SoundID.Item92, projectile.position);
                }

                // 调整伤害为原来的 5%（后调整）
                projectile.damage = (int)(projectile.damage * 0.05f);
            }
        }
    }
}
