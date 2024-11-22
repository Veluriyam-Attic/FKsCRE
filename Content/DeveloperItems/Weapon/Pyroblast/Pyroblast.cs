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
            Item.value = Item.buyPrice(0, 50, 0, 0); // 价值调整
            Item.rare = ItemRarityID.Red;
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
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddCondition(Condition.DownedMoonLord);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

    }
}



/*
炎爆
握在手中倾泻出独特的爆炸子弹，随时间不断升级，同时还能获得新的攻击手段
“烈焰于掌心涌动，随时间燃烧愈烈”
 
 
 
 
 */







