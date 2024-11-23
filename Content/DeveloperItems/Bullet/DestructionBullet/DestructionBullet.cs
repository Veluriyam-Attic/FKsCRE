using FKsCRE.Content.DeveloperItems.Bullet.AllTheBirds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;

namespace FKsCRE.Content.DeveloperItems.Bullet.DestructionBullet
{
    internal class DestructionBullet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.DestructionBullet";
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true; // 弹药是消耗品
            Item.knockBack = 3.5f;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<DestructionBulletPROJ>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Bullet; // 这是子弹类型的弹药
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe(333);
            recipe1.AddIngredient(ItemID.IchorBullet, 333); // 灵液弹
            recipe1.AddIngredient(ItemID.CursedBullet, 333); // 诅咒弹
            recipe1.AddIngredient(ItemID.VenomBullet, 333); // 毒液弹
            recipe1.AddIngredient(ItemID.GoldenBullet, 333); // 金子弹
            recipe1.AddIngredient<GalacticaSingularity>(1); // 星系异石
            recipe1.AddIngredient<DivineGeode>(1); // 神圣晶石
            recipe1.AddIngredient<UelibloomBar>(1); // 龙蒿锭
            recipe1.AddIngredient<TitanHeart>(1); // 泰坦之心
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();
        }

    }
}
