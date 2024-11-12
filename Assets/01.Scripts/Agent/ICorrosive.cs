namespace AgentManage
{
    public interface ICorrosive
    { 
        // ICorrosive 와 IDamageable을 분리한 이유: 대미지를 받는 것과 부식 당하는 것은 다른 개념이기 때문
        public void Corrode(int power);
    }
}