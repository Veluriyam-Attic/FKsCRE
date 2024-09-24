using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace FKsCRE.Content.凝胶.WulfrimGels
{
    public class WulfrimGelBuff : 凝胶_DeBuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            if(npc.boss)
            {
                npc.lifeRegen -= 20;
            }
            if(!npc.boss)
            {
                npc.velocity *= 0.95f;
            }
            base.Update(npc, ref buffIndex);
        }
    }
}
