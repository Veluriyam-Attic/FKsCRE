﻿using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using FKsCRE.Content.DeveloperItems.Bullet.ChineseChess.Pao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace FKsCRE.Content.DeveloperItems.Bullet.ChineseChess.Shi
{
    internal class Shi : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.ChineseChess.Shi";
        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true; // 弹药是消耗品
            Item.knockBack = 3.5f;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
            Item.shoot = ModContent.ProjectileType<ShiPROJ>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Bullet; // 这是子弹类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe(333);
            recipe1.AddIngredient(ItemID.AlphabetStatueS, 1);
            recipe1.AddIngredient(ItemID.AlphabetStatueH, 1);
            recipe1.AddIngredient(ItemID.AlphabetStatueI, 1);
            recipe1.AddIngredient(ItemID.CobaltBar, 1);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe(333);
            recipe2.AddIngredient(ItemID.AlphabetStatueS, 1);
            recipe2.AddIngredient(ItemID.AlphabetStatueH, 1);
            recipe2.AddIngredient(ItemID.AlphabetStatueI, 1);
            recipe2.AddIngredient(ItemID.PalladiumBar, 1);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }

    }
}