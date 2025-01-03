using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;

namespace FKsCRE.Content.Arrows.EAfterDog.AuricArrow
{
    public class AuricArrowNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4; // 设置为4帧动画
        }

        public float StoredDamage { get; set; } // Damage passed in from the projectile

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.lifeMax = 150;
            NPC.damage = 200; // No collision damage
            NPC.defense = 5;
            NPC.friendly = true; // Belongs to the player's side
            NPC.aiStyle = -1; // Custom AI
            NPC.knockBackResist = 0f; // No knockback resistance for now
            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }

        public override void AI()
        {

            // Find nearest enemy logic
            NPC target = FindClosestEnemy();
            if (target != null)
            {
                Vector2 direction = target.Center - NPC.Center;
                direction.Normalize();
                NPC.velocity = Vector2.Lerp(NPC.velocity, direction * 30f, 0.08f); // 能够逐渐加速
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += NPC.ai[0] == 2.1f ? 1.5 : 1D;
            if (Main.zenithWorld)
            {
                NPC.frameCounter += 2D;
            }
            if (NPC.frameCounter > 3D) // 使用了4帧的时间间隔
            {
                NPC.frameCounter = 0D;
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 3) // 因为总共有4帧
            {
                NPC.frame.Y = 0;
            }
        }


        private NPC FindClosestEnemy()
        {
            NPC closestNPC = null;
            float closestDistance = float.MaxValue;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.CanBeChasedBy(this))
                {
                    float distance = Vector2.Distance(NPC.Center, npc.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }

        public override void OnKill()
        {
            SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/Yharon/YharonInfernado"));
            float rotation = MathHelper.TwoPi / 3;
            for (int i = 0; i < 3; i++)
            {
                Vector2 direction = Vector2.UnitY.RotatedBy(rotation * i);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 10f, ModContent.ProjectileType<AuricArrowPROJ>(), (int)StoredDamage, 0, Main.myPlayer);
            }

            //Projectile.NewProjectile(
            //    NPC.GetSource_FromThis(),
            //    NPC.Center,
            //    Vector2.Zero, // 初始速度为零
            //    ModContent.ProjectileType<DropThing>(),
            //    1, // 设置伤害为1
            //    0,
            //    Main.myPlayer
            //);

            float rotationStep = MathHelper.TwoPi / 5; // 每两个弹幕之间的夹角
            for (int i = 0; i < 5; i++)
            {
                Vector2 direction = Vector2.UnitY.RotatedBy(rotationStep * i); // 计算方向
                Projectile.NewProjectile(
                    NPC.GetSource_FromThis(),
                    NPC.Center,
                    direction * 15f, // 设置速度为
                    ModContent.ProjectileType<DropThing>(),
                    1, // 设置伤害为1
                    0,
                    Main.myPlayer
                );
            }

            //Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<AuricArrowNPCDrop>());
        }


    }
}
