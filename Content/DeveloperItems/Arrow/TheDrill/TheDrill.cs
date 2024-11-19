using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Tools;

namespace FKsCRE.Content.DeveloperItems.Arrow.TheDrill
{
    public class TheDrill : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.TheDrill";
        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 1;
            Item.consumable = false; // 弹药是消耗品
            Item.knockBack = 3.5f;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<TheDrillPROJ>();
            Item.shootSpeed = 15f;
            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.DrillContainmentUnit, 1); // 钻头控制装置
            recipe.AddIngredient(ItemID.WoodenArrow, 3996);
            //recipe.AddIngredient<WulfrumDiggingTurtle>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}
