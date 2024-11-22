using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Typeless;

namespace FKsCRE.Content.DeveloperItems.Arrow.ExplodingRabbit
{
    public class ExplodingRabbitPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.DPreDog";
        public override string Texture => "FKsCRE/Content/DeveloperItems/Arrow/ExplodingRabbit/ExplodingRabbit";
        private int bounceCount = 0; // Tracks the number of bounces

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesFromEdge(Projectile, 0, Color.White);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 420;
            Projectile.MaxUpdates = 2;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
            Projectile.aiStyle = ProjAIStyleID.Arrow; // 让弹幕受到重力影响
        }

        public override void AI()
        {
            // 由于我们是水平贴图，因此什么也不需要转动
            Projectile.rotation = Projectile.velocity.ToRotation();

            // 添加光效
            Lighting.AddLight(Projectile.Center, Color.Lerp(Color.White, Color.WhiteSmoke, 0.5f).ToVector3() * 0.49f);

            // 子弹在出现之后很短一段时间会变得可见
            if (Projectile.timeLeft == 419)
                Projectile.alpha = 0;
        }
        public override void OnKill(int timeLeft)
        {
            // 创建爆炸弹幕
            int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FuckYou>(), (int)(Projectile.damage * 1.0f), Projectile.knockBack, Projectile.owner);
            // 修改爆炸弹幕的大小为原来的两倍
            Main.projectile[projID].scale = 2.0f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Bounce logic
            if (bounceCount >= 6)
                return true; // Kill projectile after 6 bounces

            bounceCount++;
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X; // Reflect X velocity

            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y; // Reflect Y velocity

            // Smoke effect
            CreateSmokeEffect();

            // Square particle effect
            CreateSquareParticleEffect();

            return false; // Prevent the projectile from being destroyed
        }

        private void CreateSmokeEffect()
        {
            for (int i = 0; i < 4; i++)
            {
                int type = Main.rand.Next(61, 64); // Random smoke gore
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                Gore gore = Main.gore[Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, velocity, type, 1f)];
                gore.velocity *= 0.5f; // Adjust velocity multiplier
            }
        }

        private void CreateSquareParticleEffect()
        {
            int particleCount = 100; // 粒子总数量
            float squareSize = 100f; // 正方形的最大扩散范围（像素）

            for (int i = 0; i < particleCount; i++)
            {
                // 随机选择粒子类型 31 或 35
                int type = Main.rand.Next(new int[] { 31, 35 });

                // 生成粒子的初始速度，速度范围内生成随机值
                Vector2 velocity = new Vector2(
                    Main.rand.NextFloat(-squareSize, squareSize),
                    Main.rand.NextFloat(-squareSize, squareSize)
                ).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(2f, 5f);

                // 创建粒子
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, type);
                dust.velocity = velocity;
                dust.scale = 1.5f; // 粒子的缩放比例
                dust.noGravity = true; // 让粒子不受重力影响
            }
        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }

    }
}
