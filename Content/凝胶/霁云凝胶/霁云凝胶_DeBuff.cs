using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.凝胶.霁云凝胶
{
    public class 霁云凝胶_DeBuff : 凝胶_DeBuff
    {
        //对于NPC
        public override void Update(NPC npc, ref int buffIndex)
        {
            效果上身 le = npc.GetGlobalNPC<效果上身>();
            le.cnet.Y = le.cnet.Y - 5f;
            npc.Center = le.cnet;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write(30000);
                packet.Write(le.cnet.X);
                packet.Write(le.cnet.Y);
                packet.Write(npc.whoAmI);
                packet.Send();
            }
            Main.NewText(npc.Center);
            //npc.life -= 100;
            base.Update(npc, ref buffIndex);
        }
    }
}
