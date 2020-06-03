namespace LPKService.Infrastructure.Repository
{
    public class L4L3InterfaceServiceConst
    {
        //Код обработки
        public const int L4_L3_SALES_ORDER = 4301;
        public const int L4_L3_CUSTOMER_CATALOG = 4303;
        public const int L4_L3_SHIPPING = 4304;
        public const int L4_L3_RAW_MATERIAL = 4305;

        //Статус сообщения
        public const int MSG_STATUS_INSERT = 1;
        public const int MSG_STATUS_SUCCESS = 2;
        public const int MSG_STATUS_WARNING = 3;
        public const int MSG_STATUS_ERROR = -1;

        //Код операции
        public const int OP_CODE_INUP = 0; // Insert-Update
        public const int OP_CODE_NEW = 1; // New
        public const int OP_CODE_DEL = 3; // Delete
        public const int OP_CODE_UPD = 2; // Update


        //Тип перемещения
        public const int PIECE_RETURN_FROM_CUSTOMER = 402;
        public const int PIECE_INSERTED_INVENTORY = 403;
        public const int PECE_DEL_FROM_INVENTORY = 405;

        public const int REJECT_GENERAL = 0;

        //Код позиции
        public const int BOL_NOT_SENT = 1;
        public const int BOL_SENT = 2;
        public const string BOL_HEADER_TABLE = "EXT_BOL_HEADER";
    }
}
