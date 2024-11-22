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
        //private int hitCounter = 0; // 子弹命中计数
        //private int decayTimer = 600; // 防御力持续计时器（10秒，600帧）
        //private int cooldownTimer = 0; // 冷却计时器
        //private int defenseBoost = 0; // 默认的防御提升

        //public override void ResetEffects()
        //{
        //    // 如果冷却计时器大于 0，减少冷却时间
        //    if (cooldownTimer > 0)
        //    {
        //        cooldownTimer--;
        //        return; // 冷却中不允许增加 Buff
        //    }

        //    // 如果超过 10 秒未命中敌人，移除 Buff
        //    if (decayTimer > 0)
        //    {
        //        decayTimer--;
        //    }
        //    else
        //    {
        //        Player.ClearBuff(ModContent.BuffType<PerennialBulletPBuff>()); // 移除防御提升 Buff
        //        hitCounter = 0; // 清空计数
        //    }
        //}

        //public void OnBulletHit()
        //{
        //    if (cooldownTimer > 0)
        //        return; // 冷却期间不处理命中事件

        //    hitCounter++;

        //    if (hitCounter >= 20) // 每命中 20 次给予 Buff
        //    {
        //        int defenseBoostValue = 5; // 本次防御提升值
        //        Player.AddBuff(ModContent.BuffType<PerennialBulletPBuff>(), 300); // 给予 5 秒 Buff

        //        // 将防御提升值传递给 Buff
        //        if (Player.TryGetModPlayer<PerennialBulletPlayer>(out var modPlayer))
        //        {
        //            modPlayer.AssignBuffDefense(defenseBoostValue); // 通知 Buff 防御提升值
        //        }

        //        // 防御力成功提升，生成绿色粒子特效
        //        for (int i = 0; i < 8; i++) // 生成 8 个粒子
        //        {
        //            Vector2 particleVelocity = Main.rand.NextVector2CircularEdge(2f, 2f) * 1.5f; // 较快速度，圆形边界
        //            Dust dust = Dust.NewDustPerfect(
        //                Player.Center,              // 粒子生成位置
        //                DustID.GreenFairy,          // 绿色粒子
        //                particleVelocity,           // 粒子速度
        //                150,                        // 透明度
        //                Color.LightGreen,           // 粒子颜色
        //                Main.rand.NextFloat(1.2f, 1.6f) // 粒子大小
        //            );
        //            dust.noGravity = true; // 粒子不受重力影响
        //        }

        //        hitCounter -= 20; // 重置计数器多余部分
        //    }

        //    // 每次命中重置防御衰减计时器
        //    decayTimer = 600; // 10 秒
        //}

        //// 新增方法：将防御力传递给 Buff
        //public void AssignBuffDefense(int defenseBoostValue)
        //{
        //    if (Player.HasBuff(ModContent.BuffType<PerennialBulletPBuff>()))
        //    {
        //        PerennialBulletPBuff buff = Player.GetModBuff<PerennialBulletPBuff>();
        //        if (buff != null)
        //        {
        //            buff.SetDefenseBoost(defenseBoostValue); // 调用 Buff 设置防御力
        //        }
        //    }
        //}





        //public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        //{
        //    ClearDefenseBuff();
        //}

        //public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        //{
        //    ClearDefenseBuff();
        //}

        //private void ClearDefenseBuff()
        //{
        //    // 清除Buff并进入冷却状态
        //    Player.ClearBuff(ModContent.BuffType<PerennialBulletPBuff>());
        //    cooldownTimer = 300; // 5秒冷却
        //    hitCounter = 0; // 重置计数器
        //}



        public int StackCount; // 防御力堆叠层数

        public override void ResetEffects()
        {
            // 检查玩家是否装备相关武器（如果有），否则移除 Buff 和堆叠
            //if (Player.HeldItem.type != ModContent.ItemType<PerennialBulletWeapon>())
            {
                //StackCount = 0;
                //Player.ClearBuff(ModContent.BuffType<PerennialBulletPBuff>());
            }
        }


        private int increaseStackCountCalls = 0; // 追踪调用次数

        public void IncreaseStackCount()
        {
            increaseStackCountCalls++; // 每次调用增加计数

            if (increaseStackCountCalls >= 50) // 当调用次数达到 50 时触发效果
            {
                StackCount++; // 增加堆叠层数
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
        }

    }
}
