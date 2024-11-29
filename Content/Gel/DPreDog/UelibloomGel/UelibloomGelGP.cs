using FKsCRE.Content.Gel.EAfterDog.MiracleMatterGel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace FKsCRE.Content.Gel.DPreDog.UelibloomGel
{
    public class UelibloomGelGP : GlobalProjectile
    {
        // 每个弹幕是否拥有独立的实例属性
        public override bool InstancePerEntity => true;

        // 表示该弹幕是否附魔了 CosmosGel 的标志
        public bool IsCosmosGelInfused = false;

        /// <summary>
        /// 在弹幕生成时调用。
        /// 如果弹幕是通过消耗 CosmosGel 弹药生成的，则将其附魔。
        /// </summary>
        /// <param name="projectile">生成的弹幕实例</param>
        /// <param name="source">生成来源信息</param>
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            // 检查弹幕是否由含有弹药的武器生成，且弹药类型为 CosmosGel
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<UelibloomGel>())
            {
                // 标记弹幕为已附魔
                IsCosmosGelInfused = true;

                // 确保在多人游戏中状态同步
                projectile.netUpdate = true;
            }
            base.OnSpawn(projectile, source);
        }

        /// <summary>
        /// 当弹幕击中 NPC 时调用。
        /// 如果弹幕被附魔，则为目标施加特殊 Buff 和粒子效果。
        /// </summary>
        /// <param name="projectile">当前弹幕</param>
        /// <param name="target">被击中的 NPC</param>
        /// <param name="hit">击中信息</param>
        /// <param name="damageDone">造成的伤害</param>
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 检查弹幕是否附魔，目标是否为非友方 NPC 且仍存活
            if (IsCosmosGelInfused && target.active && !target.friendly)
            {
                // 为目标添加 x，持续 300 帧（约 5 秒）
                //target.AddBuff(ModContent.BuffType<XXX>(), 300);
            }
        }

        /// <summary>
        /// 修改弹幕的伤害判定时调用（暂未使用）。
        /// </summary>
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(projectile, target, ref modifiers);
        }

        /// <summary>
        /// 修改弹幕的命中区域（Hitbox）时调用（暂未使用）。
        /// </summary>
        public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
        {
            base.ModifyDamageHitbox(projectile, ref hitbox);
        }

        /// <summary>
        /// 当弹幕被销毁时调用（例如，撞击墙壁或达到了最大寿命）。
        /// </summary>
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            base.OnKill(projectile, timeLeft);
        }
    }
}

