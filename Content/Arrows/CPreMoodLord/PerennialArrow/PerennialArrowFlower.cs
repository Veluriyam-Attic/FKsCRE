//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.ModLoader;
//using Terraria.ID;
//using CalamityMod;
//using Terraria.Audio;

//namespace FKsCRE.Content.Arrows.PerennialArrow
//{
//    public class PerennialArrowFlower : ModProjectile
//    {
//        private bool stuck = false; // 标志弹幕是否已经粘附在目标身上

//        public override void SetDefaults()
//        {
//            Projectile.width = 20;
//            Projectile.height = 20;
//            Projectile.friendly = true;
//            Projectile.timeLeft = 300; // 5秒
//            Projectile.tileCollide = false;
//            Projectile.penetrate = -1;
//        }

//        public override void AI()
//        {
//            // 获取目标 NPC
//            NPC target = Main.npc[(int)Projectile.ai[0]];

//            if (target.active)
//            {
//                // 如果弹幕还未粘附
//                if (!stuck)
//                {
//                    // 快速追踪目标
//                    float speed = 15f; // 设置一个较快的追踪速度
//                    Vector2 direction = target.Center - Projectile.Center;
//                    direction.Normalize();
//                    Projectile.velocity = direction * speed;

//                    // 如果与目标距离足够近，开始粘附
//                    if (Projectile.Distance(target.Center) < 30f)
//                    {
//                        // 开启粘附逻辑，并设置速度为零
//                        stuck = true;
//                        Projectile.velocity = Vector2.Zero; // 停止运动
//                        Projectile.StickyProjAI(10); // 粘附在目标上
//                    }
//                }
//                else
//                {
//                    // 如果已经粘附，则跟随目标
//                    Projectile.Center = target.Center;
//                }
//            }
//            else
//            {
//                Projectile.Kill(); // 如果目标消失，弹幕也消失
//            }
//        }

//        public override void OnKill(int timeLeft)
//        {
//            // 播放击杀音效
//            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

//            // 恢复玩家生命值
//            Main.player[Projectile.owner].statLife += 10;
//            Main.player[Projectile.owner].HealEffect(10);
//        }
//    }
//}