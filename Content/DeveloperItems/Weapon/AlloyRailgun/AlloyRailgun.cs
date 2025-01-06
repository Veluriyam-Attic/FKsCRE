using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Ranged;

namespace FKsCRE.Content.DeveloperItems.Weapon.AlloyRailgun
{
    internal class AlloyRailgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsRangedSpecialistWeapon[Item.type] = true;
        }
        public override void SetDefaults()
        {
            // 设置武器基本属性
            Item.width = 40; // 弓的宽度
            Item.height = 80; // 弓的高度
            Item.damage = 70; // 武器伤害
            Item.DamageType = DamageClass.Ranged; // 伤害类型：远程
            Item.useTime = 5; // 使用时间（5帧）
            Item.useAnimation = 5; // 动画时间（5帧）
            Item.useStyle = ItemUseStyleID.Shoot; // 使用风格：射击
            Item.knockBack = 4; // 击退力
            // Item.UseSound = SoundID.Item5; // 一般情况下他没有使用音效
            Item.shoot = ModContent.ProjectileType<AlloyRailgunHold>(); // 手持弹幕
            Item.shootSpeed = 16f; 
            Item.noMelee = true; // 不进行近战攻击
            Item.noUseGraphic = true; // 使用时隐藏物品模型
            Item.channel = true; // 支持长按
            Item.autoReuse = true; // 自动连点
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item77 with { Volume = SoundID.Item77.Volume * 0.7f };

            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.Calamity().canFirePointBlankShots = true;
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 shootVelocity = velocity;
            Vector2 shootDirection = shootVelocity.SafeNormalize(Vector2.UnitX * player.direction);
            Projectile.NewProjectile(source, position, shootDirection, ModContent.ProjectileType<AlloyRailgunHold>(), damage, knockback, player.whoAmI);
            return false;
        }




        // 钛金电磁炮的源代码：

        //public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        //{
        //    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai1: TitaniumRailgunScope.BaseMaxCharge * player.GetWeaponAttackSpeed(player.HeldItem));
        //    return false;
        //}

        //public override Vector2? HoldoutOffset() => new Vector2(-5, 4);

        //public override void UseItemFrame(Player player)
        //{
        //    // Thank you Mr. IbanPlay (CoralSprout.cs)
        //    // Calculate the dirction in which the players arms should be pointing at.
        //    float armPointingDirection = player.itemRotation;
        //    if (player.direction < 0)
        //        armPointingDirection += MathHelper.Pi;

        //    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, armPointingDirection - MathHelper.PiOver2);
        //    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armPointingDirection - MathHelper.PiOver2);
        //}

        //public override Vector2? HoldoutOffset() => new Vector2(0, 10);
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<TitaniumRailgun>();
            recipe.AddIngredient<AdamantiteParticleAccelerator>();
            recipe.Register();
        }
    }
}
