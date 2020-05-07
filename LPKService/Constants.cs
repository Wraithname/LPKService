namespace LPKService
{
    public static class Constants
    {
        const string L4_L3_EVENT_TABLE_NAME = "L4_L3_EVENT";
        const string L4_L3_SHIPPING_TABLE_NAME = "L4_L3_SHIPPING";
        const string L4_L3_PIECE_MOVEMENT_TABLE_NAME = "L4_L3_PIECE_MOVEMENT";
        const string CC_TABLE_NAME = "L4_L3_CUSTOMER";
        const string SO_MAIN_TABLE_NAME = "L4_L3_SO_HEADER";
        const string L4_L3_RAW_MATERIAL_TABLE_NAME = "L4_L3_RAW_MATERIAL";
        const string SAP_TORO_CATALOGUE = "L4_L3_CATALOGUE";
        const string L4_L3_EQP_STRUCTURE_TABLE = "L4_L3_STRUCTURE";
        const string L4_L3_STOP_TABLE_NAME = "L4_L3_STOP_RESULT";
        const string L4_L3_CAT_LINK_TABLE_NAME = "L4_L3_CAT_LINK";

        // L4 to L3 Messages
        const int L4_L3_SALES_ORDER       = 4301;
        const int L4_L3_CUSTOMER_CATALOG = 4303;
        const int L4_L3_SHIPPING = 4304;
        const int L4_L3_RAW_MATERIAL = 4305;
        const int L4_L3_PIECE_MOVEMENT = 4306;

        //SAP TORO Messages
        const int L4_L3_TORO_CATALOGUE = 4311;
        const int L4_L3_EQP_STRUCTURE = 4312;
        const int L4_L3_STOP_RESULT = 4313;
        const int L4_L3_CAT_LINK = 4314;
        const int L4_L3_STOP_RESULT_EXT = 4315;

        //message status code
        const int MSG_STATUS_INSERT = 1;
        const int MSG_STATUS_SUCCESS = 2;
        const int MSG_STATUS_WARNING = 3;
        const int MSG_STATUS_ERROR = -1;

        //operation code
        const int OP_CODE_INUP = 0; // Insert-Update
        const int OP_CODE_NEW = 1; // New
        const int OP_CODE_DEL = 3; // Delete
        const int OP_CODE_UPD = 2; // Update


        //piece move type
        const int PIECE_RETURN_FROM_CUSTOMER = 402;
        const int PIECE_INSERTED_INVENTORY = 403;
        const int PECE_DEL_FROM_INVENTORY = 405;

        //Rejection codes
        //GENERAL
        const int REJECT_GENERAL = 0;

        //define bol status.
        //The values are equal to Integer_value field of AUX_VALUES related to EXT_BOL_STATUS VARIABLE_ID
        const int BOL_NOT_SENT = 1;//equal to BOL_CREATED in AUX_VALUES
        const int BOL_SENT = 2;//equal to BOL_SHIPPED in AUX_VALUES
        const string BOL_HEADER_TABLE = "EXT_BOL_HEADER";

 //Constant related to piece
 //PIECE_STATUS_EXIST=STY_PIECE_STATUS_EXISTING;// defined in StockYardConsts Unit
 //PIECE_STATUS_GONE=STY_PIECE_STATUS_GONE;// defined in StockYardConsts Unit
 //PIECE_READY_TO_SHIP=YES;// defined in StockYardConsts Unit
 //PIECE_SHIPPED=STY_PIECE_EXIT_TYPE_SHIPPING;// defined in StockYardConsts Unit
    }
}
