﻿using CalamityMod;
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
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Particles;
using Terraria.ModLoader.IO;
using Terraria.Audio;
using CalamityMod.Buffs.StatDebuffs;

namespace FKsCRE.Content.DeveloperItems.Bullet.UltraLowTemp
{
    public class UltraLowTempPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.UltraLowTemp";
        public override string Texture => "CalamityMod/Projectiles/LaserProj"; // 引用原始纹理

        public Color baseColor = Color.Cyan; // 冰元素的蓝色
        public Vector2 baseVel;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // 判断 timeLeft 是否小于或等于 x
            if (Projectile.timeLeft <= 200)
            {
                Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Particles/DrainLineBloom").Value;
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], (baseColor * 0.7f) with { A = 0 }, 1, texture);
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, baseColor with { A = 0 }, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
            }
            return false; // 禁止默认绘制行为
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.MaxUpdates = 6;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        public override void AI()
        {
            // LaserProj需要这种转动（无穷的ChargedBlast）
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // 添加光效
            Lighting.AddLight(Projectile.Center, Color.Lerp(Color.Blue, Color.AliceBlue, 0.5f).ToVector3() * 0.49f);

            // 子弹在出现之后很短一段时间会变得可见
            if (Projectile.timeLeft == 255)
                Projectile.alpha = 0;

            if (baseColor == Color.Cyan)
            {
                baseVel = Projectile.velocity;
            }

            if (Projectile.timeLeft % 3 == 0)
            {
                int particleCount = Main.rand.Next(1, 3); // 生成1到2个粒子
                for (int i = 0; i < particleCount; i++)
                {
                    Particle iceParticle = new LineParticle(Projectile.Center - Projectile.velocity * 3, -Projectile.velocity * 0.05f, false, 5, 2f, Color.Cyan * 0.65f);
                    GeneralParticleHandler.SpawnParticle(iceParticle);
                }
            }


            // 检测水并替换为冰块
            {
                int tileX = (int)(Projectile.position.X / 16f);
                int tileY = (int)(Projectile.position.Y / 16f);
                // 检查是否在地图有效范围内
                if (tileX >= 0 && tileX < Main.maxTilesX && tileY >= 0 && tileY < Main.maxTilesY)
                {
                    Tile tile = Framing.GetTileSafely(tileX, tileY); // 获取当前的 Tile

                    // 检测是否是水并且有液体
                    if (tile.LiquidType == LiquidID.Water && tile.LiquidAmount > 0)
                    {
                        // 将水替换为冰块
                        WorldGen.PlaceTile(tileX, tileY, TileID.IceBlock, true, true);
                        NetMessage.SendTileSquare(-1, tileX, tileY, 1); // 同步地图更新

                        // 销毁弹幕
                        Projectile.Kill();
                    }
                }
            }


        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120); // 冰河时代
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
            Vector2 center = Projectile.Center;
            float radius = 25 * 16; // 半径25格
            for (int i = 0; i < 2; i++)
            {
                Vector2 spawnPos = center + Main.rand.NextVector2Circular(radius, radius); // 随机选择圆周上的位置
                Vector2 direction = (Projectile.Center - spawnPos).SafeNormalize(Vector2.Zero);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, direction * 10f, ModContent.ProjectileType<UltraLowTempSPIT>(), (int)(Projectile.damage * 0.6f), Projectile.knockBack, Main.myPlayer);
            }
        }
    }
}