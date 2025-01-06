//using System;
//using CalamityMod;
//using CalamityMod.Particles;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria;
//using Terraria.Audio;
//using Terraria.DataStructures;
//using Terraria.Graphics.Effects;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace FKsCRE.Content.DeveloperItems.Weapon.AlloyRailgun
//{
//    public class AlloyRailgunScope : ModProjectile
//    {
//        public override string Texture => "CalamityMod/Projectiles/InvisibleProj"; // 使用透明贴图

//        // 蓄力相关参数
//        public ref float Charge => ref Projectile.ai[0];
//        public ref float MaxChargeOrTargetRotation => ref Projectile.ai[1];
//        public const float BaseMaxCharge = 180f; // 最大蓄力时间（3秒）
//        public const float MinimumCharge = 60f; // 最小有效蓄力时间（1秒）
//        public float ChargePercent => MathHelper.Clamp(Charge / MaxChargeOrTargetRotation, 0f, 1f);

//        // 玩家与武器属性
//        public Player Owner => Main.player[Projectile.owner];
//        public Vector2 MousePosition => Owner.Calamity().mouseWorld - Owner.MountedCenter;
//        public const float WeaponLength = 52f;
//        public const float MaxSightAngle = MathHelper.Pi * (2f / 3f);
//        public Color ScopeColor => Color.White;

//        public override void SetDefaults()
//        {
//            Projectile.width = 1;
//            Projectile.height = 1;
//            Projectile.friendly = true;
//            Projectile.DamageType = DamageClass.Ranged;
//            Projectile.ignoreWater = true;
//            Projectile.tileCollide = false;
//        }

//        public override bool? CanDamage() => false; // 该弹幕不会直接造成伤害

//        public override bool ShouldUpdatePosition() => false; // 弹幕位置由玩家控制

//        public override void AI()
//        {
//            Owner.Calamity().mouseWorldListener = true; // 启用鼠标位置监听

//            if (Owner.channel && Charge != -1)
//            {
//                // 仅允许玩家控制
//                if (Projectile.owner != Main.myPlayer)
//                    return;

//                // 增加蓄力时间并设置弹幕属性
//                Charge++;
//                Projectile.rotation = MousePosition.ToRotation();
//                Projectile.Center = Projectile.rotation.ToRotationVector2() * WeaponLength + Owner.MountedCenter;

//                // 设置玩家属性
//                Owner.heldProj = Projectile.whoAmI;
//                Owner.ChangeDir(MousePosition.X >= 0 ? 1 : -1);
//                Owner.itemRotation = (MousePosition * Owner.direction).ToRotation();

//                // 保持使用时间一致，避免快速蓄力
//                Owner.itemTime++;
//                Owner.itemAnimation++;
//                Projectile.timeLeft = Owner.itemAnimation;

//                // 当蓄力达到最大时播放提示音效
//                if (Charge == MaxChargeOrTargetRotation)
//                    SoundEngine.PlaySound(SoundID.Item82 with { Volume = SoundID.Item82.Volume * 0.7f }, Owner.MountedCenter);

//                // 在最大蓄力时每帧生成粒子效果
//                if (ChargePercent == 1f && Charge % 2 == 0)
//                {
//                    Vector2 direction = MousePosition.SafeNormalize(Vector2.UnitX);
//                    Vector2 sparkVelocity = direction.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * 6f;
//                    CritSpark spark = new CritSpark(Owner.MountedCenter + direction * WeaponLength, sparkVelocity + Owner.velocity, Color.White, Color.LightBlue, 1f, 16);
//                    GeneralParticleHandler.SpawnParticle(spark);
//                }

//                Projectile.netUpdate = true; // 同步蓄力状态
//            }
//            else
//            {
//                if (Charge != -1 && Projectile.owner == Main.myPlayer)
//                {
//                    // 如果未达到最小蓄力时间，则取消发射
//                    if (Charge < MinimumCharge)
//                    {
//                        Projectile.netUpdate = true;
//                        Projectile.Kill();
//                        Owner.itemTime = 1;
//                        Owner.itemAnimation = 1;
//                        return;
//                    }

//                    // 重置状态并更新玩家动画
//                    Owner.itemTime = 1;
//                    Owner.itemAnimation = 1;
//                    Charge = -1;
//                    Projectile.netUpdate = true;
//                    return;
//                }
//            }
//        }

//        public override bool PreDraw(ref Color lightColor)
//        {
//            // 如果已经发射，则不再绘制准星
//            if (Charge == -1)
//                return false;

//            float sightsSize = 700f; // 准星范围
//            float sightsResolution = MathHelper.Lerp(0.04f, 0.2f, Math.Min(ChargePercent * 1.5f, 1));

//            // 动态计算准星的扩散角度
//            float spread = (1f - ChargePercent) * MaxSightAngle;
//            float halfAngle = spread / 2f;

//            // 准星颜色随蓄力渐变
//            Color sightsColor = Color.Lerp(Color.LightBlue, Color.Crimson, ChargePercent);

//            // 设置准星扩散效果
//            Effect spreadEffect = Filters.Scene["CalamityMod:SpreadTelegraph"].GetShader().Shader;
//            spreadEffect.Parameters["centerOpacity"].SetValue(0.9f);
//            spreadEffect.Parameters["mainOpacity"].SetValue(ChargePercent);
//            spreadEffect.Parameters["halfSpreadAngle"].SetValue(halfAngle);
//            spreadEffect.Parameters["edgeColor"].SetValue(sightsColor.ToVector3());
//            spreadEffect.Parameters["centerColor"].SetValue(sightsColor.ToVector3());
//            spreadEffect.Parameters["edgeBlendLength"].SetValue(0.07f);
//            spreadEffect.Parameters["edgeBlendStrength"].SetValue(8f);

//            Main.spriteBatch.End();
//            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, spreadEffect, Main.GameViewMatrix.TransformationMatrix);

//            // 绘制准星纹理
//            Main.EntitySpriteDraw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Vector2.Zero, sightsSize, SpriteEffects.None, 0);

//            // 激光线效果
//            Effect laserScopeEffect = Filters.Scene["CalamityMod:PixelatedSightLine"].GetShader().Shader;
//            laserScopeEffect.Parameters["mainOpacity"].SetValue(ChargePercent);
//            laserScopeEffect.Parameters["laserAngle"].SetValue(-Projectile.rotation + halfAngle);
//            laserScopeEffect.Parameters["color"].SetValue(sightsColor.ToVector3());

//            Main.EntitySpriteDraw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, Vector2.Zero, sightsSize, SpriteEffects.None, 0);

//            laserScopeEffect.Parameters["laserAngle"].SetValue(-Projectile.rotation - halfAngle);
//            Main.EntitySpriteDraw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, Vector2.Zero, sightsSize, SpriteEffects.None, 0);

//            Main.spriteBatch.End();
//            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

//            return false;
//        }
//    }
//}
