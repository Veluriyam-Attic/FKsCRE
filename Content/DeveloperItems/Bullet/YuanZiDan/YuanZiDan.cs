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
using CalamityMod.Items.DraedonMisc;

namespace FKsCRE.Content.DeveloperItems.Bullet.YuanZiDan
{
    public class YuanZiDan : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.YuanZiDan";
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 18;
            Item.damage = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<YuanZiDanPROJ>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.NanoBullet, 3996);
            recipe.AddIngredient<AuricQuantumCoolingCell>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}
