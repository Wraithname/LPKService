using System;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace Logger
{
    public enum RotatePeriod { Hour, Day, Week, Mounth };
    public class RotationFileLog : FileLog
    {
        protected const string FILENAME_HOUR_FORMAT = "yyyy-MM-dd-HH";
        protected const string FILENAME_DAY_FORMAT = "yyyy-MM-dd";
        protected const string FILENAME_WEEK_FORMAT = FILENAME_DAY_FORMAT;
        protected const string FILENAME_MOUNTH_FORMAT = "yyyy-MM";
        protected RotatePeriod LogRotatePeriod { get; set; }
        protected string LogDir { get; set; }
        protected string FileExt { get; }
        protected string FileNamePrefix { get; }

        /// <summary>
        /// Файловый логгер обеспечивающий создание новых логов согласно настройкам ротации
        /// </summary>
        /// <param name="name">Тег по умолчанию</param>
        /// <param name="logLevel">Уровень логгирования</param>
        /// <param name="rotatePeriod">Период ротации логов. См.<seealso cref="RotatePeriod"/></param>
        /// <param name="logDir">Папка с логами</param>
        /// <param name="lineFormat">Формат выводимой строки</param>
        /// <param name="fileNamePrefix">Префикс имени файла</param>
        /// <param name="fileExt">Расширение логов</param>
        public RotationFileLog(
            string name,
            LogLevel logLevel,
            RotatePeriod rotatePeriod,
            string logDir,
            string lineFormat,
            string fileNamePrefix,
            string fileExt
        ) : base(name, logLevel, null, lineFormat)
        {
            LogRotatePeriod = rotatePeriod;
            LogDir = logDir;
            FileExt = fileExt;
            FileNamePrefix = fileNamePrefix;
            if (!Directory.Exists(LogDir))
            {
                // пробуем создать директорию, если не удастся создать приложение упадет. Пусть админ решает вопрос с доступностью папки
                Directory.CreateDirectory(logDir);
            }
            // Доступна для записи?
            if (!DirectoryIsWriteble(LogDir))
            {
                // Зовите админа
                throw new AccessViolationException();
            }
            LoggerFileName = GetFileName();
        }

        /// <summary>
        /// Проверка доступности директории для записи
        /// </summary>
        /// <param name="dir">Проверяемая директория</param>
        /// <returns>true - доступна для записи</returns>
        private bool DirectoryIsWriteble(string dir)
        {
            var permission = new FileIOPermission(FileIOPermissionAccess.Write, dir);
            var permissionSet = new PermissionSet(PermissionState.None);
            permissionSet.AddPermission(permission);
            return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
        }

        /// <summary>
        /// Имя лог файла вместе с путем 
        /// </summary>
        /// <returns>Имя лог файла вместе с путем</returns>
        protected string GetFileName()
        {
            string format;
            switch (LogRotatePeriod)
            {
                case RotatePeriod.Hour:
                    {
                        format = FILENAME_HOUR_FORMAT;
                        break;
                    }
                case RotatePeriod.Week:
                    {
                        format = FILENAME_WEEK_FORMAT;
                        break;
                    }
                case RotatePeriod.Mounth:
                    {
                        format = FILENAME_MOUNTH_FORMAT;
                        break;
                    }
                case RotatePeriod.Day:
                default:
                    {
                        format = FILENAME_DAY_FORMAT;
                        break;
                    }
            }
            return Path.Combine(LogDir, $"{FileNamePrefix}_{DateTime.Now.ToString(format)}{FileExt}");
        }

        protected new void WriteLine(string text, bool append = true)
        {
            GetFileName();
            base.WriteLine(text, append);

        }
    }
}
