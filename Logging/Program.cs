using System;

namespace Logging
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Pathfinder[] pathfinders = new Pathfinder[]
            {
                new Pathfinder(new ConsoleLogger()),
                new Pathfinder(new FileLogger()),
                new Pathfinder(new FridayLogger(new ConsoleLogger())),
                new Pathfinder(new FridayLogger(new FileLogger())),
                new Pathfinder(new ParamsLogger(new ConsoleLogger(), new FridayLogger(new FileLogger())))
            };

            foreach (var pathfinder in pathfinders)
            {
                pathfinder.Find();
            }
        }
    }

    interface ILogger
    {
        void Write(string message);
    }

    class Pathfinder
    {
        private readonly ILogger _logger;

        public Pathfinder(ILogger logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger;
        }

        public void Find()
        {
            _logger.Write($"Логирую");
        }
    }

    class ConsoleLogger : ILogger
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FridayLogger : ILogger
    {
        private ILogger _logger;

        public FridayLogger(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Write(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _logger.Write(message);
            }
        }
    }

    class FileLogger : ILogger
    {
        public void Write(string message)
        {
            Console.WriteLine($"Пишем в файл [{message}]");
        }
    }

    class ParamsLogger : ILogger
    {
        private ILogger[] _loggers;

        public ParamsLogger(params ILogger[] loggers)
        {
            _loggers = loggers ?? throw new ArgumentNullException(nameof(loggers));
        }

        public void Write(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Write(message);
            }
        }
    }
}

