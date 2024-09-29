using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;



namespace FKsCRE.Content.Arrows.ScoriaArrow
{
    internal class ScoriaArrowPROJ : ModProjectile
    {
        private bool hasTriggeredUpwardMovement = false; // 添加变量来标记是否触发过向上飞行效果

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14; // 假设箭的宽度
            Projectile.height = 28; // 假设箭的高度
            Projectile.friendly = true; // 是否对玩家友好
            Projectile.DamageType = DamageClass.Ranged; // 远程伤害类型
            Projectile.arrow = true; // 标记为箭类型
            Projectile.extraUpdates = 0; // 提升更新速率
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 14; // 无敌帧冷却时间为14帧
            Projectile.timeLeft = 180; // 持续时间为3秒
            Projectile.aiStyle = ProjAIStyleID.Arrow; // 受重力影响
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            // 调整弹幕的旋转，使其在飞行时保持水平
            // 垂直朝下贴图
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;

            // 倾斜朝右下角贴图
            // Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi - MathHelper.ToRadians(45);


            // 加速效果：每帧增加2%的X轴速度，Y轴增加5个速度单位
            Projectile.velocity.X *= 1.02f;
            Projectile.velocity.Y *= 1.03f;

            // 可以加入一些基本的粒子特效
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, null, 0, Color.Orange, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            // 添加烟雾效果，每隔一段时间随机释放烟雾
            if (Main.rand.NextBool(5) && Main.netMode != NetmodeID.Server) // 20%的几率生成烟雾
            {
                int smoke = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(375, 378), 0.75f);
                Main.gore[smoke].velocity = Projectile.velocity * 0.1f;
                Main.gore[smoke].behindTiles = true;
            }

            // 自定义AI逻辑
            if (Projectile.timeLeft <= 60 && !hasTriggeredUpwardMovement) // 飞行了4秒，没有命中任何东西且没有触发过上升效果
            {
                TriggerUpwardMovement(); // 触发垂直向上飞行效果
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!hasTriggeredUpwardMovement) // 确保只触发一次
            {
                TriggerUpwardMovement(); // 触发向上飞行
            }
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!hasTriggeredUpwardMovement) // 确保只触发一次
            {
                TriggerUpwardMovement(); // 触发向上飞行
            }
        }

        // 定义垂直飞行和粒子效果的触发
        private void TriggerUpwardMovement()
        {
            // 标记已经触发
            hasTriggeredUpwardMovement = true;

            // 让弹幕迅速飞向上方，并带有随机偏移
            Projectile.velocity = new Vector2(0, -6.5f).RotatedByRandom(MathHelper.ToRadians(15)); // 随机15度偏移

            // 调整粒子颜色，使用橘黄色或深灰色
            SparkParticle Visual = new SparkParticle(Projectile.Center, Projectile.velocity * 0.1f, false, 2, 1.2f, Color.Orange);
            GeneralParticleHandler.SpawnParticle(Visual);

            // 类似的轨迹粒子效果
            if (Projectile.localAI[0] % 3 == 0)
            {
                LineParticle subTrail = new LineParticle(Projectile.Center, Projectile.velocity * 0.01f, false, 4, 1.1f, Color.Orange);
                GeneralParticleHandler.SpawnParticle(subTrail);
            }

            // 在触发垂直飞行时生成一个独立的爆炸弹幕
            if (Projectile.owner == Main.myPlayer)
            {
                int boom = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FuckYou>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
                if (boom.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[boom].DamageType = DamageClass.MeleeNoSpeed;
                }
            }

            // 在固定距离飞行后消失，并产生粒子
            if (Projectile.localAI[0] >= 100) // 假设飞行100帧的距离
            {
                Projectile.Kill();
                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, -Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)), 0, Color.Orange, Main.rand.NextFloat(1.5f, 2.5f));
                    dust.noGravity = true;
                }
            }

            // 在飞行的过程中释放波浪线粒子效果
            if (Main.rand.NextBool(2))
            {
                Vector2 offset = new Vector2(0, 5).RotatedByRandom(MathHelper.ToRadians(180));
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, DustID.Torch, null, 0, Color.Orange, 1.5f);
                dust.noGravity = true;
            }

            // 定期释放 ScoriaArrowFireball 弹幕
            if (Projectile.localAI[0] % 3 == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(5f, 5f), ModContent.ProjectileType<ScoriaArrowFireball>(), (int)(Projectile.damage * 0.33f), Projectile.knockBack, Projectile.owner);
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            // 弹幕结束时生成橘黄色粒子特效
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, -Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)), 0, Color.Orange, Main.rand.NextFloat(1.5f, 2.5f));
                dust.noGravity = true;
            }
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(5f, 5f), ModContent.ProjectileType<ScoriaArrowFireball>(), (int)(Projectile.damage * 0.33f), Projectile.knockBack, Projectile.owner);
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor, 1);
            return false;
        }

    }
}