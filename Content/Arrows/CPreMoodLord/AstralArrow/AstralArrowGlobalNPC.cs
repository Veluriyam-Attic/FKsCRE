using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace FKsCRE.Content.Arrows.CPreMoodLord.AstralArrow
{
    public class AstralArrowGlobalNPC : GlobalNPC
    {
        public bool hasAstralArrowBuff;
        private int cometTimer;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            hasAstralArrowBuff = false;
        }

        public override void AI(NPC npc)
        {
            if (hasAstralArrowBuff)
            {
                // 粒子生成逻辑：每帧生成6个粒子
                GenerateParticles(npc);

                // 你的彗星生成逻辑保持不变
                cometTimer++;
                if (cometTimer >= 10) // 每 10 帧生成一个彗星，每秒6个彗星
                {
                    cometTimer = 0;
                    SummonComet(npc);
                }
            }
        }

        private void GenerateParticles(NPC npc)
        {
            float radius = 50f * 16f; // 半径为50格，等于800像素
            Vector2 npcCenter = npc.Center;

            for (int i = 0; i < 6; i++) // 每帧生成6个粒子
            {
                // 随机生成一个角度
                float randomAngle = Main.rand.NextFloat(MathHelper.TwoPi);

                // 计算粒子的生成位置
                Vector2 spawnPosition = npcCenter + radius * new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

                // 随机选择颜色：粉红色或淡紫色
                Color particleColor = Main.rand.NextBool() ? Color.LightPink : Color.Purple;

                // 创建粒子，设置随机速度和旋转
                Dust particle = Dust.NewDustPerfect(spawnPosition, DustID.TintableDust, Vector2.Zero, 0, particleColor, 1.2f);
                particle.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.5f, 1.5f); // 随机旋转速度
                particle.noGravity = true;
                particle.fadeIn = 1.5f; // 粒子逐渐消失
            }
        }

        private void SummonComet(NPC npc)
        {
            // 你的彗星生成逻辑保持不变
            Player player = Main.player[npc.target];
            Vector2 targetPosition = npc.Center;
            float radius = 50f * 16f; // 半径为 50 格，即 800 像素
            float arrowSpeed = 10f;

            float randomAngle = Main.rand.NextFloat(MathHelper.TwoPi);
            Vector2 spawnPosition = targetPosition + radius * new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

            Vector2 direction = targetPosition - spawnPosition;
            direction.Normalize();
            float speedX = direction.X * arrowSpeed * 3f + Main.rand.Next(-120, 121) * 0.01f;
            float speedY = direction.Y * arrowSpeed * 3f + Main.rand.Next(-120, 121) * 0.01f;

            int newDamage = 30;
            Projectile.NewProjectile(npc.GetSource_FromThis(), spawnPosition, new Vector2(speedX, speedY), ModContent.ProjectileType<AstralArrowSTAR>(), newDamage, 0, player.whoAmI);
        }
    }
}