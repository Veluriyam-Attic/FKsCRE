using CalamityMod.Projectiles;
using CalamityMod;
using FKsCRE.CREConfigs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace FKsCRE.Content.WeaponToAMMO.Arrow.EtherealArrow
{
    internal class EtherealArrowPROJ : ModProjectile, ILocalizedModType
    {
        public override string Texture => "FKsCRE/Content/WeaponToAMMO/Arrow/EtherealArrow/EtherealArrow";

        public new string LocalizationCategory => "WeaponToAMMO.Arrow.MonstrousArrow";
        //public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            NPC target;
            target = Projectile.Center.ClosestNPCAt(2800);
            if (target != null)
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 28f, 0.08f);
            }

            Texture2D lightTexture = ModContent.Request<Texture2D>("FKsCRE/Content/WeaponToAMMO/Arrow/EtherealArrow/EtherealArrow").Value;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float colorInterpolation = (float)Math.Cos(Projectile.timeLeft / 32f + Main.GlobalTimeWrappedHourly / 20f + i / (float)Projectile.oldPos.Length * MathHelper.Pi) * 0.5f + 0.5f;
                Color color = Color.Lerp(Color.Cyan, Color.LightBlue, colorInterpolation) * 0.4f;
                color.A = 0;

                Vector2 drawPosition = Projectile.oldPos[i] + lightTexture.Size() * 0.5f - Main.screenPosition;
                float intensity = 0.9f + 0.15f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 60f * MathHelper.TwoPi);
                intensity *= MathHelper.Lerp(0.15f, 1f, 1f - i / (float)Projectile.oldPos.Length);
                Vector2 scale = new Vector2(1f) * intensity * 0.6f;

                // 使用 Projectile.rotation 调整粒子特效的朝向
                Main.EntitySpriteDraw(lightTexture, drawPosition, null, color, Projectile.rotation, lightTexture.Size() * 0.5f, scale, SpriteEffects.None, 0);
            }
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 200;
            Projectile.timeLeft = 200;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }
        public override void OnSpawn(IEntitySource source)
        {
            // 弹幕随机生成在以玩家为中心半径 5*16 的圆周上
            //Player player = Main.player[Projectile.owner];
            //float randomAngle = Main.rand.NextFloat(0, MathHelper.TwoPi);
            //Vector2 spawnOffset = new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle)) * 5 * 16;
            //Projectile.position = player.Center + spawnOffset - new Vector2(Projectile.width / 2, Projectile.height / 2);

            //// 调整 Rotation 面向玩家
            //Projectile.rotation = (player.Center - Projectile.Center).ToRotation();

            // 释放 Dust 特效
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustPerfect(
                    Projectile.Center,
                    Main.rand.NextBool() ? 206 : 180, // 特效类型
                    Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.5f, 1.5f), // 随机速度
                    150,
                    default,
                    Main.rand.NextFloat(0.8f, 1.5f) // 随机大小
                ).noGravity = true;
            }
        }
        private int aiPhase = 0; // 阶段标记

        public override void AI()
        {
            //switch (aiPhase)
            //{
            //    case 0: // 阶段 1：减速
            //        // 保持弹幕旋转
            //        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
            //        // Lighting - 添加深橙色光源，光照强度为 0.55
            //        Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.55f);
            //        Projectile.velocity *= 0.91f;
            //        if (++Projectile.ai[0] >= 12)
            //        {
            //            aiPhase = 1; // 进入阶段 2
            //            Projectile.ai[0] = 0;
            //        }
            //        break;

            //    case 1: // 阶段 2：调整角度
            //        // 保持弹幕旋转
            //        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
            //        // Lighting - 添加深橙色光源，光照强度为 0.55
            //        Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.55f);
            //        target = Projectile.Center.ClosestNPCAt(2800);
            //        if (target != null)
            //        {
            //            float targetAngle = (target.Center - Projectile.Center).ToRotation();
            //            float difference = MathHelper.WrapAngle(targetAngle - Projectile.rotation);
            //            Projectile.rotation += MathHelper.Clamp(difference, -MathHelper.ToRadians(1), MathHelper.ToRadians(1)); // 限制旋转角度
            //        }
            //        if (++Projectile.ai[0] >= 12)
            //        {
            //            aiPhase = 2; // 进入阶段 3
            //            Projectile.ai[0] = 0;
            //        }
            //        break;

            //    case 2: // 阶段 3：超强追踪
            //        // 保持弹幕旋转
            //        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
            //        // Lighting - 添加深橙色光源，光照强度为 0.55
            //        Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.55f);
            //        target = Projectile.Center.ClosestNPCAt(2800);
            //        if (target != null)
            //        {
            //            Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
            //            Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 28f, 0.08f);
            //        }
            //        if (Projectile.penetrate < 200)
            //        {
            //            if (Projectile.timeLeft > 60) { Projectile.timeLeft = 60; }
            //            Projectile.velocity *= 0.88f;
            //        }
            //        break;
            //}


            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
            // Lighting - 添加深橙色光源，光照强度为 0.55
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.55f);

            if (Projectile.penetrate < 200)
            {
                if (Projectile.timeLeft > 60) { Projectile.timeLeft = 60; }
                Projectile.velocity *= 0.88f;
            }
            else
            {
                NPC target;
                target = Projectile.Center.ClosestNPCAt(2800);
                if (target != null)
                {
                    Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 28f, 0.08f);
                }
            }


            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {

            }




        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {


        }

        public override void OnKill(int timeLeft)
        {



        }


    }
}