using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Dusts;
using CalamityMod.Projectiles.Typeless;
using CalamityMod;
using Terraria.ID;
using Terraria.Map;
using Terraria.Audio;
using FKsCRE.CREConfigs;

namespace FKsCRE.Content.Arrows.CPreMoodLord.AstralArrow
{
    public class AstralArrowPROJ : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // 设置拖尾长度和模式
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14; // 假设箭的宽度
            Projectile.height = 28; // 假设箭的高度
            Projectile.friendly = true; // 是否对玩家友好
            Projectile.DamageType = DamageClass.Ranged; // 远程伤害类型
            Projectile.arrow = true; // 标记为箭类型
            Projectile.extraUpdates = 0; // 弹幕额外更新次数
            Projectile.ignoreWater = true; // 不受水影响
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 14; // 无敌帧冷却时间为14帧
            Projectile.penetrate = 1; // 只穿透一次敌人
            Projectile.tileCollide = true; // 不穿透方块
            Projectile.timeLeft = 300; // 存在时间
            Projectile.light = 0.9f; // 设置发光亮度
        }

        public override void AI()
        {
            // 保持弹幕旋转
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;

            // Lighting - 添加深粉红色光源，光照强度为 0.55
            Lighting.AddLight(Projectile.Center, Color.DeepPink.ToVector3() * 0.55f);

            // 弹幕保持直线运动并逐渐加速
            Projectile.velocity *= 1.02f;

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 粒子特效：生成彩虹色粒子和橘色粒子
                if (Main.rand.NextBool(8))
                {
                    int spawnDustAmount = 2;
                    for (int i = 0; i < spawnDustAmount; i++)
                    {
                        // 原有彩虹色粒子
                        Color newColor = Main.hslToRgb(0.5f, 1f, 0.5f); // 彩虹色粒子
                        int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowMk2, 0f, 0f, 0, newColor);
                        Main.dust[dust].position = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * 0.5f;
                        Main.dust[dust].velocity *= Main.rand.NextFloat() * 0.8f;
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].fadeIn = 0.6f + Main.rand.NextFloat();
                        Main.dust[dust].velocity += Projectile.velocity.SafeNormalize(Vector2.UnitY) * 3f;
                        Main.dust[dust].scale = 0.7f;
                    }

                    //// 新增绿蓝色粒子
                    //for (int i = 0; i < spawnDustAmount; i++)
                    //{
                    //    // 生成绿蓝色粒子
                    //    Color newColor = Main.hslToRgb(0.05f, 1f, 0.5f); // 橙红色粒子
                    //    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDust, 0f, 0f, 0, newColor);
                    //    Main.dust[dust].position = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * 0.5f;
                    //    Main.dust[dust].velocity *= Main.rand.NextFloat() * 0.8f;
                    //    Main.dust[dust].noGravity = true;
                    //    Main.dust[dust].fadeIn = 0.6f + Main.rand.NextFloat();
                    //    Main.dust[dust].velocity += Projectile.velocity.SafeNormalize(Vector2.UnitY) * 3f;
                    //    Main.dust[dust].scale = 0.7f;
                    //}

                    // 生成橘黄色粒子
                    Vector2 velocity = Vector2.UnitX.RotatedByRandom(MathHelper.PiOver2).RotatedBy(Projectile.velocity.ToRotation());
                    int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150);
                    Main.dust[idx].velocity = velocity * 0.33f;
                    Main.dust[idx].position = Projectile.Center + velocity * 6f;
                }

                if (Main.rand.NextBool(24) && Main.netMode != NetmodeID.Server)
                {
                    int idx = Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity * 0.1f, 16, 1f);
                    Main.gore[idx].velocity *= 0.66f;
                    Main.gore[idx].velocity += Projectile.velocity * 0.15f;
                }
            }
                
        }


        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // 计算彗星的数量
            int starCount = 3;
            // 如果玩家在 AstralBiome 中
            //if (Main.player[Projectile.owner].GetModPlayer<AstralArrowPlayer>().ZoneAstral)
            //{
            //    starCount = 5; // AstralBiome 地形下召唤5颗彗星
            //}

            // 如果是夜晚，则增加 2 个弹幕数量
            if (!Main.dayTime)
            {
                starCount += 2;
            }

            if (Main.getGoodWorld)
            {
                starCount = 8; // Good World 模式下召唤8颗彗星
            }

            int buffDuration = 300; // Buff持续时间为5秒 (300帧)
            target.AddBuff(ModContent.BuffType<AstralArrowEBuff>(), buffDuration);

            //// 在屏幕边缘的随机位置召唤彗星
            //Player player = Main.player[Projectile.owner];
            //Vector2 screenEdgeMin = Main.screenPosition;
            //Vector2 screenEdgeMax = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight);

            //// 获取目标敌人的位置作为彗星的落点
            //Vector2 targetPosition = target.Center;
            //float arrowSpeed = Projectile.velocity.Length();

            //for (int j = 0; j < starCount; j++)
            //{
            //    Vector2 spawnPosition;

            //    // 随机选择屏幕四条边缘中的一个点作为生成位置
            //    switch (Main.rand.Next(4))
            //    {
            //        case 0: // 左边缘
            //            spawnPosition = new Vector2(screenEdgeMin.X, Main.rand.NextFloat(screenEdgeMin.Y, screenEdgeMax.Y));
            //            break;
            //        case 1: // 右边缘
            //            spawnPosition = new Vector2(screenEdgeMax.X, Main.rand.NextFloat(screenEdgeMin.Y, screenEdgeMax.Y));
            //            break;
            //        case 2: // 上边缘
            //            spawnPosition = new Vector2(Main.rand.NextFloat(screenEdgeMin.X, screenEdgeMax.X), screenEdgeMin.Y);
            //            break;
            //        case 3: // 下边缘
            //            spawnPosition = new Vector2(Main.rand.NextFloat(screenEdgeMin.X, screenEdgeMax.X), screenEdgeMax.Y);
            //            break;
            //        default:
            //            spawnPosition = screenEdgeMin; // 默认生成在左上角
            //            break;
            //    }

            //    // 方向为从生成位置指向目标敌人位置
            //    Vector2 direction = targetPosition - spawnPosition;
            //    direction.Normalize();
            //    float speedX = direction.X * arrowSpeed + Main.rand.Next(-120, 121) * 0.01f;
            //    float speedY = direction.Y * arrowSpeed + Main.rand.Next(-120, 121) * 0.01f;

            //    // 固定伤害倍率为 0.5
            //    float damageMultiplier = 0.5f;
            //    int newDamage = (int)(Projectile.damage * damageMultiplier);

            //    // 生成彗星弹幕，位置从屏幕边缘生成，目标为敌人位置
            //    Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPosition, new Vector2(speedX, speedY), ModContent.ProjectileType<AstralArrowSTAR>(), newDamage, Projectile.knockBack, Projectile.owner);
            //}


            //// 获取屏幕的边缘4个角落，作为彗星生成的可能区域
            //Player player = Main.player[Projectile.owner];
            //Vector2 screenEdgeMin = Main.screenPosition;
            //Vector2 screenEdgeMax = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight);

            //// 获取目标敌人的位置作为彗星的落点
            //Vector2 targetPosition = target.Center;
            //float arrowSpeed = Projectile.velocity.Length();

            //for (int j = 0; j < starCount; j++)
            //{
            //    // 生成彗星的随机位置，位于屏幕边缘的某个点
            //    Vector2 spawnPosition = new Vector2(
            //        Main.rand.NextBool() ? screenEdgeMin.X : screenEdgeMax.X, // 左右边缘
            //        Main.rand.NextBool() ? screenEdgeMin.Y : screenEdgeMax.Y  // 上下边缘
            //    );

            //    // 方向为从生成位置指向目标敌人位置
            //    Vector2 direction = targetPosition - spawnPosition;
            //    direction.Normalize();
            //    float speedX = direction.X * arrowSpeed + Main.rand.Next(-120, 121) * 0.01f;
            //    float speedY = direction.Y * arrowSpeed + Main.rand.Next(-120, 121) * 0.01f;

            //    // 固定伤害倍率为 0.5
            //    float damageMultiplier = 0.5f;
            //    int newDamage = (int)(Projectile.damage * damageMultiplier);

            //    // 生成彗星弹幕，位置从屏幕边缘生成，目标为敌人位置
            //    Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPosition, new Vector2(speedX, speedY), ModContent.ProjectileType<AstralArrowSTAR>(), newDamage, Projectile.knockBack, Projectile.owner);
            //}




            //// 在敌人头顶随机位置召唤彗星
            //float chance = Main.rand.NextFloat();
            ////if (chance < 0.25f) // 如果想让他有概率射出的话
            //{
            //    float arrowSpeed = Projectile.velocity.Length();
            //    Vector2 realPlayerPos = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);

            //    // 获取敌人所在位置作为彗星的落点
            //    Vector2 targetPosition = target.Center; // 使用目标敌人的中心作为彗星的落点

            //    for (int j = 0; j < starCount; j++)
            //    {
            //        // 生成彗星的随机生成点，类似于之前逻辑，只不过彗星砸向目标敌人所在位置
            //        Vector2 spawnPosition = new Vector2(
            //            Main.player[Projectile.owner].position.X + Main.player[Projectile.owner].width * 0.5f + Main.rand.Next(201) * -Main.player[Projectile.owner].direction + Main.screenPosition.X - Main.player[Projectile.owner].position.X,
            //            Main.player[Projectile.owner].MountedCenter.Y - 600f
            //        );
            //        spawnPosition.X = (spawnPosition.X + Main.player[Projectile.owner].Center.X) / 2f + Main.rand.Next(-200, 201);
            //        spawnPosition.Y -= 100 * j;

            //        // 方向改为从生成位置到目标敌人位置
            //        Vector2 direction = targetPosition - spawnPosition;
            //        direction.Normalize();
            //        float speedX = direction.X * arrowSpeed + Main.rand.Next(-120, 121) * 0.01f;
            //        float speedY = direction.Y * arrowSpeed + Main.rand.Next(-120, 121) * 0.01f;

            //        // 固定伤害倍率为0.5
            //        float damageMultiplier = 0.5f;
            //        int newDamage = (int)(Projectile.damage * damageMultiplier);

            //        // 生成彗星弹幕，发射速度和位置逻辑保持不变，但目标是敌人位置
            //        Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPosition, new Vector2(speedX, speedY), ModContent.ProjectileType<AstralArrowSTAR>(), newDamage, Projectile.knockBack, Projectile.owner);
            //    }
            //}





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

        //public override void OnKill(int timeLeft)
        //{
        //    int spawnDustAmount = 3;
        //    for (int i = 0; i < spawnDustAmount; i++)
        //    {
        //        Color newColor = Main.hslToRgb(1f, 1f, 0.5f); // 结束时的彩虹色粒子
        //        int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowMk2, 0f, 0f, 0, newColor);
        //        Main.dust[dust].position = Projectile.Center + Main.rand.NextVector2Circular((float)Projectile.width, (float)Projectile.height);
        //        Main.dust[dust].velocity *= Main.rand.NextFloat() * 2.4f;
        //        Main.dust[dust].noGravity = true;
        //        Main.dust[dust].fadeIn = 0.6f + Main.rand.NextFloat();
        //        Main.dust[dust].scale = 1.4f;
        //    }
        //}


        public override void OnKill(int timeLeft)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                Player Owner = Main.player[Projectile.owner];
                float targetDist = Vector2.Distance(Owner.Center, Projectile.Center);

                // 只搬运粒子特效的逻辑，不处理弹幕生成部分
                if (targetDist < 1400f)
                {
                    int Dusts = 8;
                    float radians = MathHelper.TwoPi / Dusts;
                    Vector2 spinningPoint = Vector2.Normalize(new Vector2(-1f, -1f));
                    for (int i = 0; i < Dusts; i++)
                    {
                        Vector2 dustVelocity = spinningPoint.RotatedBy(radians * i) * 3.5f;

                        // 创建亮橙色的粒子效果
                        Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, dustVelocity, 0, new Color(255, 165, 0), 0.9f);
                        dust.noGravity = true;

                        Dust dust2 = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, dustVelocity * 0.6f, 0, new Color(255, 165, 0), 1.2f);
                        dust2.noGravity = true;
                    }
                }

                SoundStyle onKill = new SoundStyle("CalamityMod/Sounds/Custom/Providence/ProvidenceHolyBlastShoot");
                SoundEngine.PlaySound(onKill with { Volume = 0.4f, Pitch = 0.4f }, Projectile.position);

                if (Main.netMode != NetmodeID.Server)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        // 创建亮橙色的粒子效果
                        Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Projectile.velocity.RotatedByRandom(0.4) * Main.rand.NextFloat(0.5f, 1.5f), 0, new Color(255, 165, 0));
                        dust.noGravity = true;
                        dust.scale = Main.rand.NextFloat(0.7f, 1.25f);
                        dust.alpha = 235;

                        if (Main.rand.NextBool())
                        {
                            Dust dust2 = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, new Vector2(3, 3).RotatedByRandom(100) * Main.rand.NextFloat(0.5f, 1.5f), 0, new Color(255, 165, 0));
                            dust2.noGravity = true;
                            dust2.scale = Main.rand.NextFloat(0.8f, 1.5f);
                            dust2.alpha = 70;
                        }
                    }
                    for (int k = 0; k < 2; k++)
                    {
                        // 创建亮橙色的粒子效果，带重力效果的版本
                        Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Projectile.velocity.RotatedByRandom(0.4) * Main.rand.NextFloat(0.5f, 1.5f), 0, new Color(255, 165, 0));
                        dust.noGravity = false;
                        dust.scale = Main.rand.NextFloat(0.85f, 1f);
                        dust.color = new Color(255, 165, 0);
                    }
                }
            }
               




        }



    }
}
