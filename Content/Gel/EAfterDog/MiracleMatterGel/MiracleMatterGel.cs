using CalamityMod.Items.Materials;
using FKsCRE.Content.Gel.EAfterDog.AuricGel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace FKsCRE.Content.Gel.EAfterDog.MiracleMatterGel
{
    public class MiracleMatterGel : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Gel.EAfterDog";
        public override void SetDefaults()
        {
            //Item.damage = 1;
            Item.width = 12;
            Item.height = 18;
            Item.consumable = true;
            Item.ammo = AmmoID.Gel;
            Item.maxStack = 9999;
        }

        public override void OnConsumedAsAmmo(Item weapon, Player player)
        {
            // 附魔效果，标记弹幕使用了 MiracleMatterGel
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == player.whoAmI)
                {
                    proj.GetGlobalProjectile<MiracleMatterGelGP>().IsMiracleMatterGelInfused = true;
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient<MiracleMatter>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
