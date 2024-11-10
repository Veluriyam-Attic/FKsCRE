using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
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
using Terraria.Audio;
using FKsCRE.CREConfigs;

namespace FKsCRE.Content.DeveloperItems.MaoMaoChong
{
    public class MaoMaoChongPROJ : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Projectile.type] = 2;

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
            Projectile.penetrate = 9; // 允许9次伤害
            Projectile.timeLeft = 300;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true; // 允许与方块碰撞
            Projectile.extraUpdates = 1; // 额外更新次数
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 14; // 无敌帧冷却时间为14帧
            Projectile.aiStyle = ProjAIStyleID.Arrow; // 让弹幕受到重力影响

        }

        public override void AI()
        {
            // 保持弹幕旋转（对于倾斜走向的弹幕而言）
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Lighting - 将光源颜色更改为偏黑的深蓝色，光照强度为 0.55
            Lighting.AddLight(Projectile.Center, new Vector3(0.1f, 0.1f, 0.5f) * 0.55f);

            // 动画处理
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 1) // 将 3 改为 1，因为只有两帧
            {
                Projectile.frame = 0;
            }
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 在飞行路径上生成白色电能粒子特效
                int particleCount = 5; // 控制每帧生成的粒子数量
                for (int i = 0; i < particleCount; i++)
                {
                    float angle = Main.rand.NextFloat(0, MathHelper.TwoPi); // 随机角度
                    float distance = Main.rand.NextFloat(2f, 10f); // 控制粒子生成的随机偏移范围
                    Vector2 spawnPosition = Projectile.Center + distance * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Vector2 velocity = Vector2.Normalize(spawnPosition - Projectile.Center) * Main.rand.NextFloat(0.5f, 1.5f); // 随机速度

                    // 创建电能粒子特效
                    Dust dust = Dust.NewDustPerfect(spawnPosition, DustID.Electric, velocity, 150, Color.White, Main.rand.NextFloat(0.8f, 1.5f));
                    dust.noGravity = true; // 使粒子不受重力影响
                    dust.fadeIn = 1f;
                }
            }
   
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 计算反弹的方向，遵循入射角等于出射角的原则
            Vector2 reflectDirection = Vector2.Reflect(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center));
            Projectile.velocity = reflectDirection;

            // 播放随机音效（Item57 或 Item58）
            SoundStyle[] sounds = { SoundID.Item57, SoundID.Item58 };
            SoundEngine.PlaySound(sounds[Main.rand.Next(sounds.Length)], Projectile.position);

            // 生成白色电能粒子特效
            int particleCount = 100; // 粒子数量
            for (int i = 0; i < particleCount; i++)
            {
                float angle = Main.rand.NextFloat(0, MathHelper.TwoPi); // 随机角度
                float distance = Main.rand.NextFloat(5f, 50f); // 随机距离，控制扩散范围
                Vector2 spawnPosition = Projectile.Center + distance * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                Vector2 velocity = Vector2.Normalize(spawnPosition - Projectile.Center) * Main.rand.NextFloat(1f, 3f); // 随机速度

                // 创建电能粒子特效
                Dust dust = Dust.NewDustPerfect(spawnPosition, DustID.Electric, velocity, 150, Color.White, Main.rand.NextFloat(1f, 2f));
                dust.noGravity = true; // 使粒子不受重力影响
                dust.fadeIn = 1f;
            }
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // 碰撞物块时反弹
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;

            // 播放喵喵叫音效
            // 播放随机音效（Item57 或 Item58）
            SoundStyle[] sounds = { SoundID.Item57, SoundID.Item58 };
            SoundEngine.PlaySound(sounds[Main.rand.Next(sounds.Length)], Projectile.position);

            // 每次反弹减少一次穿透次数
            Projectile.penetrate--;

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 生成白色电能粒子特效
                int particleCount = 40; // 粒子数量
                for (int i = 0; i < particleCount; i++)
                {
                    float angle = Main.rand.NextFloat(0, MathHelper.TwoPi); // 随机角度
                    float distance = Main.rand.NextFloat(5f, 50f); // 随机距离，控制扩散范围
                    Vector2 spawnPosition = Projectile.Center + distance * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Vector2 velocity = Vector2.Normalize(spawnPosition - Projectile.Center) * Main.rand.NextFloat(1f, 3f); // 随机速度

                    // 创建电能粒子特效
                    Dust dust = Dust.NewDustPerfect(spawnPosition, DustID.Electric, velocity, 150, Color.White, Main.rand.NextFloat(1f, 2f));
                    dust.noGravity = true; // 使粒子不受重力影响
                    dust.fadeIn = 1f;
                }
            }
               

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            //// 在弹幕消失时释放深蓝色粒子特效
            //for (int i = 0; i < 15; i++)
            //{
            //    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default, 2.2f);
            //}
        }



    }
}