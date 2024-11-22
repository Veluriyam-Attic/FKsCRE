using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition.BPrePlantera.StarblightSootBullet
{
    internal class StarblightSootBulletGlobalNPC : GlobalNPC
    {
        public bool MarkedByBullet; // 是否被标记

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            MarkedByBullet = false; // 每帧重置
        }

        public override void OnKill(NPC npc)
        {
            base.OnKill(npc);

            // 如果被标记且不是 Boss
            if (MarkedByBullet && !npc.boss)
            {
                Projectile.NewProjectile(
                    npc.GetSource_Death(),
                    npc.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<StarblightSootBulletArea>(),
                    0, // 不直接造成伤害
                    0,
                    Main.myPlayer
                );
            }
        }


    }
}
