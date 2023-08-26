using TaleWorlds.Library;

namespace InstantLeaveLoadingBar
{
    public class Messenger
    {
        public enum Level
        {
            Debug,
            Info,
            Warning,
            Error,
            Success,
        }
        public string prefix;
        private static Messenger instance = null;

        public static Messenger Current()
        {
            if (instance == null)
            {
                instance = new Messenger();
            }

            return instance;
        }

        public static void Prefix(string prefix)
        {
            Messenger messenger = Current();
            messenger.prefix = "[" + prefix + "] ";
        }

        public static void Log(string message, Level level = Level.Info)
        {
            Messenger messenger = Current();
            messenger.DisplayMessage(messenger.prefix + message, messenger.GetColor(level));
        }

        protected void DisplayMessage(string message, Color color)
        {
            InformationManager.DisplayMessage(new InformationMessage(message, color));
        }

        protected Color GetColor(Level level)
        {
            Color color = Colors.White;

            switch (level)
            {
                case Level.Debug:
                    color = Colors.Magenta;
                    break;
                case Level.Info:
                    color = Colors.White;
                    break;
                case Level.Warning:
                    color = Colors.Yellow;
                    break;
                case Level.Error:
                    color = Colors.Red;
                    break;
                case Level.Success:
                    color = Colors.Green;
                    break;
            }

            return color;
        }
    }
}