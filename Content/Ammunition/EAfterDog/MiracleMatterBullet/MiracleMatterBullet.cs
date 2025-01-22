﻿using CalamityMod.Items.Materials;
using FKsCRE.Content.Ammunition.BPrePlantera.CryonicBullet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Tiles.Furniture.CraftingStations;
using FKsCRE.Content.Ammunition.APreHardMode.TinkleshardBullet;
using CalamityMod.Items.Ammo;

namespace FKsCRE.Content.Ammunition.EAfterDog.MiracleMatterBullet
{
    public class MiracleMatterBullet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Ammunition.EAfterDog";
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 18;
            Item.damage = 38;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<MiracleMatterBulletPROJ>();
            Item.shootSpeed = 22f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3996);
            recipe.AddIngredient<TinkleshardBullet>(999);
            recipe.AddIngredient<CryonicBullet>(999);
            recipe.AddIngredient<HyperiusBullet>(999);
            recipe.AddIngredient<GodSlayerSlug>(999);
            recipe.AddIngredient<MiracleMatter>(1);
            recipe.AddTile<DraedonsForge>();
            recipe.Register();
        }
    }
}
