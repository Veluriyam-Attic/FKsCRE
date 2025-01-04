using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace FKsCRE.Content.DeveloperItems.Weapon.BlindBirdCry
{
    public class BlindBirdCryHoldOut : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.BlindBirdCry";
        public override string Texture => "FKsCRE/Content/DeveloperItems/Weapon/BlindBirdCry/BlindBirdCry";

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = false; // 仅用于绑定逻辑，不直接造成伤害
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false; // 不与地形碰撞
            Projectile.timeLeft = 2; // 持续刷新
            Projectile.netImportant = true;
        }

        private int linkedProj = -1; // 用于绑定的子弹幕ID

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // 检查玩家是否取消了释放
            //if (player.channel && player.HeldItem.type == ModContent.ItemType<BlindBirdCry>())
            {
                Projectile.timeLeft = 2; // 持续刷新时间
                Projectile.Center = player.MountedCenter; // 投射物跟随玩家
                if (linkedProj == -1 || !Main.projectile[linkedProj].active || Main.projectile[linkedProj].type != ModContent.ProjectileType<BlindBirdCryINVPROJ>())
                {
                    // 如果子弹幕未生成或无效，则生成新的子弹幕
                    linkedProj = Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<BlindBirdCryINVPROJ>(),
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner
                    );
                }
            }
            //else
            {
                // 玩家松开左键或切换武器时，销毁当前投射物和绑定的子弹幕
                if (linkedProj != -1 && Main.projectile[linkedProj].active && Main.projectile[linkedProj].type == ModContent.ProjectileType<BlindBirdCryINVPROJ>())
                {
                    Main.projectile[linkedProj].Kill();
                }
                Projectile.Kill();
            }
        }

        public override bool ShouldUpdatePosition() => false; // 禁止更新位置（由AI控制位置）
    }
}
