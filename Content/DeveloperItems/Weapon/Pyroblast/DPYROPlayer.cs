using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.DeveloperItems.Weapon.Pyroblast
{
    public class DPYROPlayer : ModPlayer
    {
        public bool dpsBoostActive = false; // 开关，默认关闭
        public override void ResetEffects()
        {
            dpsBoostActive = false;
        }
        public override void UpdateDead()
        {
            // 玩家死亡时关闭开关并重置 DPS
            dpsBoostActive = false;
            Main.CurrentPlayer.dpsDamage = 0;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 检查是否需要翻倍 DPS
            if (dpsBoostActive && damageDone > 0) // 只有开关启用且伤害大于0时触发
            {
                // 将伤害翻倍记录到 DPS 系统
                Main.CurrentPlayer.addDPS(damageDone);
            }
        }

        // 这个可以直接粗暴的修改DPS，但是他修改过后的会被用作下一次的起始点，也就是会一直增加一直增加
        //public override void PostUpdateMiscEffects()
        //{
        //    // 如果开关被打开且玩家已经开始计算 DPS
        //    if (dpsBoostActive && Main.CurrentPlayer.dpsStarted)
        //    {
        //        // 增加玩家的 DPS 显示值
        //        Main.CurrentPlayer.dpsDamage *= 2;
        //        //Main.CurrentPlayer.addDPS(30000);

        //        // 立即关闭 DPS 统计，强制停止当前计数
        //        Main.CurrentPlayer.dpsStarted = false;
        //    }
        //}



        //private int dpsUpdateTimer = 0; // 计时器


        //private int dpsAdjustTimer = 0; // 用于控制间隔的计时器
        //private const int updateInterval = 120; // 2 秒更新一次 (假设每秒60帧)

        //public override void PostUpdateMiscEffects()
        //{
        //    // 如果 DPS 开关启用且玩家已开始计算 DPS
        //    if (dpsBoostActive && Main.CurrentPlayer.dpsStarted)
        //    {
        //        dpsAdjustTimer++; // 每帧增加计时器

        //        // 每 2 秒更新一次 dpsStart
        //        if (dpsAdjustTimer >= updateInterval)
        //        {
        //            // 将 dpsStart 设置为 2 秒前
        //            Main.CurrentPlayer.dpsStart = DateTime.Now.AddSeconds(-2);

        //            // 重置计时器
        //            dpsAdjustTimer = 0;
        //        }
        //    }
        //    else
        //    {
        //        // 如果 DPS 关闭或未开始，重置计时器
        //        dpsAdjustTimer = 0;
        //    }
        //}


        //// 记录玩家在当前 DPS 统计周期内造成的总伤害
        //public int dpsDamage;

        //// 记录玩家最后一次造成伤害的时间，用于确定 DPS 周期的结束点
        //public DateTime dpsEnd;

        //// 记录玩家最近一次造成伤害的时间，用于判断是否需要停止 DPS 统计
        //public DateTime dpsLastHit;

        //// 记录当前 DPS 统计周期的开始时间
        //public DateTime dpsStart;

        //// 标记当前是否正在统计 DPS
        //public bool dpsStarted;
    }
}
