using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition.DPreDog.PolterplasmBullet
{
    public class PolterplasmBulletDASH : ModPlayer
    {
        public bool canDash = false; // 冲刺开关
        private bool isDashing = false;
        private int dashCooldown = 0;
        private const int dashCooldownMax = 60; // 冲刺冷却时间
        private const float dashSpeed = 20f; // 冲刺速度
        private int dnaParticleTimer = 0; // 控制粒子生成的计时器

        public override void ResetEffects()
        {
            canDash = false; // 每帧重置开关，由 Buff 控制是否打开
        }

        public override void ProcessTriggers(Terraria.GameInput.TriggersSet triggersSet)
        {
            if (canDash && CalamityMod.CalamityKeybinds.DashHotkey.JustPressed && dashCooldown == 0)
            {
                StartDash();
            }
        }


        private void StartDash()
        {
            isDashing = true;
            dashCooldown = dashCooldownMax;
            dnaParticleTimer = 0;
            Player.immuneTime = 30; // 设置无敌时间

            // 设置冲刺方向与速度
            if (Player.velocity.Length() > 0)
            {
                Player.velocity = Vector2.Normalize(Player.velocity) * dashSpeed;
            }
            else
            {
                Player.velocity = new Vector2(Player.direction, 0) * dashSpeed; // 默认向玩家面朝方向冲刺
            }
        }

        public override void PreUpdateMovement()
        {
            if (isDashing)
            {
                PerformDash();
                CheckDashCollision();
            }

            if (dashCooldown > 0)
            {
                dashCooldown--;
            }
        }

        private void PerformDash()
        {
            Player.velocity *= 0.975f; // 冲刺期间速度逐渐衰减

            // 生成 DNA 链状粒子特效
            CreateDNAParticles();

            // 如果速度低于一定阈值，停止冲刺
            if (Player.velocity.Length() < 5f)
            {
                isDashing = false;
            }
        }

        private void CheckDashCollision()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && Player.getRect().Intersects(npc.getRect()))
                {
                    // 造成伤害
                    npc.StrikeNPC(new NPC.HitInfo
                    {
                        Damage = 100,
                        Knockback = 5f,
                        HitDirection = Player.direction
                    });

                    // 释放冲击波特效
                    float particleScale = 1.0f;
                    Particle explosion = new DetailedExplosion(npc.Center, Vector2.Zero, Color.Gray * 0.6f, Vector2.One, Main.rand.NextFloat(-5, 5), 0f, particleScale + 0.07f, 20, false);
                    GeneralParticleHandler.SpawnParticle(explosion);

                    Particle explosion2 = new DetailedExplosion(npc.Center, Vector2.Zero, Color.Orange, Vector2.One, Main.rand.NextFloat(-5, 5), 0f, particleScale, 20);
                    GeneralParticleHandler.SpawnParticle(explosion2);
                }
            }
        }

        private void CreateDNAParticles()
        {
            if (++dnaParticleTimer % 5 != 0) return; // 控制粒子生成频率

            // 生成粒子的基础参数
            float baseOffset = 10f;
            float amplitude = 3f;
            int particleCount = 2; // 每帧生成的粒子数（一个左一个右）

            for (int i = 0; i < particleCount; i++)
            {
                // 左侧和右侧的 DNA 链粒子位置
                float directionOffset = i == 0 ? -amplitude : amplitude;
                Vector2 particlePosition = Player.Center + new Vector2(baseOffset, directionOffset).RotatedBy(Player.velocity.ToRotation());
                Dust dust = Dust.NewDustPerfect(particlePosition, DustID.PinkTorch, null, 150, Color.Pink, 1.2f);
                dust.noGravity = true;
            }
        }
    }
}
