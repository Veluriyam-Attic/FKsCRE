using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader;

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
                StackCount += Main.getGoodWorld ? 10 : 1; // ������1���������ҫ�������10
                Player.AddBuff(ModContent.BuffType<PerennialBulletPBuff>(), 600); // ˢ�� 10 ��� Buff

                // �������ɹ�������������ɫ������Ч
                for (int i = 0; i < 8; i++) // ���� 8 ������
                {
                    Vector2 particleVelocity = Main.rand.NextVector2CircularEdge(2f, 2f) * 1.5f; // �Ͽ��ٶȣ�Բ�α߽�
                    Dust dust = Dust.NewDustPerfect(
                        Player.Center,              // ��������λ��
                        DustID.GreenFairy,          // ��ɫ����
                        particleVelocity,           // �����ٶ�
                        150,                        // ͸����
                        Color.LightGreen,           // ������ɫ
                        Main.rand.NextFloat(1.2f, 1.6f) // ���Ӵ�С
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
