using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items;
using CalamityMod.Rarities;
using FKsCRE.Content.DeveloperItems.Bullet.TheEmpty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items.Weapons.Melee;

namespace FKsCRE.Content.DeveloperItems.Bullet.MonstrousBullet
{
    internal class MonstrousBullet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.MonstrousBullet";
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 18;
            Item.damage = 2;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.knockBack = 3f;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
            Item.shoot = ModContent.ProjectileType<MonstrousBulletPROJ>();
            Item.shootSpeed = 16f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient<MonstrousKnives>(1);
            recipe.AddCondition(Condition.NearShimmer);
            //recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}
