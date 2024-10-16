namespace AgentManage
{
    public interface IAgentComponent
    {
        /**
         * <summary>
         * 맨 처음에 컴포넌트를 찾아오며 실행
         * </summary>
         */
        public void Initialize(Agent agent);
        /**
         * <summary>
         * Init이후 Handle들을 초기에 한번 발행해주기 위해 실행
         * </summary>
         */
        public void AfterInit();
        /**
         * <summary>
         * 파괴될때 주로 이벤트 구독 해제를 위해 실행 
         * </summary>
         */
        public void Dispose();
    }
}