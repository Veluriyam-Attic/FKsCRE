using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using FKsCRE.Content.DeveloperItems.Bullet.DestructionBullet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace FKsCRE.Content.DeveloperItems.Bullet.ShadowsBullet
{
    public class ShadowsBullet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.ShadowsBullet";
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 1;
            Item.consumable = false; // 弹药是消耗品
            Item.knockBack = 3.5f;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
            Item.shoot = ModContent.ProjectileType<ShadowsBulletPROJ>();
            Item.shootSpeed = 9f;
            Item.ammo = AmmoID.Bullet; // 这是子弹类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe(1);
            recipe1.AddIngredient(ItemID.MusketBall, 1);
            recipe1.AddIngredient<ShadowspecBar>(5);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();
        }

    }
}
