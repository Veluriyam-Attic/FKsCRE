using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace FKsCRE.Content.Arrows.天蓝矢s
{
    public class 天蓝矢_Proje : ArrowProjectile
    {
        Vector2 Mouse = default;
        Vector2 spee = default;
        int rand = -1;
        int num = 0;
        int GenZnum = 0;
        int npcjl = -1;
        NanTingGProje proje = default;
        public override void SetDefaults()
        {
            Projectile.penetrate = 2;
            base.SetDefaults();
        }
        public override void AI()
        {
            proje = Projectile.GetGlobalProjectile<NanTingGProje>();
            if (num == 0)
            {
                Mouse = Main.MouseWorld;
                spee = Vector2.Normalize(Mouse - Main.player[Projectile.owner].Center);
                rand = Main.rand.Next(1, 6);
                num++;
                Projectile.netUpdate = true;
            }
            if (rand == 1)
            {
                if (GenZnum == 0)
                {
                    Projectile.velocity = spee * 15f;
                    for (int i = 0; i < Main.npc.Length - 2; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (!npc.friendly && npc.active && npc.lifeMax > 5 && Vector2.Distance(Projectile.Center, npc.Center) < 600)
                        {
                            Projectile.penetrate = 1;
                            npcjl = i;
                            GenZnum++;
                            Projectile.netUpdate = true;
                        }
                    }
                }
                if(GenZnum != 0 && Main.npc[npcjl].active)
                {
                    Projectile.velocity = Vector2.Normalize(Main.npc[npcjl].Center - Projectile.Center) * 18;
                }
                else
                {
                    Projectile.velocity = spee * 15f;
                }
            }
            else
            {
                if (proje.GetItem().Name.Equals("Daedalus Stormbow"))
                {
                }
                else
                {
                    Projectile.velocity = spee * 15f;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI/2;
            base.AI();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (GenZnum != 0)
            {
                Projectile.NewProjectileDirect(default, target.Center, Vector2.Zero, ModContent.ProjectileType<天蓝旋风>(), (proje.GetItem().damage + Projectile.damage) / 2, 0f, Projectile.owner);
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(Mouse);
            writer.WriteVector2(spee);
            writer.Write(num);
            writer.Write(GenZnum);
            writer.Write(npcjl);
            base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Mouse = reader.ReadVector2();
            spee = reader.ReadVector2();
            num = reader.ReadInt32();
            GenZnum = reader.ReadInt32();
            npcjl = reader.ReadInt32();

        }
    }
}
