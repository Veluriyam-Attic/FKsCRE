using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Ranged;
using Terraria.DataStructures;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Particles;
using FKsCRE.CREConfigs;
using Terraria.Audio;
using CalamityMod.Graphics.Primitives;
using Terraria.Graphics.Shaders;

namespace FKsCRE.Content.Arrows.EAfterDog.MiracleMatterArrow
{
    public class MiracleMatterArrowPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.EAfterDog";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            // 设置弹幕的基础属性
            Projectile.width = 11; // 弹幕宽度
            Projectile.height = 24; // 弹幕高度
            Projectile.friendly = true; // 对敌人有效
            Projectile.DamageType = DamageClass.Ranged; // 远程伤害类型
            Projectile.penetrate = 1; // 穿透力为1，击中一个敌人就消失
            Projectile.timeLeft = 600; // 弹幕存在时间为600帧
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 14; // 无敌帧冷却时间为14帧
            Projectile.ignoreWater = true; // 弹幕不受水影响
            Projectile.arrow = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            // 调整旋转
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            // 计算正前方三格（48个像素）的位置
            Vector2 forwardPosition = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 48f;

            // 检测该位置是否存在敌人
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.CanBeChasedBy(this) && npc.Hitbox.Contains(forwardPosition.ToPoint()))
                {
                    // 如果正前方三格处有敌人，销毁自身
                    Projectile.Kill();
                    return; // 确保方法中止，避免执行后续代码
                }
            }

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                Vector2 armPosition = Projectile.Center;
                Vector2 tipPosition = armPosition + Projectile.velocity * Projectile.width * 0.45f;

                // 发光效果
                Color energyColor = Color.Orange;
                Vector2 verticalOffset = Vector2.UnitY.RotatedBy(Projectile.rotation) * 8f;
                if (Math.Cos(Projectile.rotation) < 0f)
                    verticalOffset *= -1f;

                // 发射橙色光粒子
                if (Main.rand.NextBool(4))
                {
                    SquishyLightParticle exoEnergy = new(tipPosition + verticalOffset, -Vector2.UnitY.RotatedByRandom(0.39f) * Main.rand.NextFloat(0.4f, 1.6f), 0.28f, energyColor, 25);
                    GeneralParticleHandler.SpawnParticle(exoEnergy);
                }

                // 增加透明度渐变
                Projectile.Opacity = Utils.GetLerpValue(0f, 3f, Projectile.timeLeft, true);

                // 添加光照
                DelegateMethods.v3_1 = energyColor.ToVector3();
                Utils.PlotTileLine(tipPosition - verticalOffset, tipPosition + verticalOffset, 10f, DelegateMethods.CastLightOpen);
                Lighting.AddLight(tipPosition, energyColor.ToVector3());

                // 添加从暗红色到亮白色的粒子效果
                if (Main.rand.NextBool(5))
                {
                    // 颜色渐变：从深红色逐渐变成亮白色
                    float brightness = (float)Main.rand.NextDouble(); // 随机亮度值
                    Color smokeColor = Color.Lerp(Color.GhostWhite, Color.White, brightness); // 插值计算
                    Particle smoke = new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.5f, smokeColor, 20, Projectile.scale * Main.rand.NextFloat(0.6f, 1.2f), 0.8f, MathHelper.ToRadians(3f), required: true);
                    GeneralParticleHandler.SpawnParticle(smoke);
                }

                // 充能完成时的特效
                if (Projectile.timeLeft == 550) // 示例触发条件
                {
                    //SoundEngine.PlaySound(SoundID.Item158 with { Volume = 1.6f }, Projectile.Center);
                    //for (int i = 0; i < 75; i++)
                    //{
                    //    float offsetAngle = MathHelper.TwoPi * i / 75f;
                    //    float unitOffsetX = (float)Math.Pow(Math.Cos(offsetAngle), 3D);
                    //    float unitOffsetY = (float)Math.Pow(Math.Sin(offsetAngle), 3D);

                    //    Vector2 puffDustVelocity = new Vector2(unitOffsetX, unitOffsetY) * 5f;
                    //    Dust magic = Dust.NewDustPerfect(tipPosition, 267, puffDustVelocity);
                    //    magic.scale = 1.8f;
                    //    magic.fadeIn = 0.5f;
                    //    magic.color = CalamityUtils.MulticolorLerp(i / 75f, CalamityUtils.ExoPalette);
                    //    magic.noGravity = true;
                    //}
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 获取纹理资源和位置
                Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                Texture2D textureGlow = ModContent.Request<Texture2D>("FKsCRE/Content/Arrows/EAfterDog/MiracleMatterArrow/MiracleMatterArrowPROJ").Value;
                Vector2 origin = texture.Size() * 0.5f;
                Vector2 drawPosition = Projectile.Center - Main.screenPosition;

                // 计算颜色渐变，用于动态的尾迹效果
                float localIdentityOffset = Projectile.identity * 0.1372f;
                Color mainColor = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 2f + localIdentityOffset) % 1f, Color.Cyan, Color.Lime, Color.GreenYellow, Color.Goldenrod, Color.Orange);
                Color secondaryColor = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 2f + localIdentityOffset + 0.2f) % 1f, Color.Cyan, Color.Lime, Color.GreenYellow, Color.Goldenrod, Color.Orange);

                // 混合颜色，进一步控制白色和渐变色的比例
                mainColor = Color.Lerp(Color.White, mainColor, 0.85f);
                secondaryColor = Color.Lerp(Color.White, secondaryColor, 0.85f);

                // 背光效果部分，增加充能效果的光晕
                float chargeOffset = 3f; // 控制充能效果扩散的偏移量
                Color chargeColor = Color.Lerp(Color.Lime, Color.Cyan, (float)Math.Cos(Main.GlobalTimeWrappedHourly * 7.1f) * 0.5f + 0.5f) * 0.6f;
                chargeColor.A = 0; // 透明度

                // 设置固定的旋转角度，使弹幕始终保持一个方向
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
                float rotation = Projectile.rotation; // 使用Projectile.rotation作为旋转值

                // 去除原有的方向翻转逻辑
                SpriteEffects direction = SpriteEffects.None; // 保持默认值，不进行水平翻转

                // 绘制充能效果 - 圆周上绘制多个充能光效
                for (int i = 0; i < 8; i++)
                {
                    Vector2 drawOffset = (MathHelper.TwoPi * i / 8f).ToRotationVector2() * chargeOffset;
                    Main.spriteBatch.Draw(texture, drawPosition + drawOffset, null, chargeColor, rotation, origin, Projectile.scale, direction, 0f);
                }

                // 使用自定义着色器应用尾迹效果
                GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/EternityStreak"));
                GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseImage2("Images/Extra_189");
                GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseColor(mainColor);
                GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseSecondaryColor(secondaryColor);
                GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].Apply();

                // 渲染尾迹，通过存储的弹幕位置数据渲染弹幕移动的尾巴
                PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(PrimitiveWidthFunction, PrimitiveColorFunction, (_) => Projectile.Size * 0.5f, shader: GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"]), 53);

                // 渲染实际的投射物本体
                Main.spriteBatch.Draw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), rotation, origin, Projectile.scale, direction, 0f);

                // 渲染发光的投射物效果
                Main.spriteBatch.Draw(textureGlow, drawPosition, null, Color.White, rotation, origin, Projectile.scale, direction, 0f);

                return false;
            }
            else
            {
                return true;
            }
        }


        public float PrimitiveWidthFunction(float completionRatio) => Projectile.scale * 30f;

        public Color PrimitiveColorFunction(float _) => Color.Lime * Projectile.Opacity;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnKill(int timeLeft)
        {
            //// 计算随机初始角度
            //float initialAngle = Main.rand.NextFloat(0f, MathHelper.TwoPi); // 在 0 到 2π 之间随机选择一个角度
            //float angleIncrement = MathHelper.ToRadians(120f); // 每个角度之间的夹角为 120 度

            //// 计算并发射三个弹幕
            //for (int i = 0; i < 3; i++)
            //{
            //    float currentAngle = initialAngle + i * angleIncrement; // 计算当前的角度
            //    Vector2 newVelocity = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(currentAngle) * 10f; // 旋转基础速度并保持大小
            //    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, newVelocity, ModContent.ProjectileType<MiracleMatterArrowFire>(), (int)(Projectile.damage * 1.0f), Projectile.knockBack, Projectile.owner);
            //}


            // 计算并发射三个弹幕
            {
                // 计算初始方向角度
                float forwardAngle = Projectile.velocity.ToRotation(); // 当前速度的方向角
                float backwardAngle = forwardAngle + MathHelper.Pi; // 相反方向的角度

                // 定义前后角度范围（±15度）
                float angleRange = MathHelper.ToRadians(15f);

                // 存储所有可能的角度
                List<float> possibleAngles = new List<float>
{
    forwardAngle - angleRange, // 正前方左15度
    forwardAngle + angleRange, // 正前方右15度
    backwardAngle - angleRange, // 正后方左15度
    backwardAngle + angleRange  // 正后方右15度
};

                // 随机选择三个角度
                List<float> selectedAngles = possibleAngles.OrderBy(_ => Main.rand.Next()).Take(3).ToList();

                // 发射三个弹幕
                foreach (float angle in selectedAngles)
                {
                    // 计算新速度（当前速度的1/4）
                    Vector2 newVelocity = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(angle) * (Projectile.velocity.Length() / 4f);

                    // 发射弹幕
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, newVelocity, ModContent.ProjectileType<MiracleMatterArrowFire>(), (int)(Projectile.damage * 1.3f), Projectile.knockBack, Projectile.owner);
                }
            }
            



            Vector2 armPosition = Projectile.Center;
            Vector2 tipPosition = armPosition + Projectile.velocity * Projectile.width * 0.45f;

            // 释放原有的菱形粒子效果
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                SoundEngine.PlaySound(SoundID.Item158 with { Volume = 1.6f }, Projectile.Center);
                for (int i = 0; i < 75; i++)
                {
                    float offsetAngle = MathHelper.TwoPi * i / 75f;
                    float unitOffsetX = (float)Math.Pow(Math.Cos(offsetAngle), 3D);
                    float unitOffsetY = (float)Math.Pow(Math.Sin(offsetAngle), 3D);

                    Vector2 puffDustVelocity = new Vector2(unitOffsetX, unitOffsetY) * 5f;
                    Dust magic = Dust.NewDustPerfect(tipPosition, 267, puffDustVelocity);
                    magic.scale = 1.8f;
                    magic.fadeIn = 0.5f;
                    magic.color = CalamityUtils.MulticolorLerp(i / 75f, CalamityUtils.ExoPalette);
                    magic.noGravity = true;
                }

                // 新增：正前方和正后方半圆粒子效果
                Vector2 frontCenter = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 48f; // 正前方三个方块处
                Vector2 backCenter = Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.Zero) * 48f;  // 正后方三个方块处

                int halfCircleParticles = 38; // 每个半圆生成的粒子数量
                for (int i = 0; i < halfCircleParticles; i++)
                {
                    // 计算半圆角度范围 (-90 度到 90 度)
                    float angle = MathHelper.Pi * (i / (float)halfCircleParticles) - MathHelper.PiOver2;

                    // 生成正前方半圆的粒子
                    Vector2 frontOffset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 16f; // 半径调整
                    Dust frontMagic = Dust.NewDustPerfect(frontCenter + frontOffset, 267, Vector2.Zero);
                    frontMagic.scale = 1.8f;
                    frontMagic.fadeIn = 0.5f;
                    frontMagic.color = CalamityUtils.MulticolorLerp(i / (float)halfCircleParticles, CalamityUtils.ExoPalette);
                    frontMagic.noGravity = true;

                    // 生成正后方半圆的粒子
                    Vector2 backOffset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 16f;
                    Dust backMagic = Dust.NewDustPerfect(backCenter + backOffset, 267, Vector2.Zero);
                    backMagic.scale = 1.8f;
                    backMagic.fadeIn = 0.5f;
                    backMagic.color = CalamityUtils.MulticolorLerp(i / (float)halfCircleParticles, CalamityUtils.ExoPalette);
                    backMagic.noGravity = true;
                }
            }
        }






    }
}