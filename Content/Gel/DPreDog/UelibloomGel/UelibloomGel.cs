﻿using CalamityMod.Items.Materials;
using FKsCRE.Content.Gel.EAfterDog.MiracleMatterGel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace FKsCRE.Content.Gel.DPreDog.UelibloomGel
{
    public class UelibloomGel : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Gel.DPreDog";
        public override void SetDefaults()
        {
            //Item.damage = 85;
            Item.width = 12;
            Item.height = 18;
            Item.consumable = true;
            Item.ammo = AmmoID.Gel;
            Item.maxStack = 9999;
        }

        public override void OnConsumedAsAmmo(Item weapon, Player player)
        {
            // 附魔效果，标记弹幕使用了 CosmosGel
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == player.whoAmI)
                {
                    proj.GetGlobalProjectile<UelibloomGelGP>().IsUelibloomGelInfused = true;
                }
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient(ItemID.Gel, 333);
            recipe.AddIngredient<UelibloomBar>(1);
            recipe.AddTile<StaticRefiner>();
            recipe.Register();
        }
    }
}
