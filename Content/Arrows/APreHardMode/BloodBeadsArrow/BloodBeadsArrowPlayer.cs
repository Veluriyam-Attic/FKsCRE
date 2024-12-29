using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.Arrows.APreHardMode.BloodBeadsArrow
{
    public class BloodBeadsArrowPlayer : ModPlayer
    {
        public bool downedBoss2; // ��һ��а��boss����ܵĿ���
        public bool hardMode; // ����ģʽ�����Ŀ���
        public bool downedGolemBoss; // ʯ���˱���ܵĿ���

        private int cooldownTimer = 0; // ��ȴʱ�������λ��֡��1��=60֡��

        public override void ResetEffects()
        {
            // ÿ֡�ݼ���ȴʱ��
            if (cooldownTimer > 0)
            {
                cooldownTimer--;
            }
        }

        // �����ȴʱ���Ƿ����
        public bool CanApplyDebuff()
        {
            return cooldownTimer <= 0;
        }

        // ���� ApplyDebuffs ������Ӧ��debuffs��Ŀ�꣬��������ȴʱ��
        public void ApplyDebuffs(NPC target, int duration)
        {
            // ������ȴʱ��Ϊ1.25�루75֡��
            cooldownTimer = 75;

            List<int> debuffs = new List<int>()
            {
                BuffID.OnFire,           // �Ż�
                BuffID.Poisoned,         // �ж�
                BuffID.Confused,         // ����
                BuffID.Frostburn,        // ����
                BuffID.Ichor,            // ��Һ
                BuffID.CursedInferno,    // ���������
                BuffID.ShadowFlame,      // ��Ӱ��
                BuffID.Daybreak,         // ������Ƭ
                BuffID.BetsysCurse,      // ��ϣ������
                BuffID.Midas,            // ����˹
                BuffID.Wet,              // ʪ��
                BuffID.Suffocation,      // ��Ϣ
                BuffID.Burning,          // ȼ��
                BuffID.Bleeding,         // ��Ѫ
                BuffID.Weak,             // ����
                BuffID.WitheredArmor,    // ��ή�Ŀ���
                BuffID.WitheredWeapon,   // ��ή������
                BuffID.Silenced,         // ��Ĭ
                BuffID.BrokenArmor,      // ��������
                BuffID.Slimed,           // ճҺ
                BuffID.Slow,             // ����
                BuffID.Stoned,           // ʯ��
                BuffID.Venom,            // �綾
                BuffID.Dazed,            // ѣ��
                BuffID.ObsidianSkin,     // ����ʯƤ��
                BuffID.OgreSpit          // ʳ��ħ����Һ
            };

            // ���ݻ���boss�������Ϸ������Ӹ߼�debuff
            Mod calamityMod;
            if (ModLoader.TryGetMod("CalamityMod", out calamityMod))
            {
                if (Main.getGoodWorld || downedGolemBoss)
                {
                    debuffs.Add(calamityMod.Find<ModBuff>("MiracleBlight").Type);
                    debuffs.Add(calamityMod.Find<ModBuff>("VulnerabilityHex").Type);
                }

                if (hardMode)
                {
                    debuffs.Add(calamityMod.Find<ModBuff>("MarkedforDeath").Type);
                    debuffs.Add(calamityMod.Find<ModBuff>("KamiFlu").Type);
                    debuffs.Add(calamityMod.Find<ModBuff>("GlacialState").Type);
                }

                if (downedBoss2)
                {
                    debuffs.Add(calamityMod.Find<ModBuff>("HolyFlames").Type);
                    debuffs.Add(calamityMod.Find<ModBuff>("GodSlayerInferno").Type);
                }

                // ���һ���Ե� debuff
                debuffs.Add(calamityMod.Find<ModBuff>("BrimstoneFlames").Type);
                debuffs.Add(calamityMod.Find<ModBuff>("CrushDepth").Type);
                debuffs.Add(calamityMod.Find<ModBuff>("RancorBurn").Type);
            }


            // ���ѡ��һ��debuff��Ӧ��
            int randomDebuff = debuffs[Main.rand.Next(debuffs.Count)];
            target.AddBuff(randomDebuff, duration);
        }
    }
}
