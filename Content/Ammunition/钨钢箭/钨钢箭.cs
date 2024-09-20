//using CalamityMod.Tiles.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//无合成表
namespace NanTing.Content.Ammunition.钨钢箭
{
    static class ty
    {
        public static int dam = 10;
    }
    public class 钨钢箭僵硬 : GlobalNPC
    {

    }

    public class 钨钢箭 : ModItem
    {
        public override void SetDefaults()
        {
            Item.ammo = AmmoID.Arrow;
            Item.consumable = true;
            Item.damage = ty.dam;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.shoot = ModContent.ProjectileType<钨钢箭_proje>();
        }
    }

    public class 钨钢箭_proje : ModProjectile
    {
        int num = 0;
        Vector2 Mouse_initial = default;
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.damage = ty.dam;
            Projectile.timeLeft = 999;
        }
        public override void AI()
        {
            if(num == 0)
            {
                Mouse_initial = Main.MouseWorld;
                Projectile.velocity = Vector2.Normalize(Mouse_initial - Main.player[Projectile.owner].Center) * 12;
            }
            //下坠
            //Projectile.velocity.Y += (num >= 180) ? Projectile.velocity.Y += 3f : Projectile.velocity.Y += 0.05f;
            if (num >= 180) Projectile.velocity.Y += 0.25f;
            if (num < 180) Projectile.velocity.Y += 0.15f;
            //角度
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi / 2;

            num++;
            base.AI();
        }
        public static Vector2 Timev2 = default;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Timev2 = target.Center;
            //5秒
            target.AddBuff(ModContent.BuffType<钨钢箭电减_DeBuff>(),300);
            //30秒
            //int[] ints = target.buffType;
            //如果有
            if (target.HasBuff<钨钢定身DeBuff>())
            { 
            }
            else
            {
                target.AddBuff(ModContent.BuffType<钨钢定身DeBuff>(), 1800);
            }
            base.OnHitNPC(target, hit, damageDone);
        }
    }
    public class 钨钢定身DeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            //不能被护士消除
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            //不显示时间
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = false;//默认也是false 死亡清除
            Main.vanityPet[Type] = false;//宠物?
            base.SetStaticDefaults();
        }
        int num = 0;
        Vector2 ver = Vector2.Zero;
        public override void Update(NPC npc, ref int buffIndex)
        {
            if(num == 0)
            {
                ver = 钨钢箭_proje.Timev2;
            }
            num ++;
            if(num <= 60)
            {
                npc.Center = ver;
            }
            if(num >= 1800)num = 0;
            base.Update(npc, ref buffIndex);
        }
    }

    public class 钨钢箭电减_DeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            //不能被护士消除
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            //不显示时间
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = false;//默认也是false 死亡清除
            Main.vanityPet[Type] = false;//宠物?
            base.SetStaticDefaults();
        }
        int num  = 0;
        public override void Update(NPC npc, ref int buffIndex)
        {
            if(num == 0)
            {
                npc.defense -= 3;
            }
            if (num % 60 == 0)
            {
                //扣血 2点
                Rectangle rectangle = new Rectangle((int)npc.Center.X,(int)npc.Center.Y,10,10);
                npc.life -= 2;
                CombatText.NewText(rectangle /*npc.getRect()*/, Color.Aqua, "2");
                
            }
            Dust du = Dust.NewDustDirect(npc.position, npc.width,npc.height, DustID.UltraBrightTorch, 2F, 2F);
            Dust du2 = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.UltraBrightTorch, -2F, 2F);
            du.noGravity = true;
            du2.noGravity = true;
            num++;
            base.Update(npc, ref buffIndex);
        }
    }
}
