
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

namespace FKsCRE.Content.Gel.BPrePlantera.CryonicGel
{
    public class CryonicGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsCryonicGelInfused = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<CryonicGel>())
            {
                IsCryonicGelInfused = true;
                projectile.netUpdate = true;
            }
            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsCryonicGelInfused && target.active && !target.friendly)
            {
                // 调整伤害为原来的 80%
                projectile.damage = (int)(projectile.damage * 0.8f);
                // 施加 CryonicGelEDebuff，持续 300 帧（5 秒）
                target.AddBuff(ModContent.BuffType<CryonicGelEDebuff>(), 300);
            }
        }

        public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
        {
            if (IsCryonicGelInfused)
            {
                foreach (Projectile enemyProjectile in Main.projectile)
                {
                    if (enemyProjectile.active && enemyProjectile.hostile && projectile.Hitbox.Intersects(enemyProjectile.Hitbox))
                    {
                        enemyProjectile.velocity *= 0.6f; // 减速 40%

                        // 如果需要 5 秒效果，则可以使用 AI 或额外逻辑追踪时间
                        enemyProjectile.localAI[0] = 300; // 标记剩余帧数
                    }
                }
            }
        }

        public override void AI(Projectile projectile)
        {
            foreach (Projectile enemyProjectile in Main.projectile)
            {
                if (enemyProjectile.active && enemyProjectile.hostile && enemyProjectile.localAI[0] > 0)
                {
                    enemyProjectile.localAI[0]--;
                    if (enemyProjectile.localAI[0] <= 0)
                    {
                        enemyProjectile.velocity /= 0.6f; // 恢复速度
                    }
                }
            }
        }
    }
}
