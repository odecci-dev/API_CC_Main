namespace API_PCC.Constants
{
    public static class DBQuery
    {
        public static readonly String BIRTH_TYPE_SELECT = $@"SELECT * FROM A_BIRTH_TYPE ";
        public static readonly String BLOOD_COMPOSITION_SELECT = @"SELECT * FROM A_BLOOD_COMP ";
        public static readonly String BREED_SELECT = $@"SELECT * FROM A_BREED ";
        public static readonly String BUFF_ANIMAL_SELECT = $@"SELECT * FROM A_BUFF_ANIMAL ";
        public static readonly String TYPE_OWNERSHIP_SELECT = $@"SELECT * FROM A_TYPE_OWNERSHIP ";
        public static readonly String HERD_SELECT = $@"SELECT * FROM H_BUFF_HERD ";
        public static readonly String BUFFALO_TYPE_SELECT = $@"SELECT * FROM H_BUFFALO_TYPE ";
        public static readonly String FARMER_AFFILIATION_SELECT = $@"SELECT * FROM H_FARMER_AFFILIATION ";
        public static readonly String FEEDING_SYSTEM_SELECT = $@"SELECT * FROM H_FEEDING_SYSTEM ";
        public static readonly String HERD_CLASSIFICATION = $@"SELECT * FROM H_HERD_ClASSIFICATION ";

        public static readonly String CENTER_SELECT = $@"SELECT * FROM TBL_CENTERMODEL ";
        public static readonly String FARM_OWNER_SELECT = $@"SELECT * FROM TBL_FARMOWNER ";
        public static readonly String USERS_SELECT = $@"SELECT * FROM TBL_USERSMODEL ";
        public static readonly String MAIL_SENDER_CREDENTIALS_SELECT = $@"SELECT * FROM TBL_MAILSENDERCREDENTIAL ";
        public static readonly String REGISTRATION_OTP_SELECT = $@"SELECT * FROM TBL_REGISTRATIONOTPMODEL ";

        public static readonly String ACTION_TABLE_SELECT = $@"SELECT * FROM ACTION_TBL ";
        public static readonly String MODULE_TABLE_SELECT = $@"SELECT * FROM MODULE_TBL ";
    }
}
