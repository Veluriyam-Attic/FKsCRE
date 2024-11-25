using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition.CPreMoodLord.PerennialBullet
{
    public class PerennialBulletPlayer : ModPlayer
    {
      

        public int StackCount; // 防御力堆叠层数
        private int disableBuffTimer = 0; // 禁用计时器

        public override void ResetEffects()
        {
            // 每帧减少计时器
            if (disableBuffTimer > 0)
            {
                disableBuffTimer--;
            }
        }


        private int increaseStackCountCalls = 0; // 追踪调用次数

        public void IncreaseStackCount()
        {
            // 如果禁用计时器生效，直接返回
            if (disableBuffTimer > 0) return;

            increaseStackCountCalls++; // 每次调用增加计数

            if (increaseStackCountCalls >= 50) // 当调用次数达到 50 时触发效果
            {
                StackCount += Main.getGoodWorld ? 10 : 1; // 正常加1，如果是荣耀世界则加10
                Player.AddBuff(ModContent.BuffType<PerennialBulletPBuff>(), 600); // 刷新 10 秒的 Buff

                // 防御力成功提升，生成绿色粒子特效
                for (int i = 0; i < 8; i++) // 生成 8 个粒子
                {
                    Vector2 particleVelocity = Main.rand.NextVector2CircularEdge(2f, 2f) * 1.5f; // 较快速度，圆形边界
                    Dust dust = Dust.NewDustPerfect(
                        Player.Center,              // 粒子生成位置
                        DustID.GreenFairy,          // 绿色粒子
                        particleVelocity,           // 粒子速度
                        150,                        // 透明度
                        Color.LightGreen,           // 粒子颜色
                        Main.rand.NextFloat(1.2f, 1.6f) // 粒子大小
                    );
                    dust.noGravity = true; // 粒子不受重力影响
                }

                // 重置调用计数器
                increaseStackCountCalls = 0;
            }
        }


        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            ClearBuffOnHit();
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            ClearBuffOnHit();
        }

        private void ClearBuffOnHit()
        {
            // 清除 Buff 和堆叠层数
            StackCount = 0;
            Player.ClearBuff(ModContent.BuffType<PerennialBulletPBuff>());

            // 启动禁用计时器，5秒 = 300帧
            disableBuffTimer = 300;
        }
    }
}
