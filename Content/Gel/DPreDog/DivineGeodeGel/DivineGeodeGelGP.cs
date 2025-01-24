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
using CalamityMod.Projectiles.Ranged;
using FKsCRE.Content.DeveloperItems.Weapon.TheGoldenFire;


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
                projectile.damage = (int)(projectile.damage * 0.95f); // 减少 5% 伤害
            }
            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsDivineGeodeGelInfused && target.active && !target.friendly)
            {
                // 施加 HolyFlames Buff，持续 1200 帧（20 秒）
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 1200);

                // 检查场上 BlissfulBombardierDustProjectile 数量
                int existingDustCount = 0;
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active && proj.type == ModContent.ProjectileType<BlissfulBombardierDustProjectile>())
                    {
                        existingDustCount++;
                        if (existingDustCount >= 5)
                        {
                            return; // 如果数量达到或超过 5，则不生成新的弹幕
                        }
                    }
                }

                // 在原地生成 BlissfulBombardierDustProjectile
                int blissfulProjectile = Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    projectile.Center,
                    Vector2.Zero, // 无速度
                    ModContent.ProjectileType<BlissfulBombardierDustProjectile>(),
                    (int)(projectile.damage / 0.25 * 0.05f), // 伤害为原始的 5%
                    0f,
                    projectile.owner
                );

                // 设置弹幕的无敌帧属性
                if (Main.projectile.IndexInRange(blissfulProjectile))
                {
                    Projectile proj = Main.projectile[blissfulProjectile];
                    proj.friendly = true; // 弹幕对敌人友好
                    proj.hostile = false; // 弹幕对玩家无害
                    proj.localNPCHitCooldown = 35; // 每个NPC本地无敌帧为 X 帧
                    proj.usesLocalNPCImmunity = true; // 启用本地无敌帧机制
                }

            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (IsDivineGeodeGelInfused && target.active && !target.friendly)
            {
                // 对所有其他弹幕默认保持伤害不变
                modifiers.FinalDamage *= 1f;

                // 判断弹幕类型，并执行对应逻辑
                switch (projectile.type)
                {
                    case int type when type == ModContent.ProjectileType<EssenceFire>():
                        modifiers.FinalDamage /= 0.75f; // 每次击中后伤害 /0.75
                        break;

                    case int type when type == ModContent.ProjectileType<DragonsBreathFlames>():
                        modifiers.FinalDamage /= 0.75f; // 每次击中后伤害 /0.75
                        break;

                    case int type when type == ModContent.ProjectileType<ExoFire>():
                        modifiers.FinalDamage *= 1f; // 明确保持伤害为 100%
                        break;

                    case int type when type == ModContent.ProjectileType<TheGoldenFirePROJ>():
                        modifiers.FinalDamage *= 1f; // 明确保持伤害为 100%
                        break;

                    default:
                        // 对于其他弹幕，不进行修改
                        break;
                }
            }
        }











    }
}
