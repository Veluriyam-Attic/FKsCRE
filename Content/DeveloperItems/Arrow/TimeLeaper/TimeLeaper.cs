﻿using FKsCRE.Content.Arrows.EAfterDog.MiracleMatterArrow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Ammo;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;

namespace FKsCRE.Content.DeveloperItems.Arrow.TimeLeaper
{
    public class TimeLeaper : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.TimeLeaper";
        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 1;
            Item.consumable = false; // 弹药不是消耗品
            Item.knockBack = 3.5f;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
            Item.shoot = ModContent.ProjectileType<TimeLeaperPROJ>();
            Item.shootSpeed = 15f;
            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // 使用鼠标位置作为发射位置
            Vector2 mousePosition = Main.MouseWorld;

            // 计算箭矢的发射方向（从玩家位置到鼠标位置）
            Vector2 direction = Vector2.Normalize(mousePosition - player.Center) * velocity.Length();

            // 发射箭矢，从鼠标位置出发，初始方向指向玩家
            Projectile.NewProjectile(source, mousePosition, direction, type, damage, knockback, player.whoAmI);
            return false; // 返回 false 以避免默认发射逻辑
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient<TimeBolt>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
