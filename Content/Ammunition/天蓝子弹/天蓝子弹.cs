using CalamityMod.Items.Materials;
using CalamityMod.Items.Mounts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;

namespace NanTing.Content.Ammunition.天蓝子弹
{
    public class 天蓝子弹 : ModItem
    {
        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.damage = 7;
            //暴击率
            Item.crit = 5;
            Item.shoot = ModContent.ProjectileType<天蓝子弹_Proje>();
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            //火枪子弹
            recipe.AddIngredient(ItemID.MusketBall,200);
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 1);
            //日盘块
            recipe.AddIngredient(ItemID.SunplateBlock, 1);
            //天魔
            recipe.AddTile(TileID.SkyMill);
            recipe.ReplaceResult(ModContent.ItemType<天蓝子弹>(), 200);
            base.AddRecipes();
        }
    }

    public class 天蓝子弹_Proje : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 7;
            //不受水影响
            Projectile.ignoreWater = true;
            Projectile.ai[0] = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            base.SetDefaults();
        }

        int sum = 1;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.velocity = Vector2.Normalize(Main.MouseWorld - Projectile.Center) * 17;
                Projectile.rotation = Projectile.velocity.ToRotation() * (float)Math.PI / 2;
                //Projectile.Center = Main.player[Projectile.owner].Center;
            }
            //微弱的最终
            //    double sin = vector1._x * vector2._y - vector2._x * vector1._y;  
            //    double cos = vector1._x * vector2._x + vector1._y * vector2._y;
            //
            //return Math.Atan2(sin, cos) * (180 / Math.PI);
            if (sum == 1)
            {
                //foreach (NPC npc in Main.npc)
                //{
                //    Vector2 npcv2 = npc.Center;
                //    Vector2 projev2 = Projectile.Center;
                //    double sin = npcv2.X * projev2.X - npcv2.Y * projev2.Y;
                //    double cos = npcv2.X * projev2.X + projev2.Y * npcv2.Y;
                //    double e = Math.Atan2(sin, cos) * (180 / Math.PI);
                //    Main.NewText(Math.Abs(e));
                //    if (npc.friendly == false && Vector2.Distance(Projectile.Center, npc.Center) < 200 && npc.active && Math.Abs(e) < 10)
                //    {
                //        Main.NewText(true);
                //        sum++;
                //        Projectile.velocity = Vector2.Normalize(npcv2 - projev2) * 17;
                //        break;
                //    }
                //}
            }
            if (sum != 1)
            {
                //Projectile.velocity *= (float)Math.Cos(30);
            }
            

            /*
            int sc = Main.screenWidth;
            //大就是右边 小就是左边
            if(Main.MouseScreen.X >= Main.screenWidth / 2)
            {
                if (Projectile.ai[0] == 0)
                {

                }
            }
            */
            Projectile.ai[0]++;

            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.Begin();
            //spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Content/Ammunition/天蓝子弹/天蓝子弹").Value,Projectile.);

            spriteBatch.End();

            base.AI();
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.AbigailAttack,Projectile.Center);
            base.OnKill(timeLeft);
        }
    }

}
