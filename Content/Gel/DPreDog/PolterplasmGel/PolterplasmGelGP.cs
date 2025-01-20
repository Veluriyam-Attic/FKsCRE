using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;

namespace FKsCRE.Content.Gel.DPreDog.PolterplasmGel
{
    internal class PolterplasmGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsPolterplasmGelInfused = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<PolterplasmGel>())
            {
                IsPolterplasmGelInfused = true;
                projectile.netUpdate = true;
                projectile.damage = (int)(projectile.damage * 0.8f); // 减少 20% 伤害
            }
            base.OnSpawn(projectile, source);
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsPolterplasmGelInfused && target.active && !target.friendly)
            {
                // 调整伤害为原来的 80%
                //projectile.damage = (int)(projectile.damage * 0.8f);
            }
        }
        public override void AI(Projectile projectile)
        {
            if (IsPolterplasmGelInfused)
            {
                // 前 30 帧不追踪，之后开始追踪敌人
                if (projectile.ai[1] > 30)
                {
                    NPC target = projectile.Center.ClosestNPCAt(150); // 查找 x 范围内最近的敌人，范围很小，因此是弱追踪
                    if (target != null)
                    {
                        Vector2 direction = (target.Center - projectile.Center).SafeNormalize(Vector2.Zero);
                        projectile.velocity = Vector2.Lerp(projectile.velocity, direction * 12f, 0.08f); // 追踪速度
                    }
                }
                else
                {
                    projectile.ai[1]++;
                }
            }
        }
    }
}
