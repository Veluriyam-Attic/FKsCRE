﻿using CalamityMod.Items.Materials;
using FKsCRE.Content.Ammunition.APreHardMode.WulfrimBullet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Placeables;

namespace FKsCRE.Content.Ammunition.APreHardMode.TinkleshardBullet
{
    internal class TinkleshardBullet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Ammunition.APreHardMode";


        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<TinkleshardBulletPROJ>();
            Item.shootSpeed = 12f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient<SeaPrism>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
