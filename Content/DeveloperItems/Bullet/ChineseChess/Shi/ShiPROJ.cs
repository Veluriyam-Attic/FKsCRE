using CalamityMod;
using FKsCRE.CREConfigs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace FKsCRE.Content.DeveloperItems.Bullet.ChineseChess.Shi
{
    internal class ShiPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.ChineseChess.Shi";
        public override string Texture => "FKsCRE/Content/DeveloperItems/Bullet/ChineseChess/Shi/Shi";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 画残影效果
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
                return false;
            }
            return true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 450;
            Projectile.MaxUpdates = 4;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        private float orbitRadius = 3 * 16f; // 公转半径
        private float orbitSpeed = 1f / 60f; // 公转速度
        private float currentAngle = 0f; // 当前角度偏移

        public override void AI()
        {
            // 子弹旋转逻辑
            Projectile.rotation = Projectile.velocity.ToRotation();
            // 子弹在短时间后变得可见
            if (Projectile.timeLeft == 445)
                Projectile.alpha = 0;

            // 确保玩家存在
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // 更新角度
            currentAngle += orbitSpeed;

            // 计算公转位置
            Vector2 orbitCenter = player.Center;
            Vector2 offset = new Vector2(
                (float)Math.Cos(currentAngle),
                (float)Math.Sin(currentAngle)
            ) * orbitRadius;

            Projectile.Center = orbitCenter + offset;

            // 生成粒子效果（91号GemDiamond）
            if (Main.rand.NextBool(2)) // 1/2 概率
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.GemDiamond,
                    Vector2.Zero,
                    100,
                    default,
                    Main.rand.NextFloat(0.8f, 1.2f)
                );
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }
        }


        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 为敌人添加 BuffID.Ichor
            target.AddBuff(BuffID.Ichor, 300); // 持续时间为 5 秒（300 帧）
        }

        public override void OnKill(int timeLeft)
        {

        }
    }
}
