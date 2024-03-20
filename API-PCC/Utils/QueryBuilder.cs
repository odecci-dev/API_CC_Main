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
        public static String buildHerdCheckDuplicateQuery(String herdName, String herdCode)
        {
            return Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 0 AND (HERD_NAME = '" + herdName + "' OR HERD_CODE = '" + herdCode + "')";
        }

        public static String buildHerdSelectQueryById(int id)
        {
            return Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 0 AND id = " + id;
        }
        public static String buildHerdSelectDuplicateQueryByIdHerdNameHerdCode(int id, String herdName, String herdCode)
        {
            return Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 0 AND NOT id = " + id + "  AND (HERD_NAME = '" + herdName + "' OR HERD_CODE = '" + herdCode + "')";
        }

        public static String buildFarmOwnerSearchQueryByFirstNameAndLastName(String firstName, String lastName)
        {
            return Constants.DBQuery.FARM_OWNER_SELECT + "WHERE FirstName = '" + firstName + "' AND LastName = '" + lastName + "'";
        }

        public static String buildBuffAnimalSearch(String animalId, String name)
        {
            String buffAnimalSelect = Constants.DBQuery.BUFF_ANIMAL_SELECT + "WHERE DELETE_FLAG = 0";
            if (animalId != null && animalId != "")
            {
                buffAnimalSelect = buffAnimalSelect + " AND ANIMAL_ID = '" + animalId + "'";
            }
            if (name != null && name != "")
            {
                buffAnimalSelect = buffAnimalSelect + " AND Name = '" + name + "' ";
            }
            return buffAnimalSelect;
        }
    }
}
