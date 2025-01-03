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

namespace FKsCRE.Content.WeaponToAMMO.Bullet.TheEmpty
{
    public class TheEmptyPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "WeaponToAMMO.Bullet.TheEmpty";
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
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = Main.getGoodWorld ? -1 : 1; // 根据模式设置穿透次数
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
                float radius = 1 * 16f; // 半径为1格
                foreach (float initialAngle in angles)
                {
                    float angle = initialAngle + rotationAngle;
                    Vector2 position = Projectile.Center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                    int dust = Dust.NewDust(position, 0, 0, DustID.UltraBrightTorch, 0f, 0f, 100, default, Main.rand.NextFloat(1.5f, 2.5f));
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity = Vector2.Zero;
                }
            }

            // 追踪逻辑
            if (Main.getGoodWorld || Projectile.ai[1] > 15)
            {
                // 根据模式决定追踪范围和速度
                float trackingRange = Main.getGoodWorld ? 3800f : 1800f;
                float trackingSpeed = Main.getGoodWorld ? 15f : 12f;

                NPC target = Projectile.Center.ClosestNPCAt(trackingRange);
                if (target != null)
                {
                    Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * trackingSpeed, 0.08f);
                }
            }
            else
            {
                Projectile.ai[1]++;
            }
        }


        // 新增字典用于记录每个敌人的变更方向和原始缩放比例
        private Dictionary<int, bool> sizeChangeDirection = new Dictionary<int, bool>();
        private Dictionary<int, float> originalScale = new Dictionary<int, float>();

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 如果这是第一次击中该敌人，随机决定变大或变小，并记录原始缩放比例
            if (!sizeChangeDirection.ContainsKey(target.whoAmI))
            {
                sizeChangeDirection[target.whoAmI] = Main.rand.NextBool(); // true 表示变大，false 表示变小
                originalScale[target.whoAmI] = target.scale; // 记录原始缩放比例
            }

            // 根据记录决定变大或变小的逻辑
            float scaleChangeFactor = sizeChangeDirection[target.whoAmI] ? 1.01f : 0.99f;
            target.scale *= scaleChangeFactor;
            target.width = (int)(originalScale[target.whoAmI] * target.width * scaleChangeFactor);
            target.height = (int)(originalScale[target.whoAmI] * target.height * scaleChangeFactor);

            target.netUpdate = true; // 确保网络同步
        }




    }
}