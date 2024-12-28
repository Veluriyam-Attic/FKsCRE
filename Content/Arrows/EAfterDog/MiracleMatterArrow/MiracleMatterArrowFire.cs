﻿using System;
//using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;

namespace FKsCRE.Content.Arrows.EAfterDog.MiracleMatterArrow
{
    public class MiracleMatterArrowFire : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.EAfterDog";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public bool ableToHit = true;
        public NPC target;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 200;
            Projectile.extraUpdates = 7;
            Projectile.timeLeft = 1500;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 50;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public ref float Time => ref Projectile.ai[1];
        public override bool? CanDamage() => Time >= 160f; // 初始的时候不会造成伤害，直到x为止


        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.localAI[0] += 1f / (Projectile.extraUpdates + 1);

            if (Projectile.localAI[0] < 60) // 前 1 秒飞行
            {
                // 每四帧将速度乘以 0.xx
                if ((int)Projectile.localAI[0] % 4 == 0)
                {
                    Projectile.velocity *= 0.98f;
                }
                if (Projectile.localAI[0] % 1 == 0) // 每帧应用旋转效果
                {
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(2));
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

        }


        //public void FindTarget(Player player)
        //{
        //    float maxDistance = 3000f;
        //    bool foundTarget = false;
        //    if (player.HasMinionAttackTargetNPC) // 优先追踪玩家的目标敌人
        //    {
        //        NPC npc = Main.npc[player.MinionAttackTargetNPC];
        //        if (npc.CanBeChasedBy(Projectile, false))
        //        {
        //            float targetDist = Vector2.Distance(npc.Center, Projectile.Center);
        //            if (targetDist < maxDistance)
        //            {
        //                maxDistance = targetDist;
        //                foundTarget = true;
        //                target = npc;
        //            }
        //        }
        //    }
        //    if (!foundTarget)
        //    {
        //        for (int npcIndex = 0; npcIndex < Main.maxNPCs; npcIndex++)
        //        {
        //            NPC npc = Main.npc[npcIndex];
        //            if (npc.CanBeChasedBy(Projectile, false))
        //            {
        //                float targetDist = Vector2.Distance(npc.Center, Projectile.Center);
        //                if (targetDist < maxDistance)
        //                {
        //                    maxDistance = targetDist;
        //                    foundTarget = true;
        //                    target = npc;
        //                }
        //            }
        //        }
        //    }
        //    if (!foundTarget) // 如果没有找到目标，减速
        //    {
        //        Projectile.velocity *= 0.98f;
        //    }
        //    else KillTheThing(target); // 找到目标后进行追踪
        //}







        public void KillTheThing(NPC npc)
        {
            //Projectile.velocity = Projectile.SuperhomeTowardsTarget(npc, 50f / (Projectile.extraUpdates + 1), 60f / (Projectile.extraUpdates + 1), 1f / (Projectile.extraUpdates + 1));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D lightTexture = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/SmallGreyscaleCircle").Value;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float colorInterpolation = (float)Math.Cos(Projectile.timeLeft / 32f + Main.GlobalTimeWrappedHourly / 20f + i / (float)Projectile.oldPos.Length * MathHelper.Pi) * 0.5f + 0.5f;
                Color color = Color.Lerp(Color.White, Color.White, colorInterpolation) * 0.4f;
                color.A = 0;
                Vector2 drawPosition = Projectile.oldPos[i] + lightTexture.Size() * 0.5f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(-28f, -28f);
                Color outerColor = color;
                Color innerColor = color * 0.5f;
                float intensity = 0.9f + 0.15f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 60f * MathHelper.TwoPi);
                intensity *= MathHelper.Lerp(0.15f, 1f, 1f - i / (float)Projectile.oldPos.Length);
                if (Projectile.timeLeft <= 60) // 弹幕即将消失时缩小
                {
                    intensity *= Projectile.timeLeft / 60f;
                }
                Vector2 outerScale = new Vector2(1f) * intensity;
                Vector2 innerScale = new Vector2(1f) * intensity * 0.7f;
                outerColor *= intensity;
                innerColor *= intensity;
                Main.EntitySpriteDraw(lightTexture, drawPosition, null, outerColor, 0f, lightTexture.Size() * 0.5f, outerScale * 0.6f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(lightTexture, drawPosition, null, innerColor, 0f, lightTexture.Size() * 0.5f, innerScale * 0.6f, SpriteEffects.None, 0);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300); // 超位崩解
        }
    }
}
