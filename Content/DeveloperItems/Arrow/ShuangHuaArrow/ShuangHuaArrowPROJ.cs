using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using FKsCRE.CREConfigs;
using CalamityMod.Particles;
using Terraria.Audio;

namespace FKsCRE.Content.DeveloperItems.Arrow.ShuangHuaArrow
{
    public class ShuangHuaArrowPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.ShuangHuaArrow";
        public override string Texture => "FKsCRE/Content/DeveloperItems/Arrow/ShuangHuaArrow/ShuangHuaArrow";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 画残影效果
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
                return false;
            }
            return true;
        }
        public override void SetDefaults()
        {
            // 设置弹幕的基础属性
            Projectile.width = 11; // 弹幕宽度
            Projectile.height = 24; // 弹幕高度
            Projectile.friendly = true; // 对敌人有效
            Projectile.DamageType = DamageClass.Ranged; // 远程伤害类型
            Projectile.penetrate = 1; // 穿透力为1，击中一个敌人就消失
            Projectile.timeLeft = 600; // 弹幕存在时间为600帧
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 14; // 无敌帧冷却时间为14帧
            Projectile.ignoreWater = true; // 弹幕不受水影响
            Projectile.arrow = true;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = ProjAIStyleID.Arrow; // 让弹幕受到重力影响
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 变成大冰锥并向上飞行
            Projectile.width = 20;
            Projectile.height = 40;
            Projectile.velocity = new Vector2(0, -8f); // 向上飞行
            Projectile.timeLeft = 60; // 设定寿命为短暂时间内销毁自己
            Projectile.penetrate = -1; // 不再穿透

            // 禁用原始的碰撞逻辑
            Projectile.tileCollide = false;
          
        }

        public override void AI()
        {
            // 调整弹幕的旋转，使其在飞行时保持水平
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;

            // Lighting - 添加天蓝色光源，光照强度为 0.49
            Lighting.AddLight(Projectile.Center, Color.LightSkyBlue.ToVector3() * 0.49f);

            SparkParticle Visual = new SparkParticle(Projectile.Center, Projectile.velocity * 0.1f, false, 2, 1.2f, Color.SkyBlue);
            GeneralParticleHandler.SpawnParticle(Visual);
            if (Projectile.localAI[0] % 3 == 0)
            {
                LineParticle subTrail = new LineParticle(Projectile.Center, Projectile.velocity * 0.01f, false, 4, 1.1f, Color.SkyBlue);
                GeneralParticleHandler.SpawnParticle(subTrail);
            }

            // 检查是否即将销毁
            if (Projectile.timeLeft <= 10)
            {
                // 射出5个方向的 ShuangHuaArrowSPLIT
                Vector2 baseDirection = Vector2.UnitY; // 绝对正下方方向
                for (int i = -2; i <= 2; i++) // 5 个方向
                {
                    float offsetAngle = MathHelper.ToRadians(i * 5); // 每个方向的偏移角度（-10度到10度）
                    Vector2 direction = baseDirection.RotatedBy(offsetAngle); // 相对绝对正下方生成新的方向
                    direction *= 8f; // 设定飞行速度

                    // 生成新的弹幕
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction, ModContent.ProjectileType<ShuangHuaArrowSPLIT>(), (int)(Projectile.damage * 0.35f), Projectile.knockBack, Projectile.owner);
                }


                // 销毁自身
                Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item30, Projectile.position);

            int Dusts = 10;
            float radians = MathHelper.TwoPi / Dusts;
            Vector2 spinningPoint = Vector2.Normalize(new Vector2(-1f, -1f));
            for (int i = 0; i < Dusts; i++)
            {
                Vector2 dustVelocity = spinningPoint.RotatedBy(radians * i).RotatedBy(0.5f) * 6.5f;
                Particle smoke = new HeavySmokeParticle(Projectile.Center, dustVelocity * Main.rand.NextFloat(1f, 2.6f), Color.WhiteSmoke, 18, Main.rand.NextFloat(0.9f, 1.6f), 0.35f, Main.rand.NextFloat(-1, 1), true);
                GeneralParticleHandler.SpawnParticle(smoke);
            }
        }







    }
}