﻿using CalamityMod.Items.Materials;
using FKsCRE.Content.Arrows.APreHardMode.AerialiteArrow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace FKsCRE.Content.Arrows.APreHardMode.WulfrimArrow
{
    internal class WulfrimArrow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Arrows.APreHardMode";
        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true; // 弹药是消耗品
            Item.knockBack = 3.5f;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<WulfrimArrowPROJ>();
            Item.shootSpeed = 15f;
            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(200);
            recipe.AddIngredient(ItemID.WoodenArrow, 200);
            recipe.AddIngredient<WulfrumMetalScrap>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
