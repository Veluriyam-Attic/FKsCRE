using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using CalamityMod.Particles;
using Terraria.DataStructures;
using CalamityMod;

namespace FKsCRE.Content.DeveloperItems.Weapon.BrassBeast
{
    public class BrassBeastHeavySmoke : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.BrassBeast";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj"; // 使用透明贴图

        public override void SetDefaults()
        {
            Projectile.width = 160; // 碰撞箱大小
            Projectile.height = 160;
            Projectile.friendly = true; // 友方投射物
            Projectile.ignoreWater = true; // 无视水
            Projectile.tileCollide = false; // 不与地形碰撞
            Projectile.DamageType = DamageClass.Ranged; // 远程伤害
            Projectile.penetrate = -1; // 无限穿透
            Projectile.MaxUpdates = 2; // 额外更新次数
            Projectile.timeLeft = 120; // 生命周期
        }

        public override void AI()
        {
            // 减速效果
            Projectile.velocity *= 0.9f;

            // 添加粒子效果
            for (int i = 0; i < 3; i++)
            {
                Particle heavySmoke = new HeavySmokeParticle(
                    Projectile.Center + Main.rand.NextVector2Circular(Projectile.width / 2, Projectile.height / 2),
                    Main.rand.NextVector2Circular(1f, 1f),
                    Color.Black * 0.8f, // 黑色烟雾
                    Main.rand.Next(40, 60), // 粒子生命周期
                    Main.rand.NextFloat(1.5f, 2f), // 粒子缩放
                    1f, // 不透明度
                    Main.rand.NextFloat(-1f, 1f), // 随机旋转
                    true
                );
                GeneralParticleHandler.SpawnParticle(heavySmoke);
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            // 屏幕震动效果
            float shakePower = 5f; // 设置震动强度
            float distanceFactor = Utils.GetLerpValue(1000f, 0f, Projectile.Distance(Main.LocalPlayer.Center), true); // 距离衰减
            Main.LocalPlayer.Calamity().GeneralScreenShakePower = Math.Max(Main.LocalPlayer.Calamity().GeneralScreenShakePower, shakePower * distanceFactor);

            base.OnSpawn(source);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 施加火焰Debuff
            target.AddBuff(BuffID.OnFire, 300); // 燃烧
            target.AddBuff(BuffID.CursedInferno, 300); // 诅咒地狱火

            // 击中后释放火焰和烟雾粒子
            for (int i = 0; i < 8; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f) * -Projectile.velocity;
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Torch,
                    velocity,
                    100,
                    Color.OrangeRed,
                    1.5f
                );
            }
            for (int i = 0; i < 5; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f) * -Projectile.velocity;
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Smoke,
                    velocity,
                    100,
                    Color.Gray,
                    1f
                );
            }
        }
    }
}
