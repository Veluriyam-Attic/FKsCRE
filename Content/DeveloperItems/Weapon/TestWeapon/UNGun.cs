using FKsCRE.Content.DeveloperItems.Weapon.Pyroblast;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.DeveloperItems.Weapon.TestWeapon
{
    internal class UNGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 138;
            Item.height = 48;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 200;
            Item.knockBack = 2f;

            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.UseSound = SoundID.Item5;

            Item.shootSpeed = 7f;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.scale = 1.0f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return player.itemAnimation < Item.useAnimation - 2;
        }


        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-25, -2);
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2) // 右键
            {
                // 右键设置：使用弓箭相关属性
                Item.useTime = 40;
                Item.useAnimation = 40;
                Item.useAmmo = AmmoID.Arrow;
                Item.shoot = ProjectileID.WoodenArrowFriendly;

                // 右键发射五支箭矢，形成并排的排列
                const int numArrows = 5; // 射出的箭矢数量
                const float offsetDistance = 6f; // 偏移距离
                Vector2 baseVelocity = velocity;
                baseVelocity.Normalize();

                for (int i = 0; i < numArrows; ++i)
                {
                    // 计算并排发射的偏移位置
                    float arrowOffset = (i - (numArrows - 1) / 2f) * offsetDistance; // 计算每支箭的偏移距离
                    Vector2 offsetPosition = position + baseVelocity.RotatedBy(MathHelper.PiOver2) * arrowOffset; // 偏移方向与箭矢移动方向垂直

                    if (type == ProjectileID.WoodenArrowFriendly) // 检查是否为木箭
                    {
                        // 转换为 LazharSolarBeam
                        Projectile.NewProjectile(player.GetSource_ItemUse(Item), offsetPosition, velocity, ModContent.ProjectileType<PyroblastSolarBeam>(), damage, knockback, player.whoAmI);
                    }
                    else
                    {
                        // 直接发射其他箭矢
                        int proj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), offsetPosition, velocity, type, damage, knockback, player.whoAmI);
                        Main.projectile[proj].noDropItem = true; // 防止弹幕掉落物品
                    }
                }

                // 播放弓箭声音
                SoundEngine.PlaySound(SoundID.Item75, player.position);
            }
            else // 左键
            {
                // 左键设置：使用子弹相关属性
                Item.useTime = 7;
                Item.useAnimation = 7;
                Item.useAmmo = AmmoID.Bullet;
                Item.shoot = ProjectileID.Bullet;

                // 左键发射一发子弹，具有小幅随机偏移
                float randomOffsetAngle = Main.rand.NextFloat(-MathHelper.ToRadians(2), MathHelper.ToRadians(2));
                Vector2 modifiedVelocity = velocity.RotatedBy(randomOffsetAngle);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, modifiedVelocity, type, damage, knockback, player.whoAmI);

                // 播放子弹声音
                SoundEngine.PlaySound(SoundID.Item91, player.position);
            }

            return false; // 阻止默认射击行为
        }







    }
}
