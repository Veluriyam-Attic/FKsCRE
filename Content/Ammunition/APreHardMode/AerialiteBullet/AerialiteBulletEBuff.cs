using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition.APreHardMode.AerialiteBullet
{
    public class AerialiteBulletEBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true; // 设置为debuff
            Main.buffNoSave[Type] = true; // Buff不保存到存档
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            // 检查是否是Boss
            bool isBoss = npc.boss;

            // 根据是否是Boss调整速度
            if (isBoss)
            {
                // Boss速度增加至原来的1.075倍
                npc.velocity *= 1.075f;
            }
            else
            {
                // 普通敌人速度增加至原来的1.15倍
                npc.velocity *= 1.15f;
            }
        }
    }
}
