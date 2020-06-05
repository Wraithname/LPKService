namespace LPKService.Domain.Models.Work
{
    /// <summary>
    /// Результат работы обработчиков
    /// </summary>
    public class TCheckResult
    {
        public int rejType { get; set; }
        public string data { get; set; }
        public bool isOK { get; set; }
    }
}
