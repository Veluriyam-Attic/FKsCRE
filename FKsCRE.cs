using FKsCRE.Content.Ammunition.APreHardMode.AerialiteBullet;
using FKsCRE.Content.Ammunition.APreHardMode.TinkleshardBullet;
using FKsCRE.Content.Ammunition.APreHardMode.WulfrimBullet;
using FKsCRE.Content.Ammunition.BPrePlantera.CryonicBullet;
using FKsCRE.Content.Ammunition.BPrePlantera.StarblightSootBullet;
using FKsCRE.Content.Ammunition.CPreMoodLord.AstralBullet;
using FKsCRE.Content.Ammunition.CPreMoodLord.PerennialBullet;
using FKsCRE.Content.Ammunition.CPreMoodLord.PlagueBullet;
using FKsCRE.Content.Ammunition.CPreMoodLord.ScoriaBullet;
using FKsCRE.Content.Ammunition.DPreDog.DivineGeodeBullet;
using FKsCRE.Content.Ammunition.DPreDog.EffulgentFeatherBullet;
using FKsCRE.Content.Ammunition.DPreDog.PolterplasmBullet;
using FKsCRE.Content.Ammunition.DPreDog.ToothBullet;
using FKsCRE.Content.Ammunition.DPreDog.UelibloomBullet;
using FKsCRE.Content.Ammunition.EAfterDog.AuricBulet;
using FKsCRE.Content.Ammunition.EAfterDog.EndothermicEnergyBullet;
using FKsCRE.Content.Ammunition.EAfterDog.MiracleMatterBullet;
using FKsCRE.Content.Arrows.APreHardMode.AerialiteArrow;
using FKsCRE.Content.Arrows.APreHardMode.PrismArrow;
using FKsCRE.Content.Arrows.APreHardMode.WulfrimArrow;
using FKsCRE.Content.Arrows.BPrePlantera.StarblightSootArrow;
using FKsCRE.Content.Arrows.CPreMoodLord.AstralArrow;
using FKsCRE.Content.Arrows.CPreMoodLord.LifeAlloyArrow;
using FKsCRE.Content.Arrows.CPreMoodLord.PerennialArrow;
using FKsCRE.Content.Arrows.CPreMoodLord.PlagueArrow;
using FKsCRE.Content.Arrows.CPreMoodLord.ScoriaArrow;
using FKsCRE.Content.Arrows.DPreDog.DivineGeodeArrow;
using FKsCRE.Content.Arrows.DPreDog.EffulgentFeatherArrow;
using FKsCRE.Content.Arrows.DPreDog.ToothArrow;
using FKsCRE.Content.Arrows.DPreDog.UelibloomArrow;
using FKsCRE.Content.Arrows.EAfterDog.AuricArrow;
using FKsCRE.Content.Arrows.EAfterDog.EndothermicEnergyArrow;
using FKsCRE.Content.Arrows.EAfterDog.MiracleMatterArrow;
using FKsCRE.Content.Gel.ZBag;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.Testing;
using Terraria.UI;

namespace FKsCRE
{
    public class ModTime : ModSystem
    {
        public static int Time = 0;
        public override void UpdateUI(GameTime gameTime)
        {
            if (!Main.gamePaused)
            {
                Time++;
            }
            if (Time >= 86400)
            {
                Time = 0;
            }
            base.UpdateUI(gameTime);
        }
    }
    public class AnyRecipes : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup AnyArrow = new RecipeGroup(() => Language.GetTextValue("Mods.FKsCRE.RecipeGroup.Arrow"), new int[]
            {
                ModContent.ItemType<LifeAlloyArrow>(),
                ModContent.ItemType<AerialiteArrow>(),
                ModContent.ItemType<PrismArrow>(),
                ModContent.ItemType<WulfrimArrow>(),
                ModContent.ItemType<StarblightSootArrow>(),
                ModContent.ItemType<AstralArrow>(),
                ModContent.ItemType<PerennialArrow>(),
                ModContent.ItemType<PlagueArrow>(),
                ModContent.ItemType<ScoriaArrow>(),
                ModContent.ItemType<DivineGeodeArrow>(),
                ModContent.ItemType<EffulgentFeatherArrow>(),
                ModContent.ItemType<ToothArrow>(),
                ModContent.ItemType<UelibloomArrow>(),
                ModContent.ItemType<AuricArrow>(),
                ModContent.ItemType<EndothermicEnergyArrow>(),
                ModContent.ItemType<MiracleMatterArrow>()

            });
            AnyArrow.IconicItemId = ItemID.WoodenArrow;
            RecipeGroup.RegisterGroup("FKsCRE:RecipeGroupArrow", AnyArrow);


            RecipeGroup AnyBullet = new RecipeGroup(() => Language.GetTextValue("Mods.FKsCRE.RecipeGroup.Bullet"), new int[]
            {
                ModContent.ItemType<AerialiteBullet>(),
                ModContent.ItemType<TinkleshardBullet>(),
                ModContent.ItemType<WulfrimBullet>(),
                ModContent.ItemType<CryonicBullet>(),
                ModContent.ItemType<StarblightSootBullet>(),
                ModContent.ItemType<AstralBullet>(),
                ModContent.ItemType<PerennialBullet>(),
                ModContent.ItemType<PlagueBullet>(),
                ModContent.ItemType<ScoriaBullet>(),
                ModContent.ItemType<DivineGeodeBullet>(),
                ModContent.ItemType<EffulgentFeatherBullet>(),
                ModContent.ItemType<PolterplasmBullet>(),
                ModContent.ItemType<ToothBullet>(),
                ModContent.ItemType<UelibloomBullet>(),
                ModContent.ItemType<AuricBulet>(),
                ModContent.ItemType<EndothermicEnergyBullet>(),
                ModContent.ItemType<MiracleMatterBullet>()

            });
            AnyBullet.IconicItemId = ItemID.MusketBall;
            RecipeGroup.RegisterGroup("FKsCRE:RecipeGroupBullet", AnyBullet);
        }

    }
}
