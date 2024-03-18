namespace API_PCC.Utils
{
    public class QueryBuilder
    {

        public static String buildHerdSearchQuery(String searchValue)
        {
            String herdSelect = Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 0";
            if (searchValue != null && searchValue != "")
            {
                herdSelect = herdSelect + " AND (HERD_CODE = '" + searchValue + "' OR HERD_NAME = '" + searchValue + "')";
            }
            return herdSelect;
        }
        public static String buildHerdArchiveQuery()
        {
            return Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 1";
        }
        public static String buildHerdSelectForRestoreQuery(int id)
        {
            return Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 1 AND id = " + id;
        }
        public static String buildHerdCheckDuplicateForRestoreQuery(String herdName, String herdCode)
        {
            return Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 0 AND (HERD_NAME = '" + herdName + "' OR HERD_CODE = '" + herdCode + ")";
        }
    }
}
