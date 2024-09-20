using Terraria.ID;
using Terraria.ModLoader;

namespace NanTing.Content.Ammunition.钨钢弹
{
    class 统一
    {
        public static int 伤害 = 10;
    }
    public class 钨钢弹 : ModItem
    {

        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.damage = 统一.伤害;
            Item.knockBack = 1.0f;
            Item.ammo = AmmoID.Bullet;
            base.SetDefaults();
        }
    }
}
