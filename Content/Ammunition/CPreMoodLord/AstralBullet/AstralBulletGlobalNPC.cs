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
                // 粒子生成逻辑：每帧生成6个粒子
                GenerateParticles(npc);

                // 彗星生成逻辑
                cometTimer++;

                int cometSpawnRate = Main.getGoodWorld ? 1 : 5; // 根据模式设置生成间隔

                if (cometTimer >= cometSpawnRate)
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

            for (int i = 0; i < 18; i++) // 每帧生成18个粒子
            {
                // 随机生成一个角度
                float randomAngle = Main.rand.NextFloat(MathHelper.TwoPi);

                // 计算粒子的生成位置
                Vector2 spawnPosition = npcCenter + radius * new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

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


        private void SummonComet(NPC npc)
        {
            // 彗星生成逻辑保持不变
            Player player = Main.player[npc.target];
            Vector2 targetPosition = npc.Center;
            float radius = 50f * 16f; // 半径为 50 格，即 800 像素
            float arrowSpeed = 10f;

            float randomAngle = Main.rand.NextFloat(MathHelper.TwoPi);
            Vector2 spawnPosition = targetPosition + radius * new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

            Vector2 direction = targetPosition - spawnPosition;
            direction.Normalize();
            float speedMultiplier = 0.5f; // 设置一个加速倍数，专门加速初始速度的倍率
            float speedX = direction.X * arrowSpeed * 3f * speedMultiplier + Main.rand.Next(-120, 121) * 0.01f;
            float speedY = direction.Y * arrowSpeed * 3f * speedMultiplier + Main.rand.Next(-120, 121) * 0.01f;

            int baseDamage = player.HeldItem.damage;
            float damageMultiplier = player.GetDamage(player.HeldItem.DamageType).Additive; // 获取玩家的增伤系数（包括装备和Buff等增益）
            int newDamage = (int)(baseDamage * damageMultiplier * 2.5f); // 应用增伤和倍率系数
            Projectile.NewProjectile(npc.GetSource_FromThis(), spawnPosition, new Vector2(speedX, speedY), ModContent.ProjectileType<AstralBulletSTAR>(), newDamage, 0, player.whoAmI);
        }
    }
}