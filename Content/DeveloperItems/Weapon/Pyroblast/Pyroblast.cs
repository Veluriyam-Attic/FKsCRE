using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityMod.Items.Ammo;
using CalamityMod.Items.Materials;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs.Astral;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Items.Weapons.Magic;

namespace FKsCRE.Content.DeveloperItems.Weapon.Pyroblast
{
    public class Pyroblast : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.Pyroblast";
        public static readonly int OriginalUseTime = 30;

        public override void SetDefaults()
        {
            Item.damage = 250;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = OriginalUseTime;
            Item.shoot = ModContent.ProjectileType<PyroblastHoldOut>();
            Item.shootSpeed = 15f;
            Item.knockBack = 6.5f;

            Item.width = 96;
            Item.height = 42;
            Item.noMelee = true;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.useAmmo = AmmoID.Bullet;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
            Item.useStyle = ItemUseStyleID.Shoot;
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
                ModContent.ProjectileType<PyroblastHoldOut>(),
                Item.damage,
                Item.knockBack,
                player.whoAmI
            ).velocity = (player.Calamity().mouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient<PlagueTaintedSMG>(1);
            recipe.AddIngredient<ClockGatlignum>(1);
            recipe.AddIngredient<Arietes41>(1);
            recipe.AddIngredient<Shroomer>(1);
            recipe.AddIngredient<ConferenceCall>(1);
            recipe.AddIngredient<Lazhar>(1);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            //recipe.AddCondition(Condition.DownedMoonLord);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            //if (Main.zenithWorld)
            //醉酒世界drunkWorld
            //困难世界getGoodWorld
            //10周年世界tenthAnniversaryWorld
            //饥荒世界dontStarveWorld
            //蜜蜂世界notTheBeesWorld
            //地下世界remixWorld
            //陷阱世界noTrapsWorld
            //天顶世界zenithWorld

            // 第1阶段
            bool kingSlime = NPC.downedSlimeKing; // 史莱姆王
            bool eyeOfCthulhu = NPC.downedBoss1; // 眼睛
            bool desertScourge = DownedBossSystem.downedDesertScourge; // 荒漠灾虫
            bool crabulon = DownedBossSystem.downedCrabulon; // 螃蟹
            bool eaterOfWorldsOrBrain = NPC.downedBoss2; // EoW BoC
            bool hiveMind = DownedBossSystem.downedHiveMind; // 意志
            bool perforator = DownedBossSystem.downedPerforator; // 宿主
            bool queenBee = NPC.downedQueenBee; // 蜂后
            bool skeletron = NPC.downedBoss3; // 骷髅王
            bool deerclops = NPC.downedDeerclops; // 独眼巨鹿
            bool slimeGod = DownedBossSystem.downedSlimeGod; // 史莱姆之神

            // 第2阶段
            bool wallOfFlesh = Main.hardMode; // 肉山
            bool QueenSlime = NPC.downedQueenSlime; // 史莱姆女王
            bool mechanicalBosses1 = NPC.downedMechBoss1; // 毁灭者
            bool mechanicalBosses2 = NPC.downedMechBoss2; // 双子魔眼
            bool mechanicalBosses3 = NPC.downedMechBoss3; // 机械骷髅王
            bool brimstoneElemental = DownedBossSystem.downedBrimstoneElemental; // 硫磺火元素
            bool cryogen = DownedBossSystem.downedCryogen; // 极地冰灵
            bool aquaticScourge = DownedBossSystem.downedAquaticScourge; // 渊海灾虫

            // 第3阶段
            bool plantera = NPC.downedPlantBoss; // 花
            bool calamitas = DownedBossSystem.downedCalamitas; // 灾厄之影
            bool Leviathan = DownedBossSystem.downedLeviathan; // 利维坦
            bool astrumAureus = DownedBossSystem.downedAstrumAureus; // 白金
            bool golem = NPC.downedGolemBoss; // 石
            bool empress = NPC.downedEmpressOfLight; // 女皇
            bool fishron = NPC.downedFishron; // 公爵
            bool plaguebringer = DownedBossSystem.downedPlaguebringer; // 瘟疫使者歌莉娅
            bool ravager = DownedBossSystem.downedRavager; // 毁灭魔像
            bool cultist = NPC.downedAncientCultist; // 教徒
            bool astrumDeus = DownedBossSystem.downedAstrumDeus; // 星神游龙

            // 第4阶段
            bool moonLord = NPC.downedMoonlord; // 月总 
            bool dragonfolly = DownedBossSystem.downedDragonfolly; // 金龙
            bool guardians = DownedBossSystem.downedGuardians; // 亵渎守卫
            bool providence = DownedBossSystem.downedProvidence; // 亵渎天神
            bool ceaselessVoid = DownedBossSystem.downedCeaselessVoid; // 无尽虚空
            bool signus = DownedBossSystem.downedSignus; // 西格纳斯
            bool stormWeaver = DownedBossSystem.downedStormWeaver; // 风暴编织者
            bool polterghast = DownedBossSystem.downedPolterghast; // 幽花
            bool boomerDuke = DownedBossSystem.downedBoomerDuke; // 老核弹

            // 第5阶段
            bool devourerOfGods = DownedBossSystem.downedDoG; // 神吞
            bool yharon = DownedBossSystem.downedYharon; // 龙
            bool exoMechs = DownedBossSystem.downedExoMechs; // 巨械
            bool calamitasClone = DownedBossSystem.downedCalamitasClone; // 至尊灾厄
            bool BR = DownedBossSystem.downedBossRush; // BR

            // 这byd忍不了一点，单独列几个阴间的出来：
            // downedGSS：大沙狂鲨
            // downedCLAM：巨像蛤
            // downedCLAMHardMode：肉后巨像蛤
            // downedPlaguebringer：瘟疫使者（是boss）
            // CragmawMire：伽玛史莱姆
            // downedDreadnautilus：恐惧鹦鹉螺
            // Mauler：渊海狂鲨
            // CragmawMire：伽玛史莱姆           
        }

    }
}






