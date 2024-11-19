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
            // 如果至尊灾厄在这个世界上还没有被击败，同时这四个Boss（Thanatos、Artemis、Apollo、Ares）全部被击败
            if (!DownedBossSystem.downedCalamitas && DownedBossSystem.downedThanatos && DownedBossSystem.downedArtemisAndApollo && DownedBossSystem.downedAres)
            {
                // 获取玩家当前持有的武器和箭矢
                Item heldItem = Player.HeldItem;
                Item ammoItem = Player.inventory.FirstOrDefault(item => item.ammo == AmmoID.Arrow && item.stack > 0);

                // 检查玩家持有的武器是 HeavenlyGale，且箭矢是 MiracleMatterArrow
                if (heldItem != null && heldItem.type == ModContent.ItemType<HeavenlyGale>() && ammoItem != null && ammoItem.type == ModContent.ItemType<MiracleMatterArrow>())
                {
                    // 降低 ExoCrystalArrow 的伤害 75%
                    Player.GetDamage(DamageClass.Ranged) *= 0.25f;
                }
            }
        }
    }
}