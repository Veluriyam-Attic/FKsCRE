//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria;
//using System;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;

//namespace FKsCRE.Content.DeveloperItems.Weapon.BlindBirdCry
//{
//    public class BlindBirdCry : ModItem, ILocalizedModType
//    {
//        public new string LocalizationCategory => "DeveloperItems.BlindBirdCry";
//        public override void SetDefaults()
//        {
//            Item.damage = 400; // 设定攻击力（可以随时调整）
//            Item.DamageType = DamageClass.Magic; // 攻击类型为魔法
//            Item.useTime = Item.useAnimation = 30; // 使用时间
//            Item.shoot = ModContent.ProjectileType<BlindBirdCryHoldOut>(); // 绑定 BlindBirdCryHoldOut
//            Item.shootSpeed = 0f; // 不需要速度（HoldOut自己处理）
//            Item.knockBack = 5f;

//            Item.width = 50;
//            Item.height = 50;
//            Item.noMelee = true;
//            Item.channel = true; // 持续释放
//            Item.noUseGraphic = true; // 不显示物品
//            Item.mana = 10; // 每次释放消耗魔法值

//            Item.value = Item.buyPrice(0, 30, 0, 0); // 价值
//            Item.rare = ItemRarityID.Lime; // 稀有度
//            Item.useStyle = ItemUseStyleID.Shoot;
//        }

//        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;

//        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
//        {
//            Projectile.NewProjectile(
//                source,
//                player.MountedCenter,
//                Vector2.Zero,
//                ModContent.ProjectileType<BlindBirdCryHoldOut>(),
//                damage,
//                knockback,
//                player.whoAmI
//            );
//            return false; // 防止重复生成投射物
//        }
//    }
//}
