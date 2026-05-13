using System;
using System.Collections.Generic;
using System.Text;

namespace Exercice3
{
    public class Messagerie
    {
        public static event Action<string> MessageRecu;

        public static void Envoyer(string message)
        {
            MessageRecu?.Invoke(message);
        }
    }
}
