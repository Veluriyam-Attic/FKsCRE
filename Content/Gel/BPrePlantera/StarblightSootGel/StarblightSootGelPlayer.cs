using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace FKsCRE.Content.Gel.BPrePlantera.StarblightSootGel
{
    public class StarblightSootGelPlayer : ModPlayer
    {
        private int hitCount = 0;
        private int damageTier = 0;
        private int lastHitTimer = 300; // 5 秒计时器

        public override void ResetEffects()
        {
            if (lastHitTimer > 0)
            {
                lastHitTimer--;
            }
            else if (damageTier > 0)
            {
                DecreaseDamageTier();
            }
        }

        public void NotifyHit()
        {
            hitCount++;
            lastHitTimer = 300; // 重置计时器

            if (hitCount >= 20)
            {
                hitCount = 0;
                IncreaseDamageTier();
            }
        }

        private void IncreaseDamageTier()
        {
            if (damageTier < 6)
            {
                damageTier++;
                Player.GetDamage(DamageClass.Ranged) += 0.1f;
                CreateParticleArrow(true); // 往上箭头
            }
        }

        private void DecreaseDamageTier()
        {
            if (damageTier > 0)
            {
                Player.GetDamage(DamageClass.Ranged) -= 0.1f;
                CreateParticleArrow(false); // 往下箭头
                damageTier--;
            }
        }

        private void CreateParticleArrow(bool upward)
        {
            Color particleColor = Color.Cyan;
            Vector2 playerCenter = Player.Center;
            int particleCount = 30; // 增加粒子数量
            float lengthMultiplier = 15f; // 加长粒子线段

            for (int i = 0; i < particleCount; i++)
            {
                float angle = upward
                    ? (i < particleCount / 2 ? MathHelper.ToRadians(-105) : MathHelper.ToRadians(-165))
                    : (i < particleCount / 2 ? MathHelper.ToRadians(105) : MathHelper.ToRadians(165));

                Vector2 offset = new Vector2(i % (particleCount / 2), i % (particleCount / 2))
                                 * Main.rand.NextFloat(0.5f, 1.5f) * lengthMultiplier;
                Vector2 spawnPos = playerCenter + offset.RotatedBy(angle);
                Vector2 velocity = new Vector2(0, upward ? -1f : 1f); // 上升或下降速度

                //Dust dust = Dust.NewDustPerfect(spawnPos, DustID.Electric, velocity, 100, particleColor, 1.2f);
                //dust.noGravity = true;
                //dust.fadeIn = 0.1f;
                //dust.scale = Main.rand.NextFloat(0.8f, 1.2f);
            }
        }
    }
}
