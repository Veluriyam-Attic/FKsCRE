using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.Arrows
{
    public abstract class Arrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Zenith, 999);
            recipe.AddTile(TileID.AbigailsFlower);
            recipe.Register();
            base.AddRecipes();
        }
    }

    public abstract class ArrowProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.friendly = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
        }
    }
}
