using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using CalamityMod.Dusts;

namespace FKsCRE.Content.Ammunition.CPreMoodLord.AstralBullet
{
    public class AstralBulletGlobalNPC : GlobalNPC
    {
        public bool hasAstralBulletBuff;
        public int cometTimer;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            hasAstralBulletBuff = false;
        }
        public override void AI(NPC npc)
        {
            if (hasAstralBulletBuff)
            {
                // 获取 AstralBulletEBuff 的剩余时间百分比
                int buffIndex = npc.FindBuffIndex(ModContent.BuffType<AstralBulletEBuff>());
                if (buffIndex != -1) // 确保敌人有这个 Buff
                {
                    int remainingTime = npc.buffTime[buffIndex]; // Buff 剩余时间
                    float timePercentage = remainingTime / 180f; // 当前时间占比（0 到 1）

                    // 动态调整半径
                    float dynamicRadius = 50f * 16f * timePercentage; // 最大半径 50 格，随着剩余时间缩小

                    // 调整彗星生成频率（频率随时间缩短，频率为 100% ~ 200%）
                    float dynamicFrequencyMultiplier = 2f - timePercentage; // 线性频率调整
                    int cometSpawnRate = (int)(5 / dynamicFrequencyMultiplier); // 动态间隔

                    // 更新粒子生成逻辑
                    GenerateParticles(npc, dynamicRadius);

                    // 彗星生成逻辑
                    cometTimer++;
                    if (cometTimer >= cometSpawnRate)
                    {
                        cometTimer = 0;
                        SummonComet(npc, dynamicRadius); // 使用动态半径
                    }
                }
            }
        }

        private void GenerateParticles(NPC npc, float dynamicRadius)
        {
            Vector2 npcCenter = npc.Center;

            for (int i = 0; i < 18; i++) // 每帧生成18个粒子
            {
                // 随机生成一个角度
                float randomAngle = Main.rand.NextFloat(MathHelper.TwoPi);

                // 计算粒子的生成位置
                Vector2 spawnPosition = npcCenter + dynamicRadius * new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

                // 随机选择颜色：AstralOrange 或 AstralBlue
                int randomDust = Utils.SelectRandom(Main.rand, new int[]
                {
            ModContent.DustType<AstralOrange>(),
            ModContent.DustType<AstralBlue>()
                });

                // 创建粒子，设置随机速度和旋转
                Dust particle = Dust.NewDustPerfect(spawnPosition, randomDust, Vector2.Zero, 0, default, 1.2f);
                particle.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.5f, 1.5f); // 随机旋转速度
                particle.noGravity = true;
                particle.fadeIn = 1.5f; // 粒子逐渐消失
            }
        }

        private void SummonComet(NPC npc, float dynamicRadius)
        {
            Player player = Main.player[npc.target];
            Vector2 targetPosition = npc.Center;

            float randomAngle = Main.rand.NextFloat(MathHelper.TwoPi);
            Vector2 spawnPosition = targetPosition + dynamicRadius * new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

            Vector2 direction = targetPosition - spawnPosition;
            direction.Normalize();
            float arrowSpeed = 10f;
            float speedMultiplier = 0.5f; // 设置一个加速倍数，专门加速初始速度的倍率
            float speedX = direction.X * arrowSpeed * 3f * speedMultiplier + Main.rand.Next(-120, 121) * 0.01f;
            float speedY = direction.Y * arrowSpeed * 3f * speedMultiplier + Main.rand.Next(-120, 121) * 0.01f;

            int baseDamage = player.HeldItem.damage;
            float damageMultiplier = player.GetDamage(player.HeldItem.DamageType).Additive; // 获取玩家的增伤系数（包括装备和Buff等增益）
            float multiplier = Main.getGoodWorld ? 7.5f : 2.5f; // 根据条件动态调整倍率
            int newDamage = (int)(baseDamage * damageMultiplier * multiplier); // 应用增伤和倍率系数
            Projectile.NewProjectile(npc.GetSource_FromThis(), spawnPosition, new Vector2(speedX, speedY), ModContent.ProjectileType<AstralBulletSTAR>(), newDamage, 0, player.whoAmI);
        }











    }
}