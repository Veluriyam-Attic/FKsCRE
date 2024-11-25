using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.Gel.EAfterDog.CosmosGel
{
    public class CosmosGelGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool IsCosmosGelInfused = false;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsCosmosGelInfused && target.active && !target.friendly)
            {
                // 给予敌人 CosmosGelEDebuff
                target.AddBuff(ModContent.BuffType<CosmosGelEDebuff>(), 60);
            }
        }
    }
}
