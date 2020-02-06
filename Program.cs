using System;
using System.IO;

namespace ConsoleAppBuenasPracticasEjercicio1
{
    class Program
    {
        static void Main(string[] args)
        {   
            ReadFile("C:\\CursoBuenasPracticasBOT\\ConsoleAppBuenasPracticasEjercicio1", "Eventos.txt");
        }

        private static void ReadFile(string path, string fileName)
        {
            string fullPath = ValidatePath(path);
            string filePath;
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                System.Console.WriteLine("La ruta proporcionada del archivo no existe.");
            }
            else if (string.IsNullOrWhiteSpace(fileName.Trim()))
            {
                System.Console.WriteLine("El nombre del archivo es incorrecto o vacío.");
            }
            else
            {
                filePath = string.Format("{0}\\{1}", fullPath, fileName.Trim());

                if (File.Exists(filePath))
                {
                    string text = System.IO.File.ReadAllText(filePath);
                    //Imprimimos el contenido en la consola
                    //System.Console.WriteLine("Contenido del archivo = \n {0}", text);
                    CreateEventMessages(filePath);
                }
                else
                {
                    System.Console.WriteLine("El archivo no existe en la ruta especificada: " + filePath);
                }
            }
        }

        private static void CreateEventMessages(string filePath)
        {
            //Leemos linea por línea para procesar la información
            string[] lines = System.IO.File.ReadAllLines(filePath);
            DateTime dateTimeNow = DateTime.Now;
            foreach (string line in lines)
            {
                Console.WriteLine("\t" + GenerateEventMessage(line, dateTimeNow));
            }
        }

        private static string GenerateEventMessage(string line, DateTime dateTimeNow)
        {
            string[] dataValues = line.Split(",");
            DateTime dateTimeEvent;
            if (dataValues.Length != 2)
            {
                return string.Format("Evento incorrecto para la linea con valor '{0}'.", line);
            }

            if (DateTime.TryParse(dataValues[1], out dateTimeEvent))
            {
                return string.Format("{0} {1}", dataValues[0], GenerateTimeElapsedMessage(dateTimeNow, dateTimeEvent));
            }
            else
            {
                return string.Format("Fecha incorrecta para la linea con valor '{0}'.", line);
            }
        }

        private static string GenerateTimeElapsedMessage(DateTime dateTimeInit, DateTime dateTimeEnd)
        {
            string timeElapsed = "";
            TimeSpan timeSpan;
            if (dateTimeInit > dateTimeEnd)
            {
                timeSpan = dateTimeInit - dateTimeEnd;
                timeElapsed = GenerateMessage(timeSpan, "ocurrió hace ");
            }
            else if (dateTimeEnd > dateTimeInit)
            {
                timeSpan = dateTimeEnd - dateTimeInit;
                timeElapsed = GenerateMessage(timeSpan, "ocurrirá en ");
            }
            else
            {
                timeElapsed = " inicia en este mismo momento.";
            }

            return timeElapsed;
        }

        private static string GenerateMessage(TimeSpan timeSpan, string word)
        {
            string message = string.Empty;

            if (timeSpan.TotalDays > 0 && timeSpan.TotalDays >= 30)
            {
                double months = Math.Truncate(timeSpan.TotalDays / 30);
                if (months > 1)
                {
                    message = string.Format("{0} {1} meses.", word, months);
                }
                else
                {
                    message = string.Format("{0} {1} mes.", word, months);
                }
            }
            else if (timeSpan.TotalDays >= 1)
            {
                double totalHoursDays = timeSpan.TotalDays * 24;
                double diference = timeSpan.TotalHours - totalHoursDays;
                if (diference > 0 && diference > 12)
                {
                    message = string.Format("{0} {1} días.", word, timeSpan.TotalDays + 1);
                }
                else
                {
                    message = string.Format("{0} {1} días.", word, timeSpan.TotalDays);
                }
            }
            else if (timeSpan.TotalHours > 12)
            {
                message = string.Format("{0} 1 día.", word);
            }
            else if (timeSpan.TotalHours >= 1)
            {
                double hours = Math.Truncate(timeSpan.TotalHours);
                if (hours > 1)
                {
                    message = string.Format("{0} {1} horas.", word, hours);
                }
                else
                {
                    message = string.Format("{0} {1} hora.", word, hours);
                }                
            }
            else
            {
                message = string.Format("{0} {1} minutos.", word, timeSpan.Minutes);
            }

            return message;
        }

        private static string ValidatePath(string path)
        {
            string fullPath = string.Empty;
            if (!string.IsNullOrWhiteSpace(path))
            {
                fullPath = Path.GetFullPath(path);
            }

            return fullPath;
        }
    }
}
