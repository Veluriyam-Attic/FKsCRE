﻿using System;
using System.Collections.Generic;
using System.Text;
using CalamityMod.Items;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Weapons.Magic;

namespace FKsCRE.Content.WeaponToAMMO.Arrow.PlasmaDriveCorePrototypeArrow
{
    public class PlasmaDriveCorePrototypeArrow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "WeaponToAMMO.Arrow.PlasmaDriveCorePrototypeArrow";
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.knockBack = 3.5f;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
            Item.shoot = ModContent.ProjectileType<PlasmaDriveCorePrototypeArrowPROJ>();
            Item.shootSpeed = PlasmaDriveCorePrototypeArrowPROJ.InitialSpeed;
            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient<SHPC>(1);
            //recipe.AddCondition(Condition.InSpace);
            recipe.AddCondition(Condition.NearShimmer);
            //recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
