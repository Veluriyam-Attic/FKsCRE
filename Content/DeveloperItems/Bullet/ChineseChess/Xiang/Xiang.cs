using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using FKsCRE.Content.DeveloperItems.Bullet.ChineseChess.Pao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace FKsCRE.Content.DeveloperItems.Bullet.ChineseChess.Xiang
{
    internal class Xiang : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.ChineseChess.Xiang";
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true; // 弹药是消耗品
            Item.knockBack = 3.5f;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
            Item.shoot = ModContent.ProjectileType<XiangPROJ>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Bullet; // 这是子弹类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe(333);
            recipe1.AddIngredient(ItemID.AlphabetStatueX, 1);
            recipe1.AddIngredient(ItemID.AlphabetStatueI, 1);
            recipe1.AddIngredient(ItemID.AlphabetStatueA, 1);
            recipe1.AddIngredient(ItemID.AlphabetStatueN, 1);
            recipe1.AddIngredient(ItemID.AlphabetStatueG, 1);
            recipe1.AddIngredient(ItemID.MoonlordBullet, 333);
            recipe1.AddIngredient<GalacticaSingularity>(1);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();
        }

    }
}
