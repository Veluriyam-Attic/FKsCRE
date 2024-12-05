using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;

namespace FKsCRE.Content.DeveloperItems.Weapon.BrassBeast
{
    public class BrassBeast : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.BrassBeast";
        public override void SetDefaults()
        {
            Item.damage = 120; // 攻击力随意设定
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 30; // 使用时间
            Item.shoot = ModContent.ProjectileType<BrassBeastHoldOut>(); // 绑定 BrassBeastHoldOut
            Item.shootSpeed = 15f;
            Item.knockBack = 8f;

            Item.width = 120;
            Item.height = 40;
            Item.noMelee = true;
            Item.channel = true; // 支持持续攻击
            Item.noUseGraphic = true; // 不显示武器
            Item.useAmmo = AmmoID.Bullet;

            Item.value = Item.buyPrice(0, 50, 0, 0); // 设定物品价值
            Item.rare = ItemRarityID.Red; // 设定稀有度
            Item.useStyle = ItemUseStyleID.Shoot;
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanConsumeAmmo(Item ammo, Player player) => player.ownedProjectileCounts[Item.shoot] != 0;
        public override void HoldItem(Player player) => player.Calamity().mouseRotationListener = true;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) // 右键逻辑
            {
                Item.damage = 1500; // 右键攻击倍率 3 倍
                Item.useTime = Item.useAnimation = 70; // 使用时间
                Item.shoot = ModContent.ProjectileType<BrassBeastHeavySmoke>();
                Item.shootSpeed = 10f; // 弹幕速度
                Item.UseSound = SoundID.Item38; // 播放右键音效
            }
            else // 左键逻辑
            {
                Item.damage = 500; // 原始伤害
                Item.useTime = Item.useAnimation = 30; // 左键时间
                Item.shoot = ModContent.ProjectileType<BrassBeastHoldOut>();
                Item.shootSpeed = 15f;
                Item.UseSound = null; // 左键不播放音效
            }
            return player.ownedProjectileCounts[Item.shoot] == 0;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2) // 右键发射 BrassBeastHeavySmoke
            {
                Projectile.NewProjectile(source, player.MountedCenter, velocity, type, damage, knockback, player.whoAmI);
            }
            else // 左键发射 BrassBeastHoldOut
            {
                Projectile.NewProjectileDirect(
                    source,
                    player.MountedCenter,
                    Vector2.Zero,
                    ModContent.ProjectileType<BrassBeastHoldOut>(),
                    damage,
                    knockback,
                    player.whoAmI
                );
            }
            return false;
        }
    }
}
