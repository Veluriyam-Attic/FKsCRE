using System;
using System.Collections.Generic;
using System.Text;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.DeveloperItems.Arrow.PlasmaDriveCorePrototypeArrow
{
    public class PlasmaDriveCorePrototypeArrow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.SHPA";
        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.knockBack = 3.5f;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<PlasmaDriveCorePrototypeArrowPROJ>();
            Item.shootSpeed = PlasmaDriveCorePrototypeArrowPROJ.InitialSpeed;
            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient<PlasmaDriveCore>(1);
            recipe.AddCondition(Condition.InSpace);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
