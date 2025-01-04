﻿using FKsCRE.Content.Ammunition.CPreMoodLord.PerennialBullet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;

namespace FKsCRE.Content.Ammunition.CPreMoodLord.PlagueBullet
{
    internal class PlagueBullet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Ammunition.CPreMoodLord";

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 18;
            Item.damage = 22;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<PlagueBulletPROJ>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(150);
            recipe.AddIngredient(ItemID.EmptyBullet, 150);
            recipe.AddIngredient<PlagueCellCanister>(3);
            recipe.AddIngredient<InfectedArmorPlating>(1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
