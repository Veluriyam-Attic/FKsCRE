﻿using FKsCRE.Content.Gel.APreHardMode.HurricaneGel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using FKsCRE.Content.Gel.CPreMoodLord.PerennialGel;
using FKsCRE.Content.Gel.CPreMoodLord.ScoriaGel;
using FKsCRE.Content.Gel.BPrePlantera.CryonicGel;
using CalamityMod.Items.Materials;

namespace FKsCRE.Content.Gel.CPreMoodLord.LifeAlloyGel
{
    internal class LifeAlloyGel : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Gel.CPreMoodLord";
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
            // 3合1的效果，拥有他们三个的效果
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == player.whoAmI)
                {
                    proj.GetGlobalProjectile<PerennialGelGP>().IsPerennialGelInfused = true;
                    proj.GetGlobalProjectile<ScoriaGelGP>().IsScoriaGelInfused = true;
                    proj.GetGlobalProjectile<CryonicGelGP>().IsCryonicGelInfused = true;
                    proj.GetGlobalProjectile<LifeAlloyGelGP>().IsLifeAlloyGelInfused = true;
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient<LifeAlloy>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
