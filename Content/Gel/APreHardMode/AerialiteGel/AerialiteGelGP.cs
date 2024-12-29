using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace FKsCRE.Content.Gel.APreHardMode.AerialiteGel
{
    internal class AerialiteGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsAerialiteGelInfused = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            // 检查弹幕是否由含有弹药的武器生成，且弹药类型为 AerialiteGel
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<AerialiteGel>())
            {
                // 标记弹幕为已附魔
                IsAerialiteGelInfused = true;

                // 确保在多人游戏中状态同步
                projectile.netUpdate = true;
            }
            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 检查弹幕是否附魔，目标是否为非友方 NPC 且仍存活
            if (IsAerialiteGelInfused && target.active && !target.friendly)
            {
                // 为目标添加 x，持续 300 帧（约 5 秒）
                //target.AddBuff(ModContent.BuffType<XXX>(), 300);
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(projectile, target, ref modifiers);
        }


        public override void OnKill(Projectile projectile, int timeLeft)
        {
            base.OnKill(projectile, timeLeft);
        }
    }
}

