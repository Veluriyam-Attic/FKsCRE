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
    public class MiracleMatterArrowPROJ : ModProjectile
    {
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
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;



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
                Texture2D textureGlow = ModContent.Request<Texture2D>("FKsCRE/Content/Arrows/EAfterDog/MiracleMatterArrow/MiracleMatterArrow").Value;
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

                // 处理旋转方向，确保正确绘制朝向
                float rotation = Projectile.rotation;
                SpriteEffects direction = SpriteEffects.None;
                if (Math.Cos(rotation) < 0f)
                {
                    direction = SpriteEffects.FlipHorizontally;
                    rotation += MathHelper.Pi;
                }

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
            return true;
        }

        public float PrimitiveWidthFunction(float completionRatio) => Projectile.scale * 30f;

        public Color PrimitiveColorFunction(float _) => Color.Lime * Projectile.Opacity;

        public override void OnKill(int timeLeft)
        {
            Vector2 armPosition = Projectile.Center;
            Vector2 tipPosition = armPosition + Projectile.velocity * Projectile.width * 0.45f;

            // 特效在弹幕消失时可以添加额外效果
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
            }
        }





    }
}