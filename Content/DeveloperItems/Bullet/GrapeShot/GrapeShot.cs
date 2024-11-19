using FKsCRE.Content.DeveloperItems.Arrow.TheDrill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace FKsCRE.Content.DeveloperItems.Bullet.GrapeShot
{
    public class GrapeShot : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.GrapeShot";
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true; // 弹药是消耗品
            Item.knockBack = 3.5f;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<GrapeShotPROJ>();
            Item.shootSpeed = 15f;
            Item.ammo = AmmoID.Bullet; // 这是箭矢类型的弹药
        }
        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            GrapeShotPlayer grapeShotPlayer = player.GetModPlayer<GrapeShotPlayer>();
            int currentX = grapeShotPlayer.GetGrapeShotX();

            // 设置发射的弹幕的 ai[0] 为当前 x 值
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity, type, damage, 0f, player.whoAmI, currentX);

            return false; // 防止默认发射
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(500);
            recipe.AddIngredient(ItemID.Grapes, 1);
            recipe.AddCondition(Condition.Thunderstorm);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }


    }
}
