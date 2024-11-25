using CalamityMod.Items.Weapons.Ranged;
using FKsCRE.Content.DeveloperItems.Weapon.Pyroblast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Items;
using CalamityMod.Rarities;

namespace FKsCRE.Content.DeveloperItems.Weapon.TheGoldenFire
{
    public class TheGoldenFire : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.TheGoldenFire";
        public static readonly int OriginalUseTime = 30;
        //public override void SetStaticDefaults()
        //{
        //    ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        //}
        //public override bool AltFunctionUse(Player player) => true;
        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = OriginalUseTime;
            Item.shoot = ModContent.ProjectileType<TheGoldenFireHoldOut>();
            Item.shootSpeed = 15f;
            Item.knockBack = 6.5f;

            Item.width = 96;
            Item.height = 42;
            Item.noMelee = false;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.useAmmo = AmmoID.Gel;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;


            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
        }
        private Color GetGelColor(Player player)
        {
            // 获取玩家使用的当前弹药类型
            Item ammoItem = player.inventory[player.selectedItem]; // 当前手持物品
            if (ammoItem.ammo == AmmoID.Gel)
            {
                // 根据 ItemID 返回不同的颜色
                switch (ammoItem.type)
                {
                    case ItemID.Gel: // 标准凝胶
                        return Color.Blue;
                    //case ModContent.ItemType<XGel>(): // 示例：模组的特殊凝胶
                    //    return Color.Green;
                    //case ModContent.ItemType<XGel>(): // 示例：模组的特殊凝胶
                    //    return Color.Blue;
                    default:
                        return Color.Gray; // 未知类型的默认颜色
                }
            }

            // 如果不是凝胶，返回默认颜色
            return Color.White;
        }
        public static int TheFinalDamage = 0; // 用于存储最终计算的伤害值

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float damageMult = 1f; // 初始倍率

            // 第1阶段：每击败一个 Boss 提升 10%
            int stage1BossesDefeated = 0;
            stage1BossesDefeated += NPC.downedSlimeKing ? 1 : 0;
            stage1BossesDefeated += NPC.downedBoss1 ? 1 : 0;
            stage1BossesDefeated += DownedBossSystem.downedDesertScourge ? 1 : 0;
            stage1BossesDefeated += DownedBossSystem.downedCrabulon ? 1 : 0;
            stage1BossesDefeated += NPC.downedBoss2 ? 1 : 0;
            stage1BossesDefeated += DownedBossSystem.downedHiveMind ? 1 : 0;
            stage1BossesDefeated += DownedBossSystem.downedPerforator ? 1 : 0;
            stage1BossesDefeated += NPC.downedQueenBee ? 1 : 0;
            stage1BossesDefeated += NPC.downedBoss3 ? 1 : 0;
            stage1BossesDefeated += NPC.downedDeerclops ? 1 : 0;
            stage1BossesDefeated += DownedBossSystem.downedSlimeGod ? 1 : 0;

            damageMult += stage1BossesDefeated * 0.1f;

            // 第2阶段：每击败一个 Boss 提升 20%
            int stage2BossesDefeated = 0;
            stage2BossesDefeated += Main.hardMode ? 1 : 0;
            stage2BossesDefeated += NPC.downedQueenSlime ? 1 : 0;
            stage2BossesDefeated += NPC.downedMechBoss1 ? 1 : 0;
            stage2BossesDefeated += NPC.downedMechBoss2 ? 1 : 0;
            stage2BossesDefeated += NPC.downedMechBoss3 ? 1 : 0;
            stage2BossesDefeated += DownedBossSystem.downedBrimstoneElemental ? 1 : 0;
            stage2BossesDefeated += DownedBossSystem.downedCryogen ? 1 : 0;
            stage2BossesDefeated += DownedBossSystem.downedAquaticScourge ? 1 : 0;

            damageMult += stage2BossesDefeated * 0.2f;

            // 第3阶段：每击败一个 Boss 提升 30%
            int stage3BossesDefeated = 0;
            stage3BossesDefeated += NPC.downedPlantBoss ? 1 : 0;
            stage3BossesDefeated += DownedBossSystem.downedCalamitas ? 1 : 0;
            stage3BossesDefeated += DownedBossSystem.downedLeviathan ? 1 : 0;
            stage3BossesDefeated += DownedBossSystem.downedAstrumAureus ? 1 : 0;
            stage3BossesDefeated += NPC.downedGolemBoss ? 1 : 0;
            stage3BossesDefeated += NPC.downedEmpressOfLight ? 1 : 0;
            stage3BossesDefeated += NPC.downedFishron ? 1 : 0;
            stage3BossesDefeated += DownedBossSystem.downedPlaguebringer ? 1 : 0;
            stage3BossesDefeated += DownedBossSystem.downedRavager ? 1 : 0;
            stage3BossesDefeated += NPC.downedAncientCultist ? 1 : 0;
            stage3BossesDefeated += DownedBossSystem.downedAstrumDeus ? 1 : 0;

            damageMult += stage3BossesDefeated * 0.3f;

            // 第4阶段：每击败一个 Boss 提升 40%
            int stage4BossesDefeated = 0;
            stage4BossesDefeated += NPC.downedMoonlord ? 1 : 0;
            stage4BossesDefeated += DownedBossSystem.downedDragonfolly ? 1 : 0;
            stage4BossesDefeated += DownedBossSystem.downedGuardians ? 1 : 0;
            stage4BossesDefeated += DownedBossSystem.downedProvidence ? 1 : 0;
            stage4BossesDefeated += DownedBossSystem.downedCeaselessVoid ? 1 : 0;
            stage4BossesDefeated += DownedBossSystem.downedSignus ? 1 : 0;
            stage4BossesDefeated += DownedBossSystem.downedStormWeaver ? 1 : 0;
            stage4BossesDefeated += DownedBossSystem.downedPolterghast ? 1 : 0;
            stage4BossesDefeated += DownedBossSystem.downedBoomerDuke ? 1 : 0;

            damageMult += stage4BossesDefeated * 0.4f;

            // 第5阶段：每击败一个 Boss 提升 50%
            int stage5BossesDefeated = 0;
            stage5BossesDefeated += DownedBossSystem.downedDoG ? 1 : 0;
            stage5BossesDefeated += DownedBossSystem.downedYharon ? 1 : 0;
            stage5BossesDefeated += DownedBossSystem.downedExoMechs ? 1 : 0;
            stage5BossesDefeated += DownedBossSystem.downedCalamitasClone ? 1 : 0;
            stage5BossesDefeated += DownedBossSystem.downedBossRush ? 1 : 0;

            damageMult += stage5BossesDefeated * 0.5f;

            // 应用最终伤害倍率
            damage *= damageMult;
            TheFinalDamage = (int)(Item.damage * damageMult);
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;

        public override bool CanConsumeAmmo(Item ammo, Player player) => player.ownedProjectileCounts[Item.shoot] != 0;

        public override void HoldItem(Player player) => player.Calamity().mouseRotationListener = true;

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(
                source,
                player.MountedCenter,
                Vector2.Zero,
                ModContent.ProjectileType<TheGoldenFireHoldOut>(),
                TheFinalDamage,
                Item.knockBack,
                player.whoAmI
            ).velocity = (player.Calamity().mouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
            return false;
        }




        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient<SparkSpreader>(1);
            recipe.AddIngredient(ItemID.GoldBar, 10);
            //recipe.AddCondition(Condition.DownedMoonLord);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}

// 左右键逻辑专用
//public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

//public override bool CanUseItem(Player player)
//{
//    if (player.altFunctionUse == 2) // 如果是右键
//    {
//        Item.useTime = Item.useAnimation = 10; // 调整右键使用速度
//        Item.shoot = ProjectileID.Flames ; // 发射 x
//        Item.shootSpeed = 8f; // 调整右键弹幕速度
//        Item.useAmmo = AmmoID.Gel; // 右键消耗凝胶弹药
//        Item.useTime = 3;
//        Item.useAnimation = 24;
//        Item.channel = false;
//    }
//    else // 左键
//    {
//        Item.useTime = Item.useAnimation = OriginalUseTime;
//        Item.shoot = ModContent.ProjectileType<TheGoldenFireHoldOut>();
//        Item.shootSpeed = 15f;
//        Item.useAmmo = AmmoID.Gel;
//        Item.channel = true;
//        Item.useTime = Item.useAnimation = OriginalUseTime;
//    }
//    return base.CanUseItem(player);
//}


//public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
//{
//    if (player.altFunctionUse == 2) // 右键逻辑
//    {
//        Vector2 newPos = position + velocity.SafeNormalize(Vector2.UnitX) * 36f;
//        Color gelColor = GetGelColor(player); // 获取当前凝胶颜色
//        for (int i = 0; i < 3; i++) // 发射三颗火焰弹幕
//        {
//            Vector2 newVel = velocity.RotatedByRandom(MathHelper.ToRadians(5f)); // 随机角度偏移
//            int projID = Projectile.NewProjectile(source, newPos, newVel, type, damage, knockback, player.whoAmI);
//            Main.projectile[projID].localAI[0] = gelColor.R; // 存储 R 通道值
//            Main.projectile[projID].localAI[1] = gelColor.G; // 存储 G 通道值
//            Main.projectile[projID].ai[0] = gelColor.B;       // 存储 B 通道值
//        }
//        return false;
//    }
//    else // 左键逻辑
//    {
//        Projectile.NewProjectileDirect(
//            source,
//            player.MountedCenter,
//            Vector2.Zero,
//            ModContent.ProjectileType<TheGoldenFireHoldOut>(),
//            Item.damage,
//            Item.knockBack,
//            player.whoAmI
//        ).velocity = (player.Calamity().mouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
//        return false;
//    }
//}