using CalamityMod.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace FKsCRE.Content.DeveloperItems.Weapon.BlindBirdCry
{
    internal class BlindBirdCryCrods : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.BlindBirdCry";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type]; // 计算每帧的高度
            int frameY = Projectile.frame * frameHeight; // 获取当前帧在纹理中的起始Y坐标
            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight); // 定义绘制的源矩形
            Vector2 drawOrigin = new Vector2(texture.Width / 2, frameHeight / 2); // 计算绘制的中心点

            Vector2 drawPosition = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            // 判断图像是否需要翻转
            SpriteEffects spriteEffects = Projectile.velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // 绘制
            spriteBatch.Draw(texture, drawPosition, sourceRectangle, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0f);
            return false;
        }


        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 200;
            Projectile.extraUpdates = 7;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 50;
            Projectile.DamageType = DamageClass.Ranged;
        }


        public override void OnSpawn(IEntitySource source)
        {


        }
        public bool ableToHit = true;
        public ref float Time => ref Projectile.ai[1];
        public override bool? CanDamage() => Time >= 80f; // 初始的时候不会造成伤害，直到x为止

        public override void AI()
        {
            // 适用于六帧动图的切换逻辑
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4) // 调整这个值可以控制帧切换的速度
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 4) // 5帧动图时，最大帧数为 4
            {
                Projectile.frame = 0;
            }


            if (Projectile.localAI[0] < 60) // 前 1 秒飞行
            {
                if (Projectile.localAI[0] % 3 == 0) // 每 x 帧
                {
                    //Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2);
                    //Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(4));
                    Projectile.velocity *= 0.85f;
                }
                //Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 1.0f;
            }
            else // 使用新的追踪逻辑
            {
                NPC target = Projectile.Center.ClosestNPCAt(5800);
                if (target != null)
                {
                    Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 10f, 0.08f);
                }
            }

            if (Projectile.penetrate < 200) // 如果弹幕已经击中敌人，停止追踪能力
            {
                if (Projectile.timeLeft > 60) { Projectile.timeLeft = 60; } // 弹幕开始缩小并减速
                Projectile.velocity *= 0.88f;
            }

            if (Projectile.timeLeft <= 20) // 弹幕即将消失时停止造成伤害
            {
                ableToHit = false;
            }
            Time++;


            // 前30帧不追踪，之后开始追踪敌人
            if (Projectile.ai[1] > 130)
            {
                NPC target = Projectile.Center.ClosestNPCAt(5000); // 查找范围内最近的敌人
                if (target != null)
                {
                    Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 10f, 0.08f); // 追踪速度为xf
                }

                // 触发追踪能力激活的效果
                if (Projectile.ai[1] == 120)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        float angle = MathHelper.ToRadians(120 * i);
                        Vector2 particleVelocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 3f;
                        Particle spark = new PointParticle(Projectile.Center, particleVelocity, false, 7, 1.5f, Color.LightYellow);
                        GeneralParticleHandler.SpawnParticle(spark);
                    }
                }
            }
            else
            {
                Projectile.ai[1]++;
            }
            

            // 在飞行期间生成浅黄色的粒子特效
            if (Main.rand.NextBool(1)) // 随机生成粒子
            {
                Vector2 dustPosition = Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Projectile.height));
                Dust dust = Dust.NewDustDirect(dustPosition, 0, 0, DustID.Torch, 0f, 0f, 100, Color.LightYellow, 1.5f);
                dust.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            // 在死亡时向自己面向的反方向抛射出 15 个粉红色的原版粒子特效（Dust）
            for (int i = 0; i < 15; i++)
            {
                float angle = MathHelper.ToRadians(Main.rand.NextFloat(-45f, 45f)); // 在 -45 到 45 度之间随机生成角度
                Vector2 dustVelocity = Projectile.velocity.RotatedBy(angle) * -0.5f; // 反方向速度，加上随机旋转
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PinkTorch, dustVelocity.X, dustVelocity.Y);
                dust.noGravity = true;
                dust.scale = 1.5f;
            }

            for (int i = 0; i < 3; i++)
            {
                float angle = MathHelper.ToRadians(120 * i);
                Vector2 particleVelocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 3f;
                Particle spark = new PointParticle(Projectile.Center, particleVelocity, false, 7, 1.5f, Color.Yellow);
                GeneralParticleHandler.SpawnParticle(spark);
            }

            SoundEngine.PlaySound(SoundID.NPCHit51, Projectile.Center);
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {


        }



    }


}