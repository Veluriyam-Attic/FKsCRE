using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.DeveloperItems.Weapon.Pyroblast
{
    public class DPYROItem : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.Pyroblast";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true; // 饰品
            Item.rare = 2; // 稀有度
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 打开 DPS 翻倍开关
            player.GetModPlayer<DPYROPlayer>().dpsBoostActive = true;
        }
    }
}
