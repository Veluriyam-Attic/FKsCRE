﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Typeless;

namespace FKsCRE.Content.Gel.DPreDog.DivineGeodeGel
{
    internal class DivineGeodeGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsDivineGeodeGelInfused = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<DivineGeodeGel>())
            {
                IsDivineGeodeGelInfused = true;
                projectile.penetrate = -1; // 设置弹幕无限穿透
                projectile.netUpdate = true;
            }
            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsDivineGeodeGelInfused && target.active && !target.friendly)
            {
                // 调整伤害为原来的 25%
                projectile.damage = (int)(projectile.damage * 0.25f);

                // 施加 HolyFlames Buff，持续 1200 帧（20 秒）
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 1200);

                // 在原地生成 BlissfulBombardierDustProjectile
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    projectile.Center,
                    Vector2.Zero, // 无速度
                    ModContent.ProjectileType<BlissfulBombardierDustProjectile>(),
                    (int)(projectile.damage * 0.05f), // 伤害为原始的 5%
                    0f,
                    projectile.owner
                );
            }
        }
    }
}