using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition.WulfrimBullet
{
    class All
    {
        public static int Damage = 10;
    }
    public class WulfrimBullet : ModItem
    {

        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.damage = All.Damage;
            Item.knockBack = 1.0f;
            Item.ammo = AmmoID.Bullet;
            base.SetDefaults();
        }
    }
}
