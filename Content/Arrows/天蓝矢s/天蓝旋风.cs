using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace FKsCRE.Content.Arrows.天蓝矢s
{
    public class 天蓝旋风 : ArrowProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = 4;
            Projectile.timeLeft = 90;
            base.SetDefaults();
        }
        bool 上边 = false, 下边 = false, 左边 = false, 右边 = false;
        NPC npc;
        Dictionary<int, Vector2> npcmap = new Dictionary<int, Vector2>();
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            for (int i = 0; i < Main.npc.Length - 2; i++)
            {
                npc = Main.npc[i];
                if (npc.active && Vector2.Distance(npc.Center,Projectile.Center) < 500)
                {
                    if(npcmap.ContainsKey(i))
                    {
                        npcmap[i] = npc.velocity;
                    }
                    else
                    {
                        npcmap.Add(i,npc.velocity);
                    }
                    Vector2 npcv2 = npc.Center; Vector2 projve = Projectile.Center;
                    //float xj = Vector2.Distance(npcv2,projve);

                    float xj = projve.X - npcv2.X;
                    xj = (float)Math.Sqrt(xj * xj);
                    float Y = (float)Math.Sqrt((projve.Y - npcv2.Y) * (projve.Y - npcv2.Y));
                    if (projve.X > npcv2.X) {右边 = true; 左边 = false; }
                    else {左边 = true; 右边 = false; }

                    if (projve.Y < npcv2.Y) {上边 = false; 下边 = true; }
                    else {上边 = true; 下边 = false; }
                    //npc.velocity = Vector2.Zero;
                    if (左边) npc.velocity.X -= 0.5F;
                    if (右边) npc.velocity.X += 0.5F;
                    if (上边) npc.velocity.Y += 0.5F;
                    if (下边) npc.velocity.Y -= 0.5F;
                }
            }

            base.AI();
        }
        public override void OnKill(int timeLeft)
        {
            foreach(int npc in npcmap.Keys)
            {
                if (Main.npc[npc].active)
                {
                    Main.npc[npc].velocity = npcmap[npc];
                }
            }
            npcmap.Clear();
            base.OnKill(timeLeft);
        }
    }
}
