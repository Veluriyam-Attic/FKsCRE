//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System;
//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria.Audio;
//using CalamityMod.Projectiles.BaseProjectiles;
//using FKsCRE.Content.Gel.APreHardMode.AerialiteGel;
//using FKsCRE.Content.Gel.APreHardMode.GeliticGel;
//using FKsCRE.Content.Gel.APreHardMode.HurricaneGel;
//using FKsCRE.Content.Gel.APreHardMode.WulfrimGel;
//using FKsCRE.Content.Gel.BPrePlantera.CryonicGel;
//using FKsCRE.Content.Gel.BPrePlantera.StarblightSootGel;
//using FKsCRE.Content.Gel.CPreMoodLord.AstralGel;
//using FKsCRE.Content.Gel.CPreMoodLord.LifeAlloyGel;
//using FKsCRE.Content.Gel.CPreMoodLord.LivingShardGel;
//using FKsCRE.Content.Gel.CPreMoodLord.PerennialGel;
//using FKsCRE.Content.Gel.CPreMoodLord.ScoriaGel;
//using FKsCRE.Content.Gel.DPreDog.BloodstoneCoreGel;
//using FKsCRE.Content.Gel.DPreDog.DivineGeodeGel;
//using FKsCRE.Content.Gel.DPreDog.EffulgentFeatherGel;
//using FKsCRE.Content.Gel.DPreDog.PolterplasmGel;
//using FKsCRE.Content.Gel.DPreDog.UelibloomGel;
//using FKsCRE.Content.Gel.DPreDog.UnholyEssenceGel;
//using FKsCRE.Content.Gel.EAfterDog.AuricGel;
//using FKsCRE.Content.Gel.EAfterDog.CosmosGel;
//using FKsCRE.Content.Gel.EAfterDog.MiracleMatterGel;

//namespace FKsCRE.Content.DeveloperItems.Weapon.TheGoldenFire
//{
//    public class TheGoldenFireHoldOut : BaseGunHoldoutProjectile
//    {
//        public override string Texture => "FKsCRE/Content/DeveloperItems/Weapon/TheGoldenFire/TheGoldenFire";

//        public override int AssociatedItemID => ModContent.ItemType<TheGoldenFire>(); // 绑定武器

//        public override float MaxOffsetLengthFromArm => 20f; // 设置与手臂的距离
//        public override float OffsetXUpwards => -10f; // 武器向上的偏移
//        public override float BaseOffsetY => -8f; // 基础的Y轴偏移
//        public override float OffsetYDownwards => 10f; // 武器向下的偏移

//        private int frameCounter = 0; // 帧计数器

//        public override void HoldoutAI()
//        {
//            frameCounter++;

//            // 每x帧射出三发子弹
//            if (frameCounter % 3 == 0)
//            {
//                ShootTheGoldenFire();
//            }
//        }
//        // 在文件顶端添加一个静态字典容器，存储凝胶的类型和对应的编号
//        public static readonly Dictionary<int, Color> GelColors = new Dictionary<int, Color>
//        {
//            { ItemID.Gel, Color.Blue }, // 原版凝胶
//            { ModContent.ItemType<AerialiteGel>(), Color.LightSkyBlue }, // 模组中的天蓝凝胶
//            { ModContent.ItemType<GeliticGel>(), Color.LightSkyBlue }, // 双色凝胶
//            { ModContent.ItemType<HurricaneGel>(), Color.BlueViolet }, // 棱镜凝胶
//            { ModContent.ItemType<WulfrimGel>(), new Color(153, 255, 102) }, // 钨钢凝胶

//            { ModContent.ItemType<CryonicGel>(), Color.LightSkyBlue }, // 寒元凝胶
//            { ModContent.ItemType<StarblightSootGel>(), Color.Orange }, // 调星凝胶

//            { ModContent.ItemType<AstralGel>(), Color.AliceBlue }, // 幻星凝胶
//            { ModContent.ItemType<LifeAlloyGel>(), Color.SpringGreen }, // 生命合金凝胶
//            { ModContent.ItemType<LivingShardGel>(), Color.ForestGreen }, // 生命碎片凝胶
//            { ModContent.ItemType<PerennialGel>(), Color.GreenYellow }, // 永恒凝胶
//            { ModContent.ItemType<ScoriaGel>(), Color.DarkOrange }, // 熔渣凝胶

//            { ModContent.ItemType<BloodstoneCoreGel>(), Color.MediumVioletRed }, // 血石核心凝胶
//            { ModContent.ItemType<DivineGeodeGel>(), Color.Gold }, // 神圣晶石凝胶
//            { ModContent.ItemType<EffulgentFeatherGel>(), Color.LightGray }, // 金羽凝胶
//            { ModContent.ItemType<PolterplasmGel>(), Color.LightPink }, // 灵质凝胶
//            { ModContent.ItemType<UelibloomGel>(), Color.DarkGreen }, // 龙蒿凝胶
//            { ModContent.ItemType<UnholyEssenceGel>(), Color.LightYellow }, // 烛火精华凝胶

//            { ModContent.ItemType<AuricGel>(), Color.LightGoldenrodYellow }, // 金元凝胶
//            { ModContent.ItemType<CosmosGel>(), Color.MediumPurple }, // 宇宙凝胶
//            { ModContent.ItemType<MiracleMatterGel>(), Color.GhostWhite }, // 奇迹凝胶
//            // 用这个网站快速查找对应颜色的rgb数字：https://www.w3schools.com/colors/colors_picker.asp
//        };

//        private void ShootTheGoldenFire()
//        {
//            Player player = Main.player[Projectile.owner];

//            if (player.HasAmmo(player.HeldItem))
//            {
//                if (player.PickAmmo(player.HeldItem, out int ammoProjectile, out float shootSpeed, out int damage, out float knockback, out int ammoType))
//                {
//                    Color fireColor;

//                    //// 转化凝胶为对应颜色
//                    //switch (ammoType)
//                    //{
//                    //    case ItemID.Gel: // 原版凝胶
//                    //        fireColor = Color.Blue;
//                    //        break;
//                    //    case ModContent.ItemType<AerialiteGel>(): // 模组中的天蓝色凝胶
//                    //        fireColor = Color.LightSkyBlue;
//                    //        break;
//                    //    default: // 未知或未定义的凝胶
//                    //        fireColor = Color.White;
//                    //        break;
//                    //}


//                    // 使用字典 GelColors 查找对应的颜色
//                    if (GelColors.TryGetValue(ammoType, out Color mappedColor))
//                    {
//                        fireColor = mappedColor; // 找到匹配颜色
//                    }
//                    else
//                    {
//                        fireColor = Color.White; // 未知或未定义的凝胶默认使用白色
//                    }


//                    // 生成火焰粒子效果
//                    GenerateFireParticles(fireColor);

//                    // 发射火焰
//                    float baseDamage = TheGoldenFire.TheFinalDamage;
//                    float speed = Projectile.velocity.Length();

//                    for (int i = 0; i < 2; i++)
//                    {
//                        float randomAngle = MathHelper.ToRadians(Main.rand.NextFloat(-5f, 5f));
//                        Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedBy(randomAngle);
//                        float randomSpeed = speed * Main.rand.NextFloat(6f, 12f);

//                        int proj = Projectile.NewProjectile(
//                            Projectile.GetSource_FromThis(),
//                            Projectile.Center,
//                            direction * randomSpeed,
//                            ModContent.ProjectileType<TheGoldenFirePROJ>(),
//                            Projectile.damage,
//                            Projectile.knockBack,
//                            player.whoAmI
//                        );

//                        // 将颜色信息传递给弹幕
//                        if (Main.projectile[proj].ModProjectile is TheGoldenFirePROJ fireProj)
//                        {
//                            fireProj.FireColor = fireColor;
//                        }
//                    }
//                }
//            }

//            // 播放开火音效
//            SoundEngine.PlaySound(SoundID.Item34, Projectile.position);

//            // 武器后坐力
//            OffsetLengthFromArm -= 1f;
//        }

//        private void GenerateFireParticles(Color fireColor)
//        {
//            Vector2 mousePosition = Main.MouseWorld; // 获取鼠标位置

//            for (int i = 0; i < 20; i++) // 每次射击生成20个粒子
//            {
//                Vector2 baseDirection = (mousePosition - Projectile.Center).SafeNormalize(Vector2.UnitX);
//                float randomAngle = MathHelper.ToRadians(Main.rand.NextFloat(-15f, 15f));
//                Vector2 adjustedDirection = baseDirection.RotatedBy(randomAngle);
//                float speed = Main.rand.NextFloat(6f, 10f);
//                Vector2 velocity = adjustedDirection * speed;
//                int dustType = Main.rand.Next(new int[] { DustID.Torch, DustID.Lava, DustID.Smoke });

//                // 应用 fireColor 进行粒子染色
//                Dust dust = Dust.NewDustPerfect(Projectile.Center, dustType, velocity, 100, fireColor, 1.5f);
//                dust.noGravity = true;
//            }
//        }
//    }
//}
