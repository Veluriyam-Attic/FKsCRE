﻿//using System;
//using System.Collections.Generic;
//using System.Text;
//using CalamityMod.Items.Accessories;
//using CalamityMod.Items.Materials;
//using CalamityMod.Projectiles.Rogue;
//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace FKsCRE.Content.Arrows.CPreMoodLord.CoreofCalamityArrow
//{
//    public class CoreofCalamityArrow : ModItem, ILocalizedModType
//    {
//        public new string LocalizationCategory => "Arrows.CPreMoodLord";
//        public override void SetDefaults()
//        {
//            Item.damage = 20;
//            Item.DamageType = DamageClass.Ranged;
//            Item.width = 14;
//            Item.height = 32;
//            Item.maxStack = 9999;
//            Item.consumable = true; // 弹药是消耗品
//            Item.knockBack = 3.5f;
//            Item.value = 10;
//            Item.rare = ItemRarityID.Blue;
//            Item.shoot = ModContent.ProjectileType<CoreofCalamityArrowPROJ>();
//            Item.shootSpeed = 15f;
//            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
//        }

//        public override void AddRecipes()
//        {
//            Recipe recipe = CreateRecipe(333);
//            recipe.AddIngredient<CoreofCalamity>(1);
//            recipe.AddTile(TileID.Anvils);
//            recipe.Register();
//        }
//    }
//}