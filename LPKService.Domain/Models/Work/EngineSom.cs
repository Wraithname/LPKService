using LPKService.Domain.Models.CCM;
using LPKService.Domain.Models.SOM;

namespace LPKService.Domain.Models.Work
{
    public class EngineSom
    {
        public L4L3SoHeader soHeader;
        public L4L3Customer customer;
        /// <summary>
        /// Контструктор присвоения строки заказчика
        /// </summary>
        /// <param name="customer">Строка заказчика</param>
        public EngineSom(L4L3Customer customer)
        {
            this.customer = customer;
        }
        /// <summary>
        /// Конструктор присвоения строки заказов
        /// </summary>
        /// <param name="soHeader">Строка заказов</param>
        public EngineSom(L4L3SoHeader soHeader)
        {
            this.soHeader = soHeader;
        }

    }
}
