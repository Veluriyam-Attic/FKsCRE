using System;
using System.Collections.Generic;
using System.Text;
using CalamityMod.Items.Ammo;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using FKsCRE.Content.Arrows.APreHardMode.WulfrimArrow;
using FKsCRE.Content.Arrows.DPreDog.PolterplasmArrow;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.Arrows.EAfterDog.MiracleMatterArrow
{
    public class MiracleMatterArrow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Arrows.EAfterDog";
        public override void SetDefaults()
        {
            Item.damage = 43;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true; // 弹药是消耗品
            Item.knockBack = 3.5f;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<MiracleMatterArrowPROJ>();
            Item.shootSpeed = 15f;
            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
        }
        public override void OnConsumedAsAmmo(Item weapon, Player player)
        {
            // 标记玩家启用了 MiracleMatterArrow 的附魔状态
            player.GetModPlayer<MiracleMatterArrowPlayer>().IsMiracleMatterArrowActive = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3996);
            recipe.AddIngredient<APreHardMode.WulfrimArrow.WulfrimArrow>(999);
            recipe.AddIngredient<VeriumBolt>(999);
            recipe.AddIngredient<SproutingArrow>(999);
            recipe.AddIngredient<PolterplasmArrow>(999);
            recipe.AddIngredient<MiracleMatter>(1);
            recipe.AddTile<DraedonsForge>();
            recipe.Register();
        }
    }
}
