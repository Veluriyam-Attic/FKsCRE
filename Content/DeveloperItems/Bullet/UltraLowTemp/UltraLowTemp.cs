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

namespace FKsCRE.Content.DeveloperItems.Bullet.UltraLowTemp
{
    public class UltraLowTemp : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.UltraLowTemp";
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 18;
            Item.damage = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
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
