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

namespace FKsCRE.Content.Ammunition.CPreMoodLord.ScoriaBullet
{
    public class ScoriaBullet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Ammunition.CPreMoodLord";

        public override void SetDefaults()
        {
            Item.width =  14;
            Item.height = 18;
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<ScoriaBulletPROJ>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(150); 
            recipe.AddIngredient(ItemID.ExplodingBullet, 150);
            recipe.AddIngredient<ScoriaBar>(1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
