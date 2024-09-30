using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Particles;
using CalamityMod;

namespace FKsCRE.Content.Arrows.WulfrimArrow
{
    internal class WulfrimArrowPROJ : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // 设置拖尾长度和模式
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.penetrate = 1; // 穿透次数设为1
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true; // 箭矢类弹幕
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14; // 本地无敌帧冷却时间
            Projectile.aiStyle = ProjAIStyleID.Arrow; // 让弹幕受到重力影响
        }

        public override void AI()
        {
            // 控制旋转方向
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.velocity.Y += 0.05f; // 逐渐加速 y 轴速度

            //// 运动轨迹，类似 ForbiddenAxeBlade 的加速和减速
            //Projectile.ai[0]++;
            //if (Projectile.ai[0] <= 20f)
            //{
            //    Projectile.velocity *= 0.95f; // 减速
            //}
            //else if (Projectile.ai[0] > 20f && Projectile.ai[0] <= 40f)
            //{
            //    Projectile.velocity *= 1.05f; // 加速
            //}
            //else if (Projectile.ai[0] > 40f)
            //{
            //    Projectile.ai[0] = 0f; // 重置
            //}

            //// 生成双螺旋粒子效果
            //float sine = (float)Math.Sin(Projectile.timeLeft * 0.1875f / MathHelper.Pi); // 绘制速度减半
            //Vector2 offset = Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedBy(MathHelper.PiOver2) * sine * 32f; // 振动幅度扩大 100%

            //// Electric 粒子特效 (白蓝色)
            //Dust electricDust = Dust.NewDustPerfect(Projectile.Center + offset, 226, Vector2.Zero);
            //electricDust.color = Color.LightBlue;
            //electricDust.noGravity = true;

            //// WulfrumBolt 粒子特效 (保持原色)
            //Dust wulfrumDust = Dust.NewDustPerfect(Projectile.Center - offset, 267, Vector2.Zero);
            //wulfrumDust.noGravity = true;



            // 释放亮绿色粒子特效
            if (Main.rand.NextBool(5))
            {
                // 直接在弹幕中心生成粒子，没有左右偏移
                Vector2 trailPos = Projectile.Center;

                float trailScale = Main.rand.NextFloat(0.8f, 1.2f); // 维持粒子的缩放效果
                Color trailColor = Color.LimeGreen; // 固定颜色为亮绿色

                // 创建粒子
                Particle trail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, trailColor);
                GeneralParticleHandler.SpawnParticle(trail);
            }




            //// 产生DNA形状的粒子特效
            //float frequency = 30f;  // 30帧一个回合
            //float amplitude = 20f;  // 振动幅度

            //// 左侧和右侧的偏移计算
            //Vector2 leftOffset = new Vector2(-amplitude * (float)Math.Sin(Projectile.ai[0] * MathHelper.TwoPi / frequency), 0);
            //Vector2 rightOffset = new Vector2(amplitude * (float)Math.Sin(Projectile.ai[0] * MathHelper.TwoPi / frequency), 0);

            //if (Projectile.ai[0] % 2 == 0)  // 每两帧产生一次粒子
            //{
            //    // Electric 粒子特效 (白蓝色)
            //    Dust.NewDustPerfect(Projectile.Center + leftOffset, 226, Vector2.Zero, 0, Color.LightBlue, 1.2f).noGravity = true;

            //    // WulfrumBolt 粒子特效 (保持原色)
            //    Dust.NewDustPerfect(Projectile.Center + rightOffset, 267, Vector2.Zero, 0, default(Color), 1.2f).noGravity = true;
            //}

            //// 更新ai，两倍的绘制速度
            //Projectile.ai[0] += 1f;


            // 添加光效
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.5f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // 画残影效果
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 判断是否为Boss，如果不是Boss，才施加WulfrimArrowEBuff
            if (!target.boss)
            {
                // 击中非Boss敌人时添加 WulfrimArrowEBuff，持续 0.25 秒
                target.AddBuff(ModContent.BuffType<WulfrimArrowEBuff>(), 15);
            }

            // 生成大量向上抛射的电能粒子特效
            for (int i = 0; i < 10; i++) // 生成 10 个粒子
            {
                Vector2 velocity = new Vector2(0, -1f).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(2f, 5f); // 随机角度和速度
                Dust electricDust = Dust.NewDustPerfect(target.Center, 226, velocity); // 226 为 Electric 粒子特效的编号
                electricDust.color = Color.LightGreen; // 设定颜色为浅绿色
                electricDust.noGravity = true; // 无重力效果，使其向上抛射
                electricDust.scale = Main.rand.NextFloat(1.2f, 1.8f); // 随机缩放
            }


        }









    }
}