//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace FKsCRE.Content.Arrows
//{
//    public abstract class Arrow : ModItem
//    {
//        public override void SetDefaults()
//        {
//            Item.consumable = true;
//            Item.ammo = AmmoID.Arrow;
//            Item.damage = 10;
//            Item.DamageType = DamageClass.Ranged;
//            Item.maxStack = 9999;
//            base.SetDefaults();
//        }

//        public override void AddRecipes()
//        {
//            Recipe recipe = CreateRecipe();
//            recipe.AddIngredient(ItemID.Zenith, 999);
//            recipe.AddTile(TileID.AbigailsFlower);
//            recipe.Register();
//            base.AddRecipes();
//        }
//    }

//    public abstract class ArrowProjectile : ModProjectile
//    {
//        public override void SetDefaults()
//        {
//            Projectile.damage = 10;
//            Projectile.friendly = true;
//            Projectile.aiStyle = ProjAIStyleID.Arrow;
//        }
//    }

//    public class GArrow : GlobalProjectile
//    {
//        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
//        {
//            if(projectile.ModProjectile != null && projectile.ModProjectile.Name.Equals("天蓝矢_Proje"))
//            {
//                if(projectile.maxPenetrate == 1)
//                {
//                    modifiers.FinalDamage *= 0.5f;
//                }
//                projectile.maxPenetrate -= 1;
//            }
//            base.ModifyHitNPC(projectile, target, ref modifiers);
//        }
//    }
//}
