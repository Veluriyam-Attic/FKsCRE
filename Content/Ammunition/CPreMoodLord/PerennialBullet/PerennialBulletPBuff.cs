using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition.CPreMoodLord.PerennialBullet 
{ 
    public class PerennialBulletPBuff : ModBuff, ILocalizedModType
    {
        public new string LocalizationCategory => "Buffs";
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true; // 不保存 Buff
            Main.debuff[Type] = false;   // Buff 是增益效果
        }

        public override void Update(Player player, ref int buffIndex)
        {
            int stackCount = player.GetModPlayer<PerennialBulletPlayer>().StackCount; // 获取堆叠层数
            int defenseBoost = Math.Min(stackCount, 20); // 防御力加成上限为 20 点
            player.statDefense += defenseBoost; // 根据堆叠层数动态增加防御力
        }

    }

}
