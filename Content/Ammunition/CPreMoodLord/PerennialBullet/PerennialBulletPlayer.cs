using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader;
using System;

namespace FKsCRE.Content.Ammunition.CPreMoodLord.PerennialBullet
{
    public class PerennialBulletPlayer : ModPlayer
    {
      

        public int StackCount; // �������ѵ�����
        private int disableBuffTimer = 0; // ���ü�ʱ��

        public override void ResetEffects()
        {
            // ÿ֡���ټ�ʱ��
            if (disableBuffTimer > 0)
            {
                disableBuffTimer--;
            }
        }


        private int increaseStackCountCalls = 0; // ׷�ٵ��ô���

        public void IncreaseStackCount()
        {
            // ������ü�ʱ����Ч��ֱ�ӷ���
            if (disableBuffTimer > 0) return;

            increaseStackCountCalls++; // ÿ�ε������Ӽ���

            if (increaseStackCountCalls >= 50) // �����ô����ﵽ 50 ʱ����Ч��
            {
                if (StackCount < 10) // �ȼ���������� 10
                {
                    StackCount++; // ����һ��
                    int maxHealthIncrease = Main.getGoodWorld ? 100 : 10; // ÿ�����ӵ�����ֵ
                    //Player.statLifeMax2 += maxHealthIncrease; // �����������ֵ
                }
                Player.AddBuff(ModContent.BuffType<PerennialBulletPBuff>(), 600); // ˢ�� 10 ��� Buff

                // ����������ɢ����Ч��
                for (int i = 0; i < 40; i++) // �������� 40
                {
                    double angle = Math.PI * 2 * i / 40; // �������εĵ�λ��
                    float x = (float)(16 * Math.Sin(angle) * Math.Sin(angle) * Math.Sin(angle)); // x ���깫ʽ
                    float y = (float)(13 * Math.Cos(angle) - 5 * Math.Cos(2 * angle) - 2 * Math.Cos(3 * angle) - Math.Cos(4 * angle)); // y ���깫ʽ
                    Vector2 particlePosition = new Vector2(x, y) * 0.5f; // �������δ�С

                    Vector2 particleVelocity = particlePosition * Main.rand.NextFloat(0.5f, 1.5f); // �ٶ������η�����ɢ
                    Dust dust = Dust.NewDustPerfect(
                        Player.Center + particlePosition, // �����Ϊ����
                        DustID.GreenFairy,                // ��ɫ����
                        particleVelocity,                 // �����ٶ�
                        150,                              // ͸����
                        Color.LightGreen,                 // ������ɫ
                        Main.rand.NextFloat(1.2f, 1.6f)   // ���Ӵ�С
                    );
                    dust.noGravity = true; // ���Ӳ�������Ӱ��
                }

                // ���õ��ü�����
                increaseStackCountCalls = 0;
            }
        }


        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            ClearBuffOnHit();
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            ClearBuffOnHit();
        }

        private void ClearBuffOnHit()
        {
            // ��� Buff �Ͷѵ�����
            StackCount = 0;
            Player.ClearBuff(ModContent.BuffType<PerennialBulletPBuff>());

            // �������ü�ʱ����5�� = 300֡
            disableBuffTimer = 300;
        }
    }
}
