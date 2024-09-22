using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition.WulfrimBullet
{
    class All
    {
        public static int Damage = 10;
    }
    public class WulfrimBullet : 子弹
    {
        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.damage = All.Damage;
            Item.knockBack = 1.0f;
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<WulfrimBullet_Proje>();
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AbigailsFlower, 10);
            recipe.ReplaceResult(ModContent.ItemType<WulfrimBullet>(), 200);
            recipe.Register();
            base.AddRecipes();
        }
    }
}
