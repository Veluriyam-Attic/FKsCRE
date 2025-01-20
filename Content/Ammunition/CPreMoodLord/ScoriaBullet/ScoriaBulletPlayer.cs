using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition.CPreMoodLord.ScoriaBullet
{
    public class ScoriaBulletPlayer : ModPlayer
    {
        private float accelerationBonus = 0f; // 当前的加速度加成
        private int bonusDamagePercentage = 0; // 全职业伤害提升百分比
        private int lastHitTime = 0; // 记录最后一次命中的时间

        public override void ResetEffects()
        {
            // 每帧重置玩家的加速度和全职业伤害提升
            Player.runAcceleration -= accelerationBonus; // 移除上一帧的加速度加成
            Player.GetDamage(DamageClass.Generic) -= bonusDamagePercentage / 100f; // 移除全职业伤害提升

            // 计算时间差，如果超过10秒未命中敌人，则移除所有效果
            if (lastHitTime > 0 && Main.GameUpdateCount - lastHitTime > 600)
            {
                accelerationBonus = 0f;
                bonusDamagePercentage = 0;
            }

            // 应用新的加速度和全职业伤害提升
            Player.runAcceleration += accelerationBonus;
            Player.GetDamage(DamageClass.Generic) += bonusDamagePercentage / 100f;
        }

        public void OnScoriaBulletHit()
        {
            // 如果加速度和伤害提升已达到上限，则直接返回
            if (accelerationBonus >= 0.30f)
                return;

            // 增加加速度和全职业伤害提升
            accelerationBonus = MathHelper.Clamp(accelerationBonus + 0.01f, 0f, 0.30f); // 最大加速度为 0.30
            bonusDamagePercentage = (int)(accelerationBonus * 100); // 每 0.01 加速度对应 1% 全职业伤害提升

            // 更新最后命中时间
            lastHitTime = (int)Main.GameUpdateCount;
        }
    }
}
