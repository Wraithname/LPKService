namespace Work
{
    public class L4L3InterfaceServiceConst
    {
        public const string L4_L3_EVENT_TABLE_NAME = "L4_L3_EVENT";
        public const string L4_L3_SHIPPING_TABLE_NAME = "L4_L3_SHIPPING";
        public const string L4_L3_PIECE_MOVEMENT_TABLE_NAME = "L4_L3_PIECE_MOVEMENT";
        public const string CC_TABLE_NAME = "L4_L3_CUSTOMER";
        public const string SO_MAIN_TABLE_NAME = "L4_L3_SO_HEADER";
        public const string L4_L3_RAW_MATERIAL_TABLE_NAME = "L4_L3_RAW_MATERIAL";
        public const string SAP_TORO_CATALOGUE = "L4_L3_CATALOGUE";
        public const string L4_L3_EQP_STRUCTURE_TABLE = "L4_L3_STRUCTURE";
        public const string L4_L3_STOP_TABLE_NAME = "L4_L3_STOP_RESULT";
        public const string L4_L3_CAT_LINK_TABLE_NAME = "L4_L3_CAT_LINK";

        // L4 to L3 Messages
        public const int L4_L3_SALES_ORDER       = 4301;
        public const int L4_L3_CUSTOMER_CATALOG = 4303;
        public const int L4_L3_SHIPPING = 4304;
        public const int L4_L3_RAW_MATERIAL = 4305;
        public const int L4_L3_PIECE_MOVEMENT = 4306;

        //SAP TORO Messages
        public const int L4_L3_TORO_CATALOGUE = 4311;
        public const int L4_L3_EQP_STRUCTURE = 4312;
        public const int L4_L3_STOP_RESULT = 4313;
        public const int L4_L3_CAT_LINK = 4314;
        public const int L4_L3_STOP_RESULT_EXT = 4315;

        //message status code
        public const int MSG_STATUS_INSERT = 1;
        public const int MSG_STATUS_SUCCESS = 2;
        public const int MSG_STATUS_WARNING = 3;
        public const int MSG_STATUS_ERROR = -1;

        //operation code
        public const int OP_CODE_INUP = 0; // Insert-Update
        public const int OP_CODE_NEW = 1; // New
        public const int OP_CODE_DEL = 3; // Delete
        public const int OP_CODE_UPD = 2; // Update


        //piece move type
        public const int PIECE_RETURN_FROM_CUSTOMER = 402;
        public const int PIECE_INSERTED_INVENTORY = 403;
        public const int PECE_DEL_FROM_INVENTORY = 405;

        //Rejection codes
        //GENERAL
        public const int REJECT_GENERAL = 0;

        //define bol status.
        //The values are equal to Integer_value field of AUX_VALUES related to EXT_BOL_STATUS VARIABLE_ID
        public const int BOL_NOT_SENT = 1;//equal to BOL_CREATED in AUX_VALUES
        public const int BOL_SENT = 2;//equal to BOL_SHIPPED in AUX_VALUES
        public const string BOL_HEADER_TABLE = "EXT_BOL_HEADER";

 //Constant related to piece
 //PIECE_STATUS_EXIST=STY_PIECE_STATUS_EXISTING;// defined in StockYardConsts Unit
 //PIECE_STATUS_GONE=STY_PIECE_STATUS_GONE;// defined in StockYardConsts Unit
 //PIECE_READY_TO_SHIP=YES;// defined in StockYardConsts Unit
 //PIECE_SHIPPED=STY_PIECE_EXIT_TYPE_SHIPPING;// defined in StockYardConsts Unit
    }
}
