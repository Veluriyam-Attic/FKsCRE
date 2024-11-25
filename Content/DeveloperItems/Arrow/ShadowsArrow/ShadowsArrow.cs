using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using FKsCRE.Content.DeveloperItems;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;

namespace FKsCRE.Content.DeveloperItems.Arrow.ShadowsArrow
{
    public class ShadowsArrow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.ShadowsArrow";
        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.knockBack = 3.5f;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
            Item.shoot = ModContent.ProjectileType<ShadowsArrowPROJ>();
            Item.shootSpeed = 7f;
            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.WoodenArrow, 1); // 配方示例
            recipe.AddIngredient<ShadowspecBar>(5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
