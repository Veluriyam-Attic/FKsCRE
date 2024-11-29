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

namespace FKsCRE.Content.DeveloperItems.Bullet.ChineseChess.Ma
{
    internal class MaPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.ChineseChess.Ma";
        public override string Texture => "FKsCRE/Content/DeveloperItems/Bullet/ChineseChess/Ma/Ma";

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
            Projectile.penetrate = 1;
            Projectile.timeLeft = 450;
            Projectile.MaxUpdates = 4;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        private bool turnLeft = true; // 用于控制左右拐弯
        private int frameCounter = 0; // 帧计数器

        public override void AI()
        {
            // 子弹旋转逻辑
            Projectile.rotation = Projectile.velocity.ToRotation();

            // 子弹在短时间后变得可见
            if (Projectile.timeLeft == 445)
                Projectile.alpha = 0;

            // 每飞行 20 帧进行拐弯
            frameCounter++;
            if (frameCounter >= 20)
            {
                frameCounter = 0; // 重置计数器

                // 随机获取 20~80 度的拐弯角度
                float turnAngle = MathHelper.ToRadians(Main.rand.Next(20, 81));

                // 如果当前为左拐，角度为正，反之为负
                turnAngle = turnLeft ? turnAngle : -turnAngle;

                // 修改弹幕的速度方向
                Projectile.velocity = Projectile.velocity.RotatedBy(turnAngle);

                // 在拐弯的反方向释放烟雾粒子
                Vector2 smokeDirection = Projectile.velocity.RotatedBy(MathHelper.Pi); // 反方向
                for (int i = 0; i < 15; i++)
                {
                    Vector2 smokeVelocity = smokeDirection.RotatedByRandom(MathHelper.ToRadians(15)) * Main.rand.NextFloat(1f, 3f); // 随机化速度
                    Dust smoke = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, smokeVelocity, 100, default, Main.rand.NextFloat(1.0f, 1.5f));
                    smoke.noGravity = true;
                }

                // 切换拐弯方向
                turnLeft = !turnLeft;
            }

            // 飞行过程中添加粒子效果
            for (int i = -1; i <= 1; i += 2) // 往两边排开
            {
                Vector2 offset = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90) * i) * 1f; // 左右偏移 10 像素
                Dust trail = Dust.NewDustPerfect(Projectile.Center + offset, DustID.Terra, Vector2.Zero, 100, default, Main.rand.NextFloat(1.0f, 1.5f));
                trail.noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner]; // 获取弹幕所属的玩家

            // 检查玩家是否骑乘坐骑
            if (player.mount.Active) // 判断玩家是否处于骑乘状态
            {
                Projectile.damage = (int)(Projectile.originalDamage * 1.5f); // 增加 50% 伤害
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }

        public override void OnKill(int timeLeft)
        {

        }
    }
}
