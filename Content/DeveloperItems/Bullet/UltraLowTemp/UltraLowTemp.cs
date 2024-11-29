using CalamityMod.Items.Weapons.Ranged;
using FKsCRE.Content.DeveloperItems.Bullet.TheEmpty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Ammo;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;

namespace FKsCRE.Content.DeveloperItems.Bullet.UltraLowTemp
{
    public class UltraLowTemp : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.UltraLowTemp";
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 18;
            Item.damage = 40;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
            Item.shoot = ModContent.ProjectileType<UltraLowTempPROJ>();
            Item.shootSpeed = 19f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient<HailstormBullet>(100);
            recipe.AddIngredient<EndothermicEnergy>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}
