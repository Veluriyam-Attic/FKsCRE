﻿using CalamityMod;
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
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Graphics.Primitives;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using CalamityMod.Particles;

namespace FKsCRE.Content.Ammunition.EAfterDog.MiracleMatterBullet
{
    public class MiracleMatterBulletPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.EAfterDog";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public static int MaxUpdate = 5; // 定义一个静态变量，表示弹幕每次更新的最大次数
        private int Lifetime = 1100; // 定义弹幕的生命周期为x

        // 更改颜色：深绿色、黑色、另一种深绿色
        private static Color ShaderColorOne = Color.Cyan; // 着色器颜色1，设置为深绿色
        private static Color ShaderColorTwo = Color.Lime; // 着色器颜色2，设置为黑色
        private static Color ShaderEndColor = Color.White; // 着色器结束颜色，设置为森林绿色（另一种深绿色）

        private Vector2 altSpawn; // 定义一个备用生成位置向量

        public override void SetStaticDefaults() // 设置弹幕的静态默认值
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2; // 设置拖尾模式为2
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40; // 设置拖尾缓存长度为x
        }

        public override void SetDefaults() // 设置弹幕的默认值
        {
            Projectile.width = Projectile.height = 24;
            Projectile.arrow = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = Lifetime;
            Projectile.MaxUpdates = MaxUpdate;
            Projectile.penetrate = 201;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
        }

        private bool collidedWithNPC = false; // 是否与敌人发生过碰撞
        private int collisionTimer = 0; // 碰撞后的计时器

        // 刚出现的前15帧不追踪敌人
        public override void AI()
        {
            Projectile.ai[0]++; // 弹幕AI计数器递增

            if (Projectile.timeLeft <= 5)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(9, 9) - Projectile.velocity * 5, DustID.GemDiamond, Projectile.velocity * 30 * Main.rand.NextFloat(0.1f, 0.95f));
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(0.9f, 1.45f);
                dust.alpha = 235;
                dust.color = Color.White;
            }

            Player player = Main.player[Projectile.owner];
            Projectile.localAI[0] += 1f / (Projectile.extraUpdates + 1);

            // 随机飞行逻辑
            if (Projectile.ai[1] != 1 && !collidedWithNPC)
            {
                bool turnLeft = Main.rand.NextBool();
                float turnIncrement = MathHelper.ToRadians(1.5f); // 每帧转向增量
                float currentTurn = turnLeft ? -turnIncrement : turnIncrement; // 随机方向
                Projectile.velocity = Projectile.velocity.RotatedBy(currentTurn);
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 7.5f;
            }

            // 检测与敌人的碰撞
            if (!collidedWithNPC)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.CanBeChasedBy() &&
                        Projectile.Hitbox.Intersects(npc.Hitbox))
                    {
                        collidedWithNPC = true; // 标记碰撞
                        collisionTimer = (int)Main.GameUpdateCount; // 记录当前帧数
                        break;
                    }
                }
            }

            // 开启追踪模式
            if (collidedWithNPC && (int)Main.GameUpdateCount - collisionTimer >= 60)
            {
                Projectile.ai[1] = 1; // 开启追踪模式
            }

            // 追踪模式逻辑
            if (Projectile.ai[1] == 1 || Projectile.ai[0] > 300) // 手动开启或飞行时间超过150帧
            {
                NPC target = Projectile.Center.ClosestNPCAt(3800); // 查找范围内最近的敌人
                if (target != null)
                {
                    Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 12f, 0.08f); // 平滑追踪
                }
            }


            // 造成了一次伤害之后就直接关闭伤害检测并快速降低速度，并让自己停下来
            if (Projectile.penetrate < 200)
            {
                if (Projectile.timeLeft > 60) { Projectile.timeLeft = 60; } //The projectile start shrinking and slowing down. it can still hit for a bit during this, to allow a bit of multi-target if the enemies are really close to eachother.
                Projectile.velocity *= 0.88f;
            }

        }


        public override bool? CanDamage()
        {
            // 如果未进入追踪模式或未经过延迟，则不允许造成伤害
            return collidedWithNPC && (int)Main.GameUpdateCount - collisionTimer >= 10 ? (bool?)true : (bool?)false;
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
            //Projectile.ai[1] = 1; // 标记追踪逻辑开启
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[1] = 1; // 标记追踪逻辑开启
            return base.OnTileCollide(oldVelocity);
        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }

        public override void OnKill(int timeLeft)
        {
            // 在原地生成一个随机方向的旋转粒子特效
            for (int i = 0; i < 2; i++) // 生成 8 个粒子
            {
                // 随机偏移量
                Vector2 sparkOffset = Main.rand.NextVector2Circular(10f, 10f); // 随机半径为10像素的圆形偏移

                // 随机初始颜色和结束颜色
                Color startColor = Color.LightBlue * 0.5f; // 初始颜色（亮度减半）
                Color endColor = Color.White * 0.5f;      // 结束颜色（亮度减半）

                // 创建旋转粒子特效
                GenericSparkle sparker = new GenericSparkle(
                    Projectile.Center + sparkOffset,       // 粒子起始位置
                    Vector2.Zero,                          // 粒子初速度
                    startColor,                            // 起始颜色
                    endColor,                              // 结束颜色
                    Main.rand.NextFloat(2.5f, 2.9f),       // 粒子大小
                    14,                                    // 生命周期
                    Main.rand.NextFloat(-0.01f, 0.01f),    // 随机旋转速度
                    2.5f                                   // 粒子亮度
                );

                GeneralParticleHandler.SpawnParticle(sparker); // 生成粒子
            }
        }


        private float PrimitiveWidthFunction(float completionRatio)
        {
            float arrowheadCutoff = 0.36f;
            float width = 24f;
            float minHeadWidth = 0.03f;
            float maxHeadWidth = width;
            if (completionRatio <= arrowheadCutoff)
                width = MathHelper.Lerp(minHeadWidth, maxHeadWidth, Utils.GetLerpValue(0f, arrowheadCutoff, completionRatio, true));
            return width;
        }

        private Color PrimitiveColorFunction(float completionRatio)
        {
            float endFadeRatio = 0.41f;
            float completionRatioFactor = 2.7f;
            float globalTimeFactor = 5.3f;
            float endFadeFactor = 3.2f;
            float endFadeTerm = Utils.GetLerpValue(0f, endFadeRatio * 0.5f, completionRatio, true) * endFadeFactor;
            float cosArgument = completionRatio * completionRatioFactor - Main.GlobalTimeWrappedHourly * globalTimeFactor + endFadeTerm;
            float startingInterpolant = (float)Math.Cos(cosArgument) * 0.5f + 0.5f;

            float colorLerpFactor = 0.6f;
            Color startingColor = Color.Lerp(ShaderColorOne, ShaderColorTwo, startingInterpolant * colorLerpFactor);
            return Color.Lerp(startingColor, ShaderEndColor, MathHelper.SmoothStep(0f, 1f, Utils.GetLerpValue(0f, endFadeRatio, completionRatio, true)));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            GameShaders.Misc["CalamityMod:TrailStreak"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/FabstaffStreak"));
            Vector2 overallOffset = Projectile.Size * 0.5f;
            overallOffset += Projectile.velocity * 1.4f;
            int numPoints = 46;
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(PrimitiveWidthFunction, PrimitiveColorFunction, (_) => overallOffset, shader: GameShaders.Misc["CalamityMod:TrailStreak"]), numPoints);
            return false;
        }





    }
}