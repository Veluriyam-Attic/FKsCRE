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

namespace FKsCRE.Content.DeveloperItems.TheEmpty
{
    public class TheEmptyPROJ : ModProjectile
    {
        private float rotationAngle = 0f; // 用于粒子旋转的角度
        private const float rotationSpeed = 0.05f; // 粒子旋转速度
        private static Dictionary<int, bool> sizeChangeRegistry = new Dictionary<int, bool>(); // 记录已改变大小的敌人

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

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
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        public override void AI()
        {
            // 保持弹幕旋转（对于倾斜走向的弹幕而言）
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            // Lighting - 添加深橙色光源，光照强度为 0.55
            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 0.55f);

            // 生成淡蓝色粒子环绕效果
            rotationAngle += rotationSpeed;
            if (rotationAngle > MathHelper.TwoPi)
            {
                rotationAngle -= MathHelper.TwoPi;
            }


            // 这个特效不可关闭，因为子弹的贴图是透明的，关闭了之后子弹就看不到了
            {
                // 粒子位置计算，形成等边三角形环绕
                float[] angles = { 0f, MathHelper.TwoPi / 3f, MathHelper.TwoPi * 2f / 3f };
                float radius = 2 * 16f; // 半径为2格
                foreach (float initialAngle in angles)
                {
                    float angle = initialAngle + rotationAngle;
                    Vector2 position = Projectile.Center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                    int dust = Dust.NewDust(position, 0, 0, DustID.BlueCrystalShard, 0f, 0f, 100, default, Main.rand.NextFloat(1.5f, 2.5f));
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity = Vector2.Zero;
                }
            }
            

            // 追踪逻辑，前15帧不追踪
            if (Projectile.ai[1] > 15)
            {
                NPC target = Projectile.Center.ClosestNPCAt(1800);
                if (target != null)
                {
                    Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 12f, 0.08f);
                }
            }
            else
            {
                Projectile.ai[1]++;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 检查阀门状态，确保只改变一次大小
            if (!sizeChangeRegistry.ContainsKey(target.whoAmI))
            {
                // 50% 概率使敌人变小或变大
                if (Main.rand.NextBool(2))
                {
                    // 变小
                    target.scale *= 0.5f;
                    target.width = (int)(target.width * target.scale);
                    target.height = (int)(target.height * target.scale);
                }
                else
                {
                    // 变大
                    target.scale *= 2f;
                    target.width = (int)(target.width * 2f);
                    target.height = (int)(target.height * 2f);
                }
                target.netUpdate = true; // 确保网络同步

                // 记录该敌人已改变大小
                sizeChangeRegistry[target.whoAmI] = true;
            }
        }


    }
}