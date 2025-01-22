﻿using System;
using System.Collections.Generic;
using System.Text;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;



namespace FKsCRE.Content.Arrows.APreHardMode.AerialiteArrow
{
    public class AerialiteArrow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Arrows.APreHardMode";
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true; // 弹药是消耗品
            Item.knockBack = 3.5f;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<AerialiteArrowPROJ>();
            Item.shootSpeed = 15f;
            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(200);
            recipe.AddIngredient(ItemID.WoodenArrow, 200);
            recipe.AddIngredient<AerialiteBar>();
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
