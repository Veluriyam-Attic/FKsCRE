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

namespace FKsCRE.Content.Ammunition.DPreDog.UelibloomBullet
{
    public class UelibloomBullet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Ammunition.DPreDog";
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 18;
            Item.damage = 25;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<UelibloomBulletPROJ>();
            Item.shootSpeed = 5f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient<UelibloomBar>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}