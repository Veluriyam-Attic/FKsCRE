﻿using CalamityMod.Items.Ammo;
using CalamityMod.Items.Materials;
using FKsCRE.Content.Ammunition.DPreDog.DivineGeodeBullet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using FKsCRE.Content.Ammunition.CPreMoodLord.PerennialBullet;
using CalamityMod.Items.Accessories;

namespace FKsCRE.Content.Ammunition.DPreDog.ToothBullet
{
    internal class ToothBullet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Ammunition.DPreDog";


        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 18;
            Item.damage = 13;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<ToothBulletPROJ>();
            Item.shootSpeed = 9f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient<PerennialBullet>(333);
            recipe.AddIngredient<ReaperTooth>(1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
