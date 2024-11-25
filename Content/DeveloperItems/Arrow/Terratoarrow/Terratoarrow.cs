using FKsCRE.Content.DeveloperItems.Arrow.TheDrill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Ammo;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;

namespace FKsCRE.Content.DeveloperItems.Arrow.Terratoarrow
{
    public class Terratoarrow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.Terratoarrow";
        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true; // 弹药是消耗品
            Item.knockBack = 3.5f;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
            Item.shoot = ModContent.ProjectileType<TerratoarrowPROJ>();
            Item.shootSpeed = 10f;
            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient<SproutingArrow>(333);
            recipe.AddIngredient<SproutingArrow>(333);
            recipe.AddIngredient<SproutingArrow>(333);
            recipe.AddIngredient<UelibloomBar>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }


    }
}
