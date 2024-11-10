using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using FKsCRE.CREConfigs;
using Terraria.Audio;

namespace FKsCRE.Content.DeveloperItems.TheDrill
{
    public class TheDrillPROJ : ModProjectile
    {
        public override string Texture => "FKsCRE/Content/DeveloperItems/TheDrill/TheDrill";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1; // 只允许一次伤害
            Projectile.timeLeft = 240;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true; // 允许与方块碰撞
            Projectile.extraUpdates = 1; // 额外更新次数
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 14; // 无敌帧冷却时间为14帧
        }

        public override bool? CanDamage()
        {
            // 永久关闭伤害检测
            return false;
        }


        public override void AI()
        {
            // 调整弹幕的旋转，使其在飞行时保持水平
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
            // Lighting - 添加深橙色光源，光照强度为 0.55
            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 0.55f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item22, Projectile.position);

            // 销毁方块
            int range = 3; // 控制破坏的范围
            int minX = (int)(Projectile.Center.X / 16f - range);
            int maxX = (int)(Projectile.Center.X / 16f + range);
            int minY = (int)(Projectile.Center.Y / 16f - range);
            int maxY = (int)(Projectile.Center.Y / 16f + range);

            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    if (Main.tile[i, j] != null && Main.tile[i, j].HasTile)
                    {
                        WorldGen.KillTile(i, j, false, false, false);
                        if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                        {
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
                        }
                    }
                }
            }

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 生成大量的橙色火把和岩浆粒子特效
                for (int k = 0; k < 50; k++)
                {
                    Vector2 position = Projectile.Center + Main.rand.NextVector2Circular(16f, 16f);
                    Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);

                    // 创建橙色火把粒子
                    int dustTorch = Dust.NewDust(position, 0, 0, DustID.Torch, velocity.X, velocity.Y, 100, default, Main.rand.NextFloat(1.5f, 2.5f));
                    Main.dust[dustTorch].noGravity = true;

                    // 创建岩浆粒子
                    int dustLava = Dust.NewDust(position, 0, 0, DustID.Lava, velocity.X, velocity.Y, 100, default, Main.rand.NextFloat(1.5f, 2.5f));
                    Main.dust[dustLava].noGravity = true;
                }
            }              

            return true; // 保持原有的碰撞处理
        }


    }
}