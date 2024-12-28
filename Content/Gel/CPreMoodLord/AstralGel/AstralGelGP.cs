using FKsCRE.Content.Gel.DPreDog.UelibloomGel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;

namespace FKsCRE.Content.Gel.CPreMoodLord.AstralGel
{
    public class AstralGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsAstralGelInfused = false;

        private int staticTimer = 0; // 计时器，用于控制静止状态
        private bool isStatic = false; // 是否处于静止状态

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<AstralGel>())
            {
                IsAstralGelInfused = true;
                projectile.netUpdate = true;
            }
            base.OnSpawn(projectile, source);
        }

        public override void AI(Projectile projectile)
        {
            if (IsAstralGelInfused)
            {
                staticTimer++;

                if (staticTimer == 105) // 1.75 秒后（60 帧/秒）
                {
                    projectile.velocity = Vector2.Zero; // 强制静止
                    isStatic = true;
                }

                if (isStatic && staticTimer >= 225) // 静止 2 秒后（225 = 105 + 120）
                {
                    projectile.Kill(); // 强制销毁弹幕
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsAstralGelInfused && target.active && !target.friendly)
            {
                // 伤害调整
                if (isStatic)
                {
                    damageDone = (int)(damageDone * 0.85f * 0.25f); // 静止状态期间伤害 *85% *25%
                }
                else
                {
                    damageDone = (int)(damageDone * 0.85f); // 普通伤害 *85%
                }

                // 添加 AstralInfectionDebuff，持续 300 帧（5 秒）
                target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
            }
        }
    }
}

