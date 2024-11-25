using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.ID;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs.ExoMechs; // 引用 CalamityMod

namespace FKsCRE.Content.Arrows.EAfterDog.MiracleMatterArrow
{
    public class MiracleMatterArrowPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            // 如果至尊灾厄在这个世界上还没有被击败，同时机器人组合ExoMechs全部被击败
            // if (!DownedBossSystem.downedCalamitas && DownedBossSystem.downedExoMechs)
            if (!DownedBossSystem.downedCalamitas)
            {
                // 获取玩家当前持有的武器和箭矢
                Item heldItem = Player.HeldItem;

                // 检查玩家是否持有 HeavenlyGale 武器，并且背包中有 MiracleMatterArrow 弹药
                if (heldItem != null && heldItem.type == ModContent.ItemType<HeavenlyGale>() &&
                    Player.inventory.Any(item => item.type == ModContent.ItemType<MiracleMatterArrow>() && item.stack > 0))
                {
                    // 降低远程伤害为 25%
                    Player.GetDamage(DamageClass.Ranged) *= 0.25f;
                }
            }
        }
    }
}