﻿using FKsCRE.Content.Gel.DPreDog.UelibloomGel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;

namespace FKsCRE.Content.Gel.CPreMoodLord.AstralGel
{
    public class AstralGel : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Gel.CPreMoodLord";
        public override void SetDefaults()
        {
            Item.damage = 85;
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
                    proj.GetGlobalProjectile<AstralGelGP>().IsCosmosGelInfused = true;
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient<AstralBar>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
