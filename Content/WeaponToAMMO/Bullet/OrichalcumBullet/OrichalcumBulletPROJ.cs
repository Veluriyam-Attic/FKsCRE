using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using FKsCRE.CREConfigs;

namespace FKsCRE.Content.WeaponToAMMO.Bullet.OrichalcumBullet
{
    internal class OrichalcumBulletPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "WeaponToAMMO.Bullet.OrichalcumBullet";
        //public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1; // 根据模式设置穿透次数
            Projectile.timeLeft = 300;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        public override void AI()
        {
            // 保持弹幕旋转
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Lighting - 添加深橙色光源，光照强度为 0.55
            Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3() * 0.55f);

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // Add flying particles
                if (Main.rand.NextBool(1)) // 随机 1/3 概率
                {
                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center,
                        Main.rand.NextBool() ? 145 : 69, // 粒子特效 ID 145 和 69 交替
                        -Projectile.velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(0.01f, 0.3f)
                    );
                    dust.noGravity = true; // 粒子无重力
                    dust.scale = Main.rand.NextFloat(0.5f, 0.9f); // 随机大小
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 花瓣弹幕逻辑
            for (int i = 0; i < 2; i++)
            {
                int direction = Main.player[Projectile.owner].direction;
                float xStart = Main.screenPosition.X;
                if (direction < 0)
                    xStart += Main.screenWidth;
                float yStart = Main.screenPosition.Y + Main.rand.Next(Main.screenHeight);
                Vector2 startPos = new Vector2(xStart, yStart);
                Vector2 pathToTravel = target.Center - startPos;
                pathToTravel.X += Main.rand.NextFloat(-50f, 50f) * 0.1f;
                pathToTravel.Y += Main.rand.NextFloat(-50f, 50f) * 0.1f;
                float speedMult = 24f / pathToTravel.Length();
                pathToTravel *= speedMult;
                //int petal = Projectile.NewProjectile(Projectile.GetSource_FromThis(), startPos, pathToTravel, ProjectileID.FlowerPetal, (int)((damageDone) * 0.55), 0f, Projectile.owner);
                int petal = Projectile.NewProjectile(Projectile.GetSource_FromThis(), startPos, pathToTravel, ProjectileID.FlowerPetal, (int)(Projectile.damage * 1.1), 0f, Projectile.owner);
                if (petal.WithinBounds(Main.maxProjectiles))
                    Main.projectile[petal].DamageType = DamageClass.Ranged; // 改为射手类伤害
            }

            // 粒子特效：正方形弹开
            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = i switch
                {
                    0 => new Vector2(-1, -1), // 左上
                    1 => new Vector2(1, -1),  // 右上
                    2 => new Vector2(1, 1),   // 右下
                    _ => new Vector2(-1, 1),  // 左下
                };
                Dust dust = Dust.NewDustPerfect(
                    target.Center + offset * 20f, // 粒子起点偏移
                    i % 2 == 0 ? 145 : 69,       // 使用交替的粒子 ID
                    offset * Main.rand.NextFloat(1f, 2f) // 偏移方向和速度
                );
                dust.noGravity = true; // 粒子无重力
                dust.scale = Main.rand.NextFloat(1f, 1.5f); // 粒子缩放大小
            }
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }


    }
}