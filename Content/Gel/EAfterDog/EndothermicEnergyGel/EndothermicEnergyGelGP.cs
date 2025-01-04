using FKsCRE.Content.DeveloperItems.Gel.Pyrogeist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace FKsCRE.Content.Gel.EAfterDog.EndothermicEnergyGel
{
    internal class EndothermicEnergyGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsEndothermicEnergyGelInfused = false;


        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<EndothermicEnergyGel>())
            {
                IsEndothermicEnergyGelInfused = true;
                projectile.timeLeft *= 2; // 弹幕寿命翻倍
                projectile.extraUpdates += 1; // 更新次数增加 1
                projectile.damage = (int)(projectile.damage * 1.25f); // 伤害变为原来的 1.25 倍
                projectile.penetrate = 1; // 穿透次数设为 1
                projectile.netUpdate = true;
            }

            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsEndothermicEnergyGelInfused && target.active && !target.friendly)
            {

            }
        }
    }
}