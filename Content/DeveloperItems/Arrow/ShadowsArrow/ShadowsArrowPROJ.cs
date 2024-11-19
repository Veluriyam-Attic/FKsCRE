using CalamityMod;
using FKsCRE.CREConfigs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Items.Ammo;
using CalamityMod.Projectiles.Magic;


namespace FKsCRE.Content.DeveloperItems.Arrow.ShadowsArrow
{
    public class ShadowsArrowPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.ShadowsArrow";
        public override string Texture => "FKsCRE/Content/DeveloperItems/Arrow/ShadowsArrow/ShadowsArrow";

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

        private bool hasSplit = false; // 确保弹幕只分裂一次
        private int frameCounter = 0; // 用于记录帧数
        private static readonly int[] VanillaProjectiles = new int[]
        {
            1, 4, 5, 41, 82, 91, 103, 117, 120, 172, 225, 282, 357, 469, 474, 485, 495, 631, 639, 932, 1006
        };
        private static readonly int[] CalamityProjectiles = new int[]
        {
            ModContent.ProjectileType<BarinadeArrow>(),
            ModContent.ProjectileType<Shell>(),
            ModContent.ProjectileType<ToxicArrow>(),
            ModContent.ProjectileType<FeatherLarge>(),
            ModContent.ProjectileType<SlimeStream>(),
            ModContent.ProjectileType<LunarBolt>(),

            ModContent.ProjectileType<FlareBat>(),
            ModContent.ProjectileType<BrimstoneBolt>(),
            ModContent.ProjectileType<BoltArrow>(),
            ModContent.ProjectileType<CorrodedShell>(),
            ModContent.ProjectileType<MistArrow>(),
            ModContent.ProjectileType<LeafArrow>(),
            ModContent.ProjectileType<SporeBomb>(),
            ModContent.ProjectileType<VernalBolt>(),
            ModContent.ProjectileType<BallistaGreatArrow>(),
            ModContent.ProjectileType<PlagueArrow>(),

            ModContent.ProjectileType<PlanetaryAnnihilationProj>(),
            ModContent.ProjectileType<AstrealArrow>(),
            ModContent.ProjectileType<PrecisionBolt>(),
            ModContent.ProjectileType<TelluricGlareArrow>(),
            ModContent.ProjectileType<Bolt>(),
            ModContent.ProjectileType<TheMaelstromShark>(),
            ModContent.ProjectileType<TyphoonArrow>(),
            ModContent.ProjectileType<MiniSharkron>(),
            ModContent.ProjectileType<DWArrow>(),
            ModContent.ProjectileType<UltimaBolt>(),
            ModContent.ProjectileType<UltimaRay>(),
            ModContent.ProjectileType<UltimaSpark>(),
            ModContent.ProjectileType<DrataliornusFlame>(),
            ModContent.ProjectileType<DrataliornusExoArrow>(),
            ModContent.ProjectileType<DrataliornusExoArrow>(),
            ModContent.ProjectileType<SkyFlareFriendly>(),
            ModContent.ProjectileType<ExoCrystalArrow>(),
            ModContent.ProjectileType<CondemnationArrow>(),
            ModContent.ProjectileType<CondemnationArrowHoming>(),
            ModContent.ProjectileType<ContagionArrow>(),

            ModContent.ProjectileType<CinderArrowProj>(),
            ModContent.ProjectileType<VeriumBoltProj>(),
            ModContent.ProjectileType<SproutingArrowMain>(),
            ModContent.ProjectileType<IcicleArrowProj>(),
            ModContent.ProjectileType<ElysianArrowProj>(),
            ModContent.ProjectileType<BloodfireArrowProj>(),
            ModContent.ProjectileType<VanquisherArrowProj>()
        };
        private static readonly string[] CustomModProjectiles = new string[]
        {
            "AerialiteArrowPROJ","BloodBeadsArrowPROJ","PurifiedGelArrowPROJ",
            "StarblightSootArrowPROJ",
            "AstralArrowPROJ","LifeAlloyArrowPROJ","PerennialArrowPROJ","ScoriaArrowPROJ",
            "DivineGeodeArrowPROJ","EffulgentFeatherArrowPROJ","PolterplasmArrowPROJ","UelibloomArrowPROJ",
            "AuricArrowPROJ", "MiracleMatterArrowPROJ",

            "MaoMaoChongPROJ","PlasmaDriveCorePrototypeArrowPROJ","TimeLeaperPROJ"
        };

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1; // 无限穿透
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1; // 增加更新次数
        }

        public override void AI()
        {
            // 调整弹幕的旋转，使其在飞行时保持水平
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
            frameCounter++;
            Projectile.velocity *= 1.02f; // 逐渐加速

            // 在飞行过程中生成黑色粒子特效
            for (int j = 0; j < 2; j++)
            {
                Vector2 particleVelocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(15 * (j % 2 == 0 ? 1 : -1))) * Main.rand.NextFloat(1f, 2.6f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, particleVelocity, 0, Color.Black, Main.rand.NextFloat(0.9f, 1.6f));
                dust.noGravity = true;
            }

            if (frameCounter >= 10 && !hasSplit)
            {
                SplitProjectile();
                hasSplit = true; // 确保只分裂一次
            }
        }

        private void SplitProjectile()
        {
            int splitCount = Main.rand.Next(2, 5); // 随机生成 2 到 4 个弹幕

            for (int i = 0; i < splitCount; i++)
            {
                float angle = MathHelper.ToRadians(Main.rand.Next(-10, 11));
                Vector2 newVelocity = Projectile.velocity.RotatedBy(angle) * 0.9f;

                int randomType = Main.rand.Next(3); // 随机选择 0-原版弹幕, 1-CalamityMod, 2-自定义模组弹幕
                if (randomType == 0)
                {
                    // 原版弹幕
                    int selectedVanilla = VanillaProjectiles[Main.rand.Next(VanillaProjectiles.Length)];
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, newVelocity, selectedVanilla, Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                else if (randomType == 1)
                {
                    // Calamity 弹幕
                    int selectedCalamity = CalamityProjectiles[Main.rand.Next(CalamityProjectiles.Length)];
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, newVelocity, selectedCalamity, Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                else
                {
                    // 自定义模组弹幕
                    string selectedProjectile = CustomModProjectiles[Main.rand.Next(CustomModProjectiles.Length)];
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, newVelocity, Mod.Find<ModProjectile>(selectedProjectile).Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                }

                // 黑色粒子效果
                for (int j = 0; j < 50; j++)
                {
                    Vector2 particleVelocity = newVelocity.RotatedBy(MathHelper.ToRadians(15 * (j % 2 == 0 ? 1 : -1))) * Main.rand.NextFloat(1f, 2.6f);
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, particleVelocity, 0, Color.Black, Main.rand.NextFloat(0.9f, 1.6f));
                    dust.noGravity = true;
                }
            }
        }
    }
}