namespace ItemSystem
{
    public interface IItemCollectable
    {
        /**
         * <param name="id">
         * 얻을 아이템 ID
         * </param>
         * <param name="amount">
         * 얻을 아이템 개수
         * </param>
         * <summary>
         * 아이템 수집해주는 함수
         * </summary>
         * <returns>
         * 얻고 남은 아이템
         * </returns>
         */
        public int CollectItem(int id, int amount);
    }
}