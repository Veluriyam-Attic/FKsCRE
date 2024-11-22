using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using FKsCRE.Content.Ammunition.BPrePlantera.StarblightSootBullet;
using CalamityMod.Dusts;

namespace FKsCRE.Content.Ammunition.BPrePlantera.StarblightSootBullet
{
    internal class StarblightSootBulletArea : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 1000;
            Projectile.height = 1000;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180; // 存在 3 秒
            Projectile.tileCollide = false; // 不与方块碰撞
        }

        public override void AI()
        {
            // 粒子特效
            float rotationOffset = Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4); // 随机旋转
            Vector2 direction = Vector2.UnitX.RotatedBy(rotationOffset).SafeNormalize(Vector2.Zero);
            int randomDust = Utils.SelectRandom(Main.rand, new int[]
            {
                ModContent.DustType<AstralOrange>(),
                ModContent.DustType<AstralBlue>()
            });
            Dust dust = Dust.NewDustPerfect(Projectile.Center + direction * Main.rand.NextFloat(100f, 500f), randomDust, direction * Main.rand.NextFloat(3f, 6f));
            dust.noGravity = true;
            dust.scale = 1.2f + Main.rand.NextFloat(0.3f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 给接触的敌人施加 StarblightSootBulletEBuff
            target.AddBuff(ModContent.BuffType<StarblightSootBulletEBuff>(), 300);
        }
    }
}
