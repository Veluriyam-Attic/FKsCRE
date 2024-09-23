using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.凝胶.寒元凝胶
{
    public class 寒元凝胶_DeBuff : 凝胶_DeBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            //player.GetDamage(DamageClass.Generic) *= 1.1F;
            //player.GetArmorPenetration(DamageClass.Generic) += 5;
            base.Update(player, ref buffIndex);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            for(int i =0;i<Main.player.Length -2;i++)
            {
                if (ModTime.Time % 12 == 0)
                {
                    if (Vector2.Distance(Main.player[i].Center, npc.Center) < 50)
                    {
                        Main.player[i].statLife -= 1;
                    }
                }
                if (Vector2.Distance(Main.player[i].Center, npc.Center) < 100)
                {
                    //Main.player[i].GetArmorPenetration(DamageClass.Generic) += 5;
                    Main.player[i].GetDamage(DamageClass.Generic) *= 1.1f;
                    Main.player[i].velocity *= 0.8f;
                }
            }
            base.Update(npc, ref buffIndex);
        }
    }
}
