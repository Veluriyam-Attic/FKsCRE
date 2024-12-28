using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace FKsCRE.Content.Ammunition.DPreDog.DivineGeodeBullet
{
    internal class DivineGeodeBulletEDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true; // 标记为减益效果
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.defense -= 10; // 减少 10 点防御力
        }
    }
}