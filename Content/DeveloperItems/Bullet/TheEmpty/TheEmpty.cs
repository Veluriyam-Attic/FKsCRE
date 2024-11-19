using FKsCRE.Content.Ammunition.BPrePlantera.CryonicBullet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;

namespace FKsCRE.Content.DeveloperItems.Bullet.TheEmpty
{
    public class TheEmpty : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.TheEmpty";
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 18;
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<TheEmptyPROJ>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient<NullificationPistol>(3);
            recipe.AddCondition(Condition.NearShimmer);
            //recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}
