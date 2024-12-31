using CalamityMod.Items;
using CalamityMod.Rarities;
using FKsCRE.Content.DeveloperItems.Arrow.MaoMaoChong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Ranged;

namespace FKsCRE.Content.DeveloperItems.Arrow.MarksmanArrow
{
    internal class MarksmanArrow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.MarksmanArrow";
        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true; // 弹药是消耗品
            Item.knockBack = 3.5f;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().donorItem = true;
            Item.shoot = ModContent.ProjectileType<MarksmanArrowPROJ>();
            Item.shootSpeed = 1f;
            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient<MidasPrime>(1);
            recipe.AddCondition(Condition.NearShimmer);
            //recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
