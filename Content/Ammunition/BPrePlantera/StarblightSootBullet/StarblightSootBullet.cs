using FKsCRE.Content.Ammunition.CPreMoodLord.AstralBullet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;

namespace FKsCRE.Content.Ammunition.BPrePlantera.StarblightSootBullet
{
    internal class StarblightSootBullet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Ammunition.BPrePlantera";


        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 18;
            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<StarblightSootBulletPROJ>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient<StarblightSoot>(5);
            recipe.AddIngredient<TitanHeart>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
