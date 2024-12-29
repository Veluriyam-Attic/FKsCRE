using CalamityMod.Items.Materials;
using FKsCRE.Content.Arrows.CPreMoodLord.CoreofCalamityArrow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace FKsCRE.Content.DeveloperItems.Arrow.GlitchArrow
{
    internal class GlitchArrow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.GlitchArrow";
        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true; // 弹药是消耗品
            Item.knockBack = 3.5f;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<GlitchArrowPROJ>();
            Item.shootSpeed = 15f;
            Item.ammo = AmmoID.Arrow; // 这是箭矢类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient<EssenceofHavoc>(1); // 混乱精华
            recipe.AddIngredient(ItemID.Nanites, 1); // 纳米机器人
            recipe.AddIngredient(ItemID.Cog, 1); // 齿轮
            recipe.AddIngredient(ItemID.SpectrePaintbrush, 1); // 幽灵刷子
            recipe.AddIngredient(ItemID.MartianLamppost, 1); // 火星灯柱
            recipe.AddIngredient(ItemID.ShimmerCampfire, 1); // 以太篝火
            recipe.AddIngredient(ItemID.JimsDrone, 1); // 四轴竞速无人机
            recipe.AddIngredient(ItemID.SkywarePiano, 1); // 天域钢琴
            recipe.AddIngredient(ItemID.BorealWoodBow, 1); // 针叶木弓
            recipe.AddIngredient(ItemID.PalladiumColumn, 1); // 钯金柱
            recipe.AddIngredient(ItemID.IchorTorch, 1); // 灵液火把
            recipe.AddIngredient(ItemID.Cannonball, 1); // 炮弹
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.Register();
        }
    }
}
