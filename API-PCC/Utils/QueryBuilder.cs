using API_PCC.ApplicationModels;
using static API_PCC.Controllers.BuffAnimalsController;

namespace API_PCC.Utils
{
    public class QueryBuilder
    {

        public static String buildHerdSearchQuery(BuffHerdSearchFilterModel searchFilterModel)
        {
            String herdSelect = Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 0 ";
            if (searchFilterModel.searchValue != null && searchFilterModel.searchValue != "")
            {
                herdSelect = herdSelect + "AND (HERD_CODE LIKE '%" + searchFilterModel.searchValue + "%' OR HERD_NAME = '%" + searchFilterModel.searchValue + "%') ";
            }

            if (searchFilterModel.filterBy != null)
            {
                if (searchFilterModel.filterBy.BreedTypeCode != null && searchFilterModel.filterBy.BreedTypeCode != "")
                {
                    herdSelect = herdSelect + "AND BREED_TYPE_CODE = '" + searchFilterModel.filterBy.BreedTypeCode + "' ";
                }

                if (searchFilterModel.filterBy.HerdClassDesc != null && searchFilterModel.filterBy.HerdClassDesc != "")
                {
                    herdSelect = herdSelect + "AND HERD_CLASS_DESC = '" + searchFilterModel.filterBy.HerdClassDesc + "' ";
                }

                if (searchFilterModel.filterBy.feedingSystemCode != null && searchFilterModel.filterBy.feedingSystemCode != "")
                {
                    herdSelect = herdSelect + "AND FEEDING_SYSTEM_CODE = '" + searchFilterModel.filterBy.feedingSystemCode + "' ";
                }
            }

            if (searchFilterModel.dateFrom != null)
            {
                herdSelect = herdSelect + "AND DATE_CREATED >= '" + Convert.ToDateTime(searchFilterModel.dateFrom).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }

            if (searchFilterModel.dateTo != null)
            {
                herdSelect = herdSelect + "AND DATE_CREATED <= '" + Convert.ToDateTime(searchFilterModel.dateFrom).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }

            if (searchFilterModel.sortBy != null)
            {
                if (searchFilterModel.sortBy.Field != null && searchFilterModel.sortBy.Field != "")
                {
                    herdSelect = herdSelect + "ORDER BY " + searchFilterModel.sortBy.Field + " ";
                }
                if (searchFilterModel.sortBy.Sort != null && searchFilterModel.sortBy.Sort != "")
                {
                    herdSelect = herdSelect + searchFilterModel.sortBy.Sort;
                }
            }

            return herdSelect;
        }

        public static String buildHerdViewQuery(String herdCode)
        {
            String herdSelect = Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 0";
            if (herdCode!= null && herdCode != "")
            {
                herdSelect = herdSelect + " AND (HERD_CODE LIKE '%" + herdCode + "%' OR HERD_NAME = '%" + herdCode + "%')";
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
            return Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 0 AND (HERD_NAME = '" + herdName + "' AND HERD_CODE = '" + herdCode + "')";
        }

        public static String buildHerdSelectQueryById(int id)
        {
            return Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 0 AND id = " + id;
        }
        public static String buildHerdSelectDuplicateQueryByIdHerdNameHerdCode(int id, String herdName, String herdCode)
        {
            return Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 0 AND NOT id = " + id + "  AND HERD_NAME = '" + herdName + "' AND HERD_CODE = '" + herdCode + "'";
        }

        public static String buildFarmOwnerSearchQueryByFirstNameAndLastName(String firstName, String lastName)
        {
            return Constants.DBQuery.FARM_OWNER_SELECT + "WHERE FirstName = '" + firstName + "' AND LastName = '" + lastName + "'";
        }
        public static String buildFarmOwnerSearchQueryById(int id)
        {
            return Constants.DBQuery.FARM_OWNER_SELECT + "WHERE id = " + id;
        }

        public static String buildBuffAnimalSearch(BuffAnimalSearchFilterModel searchFilterModel)
        {
            String buffAnimalSelect = Constants.DBQuery.BUFF_ANIMAL_SELECT + "WHERE DELETE_FLAG = 0 ";
            if (searchFilterModel.searchValue != null && searchFilterModel.searchValue != "")
            {
                buffAnimalSelect = buffAnimalSelect + "AND (ANIMAL_ID_NUMBER LIKE '%" + searchFilterModel.searchValue + "%' OR ANIMAL_NAME = '%" + searchFilterModel.searchValue + "%') ";
            }

            if (searchFilterModel.sex != null && searchFilterModel.sex != "")
            {
                buffAnimalSelect = buffAnimalSelect + "AND (SEX = '" + searchFilterModel.sex + "' ";
            }

            if (searchFilterModel.status != null && searchFilterModel.status != "")
            {
                buffAnimalSelect = buffAnimalSelect + "AND (STATUS = '" + searchFilterModel.status + "' ";
            }

            if (searchFilterModel.filterBy != null)
            {
                if (searchFilterModel.filterBy.BloodCode != null && searchFilterModel.filterBy.BloodCode != "")
                {
                    buffAnimalSelect = buffAnimalSelect + "AND BLOOD_CODE = '" + searchFilterModel.filterBy.BloodCode + "' ";
                }

                if (searchFilterModel.filterBy.BreedCode != null && searchFilterModel.filterBy.BreedCode != "")
                {
                    buffAnimalSelect = buffAnimalSelect + "AND BREED_CODE = '" + searchFilterModel.filterBy.BreedCode + "' ";
                }

                if (searchFilterModel.filterBy.TypeOfOwnership != null && searchFilterModel.filterBy.TypeOfOwnership != "")
                {
                    buffAnimalSelect = buffAnimalSelect + "AND TYPE_OF_OWNERSHIP = '" + searchFilterModel.filterBy.TypeOfOwnership + "' ";
                }
            }

            if (searchFilterModel.sortBy != null)
            {
                if (searchFilterModel.sortBy.Field != null && searchFilterModel.sortBy.Field != "")
                {
                    buffAnimalSelect = buffAnimalSelect + "ORDER BY " + searchFilterModel.sortBy.Field + " ";
                }
                if (searchFilterModel.sortBy.Sort != null && searchFilterModel.sortBy.Sort != "")
                {
                    buffAnimalSelect = buffAnimalSelect + searchFilterModel.sortBy.Sort;
                }
            }

            return buffAnimalSelect;
        }

        public static String buildBuffAnimalSearchByReferenceNumber(String referencenUmber)
        {
            String buffAnimalSelect = Constants.DBQuery.BUFF_ANIMAL_SELECT + "WHERE DELETE_FLAG = 0 AND ( DAM_REGISTRATION_NUMBER = '" + referencenUmber + "' OR RFID_NUMBER = '" + referencenUmber + "')";
            return buffAnimalSelect;
        }

        public static String buildBuffAnimalDuplicateQuery(BuffAnimalRegistrationModel buffAnimalRegistrationModel)
        {
            String buffAnimalDuplicateQuery = Constants.DBQuery.BUFF_ANIMAL_SELECT + "WHERE DELETE_FLAG = 0 AND ( HERD_CODE = '" + buffAnimalRegistrationModel.HerdCode + "' AND ANIMAL_ID_Number = '" + buffAnimalRegistrationModel.AnimalIdNumber;
            return buffAnimalDuplicateQuery;
        }

        public static String buildBuffAnimalSearchById(int id)
        {
            String buffAnimalDuplicateQuery = Constants.DBQuery.BUFF_ANIMAL_SELECT + "WHERE DELETE_FLAG = 0 AND id = " + id;
            return buffAnimalDuplicateQuery;
        }

        public static String buildBuffAnimalSelectDuplicateQueryByIdAnimalIdNumberName(int id, String animalIdNumber, String animalName)
        {
            return Constants.DBQuery.HERD_SELECT + "WHERE DELETE_FLAG = 0 AND NOT id = " + id + "  AND ANIMAL_ID_NUMBER = '" + animalIdNumber + "' AND ANIMAL_NAME = '" + animalName + "'";
        }

        public static String buildSireSearchQueryById(int id)
        {
            return Constants.DBQuery.SIRE_TABLE_SELECT + "WHERE id = " + id;
        }

        public static String buildDamSearchQueryById(int id)
        {
            return Constants.DBQuery.DAM_TABLE_SELECT + "WHERE id = " + id;
        }

    }

}
